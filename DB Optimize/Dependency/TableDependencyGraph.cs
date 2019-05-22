using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DB_Optimize.DB;

namespace DB_Optimize.Dependency
{
    public class TableDependencyGraph
    {
        private IDictionary<int, DependencyNode> _tables;

        public DependencyNode this[DatabaseObject table] => _tables[table.ID];

        public IList<DependencyEdge> DependencyEdges { get; private set; }

        public ISet<DependencyNode> NodesWithoutEdges
        {
            get
            {
                ISet<DependencyNode> result = new HashSet<DependencyNode>();
                foreach (DependencyNode node in _tables.Values)
                {
                    if (node.Parents.Count == 0 && node.Children.Count == 0)
                    {
                        result.Add(node);
                    }
                }
                return result;
            }
        }

        public ISet<DependencyNode> Nodes => new HashSet<DependencyNode>(_tables.Values);

        public TableDependencyGraph() : this(null, false)
        {
        }

        public TableDependencyGraph(IList<DatabaseObject> tablesLimit, bool onlyTablesInLimit)
        {
            GenerateTableDependencyGraph(tablesLimit, onlyTablesInLimit);
        }

        public IList<DependencyNode> GetTopologicallySortedNodes(DatabaseObject startNode, bool selectParents, out ISet<DependencyEdge> cycleEdges)
        {
            IList<DependencyNode> result = new List<DependencyNode>();
            ISet<DependencyNode> processed = new HashSet<DependencyNode>();
            ISet<DependencyNode> processing = new HashSet<DependencyNode>();
            Func<DependencyNode, ISet<DependencyNode>> selectionNodesFunc;
            Func<DependencyNode, ISet<DependencyEdge>> selectionEdgesFunc;
            Func<DependencyEdge, DependencyNode, DependencyNode, bool> checkEdgeFunc;
            cycleEdges = new HashSet<DependencyEdge>();
            if (selectParents)
            {
                selectionNodesFunc = n => n.Parents;
                selectionEdgesFunc = n => n.ChildEdges;
                checkEdgeFunc = (e, pn, n) => e.Child.Equals(pn) && e.Parent.Equals(n);
            }
            else
            {
                selectionNodesFunc = n => n.Children;
                selectionEdgesFunc = n => n.ParentEdges;
                checkEdgeFunc = (e, pn, n) => e.Child.Equals(n) && e.Parent.Equals(pn);
            }
            GetTopologicallySortedNodesVisitNode(_tables[startNode.ID], null, result, processed, processing, selectionNodesFunc, selectionEdgesFunc, checkEdgeFunc, cycleEdges);
            return result;
        }

        private void GenerateTableDependencyGraph(IList<DatabaseObject> tablesLimit, bool onlyTablesInLimit)
        {
            if (tablesLimit != null)
            {
                tablesLimit = new List<DatabaseObject>(tablesLimit);
            }
            _tables = new Dictionary<int, DependencyNode>();
            DatabaseOperations databaseOperations = new DatabaseOperations(Database.Instance);
            try
            {
                IList<DatabaseObject> tables = onlyTablesInLimit && tablesLimit != null ? new List<DatabaseObject>(tablesLimit) : databaseOperations.GetTables();
                foreach (DatabaseObject table in tables)
                {
                    _tables.Add(table.ID, new DependencyNode(table));
                }
                DependencyEdges = onlyTablesInLimit ? databaseOperations.GetDependencyEdges(tablesLimit) : databaseOperations.GetDependencyEdges();
                foreach (DependencyEdge edge in DependencyEdges)
                {
                    _tables[edge.Child.DbObject.ID].AddDependencyEdge(edge);
                    _tables[edge.Parent.DbObject.ID].AddDependencyEdge(edge);
                }
                if (tablesLimit != null && !onlyTablesInLimit)
                {
                    IList<DependencyNode> tablesNotInLimit = new List<DependencyNode>(_tables.Values.Where(v => !tablesLimit.Contains(v.DbObject)));
                    ISet<DependencyNode> notProcessedTables = new HashSet<DependencyNode>(_tables.Values);
                    while (notProcessedTables.Count > 0)
                    {
                        DependencyNode start = notProcessedTables.First();
                        bool neighbourWithLimit = false;
                        ISet<DependencyNode> processing = new HashSet<DependencyNode>(start.Children);
                        IList<DependencyNode> processed = new List<DependencyNode>();
                        processing.UnionWith(start.Parents);
                        processing.Add(start);
                        while (processing.Count > 0)
                        {
                            DependencyNode node = processing.First();
                            processing.Remove(node);
                            if (!notProcessedTables.Contains(node))
                            {
                                continue;
                            }
                            notProcessedTables.Remove(node);
                            if (!tablesNotInLimit.Contains(node))
                            {
                                neighbourWithLimit = true;
                            }
                            else
                            {
                                processed.Add(node);
                            }
                            processing.UnionWith(node.Children);
                            processing.UnionWith(node.Parents);
                        }
                        if (neighbourWithLimit)
                        {
                            foreach (DependencyNode node in processed)
                            {
                                tablesLimit.Add(node.DbObject);
                            }
                        }
                    }
                    GenerateTableDependencyGraph(tablesLimit, true);
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                throw new TableDependencyException("Cannot generate table dependencies", exc);
            }
        }

        private void GetTopologicallySortedNodesVisitNode(DependencyNode node, DependencyNode previousNode, IList<DependencyNode> result, ISet<DependencyNode> processed, ISet<DependencyNode> processing, Func<DependencyNode, ISet<DependencyNode>> nodesSelectionFunc, Func<DependencyNode, ISet<DependencyEdge>> edgesSelectionFunc, Func<DependencyEdge, DependencyNode, DependencyNode, bool> checkEdge, ISet<DependencyEdge> cycleEdges)
        {
            if (processed.Contains(node))
            {
                return;
            }
            if (processing.Contains(node))
            {
                foreach (DependencyEdge edge in edgesSelectionFunc(node))
                {
                    if (checkEdge(edge, previousNode, node)) // looking for cycle edge (edge connecting current node and previous node)
                    {
                        cycleEdges.Add(edge); // adds one edge, which is par of cycle and can be used to break cycle
                    }
                }
                return;
            }
            processing.Add(node);
            foreach (DependencyNode selectedNode in nodesSelectionFunc(node))
            {
                GetTopologicallySortedNodesVisitNode(selectedNode, node, result, processed, processing, nodesSelectionFunc, edgesSelectionFunc, checkEdge, cycleEdges);
            }
            processing.Remove(node);
            processed.Add(node);
            result.Add(node);
        }

        public ISet<DependencyNode> GetAncestors(DatabaseObject databaseObject)
        {
            ISet<DependencyEdge> edges;
            ISet<DependencyNode> result;
            GetAncestorOrDescendantNodeEdge(databaseObject, false, out result, out edges);
            return result;
        }

        public ISet<DependencyNode> GetDescendants(DatabaseObject databaseObject)
        {
            return GetAncestorsOrDescendants(databaseObject, true);
        }

        public ISet<DependencyEdge> GetDescendantEdges(DatabaseObject databaseObject)
        {
            ISet<DependencyEdge> result;
            ISet<DependencyNode> nodes;
            GetAncestorOrDescendantNodeEdge(databaseObject, true, out nodes, out result);
            return result;
        }

        private ISet<DependencyNode> GetAncestorsOrDescendants(DatabaseObject databaseObject, bool descendants)
        {
            ISet<DependencyNode> result = new HashSet<DependencyNode>();
            ISet<DependencyNode> initValues = descendants ? _tables[databaseObject.ID].Children : _tables[databaseObject.ID].Parents;
            Queue<DependencyNode> toProcess = new Queue<DependencyNode>(initValues);
            DependencyNode startNode = _tables[databaseObject.ID];
            while (toProcess.Count > 0)
            {
                DependencyNode actualNode = toProcess.Dequeue();
                if (startNode.Equals(actualNode) || result.Contains(actualNode))
                {
                    continue;
                }
                ISet<DependencyNode> searchValues = descendants ? actualNode.Children : actualNode.Parents;
                foreach (DependencyNode node in searchValues)
                {
                    if (!startNode.Equals(node) && !result.Contains(node))
                    {
                        toProcess.Enqueue(node);
                    }
                }
                result.Add(actualNode);
            }
            return result;
        }

        private void GetAncestorOrDescendantNodeEdge(DatabaseObject databaseObject, bool descendants, out ISet<DependencyNode> resultNodes, out ISet<DependencyEdge> resultEdges)
        {
            resultNodes = new HashSet<DependencyNode>();
            resultEdges = new HashSet<DependencyEdge>();
            ISet<DependencyEdge> initValues = descendants ? _tables[databaseObject.ID].ChildEdges : _tables[databaseObject.ID].ParentEdges;
            Queue<DependencyEdge> toProcess = new Queue<DependencyEdge>(initValues);
            while (toProcess.Count > 0)
            {
                DependencyEdge actualEdge = toProcess.Dequeue();
                if (resultEdges.Contains(actualEdge))
                {
                    continue;
                }
                ISet<DependencyEdge> searchValues = descendants ? actualEdge.Child.ChildEdges : actualEdge.Parent.ParentEdges;
                foreach (DependencyEdge edge in searchValues)
                {
                    if (!resultEdges.Contains(edge))
                    {
                        toProcess.Enqueue(edge);
                    }
                }
                resultEdges.Add(actualEdge);
                resultNodes.Add(descendants ? actualEdge.Child : actualEdge.Parent);
            }
            resultNodes.Remove(_tables[databaseObject.ID]); // start node is not descendant or ancestor
        }

        public void PrintDependencies(DatabaseObject databaseObject)
        {
            if (databaseObject == null)
            {
                throw new ArgumentNullException(nameof(databaseObject));
            }
            Console.Out.WriteLine("Table {0}", databaseObject);
            string parentText;
            if (_tables[databaseObject.ID].Parents.Count > 0)
            {
                parentText = $"Parents: {string.Join(", ", _tables[databaseObject.ID].Parents)}";
            }
            else
            {
                parentText = "Parents: <none>";
            }
            Console.Out.WriteLine(parentText);
            string childrenText;
            if (_tables[databaseObject.ID].Children.Count > 0)
            {
                childrenText = $"Children: {string.Join(", ", _tables[databaseObject.ID].Children)}";
            }
            else
            {
                childrenText = "Children: <none>";
            }
            Console.Out.WriteLine(childrenText);
        }

        public void PrintAllDependencies()
        {
            foreach (DependencyNode node in _tables.Values)
            {
                foreach (DependencyNode child in node.Children)
                {
                    Console.Out.WriteLine("{0} -> {1}", node, child);
                }
            }
        }

        public string ToDotFormat()
        {
            StringBuilder export = new StringBuilder("digraph ");
            export.AppendLine($"\"{Database.Instance.DatabaseName}\" {{");
            foreach (DependencyEdge edge in DependencyEdges)
            {
                export.AppendLine($"    \"{edge.Parent.DbObject.NameWithSchema}\" -> \"{edge.Child.DbObject.NameWithSchema}\";");
            }
            foreach (DependencyNode node in NodesWithoutEdges)
            {
                export.AppendLine($"    \"{node.DbObject.NameWithSchema}\";");
            }
            export.Append("}");
            return export.ToString();
        }
    }
}

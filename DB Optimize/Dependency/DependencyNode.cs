using System.Collections.Generic;
using System.Linq;
using DB_Optimize.DB;

namespace DB_Optimize.Dependency
{
    public class DependencyNode
    {
        public DatabaseObject DbObject { get; }

        public ISet<DependencyNode> Parents
        {
            get { return new HashSet<DependencyNode>(ParentEdges.Select(e => e.Parent)); }
        }

        public ISet<DependencyNode> Children
        {
            get { return new HashSet<DependencyNode>(ChildEdges.Select(e => e.Child)); }
        }

        public ISet<DependencyEdge> ParentEdges { get; }

        public ISet<DependencyEdge> ChildEdges { get; }

        public DependencyNode(DatabaseObject dbObject)
        {
            DbObject = dbObject;
            ParentEdges = new HashSet<DependencyEdge>();
            ChildEdges = new HashSet<DependencyEdge>();
        }

        public void AddDependencyEdge(DependencyEdge edge)
        {
            if (Equals(edge.Parent.DbObject, DbObject))
            {
                ChildEdges.Add(edge);
            }
            if (Equals(edge.Child.DbObject, DbObject))
            {
                ParentEdges.Add(edge);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return ((DependencyNode) obj).DbObject.Equals(DbObject);
        }

        public override int GetHashCode()
        {
            return DbObject.ID;
        }

        public override string ToString()
        {
            return DbObject.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using ForeignKey = DB_Optimize.DB.ForeignKey;
using Database = DB_Optimize.DB.Database;
using DatabaseObject = DB_Optimize.DB.DatabaseObject;
using DependencyNode = DB_Optimize.Dependency.DependencyNode;

namespace DB_Optimize.Optimization
{
    public class Optimizer
    {
        private const int MAX_VALUES_IN_INSERT = 25;
        private const int RECOMMEND_MAX_COLUMN_SIZE_INDEX = 73;

        private readonly DatabaseOperations _databaseOperations = new DatabaseOperations(Database.Instance);

        public TableDependencyGraph DependencyGraph => new TableDependencyGraph();

        public static Optimizer Instance { get; } = new Optimizer();

        private Optimizer()
        {
        }

        public bool ArchiveTable(DatabaseObject tableToArchive, IDictionary<string, string> convertorTablenames, bool createTable, string whereConditions, bool useInsertIntoSelect, bool archiveAllEntries, StreamWriter streamToSave)
        {
            bool exportIsNotEmpty = false;
            ISet<DependencyEdge> cycleEdges;
            IList<DependencyNode> orderedTables = DependencyGraph.GetTopologicallySortedNodes(tableToArchive, true, out cycleEdges);
            if (createTable)
            {
                Server server = new Server(new ServerConnection(Database.Instance.Connection));
                Microsoft.SqlServer.Management.Smo.Database database = server.Databases[Database.Instance.DatabaseName];
                Transfer transfer = new Transfer(database)
                {
                    CopyAllObjects = false,
                    CopyAllUserDefinedDataTypes = true,
                    CopyAllSchemas = true,
                    CopyAllXmlSchemaCollections = true,
                    Options = new ScriptingOptions
                    {
                        FullTextCatalogs = true,
                        FullTextStopLists = true,
                        FullTextIndexes = true,
                        DriAll = true,
                        ExtendedProperties = true,
                        Indexes = true,
                        NonClusteredIndexes = true,
                        Triggers = true,
                        IncludeIfNotExists = true
                    }
                };
                foreach (DependencyNode table in orderedTables)
                {
                    transfer.ObjectList.Add(database.Tables[table.DbObject.Name, table.DbObject.Schema]);
                }
                StringBuilder archiveCreateTable = new StringBuilder();
                foreach (string line in transfer.ScriptTransfer())
                {
                    archiveCreateTable.AppendLine(line);
                    archiveCreateTable.AppendLine("GO");
                }
                foreach (DependencyNode table in orderedTables)
                {
                    archiveCreateTable.Replace(table.DbObject.NameWithSchemaBrackets, table.DbObject.NameWithSchema.Replace(table.DbObject.NameWithSchemaBrackets, $"[{convertorTablenames[table.DbObject.NameWithSchema.ToLower()].Replace(".", "].[")}]"));
                    archiveCreateTable.Replace($"TABLE {table.DbObject.NameWithSchema}", $"TABLE {convertorTablenames[table.DbObject.NameWithSchema.ToLower()]}");
                    archiveCreateTable.Replace($"ON {table.DbObject.NameWithSchema}", $"ON {convertorTablenames[table.DbObject.NameWithSchema.ToLower()]}");
                    string oldTablename = table.DbObject.Name;
                    string newTablename = convertorTablenames[table.DbObject.NameWithSchema.ToLower()];
                    newTablename = newTablename.Substring(Math.Min(newTablename.LastIndexOf(".") + 1, newTablename.Length - 1)).Replace("[", "").Replace("]", "");
                    archiveCreateTable.Replace($"_{oldTablename}_", $"_{newTablename}_");
                    archiveCreateTable.Replace($"name=N'{oldTablename}'", $"name=N'{newTablename}'");
                }
                streamToSave.Write(archiveCreateTable.ToString());
                exportIsNotEmpty = true;
                archiveCreateTable.Clear();
            }
            IDictionary<DependencyNode, string> tableSubSelects = new Dictionary<DependencyNode, string>(orderedTables.Count);
            if (!archiveAllEntries)
            {
                foreach (DependencyNode table in orderedTables.Reverse())
                {
                    if (table.DbObject.Equals(tableToArchive))
                    {
                        tableSubSelects.Add(table, $"FROM {table.DbObject.NameWithSchemaBrackets}{(whereConditions.Trim().Length > 0 ? $" WHERE {whereConditions}" : "")}");
                    }
                    foreach (DependencyEdge edge in table.ParentEdges)
                    {
                        DependencyNode parent = edge.Parent;
                        if (parent.DbObject.Equals(tableToArchive))
                        {
                            continue;
                        }
                        if (orderedTables.Contains(parent))
                        {
                            IList<string> parentColumnsConditions = new List<string>();
                            foreach (DependencyColumn column in edge.Columns)
                            {
                                parentColumnsConditions.Add($"{parent.DbObject.NameWithSchemaBrackets}.[{column.ParentColumn}] IN (SELECT DISTINCT {table.DbObject.NameWithSchemaBrackets}.[{column.ChildColumn}] {tableSubSelects[edge.Child]})");
                            }
                            if (!tableSubSelects.ContainsKey(parent))
                            {
                                tableSubSelects[parent] = $"FROM {parent.DbObject.NameWithSchemaBrackets} WHERE ({string.Join(" AND ", parentColumnsConditions)})";
                            }
                            else
                            {
                                tableSubSelects[parent] = tableSubSelects[parent] + $" OR ({string.Join(" AND ", parentColumnsConditions)})";
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DependencyNode table in orderedTables)
                {
                    if (table.DbObject.Equals(tableToArchive) && whereConditions.Trim().Length > 0)
                    {
                        tableSubSelects.Add(table, $"FROM {table.DbObject.NameWithSchemaBrackets} WHERE {whereConditions}");
                    }
                    else
                    {
                        tableSubSelects.Add(table, $"FROM {table.DbObject.NameWithSchemaBrackets}");
                    }
                }
            }
            IDictionary<DependencyNode, bool> valuesInTables = new Dictionary<DependencyNode, bool>(orderedTables.Count);
            bool valuesGenerated = false;
            foreach (DependencyNode table in orderedTables)
            {
                valuesInTables[table] = (Database.Instance.ExecuteScalarSelect($"SELECT COUNT(*) {tableSubSelects[table]}") ?? 0) > 0;
                if (valuesInTables[table])
                {
                    valuesGenerated = true;
                }
            }
            StringBuilder noCheckConstraints = new StringBuilder();
            StringBuilder witchCheckConstraints = new StringBuilder();
            foreach (DependencyEdge edge in cycleEdges)
            {
                noCheckConstraints.AppendLine($"ALTER TABLE {edge.Child.DbObject.NameWithSchemaBrackets} NOCHECK CONSTRAINT {edge.Name};");
                witchCheckConstraints.AppendLine($"ALTER TABLE {edge.Child.DbObject.NameWithSchemaBrackets} WITH CHECK CHECK CONSTRAINT {edge.Name};");
            }
            if (valuesGenerated || useInsertIntoSelect)
            {
                streamToSave.Write(noCheckConstraints.ToString());
            }
            foreach (DependencyNode table in orderedTables)
            {
                string tableName = convertorTablenames[table.DbObject.NameWithSchema.ToLower()];
                if (!tableName.Contains("[") && !tableName.Contains("]"))
                {
                    tableName = "[" + tableName.Replace(".", "].[") + "]";
                }
                IList<string> columnsList = _databaseOperations.GetColumnNames(table.DbObject, false);
                string columns = "[" + string.Join("], [", columnsList) + "]";
                if (!useInsertIntoSelect || table.DbObject.NameWithSchema == convertorTablenames[table.DbObject.NameWithSchema.ToLower()])
                {
                    if (!valuesInTables[table])
                    {
                        continue;
                    }
                    if (_databaseOperations.HasTableIdentityColumn(table.DbObject))
                    {
                        streamToSave.WriteLine($"SET IDENTITY_INSERT {tableName} ON;");
                    }
                    int values = 0;
                    foreach (string insert in new DatabaseOperations.ValueInsertGetter($"SELECT {columns} {tableSubSelects[table]}"))
                    {
                        if (values % MAX_VALUES_IN_INSERT == 0)
                        {
                            if (values > 0)
                            {
                                streamToSave.WriteLine(";");
                                streamToSave.WriteLine("GO");
                            }
                            streamToSave.Write($"INSERT INTO {tableName} ({columns}) VALUES ");
                        }
                        else
                        {
                            streamToSave.Write(", ");
                        }
                        streamToSave.Write(insert);
                        values++;
                    }
                    if (values > 0)
                    {
                        streamToSave.WriteLine(";");
                        streamToSave.WriteLine("GO");
                    }
                    if (_databaseOperations.HasTableIdentityColumn(table.DbObject))
                    {
                        streamToSave.WriteLine($"SET IDENTITY_INSERT {tableName} OFF;");
                    }
                }
                else
                {
                    exportIsNotEmpty = true;
                    if (_databaseOperations.HasTableIdentityColumn(table.DbObject))
                    {
                        streamToSave.WriteLine($"SET IDENTITY_INSERT {tableName} ON;");
                    }
                    streamToSave.WriteLine($"INSERT INTO {tableName} ({columns}) SELECT * {tableSubSelects[table]};");
                }
            }
            if (valuesGenerated || useInsertIntoSelect)
            {
                exportIsNotEmpty = true;
                streamToSave.Write(witchCheckConstraints.ToString());
            }
            streamToSave.Flush();
            return exportIsNotEmpty;
        }

        public string DeleteEntriesFromTable(DatabaseObject tableWhereDelete, string whereConditions, IDictionary<DB.ForeignKey, DB.ForeignKey.DeleteActions> foreignKeyActions)
        {
            StringBuilder delete = new StringBuilder();
            DependencyNode startTable = DependencyGraph[tableWhereDelete];
            IList<DependencyNode> orderedTables = new List<DependencyNode>();
            IDictionary<DependencyNode, IList<DependencyEdge>> childEdges = new Dictionary<DependencyNode, IList<DependencyEdge>>();
            childEdges[startTable] = new List<DependencyEdge>();
            foreach (DependencyNode table in DependencyGraph.GetDescendants(tableWhereDelete))
            {
                childEdges[table] = new List<DependencyEdge>();
            }
            ISet<DependencyNode> processing = new HashSet<DependencyNode>();
            ISet<DependencyNode> processed = new HashSet<DependencyNode>();
            ProccessDeleteSubhierarchy(startTable, null, orderedTables, childEdges, foreignKeyActions, processing, processed);
            IList<string> queries = new List<string>();
            IDictionary<DependencyNode, string> tableSubSelects = new Dictionary<DependencyNode, string>(orderedTables.Count);
            foreach (DependencyNode table in orderedTables.Reverse())
            {
                if (table.DbObject.Equals(tableWhereDelete))
                {
                    tableSubSelects.Add(table, $"{(whereConditions.Trim().Length > 0 ? $" WHERE {whereConditions}" : "")}");
                    queries.Add($"DELETE FROM {tableWhereDelete.NameWithSchemaBrackets} {tableSubSelects[table]}");
                }
                foreach (DependencyEdge edge in childEdges[table])
                {
                    DependencyNode child = edge.Child;
                    IList<string> childColumnsConditions = new List<string>();
                    foreach (DependencyColumn column in edge.Columns)
                    {
                        childColumnsConditions.Add($"{child.DbObject.NameWithSchemaBrackets}.[{column.ChildColumn}] IN (SELECT DISTINCT {table.DbObject.NameWithSchemaBrackets}.[{column.ParentColumn}] FROM {edge.Parent.DbObject.NameWithSchemaBrackets} {tableSubSelects[edge.Parent]})");
                    }
                    if (!tableSubSelects.ContainsKey(child))
                    {
                        tableSubSelects[child] = $"WHERE ({string.Join(" OR ", childColumnsConditions)})";
                    }
                    else if(!child.DbObject.Equals(tableWhereDelete))
                    {
                        tableSubSelects[child] = tableSubSelects[child] + $" OR ({string.Join(" OR ", childColumnsConditions)})";
                    }
                    if (foreignKeyActions[edge.ForeignKey] == DB.ForeignKey.DeleteActions.Cascade)
                    {
                        queries.Add($"DELETE FROM {child.DbObject.NameWithSchemaBrackets} {tableSubSelects[child]}");
                    }
                    else if (foreignKeyActions[edge.ForeignKey] == DB.ForeignKey.DeleteActions.SetNull)
                    {
                        IList<string> setColumns = new List<string>();
                        foreach (DependencyColumn column in edge.Columns)
                        {
                            setColumns.Add($"{child.DbObject.NameWithSchemaBrackets}.[{column.ChildColumn}] = NULL");
                        }
                        queries.Add($"UPDATE {child.DbObject.NameWithSchemaBrackets} SET {string.Join(", ", setColumns)} {tableSubSelects[child]}");
                    }
                    else if (foreignKeyActions[edge.ForeignKey] == DB.ForeignKey.DeleteActions.SetDefault)
                    {
                        IList<string> setColumns = new List<string>();
                        foreach (DependencyColumn column in edge.Columns)
                        {
                            string defaultValue = column.ChildColumn.DefaultValue;
                            if (defaultValue == null)
                            {
                                defaultValue = "NULL";
                            }
                            setColumns.Add($"{child.DbObject.NameWithSchemaBrackets}.[{column.ChildColumn}] = {defaultValue}");
                        }
                        queries.Add($"UPDATE {child.DbObject.NameWithSchemaBrackets} SET {string.Join(", ", setColumns)} {tableSubSelects[child]}");
                    }
                }
            }
            delete.AppendLine("BEGIN TRY").AppendLine("BEGIN TRANSACTION");
            foreach (string query in queries.Reverse())
            {
                delete.Append(query).AppendLine(";");
            }
            delete.AppendLine("COMMIT").AppendLine("END TRY").AppendLine("BEGIN CATCH");
            delete.AppendLine("IF @@TRANCOUNT > 0").AppendLine("ROLLBACK TRAN");
            delete.AppendLine("DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()");
            delete.AppendLine("DECLARE @ErrorSeverity INT = ERROR_SEVERITY()");
            delete.AppendLine("DECLARE @ErrorState INT = ERROR_STATE()");
            delete.AppendLine("RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState); ");
            delete.AppendLine("END CATCH");
            return delete.ToString();
        }

        public IList<Tuple<DependencyEdge, string>> CheckDeleteCascade(DependencyNode table, out bool OK, out string explanation)
        {
            IList<Tuple<DependencyEdge, string>> result = GetOrderedEdges(table);
            OK = true;
            explanation = "";
            foreach (Tuple<DependencyEdge, string> edge in result)
            {
                if (edge.Item1.ForeignKey.DeleteAction == ForeignKey.DeleteActions.NoAction)
                {
                    OK = false;
                    explanation = $"foreign key {edge.Item1.ForeignKey} has on delete No action and blocks delete procedure";
                    break;
                }
                else if (explanation.Length == 0 && edge.Item1.ForeignKey.DeleteAction != ForeignKey.DeleteActions.Cascade)
                {
                    explanation = $"foreign key {edge.Item1.ForeignKey} has on delete {ForeignKey.DeleteActionToString(edge.Item1.ForeignKey.DeleteAction)}";
                }
            }
            return result;
        }

        public IList<IList<string>> EvaluateColumnDataType(TableColumn column, DatabaseObject table)
        {
            IList<IList<string>> result = new List<IList<string>>();
            result.Add(new List<string>()
                {
                    "Datatype:",
                    column.TypeWithLength
                });
            if (column.IsUserType)
            {
                result.Add(new List<string>()
                {
                    "Suggestion:",
                    "It is a user defined type so optimal data type is not resolved"
                });
            }
            else if (column.DataTypeIsChar() || column.DataTypeIsBinary())
            {
                Tuple<long, long> minMax = _databaseOperations.GetMinMaxColumnValues(column, table);
                result.Add(new List<string>
                {
                    "Max length in values:",
                    minMax.Item2.ToString()
                });
                if (minMax.Item2 < column.Length)
                {
                    result.Add(new List<string>
                    {
                        "Max length which can be stored:",
                        column.Length.ToString(),
                    });
                    result.Add(new List<string>
                    {
                        "Suggestion:",
                        $"Bytes can be lowered from {column.Length} to {Math.Max(minMax.Item2, DataTypeConstants.MIN_LENGTH_IN_BYTES)} => {column.Type}({Math.Max(minMax.Item2, DataTypeConstants.MIN_LENGTH_IN_BYTES)})"
                    });
                }
                else if (column.Length == -1 && minMax.Item2 <= column.MaxLength)
                {
                    result.Add(new List<string>
                    {
                        "Suggestion:",
                        $"Bytes can be lowered from MAX to {minMax.Item2} => {column.Type}({minMax.Item2})"
                    });
                }
                else
                {
                    result.Add(new List<string>
                    {
                        "No suggestion"
                    });
                }
            }
            else if (column.DataTypeIsInteger())
            {
                Tuple<long, long> minMax = _databaseOperations.GetMinMaxColumnValues(column, table);
                Tuple<long, long> minMaxDataType;
                switch (column.Type)
                {
                    case DataTypeConstants.TINYINT:
                        minMaxDataType = new Tuple<long, long>(DataTypeConstants.TINYINT_MIN_VALUE, DataTypeConstants.TINYINT_MAX_VALUE);
                        break;
                    case DataTypeConstants.SMALLINT:
                        minMaxDataType = new Tuple<long, long>(DataTypeConstants.SMALLINT_MIN_VALUE, DataTypeConstants.INT_MAX_VALUE);
                        break;
                    case DataTypeConstants.INT:
                        minMaxDataType = new Tuple<long, long>(DataTypeConstants.INT_MIN_VALUE, DataTypeConstants.INT_MAX_VALUE);
                        break;
                    case DataTypeConstants.BIGINT:
                        minMaxDataType = new Tuple<long, long>(DataTypeConstants.BIGINT_MIN_VALUE, DataTypeConstants.BIGINT_MAX_VALUE);
                        break;
                    default:
                        result.Add(new List<string>{"Unknown integer value"});
                        return result;
                }
                result.Add(new List<string>
                {
                    "Min value in values:",
                    minMax.Item1.ToString()
                });
                result.Add(new List<string>
                {
                    "Max value in values:",
                    minMax.Item2.ToString()
                });
                if (minMax.Item1 >= DataTypeConstants.TINYINT_MIN_VALUE && minMax.Item2 <= DataTypeConstants.TINYINT_MAX_VALUE)
                {
                    result.Add(GenerateSuggestionForIntDataType(column, minMaxDataType, DataTypeConstants.TINYINT, DataTypeConstants.TINYINT_MIN_VALUE, DataTypeConstants.TINYINT_MAX_VALUE));
                }
                else if (minMax.Item1 >= DataTypeConstants.SMALLINT_MIN_VALUE && minMax.Item2 <= DataTypeConstants.SMALLINT_MAX_VALUE)
                {
                    result.Add(GenerateSuggestionForIntDataType(column, minMaxDataType, DataTypeConstants.SMALLINT, DataTypeConstants.SMALLINT_MIN_VALUE, DataTypeConstants.SMALLINT_MAX_VALUE));
                }
                else if (minMax.Item1 >= DataTypeConstants.INT_MIN_VALUE && minMax.Item2 <= DataTypeConstants.INT_MAX_VALUE)
                {
                    result.Add(GenerateSuggestionForIntDataType(column, minMaxDataType, DataTypeConstants.INT, DataTypeConstants.INT_MIN_VALUE, DataTypeConstants.INT_MAX_VALUE));
                }
                else // It must be in range of bigint, no need to check it and no suggestion (cannot be used smaller data type)
                {
                    result.Add(new List<string>
                    {
                        "No suggestion"
                    });
                }
            }
            else
            {
                result.Add(new List<string>
                {
                    "No suggestion for this data type"
                });
            }
            return result;
        }

        public IList<IList<string>> EvaluateForeignKeyDataType(ForeignKey key, int keySize)
        {
            IList<IList<string>> result = new List<IList<string>>();
            if (keySize > RECOMMEND_MAX_COLUMN_SIZE_INDEX && key.Columns.All(c => c.Type != DataTypeConstants.HIERARCHYID)) // hierarchyid can be ignored, it can be very large, but it is his property
            {
                string columnSingular = "column";
                string columnPlural = "columns";
                string considerLowerSize = "";
                string considerReplaceCharWithNumeric = "";
                IList<string> columns;
                if (key.Columns.Any(c => c.DataTypeIsChar() || c.DataTypeIsBinary()))
                {
                    columns = key.Columns.Where(c => c.DataTypeIsChar() || c.DataTypeIsBinary()).Select(c => $"{c.Name} ({c.TypeWithLength})").ToList();
                    considerLowerSize = $" Consider lowering length of {(columns.Count() == 1 ? columnSingular : columnPlural)} {string.Join(", ", columns)}.";
                    if (key.Columns.Any(c => c.DataTypeIsChar()))
                    {
                        considerReplaceCharWithNumeric = $" Consider replacement of string columns ({string.Join(", ", key.Columns.Where(c => c.DataTypeIsChar()).Select(c => c.Name))}) with numeric ID.";
                    }
                }
                columns = key.Columns.Select(c => c.TypeWithLength).ToList();
                result.Add(new List<string>
                    {
                        "Suggestion:",
                        $"Foreign key is consisted of {(columns.Count == 1 ? columnSingular : columnPlural)} {string.Join(", ", key.Columns.Select(c => c.TypeWithLength))} and {(columns.Count == 1 ? "its" : "their")} size exceeds {RECOMMEND_MAX_COLUMN_SIZE_INDEX} bytes.{considerLowerSize}{considerReplaceCharWithNumeric}"
                    });
            }
            else
            {
                result.Add(new List<string>
                    {
                        "No suggestion, data type is OK for indexing"
                    });
            }
            return result;
        }

        public void ChangeCollation(string collation)
        {
            string filename;
            FileStream fileStream = GetTempFileStreamWriter(out filename);
            fileStream.Close();
            Server server = new Server(new ServerConnection(Database.Instance.Connection));
            Microsoft.SqlServer.Management.Smo.Database smoDatabase = server.Databases[Database.Instance.DatabaseName];
            ScriptingOptions scriptingOptionDBStructure = new ScriptingOptions
            {
                ToFileOnly = true,
                NoCollation = true,
                DriAll = true,
                ExtendedProperties = true,
                Indexes = true,
                Triggers = true,
                AppendToFile = true,
                FileName = filename
            };
            ScriptingOptions scriptingOptionData = new ScriptingOptions
            {
                ToFileOnly = true,
                NoCollation = true,
                ScriptData = true,
                ScriptSchema = false,
                AppendToFile = true,
                FileName = filename
            };
            Transfer transfer = new Transfer(smoDatabase)
            {
                CopyAllObjects = false,
                CopyAllTables = true,
                CopyAllViews = true,
                CopyAllUserDefinedFunctions = true,
                Options = scriptingOptionDBStructure
            };
            transfer.ScriptTransfer();
            StringBuilder noCheckConstraints = new StringBuilder();
            StringBuilder witchCheckConstraints = new StringBuilder();
            foreach (DatabaseObject constraint in _databaseOperations.GetActiveForeignKeys())
            {
                noCheckConstraints.AppendLine($"ALTER TABLE {constraint.Schema} NOCHECK CONSTRAINT {constraint.Name};");
                witchCheckConstraints.AppendLine($"ALTER TABLE {constraint.Schema} WITH CHECK CHECK CONSTRAINT {constraint.Name};");
            }
            using (StreamWriter writer = new StreamWriter(filename, true, Encoding.Unicode))
            {
                writer.Write(noCheckConstraints.ToString());
                writer.WriteLine("GO");
            }
            Scripter scripter = new Scripter(server);
            scripter.Options = scriptingOptionData;
            foreach (Table table in smoDatabase.Tables)
            {
                scripter.EnumScript(new[] { table.Urn });
            }
            using (StreamWriter writer = new StreamWriter(filename, true, Encoding.Unicode))
            {
                writer.Write(witchCheckConstraints.ToString());
            }
            Database database = Database.Instance;
            foreach (DatabaseObject index in _databaseOperations.GetIndexes())
            {
                database.ExecuteNonResultQuery($"DROP INDEX [{index.Name}] ON {index.Schema};");
            }
            foreach (DatabaseObject constraint in _databaseOperations.GetConstraints())
            {
                database.ExecuteNonResultQuery($"ALTER TABLE {constraint.Schema} DROP CONSTRAINT [{constraint.Name}];");
            }
            foreach (DatabaseObject view in _databaseOperations.GetViews())
            {
                database.ExecuteNonResultQuery($"DROP VIEW {view.NameWithSchemaBrackets};");
            }
            foreach (DatabaseObject table in _databaseOperations.GetTables())
            {
                database.ExecuteNonResultQuery($"DROP TABLE {table.NameWithSchemaBrackets};");
            }
            foreach (DatabaseObject function in _databaseOperations.GetFunctions())
            {
                database.ExecuteNonResultQuery($"DROP FUNCTION {function.NameWithSchemaBrackets};");
            }
            database.ExecuteNonResultQuery($"ALTER DATABASE {database.DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
            database.ExecuteNonResultQuery($"ALTER DATABASE {database.DatabaseName} COLLATE {collation};");
            database.ExecuteNonResultQuery($"ALTER DATABASE {database.DatabaseName} SET MULTI_USER;");
            using (StreamReader file = File.OpenText(filename))
            {
                string line;
                StringBuilder buffer = new StringBuilder();
                while ((line = file.ReadLine()) != null)
                {
                    if (line == "GO")
                    {
                        database.ExecuteNonResultQuery(buffer.ToString());
                        buffer.Clear();
                    }
                    else
                    {
                        buffer.AppendLine(line);
                    }
                }
                if (buffer.Length > 0)
                {
                    database.ExecuteNonResultQuery(buffer.ToString());
                    buffer.Clear();
                }
            }
            File.Delete(filename);
        }

        public static string CreateIndexesForeignKey(IList<ForeignKeyMissingIndexes> keys, bool full)
        {
            StringBuilder createIndexesQueryStringBuilder = new StringBuilder();
            foreach (ForeignKeyMissingIndexes foreignKey in keys)
            {
                IList<TableColumn> columnNames = full ? foreignKey.Columns : foreignKey.MissingIndexColumns;
                createIndexesQueryStringBuilder.AppendLine($"CREATE INDEX IX_{foreignKey.Table.Name}_{string.Join("_", columnNames)} ON {foreignKey.Table} ({string.Join(",", columnNames)});");
            }
            return createIndexesQueryStringBuilder.ToString();
        }

        public static FileStream GetTempFileStreamWriter(out string filename)
        {
            int attempts = 0;
            while (true)
            {
                filename = Path.GetRandomFileName();
                filename = Path.ChangeExtension(filename, "sql");
                filename = Path.Combine(Path.GetTempPath(), filename);
                try
                {
                    return new FileStream(filename, FileMode.CreateNew);
                }
                catch (IOException exc)
                {
                    attempts++;
                    if (attempts >= 10)
                    {
                        Debug.WriteLine(exc);
                        throw new IOException("Cannot create temp file to store data.", exc);
                    }
                }
            }
        }

        private IList<string> GenerateSuggestionForIntDataType(TableColumn column, Tuple<long, long> minMaxOriginalDataType, string suggestedDataType, long suggestedDataTypeMinValue, long suggestedDataTypeMaxValue)
        {
            if (column.Type != DataTypeConstants.TINYINT)
            {
                return new List<string>
                        {
                            "Suggestion:",
                            $"Because data type {column.TypeWithLength} has range from {minMaxOriginalDataType.Item1} to {minMaxOriginalDataType.Item2}, data type of column {column.Name} can be changed to {suggestedDataType} (value range is from {suggestedDataTypeMinValue} to {suggestedDataTypeMaxValue})"
                        };
            }
            else
            {
                return new List<string>
                    {
                        "No suggestion"
                    };
            }
        }

        private void ProccessDeleteSubhierarchy(DependencyNode node, DependencyEdge edge, IList<DependencyNode> orderedTables, IDictionary<DependencyNode, IList<DependencyEdge>> childEdges, IDictionary<DB.ForeignKey, DB.ForeignKey.DeleteActions> foreignKeyActions, ISet<DependencyNode> processing, ISet<DependencyNode> processed)
        {
            if (processing.Contains(node) && foreignKeyActions[edge.ForeignKey] == DB.ForeignKey.DeleteActions.Cascade)
            {
                throw new DeleteDependencyException($"In delete tree is a cycle of cascade delete (for example between {edge.Parent} and {edge.Child} - foreign key {edge.ForeignKey}), so it is not possible to execute deletion procedure.");
            }
            processing.Add(node);
            foreach (DependencyEdge childEdge in node.ChildEdges)
            {
                if (foreignKeyActions[childEdge.ForeignKey] == DB.ForeignKey.DeleteActions.NoAction)
                {
                    throw new DeleteDependencyException($"Foreign key {childEdge.ForeignKey} has No action on delete and it blocks deletion procedure.");
                }
                else if (foreignKeyActions[childEdge.ForeignKey] == DB.ForeignKey.DeleteActions.Cascade)
                {
                    ProccessDeleteSubhierarchy(childEdge.Child, childEdge, orderedTables, childEdges, foreignKeyActions, processing, processed);
                }
                childEdges[node].Add(childEdge);
            }
            processing.Remove(node);
            processed.Add(node);
            orderedTables.Add(node);
        }

        private IList<Tuple<DependencyEdge, string>> GetOrderedEdges(DependencyNode table, string levelPrefix = "")
        {
            int level = 1;
            List<Tuple<DependencyEdge, string>> result = new List<Tuple<DependencyEdge, string>>();
            foreach (DependencyEdge edge in table.ChildEdges)
            {
                result.Add(new Tuple<DependencyEdge, string>(edge, $"{levelPrefix}{level}"));
                if (edge.ForeignKey.DeleteAction == ForeignKey.DeleteActions.Cascade)
                {
                    result.AddRange(GetOrderedEdges(edge.Child, $"{levelPrefix}{level}."));
                }
                level++;
            }
            return result;
        }
    }
}

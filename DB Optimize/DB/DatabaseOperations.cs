using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;

namespace DB_Optimize.DB
{
    public class DatabaseOperations
    {
        private readonly Database _database;

        public DatabaseOperations(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }
            _database = database;
        }

        public IList<DatabaseObject> GetTables()
        {
            string tablesCondition = "";
            IList<DatabaseObject> tablesList = new List<DatabaseObject>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT sys.tables.object_id, sys.tables.name AS tablename, SCHEMA_NAME(schema_id) AS schema_name FROM sys.tables WHERE sys.tables.name != 'sysdiagrams' {tablesCondition} ORDER BY OBJECT_SCHEMA_NAME(sys.tables.object_id), sys.tables.name;"))
                {
                    int columnIdOrdinal = resultReader.GetOrdinal("object_id");
                    int columnNameOrdinal = resultReader.GetOrdinal("tablename");
                    int columnSchemaNameOrdinal = resultReader.GetOrdinal("schema_name");
                    while (resultReader.Read())
                    {
                        tablesList.Add(new DatabaseObject(resultReader.GetInt32(columnIdOrdinal), resultReader.GetString(columnSchemaNameOrdinal), resultReader.GetString(columnNameOrdinal), DatabaseObject.DatabaseObjectType.Table));
                    }
                    return tablesList;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("List of tables cannot be loaded", exc);
            }
        }

        public IList<DatabaseObject> GetViews()
        {
            IList<DatabaseObject> result = new List<DatabaseObject>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect("SELECT sys.objects.object_id AS id, SCHEMA_NAME(sys.objects.schema_id) AS schemaName, sys.objects.name AS viewName FROM sys.objects WHERE sys.objects.type = 'V';"))
                {
                    int columnIDOrdinal = resultReader.GetOrdinal("id");
                    int columnSchemaNameOrdinal = resultReader.GetOrdinal("schemaName");
                    int columnViewNameOrdinal = resultReader.GetOrdinal("viewName");
                    while (resultReader.Read())
                    {
                        DatabaseObject view = new DatabaseObject(resultReader.GetInt32(columnIDOrdinal), resultReader.GetString(columnSchemaNameOrdinal), resultReader.GetString(columnViewNameOrdinal), DatabaseObject.DatabaseObjectType.View);
                        result.Add(view);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Views cannot be loaded", exc);
            }
        }

        public IList<DatabaseObject> GetFunctions()
        {
            IList<DatabaseObject> result = new List<DatabaseObject>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect("SELECT sys.objects.object_id AS id, SCHEMA_NAME(sys.objects.schema_id) AS schemaName, sys.objects.name AS functionName FROM sys.objects WHERE sys.objects.type IN ('IF','TF','FN');"))
                {
                    int columnIDOrdinal = resultReader.GetOrdinal("id");
                    int columnSchemaNameOrdinal = resultReader.GetOrdinal("schemaName");
                    int columnFunctionNameOrdinal = resultReader.GetOrdinal("functionName");
                    while (resultReader.Read())
                    {
                        DatabaseObject view = new DatabaseObject(resultReader.GetInt32(columnIDOrdinal), resultReader.GetString(columnSchemaNameOrdinal), resultReader.GetString(columnFunctionNameOrdinal), DatabaseObject.DatabaseObjectType.Function);
                        result.Add(view);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Functions cannot be loaded", exc);
            }
        }

        public IList<DatabaseObject> GetConstraints()
        {
            IList<DatabaseObject> result = new List<DatabaseObject>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect("SELECT sys.objects.object_id AS id, OBJECT_NAME(sys.objects.parent_object_id) AS tablename, SCHEMA_NAME(sys.objects.schema_id) AS schemaName, sys.objects.name AS constraintName FROM sys.objects WHERE sys.objects.type IN ('C','D','F','PK','EC','UQ') ORDER BY constraintName;"))
                {
                    int columnIDOrdinal = resultReader.GetOrdinal("id");
                    int columnSchemaNameOrdinal = resultReader.GetOrdinal("schemaName");
                    int columnTablenameOrdinal = resultReader.GetOrdinal("tablename");
                    int columnConstraintNameOrdinal = resultReader.GetOrdinal("constraintName");
                    while (resultReader.Read())
                    {
                        DatabaseObject view = new DatabaseObject(resultReader.GetInt32(columnIDOrdinal), $"[{resultReader.GetString(columnSchemaNameOrdinal)}].[{resultReader.GetString(columnTablenameOrdinal)}]", resultReader.GetString(columnConstraintNameOrdinal), DatabaseObject.DatabaseObjectType.Constraint);
                        result.Add(view);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Constraints cannot be loaded", exc);
            }
        }

        public IList<DatabaseObject> GetActiveForeignKeys()
        {
            IList<DatabaseObject> result = new List<DatabaseObject>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect("SELECT sys.foreign_keys.object_id AS fkID, sys.foreign_keys.name AS fkName, OBJECT_NAME(sys.foreign_keys.parent_object_id) AS tablename, OBJECT_SCHEMA_NAME(sys.foreign_keys.parent_object_id) AS schemaName FROM sys.foreign_keys WHERE sys.foreign_keys.is_disabled=0;"))
                {
                    int columnFKIDOrdinal = resultReader.GetOrdinal("fkID");
                    int columnSchemaNameOrdinal = resultReader.GetOrdinal("schemaName");
                    int columnTablenameOrdinal = resultReader.GetOrdinal("tablename");
                    int columnFKNameOrdinal = resultReader.GetOrdinal("fkName");
                    while (resultReader.Read())
                    {
                        DatabaseObject view = new DatabaseObject(resultReader.GetInt32(columnFKIDOrdinal), $"[{resultReader.GetString(columnSchemaNameOrdinal)}].[{resultReader.GetString(columnTablenameOrdinal)}]", resultReader.GetString(columnFKNameOrdinal), DatabaseObject.DatabaseObjectType.Constraint);
                        result.Add(view);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Foreign keys cannot be loaded", exc);
            }
        }

        public IList<DatabaseObject> GetIndexes()
        {
            IList<DatabaseObject> result = new List<DatabaseObject>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect("SELECT *, sys.indexes.object_id AS id, sys.indexes.name AS indexName, OBJECT_NAME(sys.indexes.object_id) AS tablename, OBJECT_SCHEMA_NAME(sys.indexes.object_id) AS schemaName FROM sys.indexes LEFT JOIN sys.xml_indexes ON sys.xml_indexes.object_id=sys.indexes.object_id AND sys.xml_indexes.index_id=sys.indexes.index_id WHERE OBJECT_SCHEMA_NAME(sys.indexes.object_id)!='sys' AND sys.indexes.is_primary_key=0 AND sys.indexes.is_unique_constraint=0 AND sys.indexes.name IS NOT NULL AND sys.xml_indexes.using_xml_index_id IS NULL;"))
                {
                    int columnIDOrdinal = resultReader.GetOrdinal("id");
                    int columnSchemaNameOrdinal = resultReader.GetOrdinal("schemaName");
                    int columnTablenameOrdinal = resultReader.GetOrdinal("tablename");
                    int columnIndexNameOrdinal = resultReader.GetOrdinal("indexName");
                    while (resultReader.Read())
                    {
                        DatabaseObject view = new DatabaseObject(resultReader.GetInt32(columnIDOrdinal), $"[{resultReader.GetString(columnSchemaNameOrdinal)}].[{resultReader.GetString(columnTablenameOrdinal)}]", resultReader.GetString(columnIndexNameOrdinal), DatabaseObject.DatabaseObjectType.Index);
                        result.Add(view);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Constraints cannot be loaded", exc);
            }
        }

        public IList<string> GetColumnNames(DatabaseObject table, bool includeComputed = true)
        {
            IList<string> result = new List<string>();
            try
            {
                string whereCondition = "";
                if (!includeComputed)
                {
                    whereCondition = " AND sys.columns.is_computed=0";
                }
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT name FROM sys.columns WHERE object_id={table.ID} {whereCondition} ORDER BY column_id;"))
                {
                    int columnNameOrdinal = resultReader.GetOrdinal("name");
                    while (resultReader.Read())
                    {
                        result.Add(resultReader.GetString(columnNameOrdinal));
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Table columns cannot be loaded", exc);
            }
        }

        public Tuple<long, long> GetMinMaxColumnValues(TableColumn column, DatabaseObject table)
        {
            try
            {
                string function;
                if (column.DataTypeIsChar() || column.DataTypeIsBinary())
                {
                    function = "DATALENGTH";
                }
                else if (column.DataTypeIsInteger())
                {
                    function = "";
                }
                else
                {
                    throw new InvalidOperationException($"Data type {column.Type} is not supported for getting min and max values");
                }
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT MIN({function}({table.NameWithSchemaBrackets}.[{column.Name}])) AS minValue, MAX({function}({table.NameWithSchemaBrackets}.[{column.Name}])) AS maxValue FROM {table.NameWithSchemaBrackets};"))
                {
                    int columnMinOrdinal = resultReader.GetOrdinal("minValue");
                    int columnMaxOrdinal = resultReader.GetOrdinal("maxValue");
                    long min = -1;
                    long max = -1;
                    while (resultReader.Read())
                    {
                        if (!resultReader.IsDBNull(columnMinOrdinal))
                        {
                            min = GetIntValue(resultReader, columnMinOrdinal);
                        }
                        if (!resultReader.IsDBNull(columnMaxOrdinal))
                        {
                            max = GetIntValue(resultReader, columnMaxOrdinal);
                        }
                        if (column.IsUnicode)
                        {
                            min /= 2;
                            max /= 2;
                        }
                    }
                    return new Tuple<long, long>(min, max);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException($"Cannot load min and max values in colum {column.Name}", exc);
            }
        }

        public IList<TableColumn> GetTableColumns(DatabaseObject table)
        {
            IList<TableColumn> result = new List<TableColumn>();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT sys.columns.name AS columnName, sys.types.name AS typeName, sys.columns.max_length AS maxLength, sys.types.is_user_defined AS isUserType, sys.foreign_key_columns.constraint_object_id AS FK_ID, sys.columns.is_nullable AS isNullable, OBJECT_DEFINITION(sys.columns.default_object_id) AS defaultValue FROM sys.columns LEFT JOIN sys.types ON sys.types.user_type_id = sys.columns.user_type_id LEFT JOIN sys.foreign_key_columns ON sys.foreign_key_columns.parent_object_id = sys.columns.object_id AND sys.foreign_key_columns.parent_column_id = sys.columns.column_id WHERE sys.columns.object_id={table.ID} ORDER BY sys.columns.column_id;"))
                {
                    int columnNameOrdinal = resultReader.GetOrdinal("columnName");
                    int columnDatatypeNameOrdinal = resultReader.GetOrdinal("typeName");
                    int columnIsUserTypeOrdinal = resultReader.GetOrdinal("isUserType");
                    int columnFKIDOrdinal = resultReader.GetOrdinal("FK_ID");
                    int columnIsNullableOrdinal = resultReader.GetOrdinal("isNullable");
                    int columnDefaultValueOrdinal = resultReader.GetOrdinal("defaultValue");
                    int columnMaxLengthOrdinal = resultReader.GetOrdinal("maxLength");
                    while (resultReader.Read())
                    {
                        bool isForeignKey = !resultReader.IsDBNull(columnFKIDOrdinal);
                        string defaultValue = null;
                        if (!resultReader.IsDBNull(columnDefaultValueOrdinal))
                        {
                            defaultValue = resultReader.GetString(columnDefaultValueOrdinal);
                        }
                        TableColumn column = new TableColumn(resultReader.GetString(columnNameOrdinal), defaultValue, resultReader.GetBoolean(columnIsNullableOrdinal), resultReader.GetString(columnDatatypeNameOrdinal), resultReader.GetInt16(columnMaxLengthOrdinal),resultReader.GetBoolean(columnIsUserTypeOrdinal), isForeignKey);
                        result.Add(column);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Table columns cannot be loaded", exc);
            }
        }

        public DataTable ValuesNotUsedInForeignKeys(DependencyEdge edge, int limit = -1)
        {
            string top = "";
            if (limit > 0)
            {
                top = $"TOP {limit}";
            }
            DataTable result = new DataTable(edge.Name);
            try
            {
                IList<string> columns = GetColumnNames(edge.Parent.DbObject);
                foreach (string column in columns)
                {
                    result.Columns.Add(column);
                }
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT {top} * {GetSqlQueryValuesNotUsedInForeignKeys(edge)}"))
                {
                    while (resultReader.Read())
                    {
                        DataRow row = result.NewRow();
                        for (int i = 0; i < resultReader.FieldCount; i++)
                        {
                            row[columns[i]] = GetStringRepresentationOfSqlValue(resultReader, i, true);
                        }
                        result.Rows.Add(row);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Values cannot be loaded", exc);
            }
        }

        public IDictionary<ForeignKey, int> GetForeignKeySize(IList<ForeignKey> foreignKeys)
        {
            IDictionary<ForeignKey, int> result = new Dictionary<ForeignKey, int>(foreignKeys.Count);
            string whereCondition = "";
            if (foreignKeys != null)
            {
                if (foreignKeys.Count == 0)
                {
                    return null;
                }
                whereCondition = $"WHERE sys.foreign_key_columns.constraint_object_id IN ({string.Join(",", foreignKeys.Select(fk => fk.ID))})";
            }
            IDictionary<int, ForeignKey> foreignKeyIndices = new Dictionary<int, ForeignKey>(foreignKeys.Count);
            foreach (ForeignKey foreignKey in foreignKeys)
            {
                result.Add(foreignKey, 0);
                foreignKeyIndices.Add(foreignKey.ID, foreignKey);
            }
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT COL_LENGTH(OBJECT_SCHEMA_NAME(sys.foreign_key_columns.parent_object_id) + '.' + OBJECT_NAME(sys.foreign_key_columns.parent_object_id), COL_NAME(sys.foreign_key_columns.parent_object_id, sys.foreign_key_columns.parent_column_id)) AS size, sys.foreign_key_columns.constraint_object_id AS fk_id FROM sys.foreign_key_columns {whereCondition}"))
                {
                    int columnSizeOrdinal = resultReader.GetOrdinal("size");
                    int columnFKIDOrdinal = resultReader.GetOrdinal("fk_id");
                    while (resultReader.Read())
                    {
                        result[foreignKeyIndices[resultReader.GetInt32(columnFKIDOrdinal)]] += resultReader.GetInt16(columnSizeOrdinal);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Cannot get foreign keys size", exc);
            }
            return result;
        }

        public DataTable MissingIndexesFromStatistics(IList<DatabaseObject> tablesLimit)
        {
            string whereCondition = "";
            if (tablesLimit != null)
            {
                whereCondition = $"AND sys.dm_db_missing_index_details.object_id IN ({string.Join(",", tablesLimit.Select(t => t.ID))})";
            }
            DataTable result = new DataTable();
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT OBJECT_SCHEMA_NAME(sys.dm_db_missing_index_details.object_id) AS Schema_name, OBJECT_NAME(sys.dm_db_missing_index_details.object_id) AS Tablename, sys.dm_db_missing_index_details.equality_columns AS Equality_columns, sys.dm_db_missing_index_details.inequality_columns AS Inequality_columns, sys.dm_db_missing_index_details.included_columns AS Included_columns, sys.dm_db_missing_index_group_stats.unique_compiles AS Unique_compiles, sys.dm_db_missing_index_group_stats.user_seeks AS User_seeks, sys.dm_db_missing_index_group_stats.user_scans AS User_scans, sys.dm_db_missing_index_group_stats.last_user_seek AS Last_user_seek, sys.dm_db_missing_index_group_stats.last_user_scan AS Last_user_scan, sys.dm_db_missing_index_group_stats.avg_total_user_cost AS Average_total_user_cost, sys.dm_db_missing_index_group_stats.avg_user_impact AS Average_user_impact, sys.dm_db_missing_index_group_stats.system_seeks AS System_seeks, sys.dm_db_missing_index_group_stats.system_scans AS System_scans, sys.dm_db_missing_index_group_stats.last_system_seek AS Last_system_seek, sys.dm_db_missing_index_group_stats.last_system_scan AS Last_system_scan, sys.dm_db_missing_index_group_stats.avg_total_system_cost AS Average_total_system_cost, sys.dm_db_missing_index_group_stats.avg_system_impact AS Average_system_impact FROM sys.dm_db_missing_index_details LEFT JOIN sys.dm_db_missing_index_groups ON sys.dm_db_missing_index_groups.index_handle = sys.dm_db_missing_index_details.index_handle LEFT JOIN sys.dm_db_missing_index_group_stats ON sys.dm_db_missing_index_group_stats.group_handle = sys.dm_db_missing_index_groups.index_group_handle WHERE DB_NAME(sys.dm_db_missing_index_details.database_id)='{_database.DatabaseName}' {whereCondition}"))
                {
                    IList<string> columns = new List<string>(resultReader.FieldCount);
                    for (int i = 0; i < resultReader.FieldCount; i++)
                    {
                        string column = resultReader.GetName(i).Replace("_", " ");
                        columns.Insert(i, column);
                        result.Columns.Add(column);
                    }
                    while (resultReader.Read())
                    {
                        DataRow row = result.NewRow();
                        for (int i = 0; i < resultReader.FieldCount; i++)
                        {
                            row[columns[i]] = GetStringRepresentationOfSqlValue(resultReader, i, true);
                        }
                        result.Rows.Add(row);
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Values cannot be loaded", exc);
            }
        }

        public int CountValuesNotUsedInForeignKeys(DependencyEdge edge)
        {
            try
            {
                return (int)Database.Instance.ExecuteScalarSelect($"SELECT COUNT(*) {GetSqlQueryValuesNotUsedInForeignKeys(edge)}");
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                if (exc is SqlException || exc is InvalidOperationException)
                {
                    throw new DatabaseException("Not used values of foreign keys is not possible to obtain", exc);
                }
                throw;
            }
        }

        private string GetSqlQueryValuesNotUsedInForeignKeys(DependencyEdge edge)
        {
            IList<string> columns = new List<string>(edge.Columns.Count);
            foreach (DependencyColumn column in edge.Columns)
            {
                columns.Add($"{edge.Parent.DbObject.NameWithSchemaBrackets}.[{column.ParentColumn.Name}] NOT IN (SELECT DISTINCT {edge.Child.DbObject.NameWithSchemaBrackets}.[{column.ChildColumn.Name}] FROM {edge.Child.DbObject.NameWithSchemaBrackets} WHERE {edge.Child.DbObject.NameWithSchemaBrackets}.[{column.ChildColumn.Name}] IS NOT NULL)");
            }
            return $"FROM {edge.Parent.DbObject.NameWithSchemaBrackets} WHERE {string.Join(" AND ", columns)}";
        }

        public IList<DependencyEdge> GetDependencyEdges()
        {
            return GetDependencyEdges(null);
        }

        public IList<DependencyEdge> GetDependencyEdges(IList<DatabaseObject> tablesLimit)
        {
            string tablesCondition = "";
            IList<DatabaseObject> tableList = null;
            if (tablesLimit != null)
            {
                tableList = tablesLimit;
                string tableIDs = string.Join(",", tablesLimit.Select(t => t.ID));
                tablesCondition = $"WHERE sys.foreign_key_columns.parent_object_id IN({tableIDs}) AND sys.foreign_key_columns.referenced_object_id IN({tableIDs})";
            }
            IDictionary<int, DependencyEdge> edges = new Dictionary<int, DependencyEdge>();
            IDictionary<int, DependencyNode> tables = new Dictionary<int, DependencyNode>();
            try
            {
                if (tableList == null)
                {
                    tableList = GetTables();
                }
                foreach (DatabaseObject dbObject in tableList)
                {
                    tables.Add(dbObject.ID, new DependencyNode(dbObject));
                }
                using (SqlDataReader resultReader = Database.Instance.ExecuteSelect($"SELECT sys.foreign_key_columns.constraint_object_id AS id, OBJECT_NAME(sys.foreign_key_columns.constraint_object_id) AS name, sys.foreign_key_columns.parent_object_id AS child, COL_NAME(sys.foreign_key_columns.parent_object_id, sys.foreign_key_columns.parent_column_id) AS child_column, sys.foreign_key_columns.referenced_object_id AS parent, COL_NAME(sys.foreign_key_columns.referenced_object_id, sys.foreign_key_columns.referenced_column_id) AS parent_column, sys.foreign_keys.delete_referential_action AS delete_action, child_columns_table.is_nullable AS child_can_be_null, OBJECT_DEFINITION(child_columns_table.default_object_id) AS child_default_value, parent_columns_table.is_nullable AS parent_can_be_null, OBJECT_DEFINITION(parent_columns_table.default_object_id) AS parent_default_value, sys.types.name AS typeName, parent_columns_table.max_length AS maxLength, sys.types.is_user_defined AS isUserType FROM sys.foreign_key_columns LEFT JOIN sys.foreign_keys ON sys.foreign_keys.object_id = sys.foreign_key_columns.constraint_object_id LEFT JOIN sys.columns AS child_columns_table ON child_columns_table.object_id = sys.foreign_key_columns.parent_object_id AND child_columns_table.column_id = sys.foreign_key_columns.parent_column_id LEFT JOIN sys.columns AS parent_columns_table ON parent_columns_table.object_id = sys.foreign_key_columns.referenced_object_id AND parent_columns_table.column_id = sys.foreign_key_columns.referenced_column_id LEFT JOIN sys.types ON sys.types.user_type_id = parent_columns_table.user_type_id {tablesCondition};"))
                {
                    int columnIdOrdinal = resultReader.GetOrdinal("id");
                    int columnNameOrdinal = resultReader.GetOrdinal("name");
                    int columnChildOrdinal = resultReader.GetOrdinal("child");
                    int columnParentOrdinal = resultReader.GetOrdinal("parent");
                    int columnChildColumnOrdinal = resultReader.GetOrdinal("child_column");
                    int columnParentColumnOrdinal = resultReader.GetOrdinal("parent_column");
                    int columnDeleteActionColumnOrdinal = resultReader.GetOrdinal("delete_action");
                    int columnChildCanBeNullColumnOrdinal = resultReader.GetOrdinal("child_can_be_null");
                    int columnChildDefaultValueColumnOrdinal = resultReader.GetOrdinal("child_default_value");
                    int columnParentCanBeNullColumnOrdinal = resultReader.GetOrdinal("parent_can_be_null");
                    int columnParentDefaultValueColumnOrdinal = resultReader.GetOrdinal("parent_default_value");
                    int columnDatatypeNameOrdinal = resultReader.GetOrdinal("typeName");
                    int columnMaxLengthOrdinal = resultReader.GetOrdinal("maxLength");
                    int columnIsUserTypeOrdinal = resultReader.GetOrdinal("isUserType");
                    while (resultReader.Read())
                    {
                        string childDefaultValue = null;
                        if (!resultReader.IsDBNull(columnChildDefaultValueColumnOrdinal))
                        {
                            childDefaultValue = resultReader.GetString(columnChildDefaultValueColumnOrdinal);
                        }
                        string parentDefaultValue = null;
                        if (!resultReader.IsDBNull(columnParentDefaultValueColumnOrdinal))
                        {
                            parentDefaultValue = resultReader.GetString(columnParentDefaultValueColumnOrdinal);
                        }
                        TableColumn childColumn = new TableColumn(resultReader.GetString(columnChildColumnOrdinal), childDefaultValue, resultReader.GetBoolean(columnChildCanBeNullColumnOrdinal), resultReader.GetString(columnDatatypeNameOrdinal), resultReader.GetInt16(columnMaxLengthOrdinal), resultReader.GetBoolean(columnIsUserTypeOrdinal), true);
                        TableColumn parentColumn = new TableColumn(resultReader.GetString(columnParentColumnOrdinal), parentDefaultValue, resultReader.GetBoolean(columnParentCanBeNullColumnOrdinal), resultReader.GetString(columnDatatypeNameOrdinal), resultReader.GetInt16(columnMaxLengthOrdinal), resultReader.GetBoolean(columnIsUserTypeOrdinal), false);
                        DependencyEdge edge;
                        if (!edges.TryGetValue(resultReader.GetInt32(columnIdOrdinal), out edge))
                        {
                            ForeignKey.DeleteActions deleteAction;
                            if (!Enum.TryParse(resultReader.GetByte(columnDeleteActionColumnOrdinal).ToString(), out deleteAction))
                            {
                                deleteAction = ForeignKey.DeleteActions.NoAction;
                            }
                            ForeignKey fk = new ForeignKey(resultReader.GetInt32(columnIdOrdinal), tables[resultReader.GetInt32(columnChildOrdinal)].DbObject, resultReader.GetString(columnNameOrdinal), deleteAction, new List<TableColumn> { childColumn });
                            edge = new DependencyEdge(fk, tables[resultReader.GetInt32(columnParentOrdinal)], parentColumn, tables[resultReader.GetInt32(columnChildOrdinal)], childColumn);
                            edges.Add(resultReader.GetInt32(columnIdOrdinal), edge);
                            edge.Parent.AddDependencyEdge(edge);
                            edge.Child.AddDependencyEdge(edge);
                        }
                        else
                        {
                            edge.AddDependencyColumn(parentColumn, childColumn);
                        }
                    }
                    return edges.Values.ToList();
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("List of edges cannot be loaded", exc);
            }
        }

        public IList<ForeignKeyMissingIndexes> GetForeignKeysWithoutIndexes(IList<DatabaseObject> tablesList)
        {
            IList<ForeignKeyMissingIndexes> keys = new List<ForeignKeyMissingIndexes>();
            string tablesWhere;
            if (tablesList != null)
            {
                tablesWhere = "WHERE main.parent_object_id IN (" + string.Join(", ", tablesList.Select(table => table.ID)) + ")";
            }
            else
            {
                tablesWhere = "";
            }
            try
            {
                IDictionary<int, ForeignKey> allKeys = new Dictionary<int, ForeignKey>();
                using (SqlDataReader resultReader = Database.Instance.ExecuteSelect($"SELECT main.constraint_object_id AS id, OBJECT_SCHEMA_NAME(main.parent_object_id) AS schema_name, main.parent_object_id AS object_id, OBJECT_NAME(main.parent_object_id) AS tablename, OBJECT_NAME(main.constraint_object_id) AS FK_name, COL_NAME(main.parent_object_id, main.parent_column_id) AS column_name, sys.types.name AS type_name, sys.types.is_user_defined AS is_user_type, sys.columns.max_length AS maxLength, sys.columns.is_nullable AS is_nullable, OBJECT_DEFINITION(sys.columns.default_object_id) AS default_value FROM sys.foreign_key_columns AS main LEFT JOIN sys.columns ON sys.columns.object_id = main.parent_object_id AND sys.columns.column_id = main.parent_column_id LEFT JOIN sys.types ON sys.types.user_type_id = sys.columns.user_type_id {tablesWhere} ORDER BY schema_name, tablename, id;"))
                {
                    int columnFKIDOrdinal = resultReader.GetOrdinal("id");
                    int columnTableIDOrdinal = resultReader.GetOrdinal("object_id");
                    int columnTablenameOrdinal = resultReader.GetOrdinal("tablename");
                    int columnSchemaOrdinal = resultReader.GetOrdinal("schema_name");
                    int columnFKNameOrdinal = resultReader.GetOrdinal("FK_name");
                    int columnNameOrdinal = resultReader.GetOrdinal("column_name");
                    int columnTypeNameOrdinal = resultReader.GetOrdinal("type_name");
                    int columnMaxLengthOrdinal = resultReader.GetOrdinal("maxLength");
                    int columnIsUserTypeOrdinal = resultReader.GetOrdinal("is_user_type");
                    int columnIsNullableOrdinal = resultReader.GetOrdinal("is_nullable");
                    int columnDefaultValueOrdinal = resultReader.GetOrdinal("default_value");
                    ForeignKey fk = null;
                    int lastFK_id = -1;
                    string lastColumn = "";
                    while (resultReader.Read())
                    {
                        int FK_id = resultReader.GetInt32(columnFKIDOrdinal);
                        string columnName = resultReader.GetString(columnNameOrdinal);
                        IList<TableColumn> columns = null;
                        if (FK_id != lastFK_id || (FK_id == lastFK_id && lastColumn != columnName))
                        {
                            string defaultValue = null;
                            if (!resultReader.IsDBNull(columnDefaultValueOrdinal))
                            {
                                defaultValue = resultReader.GetString(columnDefaultValueOrdinal);
                            }
                            TableColumn column = new TableColumn(columnName, defaultValue, resultReader.GetBoolean(columnIsNullableOrdinal), resultReader.GetString(columnTypeNameOrdinal), resultReader.GetInt16(columnMaxLengthOrdinal), resultReader.GetBoolean(columnIsUserTypeOrdinal), true);
                            if (FK_id != lastFK_id)
                            {
                                columns = new List<TableColumn>(1);
                            }
                            else
                            {
                                columns = fk.Columns;
                            }
                            columns.Add(column);
                        }
                        if (lastFK_id != FK_id)
                        {
                            if (fk != null)
                            {
                                allKeys.Add(fk.ID, fk);
                            }
                            fk = new ForeignKey(FK_id, new DatabaseObject(resultReader.GetInt32(columnTableIDOrdinal), resultReader.GetString(columnSchemaOrdinal), resultReader.GetString(columnTablenameOrdinal), DatabaseObject.DatabaseObjectType.Table), resultReader.GetString(columnFKNameOrdinal), ForeignKey.DeleteActions.NoAction, columns);
                            lastFK_id = FK_id;
                        }
                    }
                    if (lastFK_id > 0)
                    {
                        allKeys.Add(fk.ID, fk);
                    }
                }
                using (SqlDataReader resultReader = Database.Instance.ExecuteSelect($"SELECT main.constraint_object_id AS id, sys.index_columns.index_column_id AS index_id, sys.index_columns.index_column_id AS index_order, sys.columns.name AS index_column_name FROM sys.foreign_key_columns AS main LEFT JOIN sys.index_columns AS index_columns_temp ON index_columns_temp.object_id=main.parent_object_id AND index_columns_temp.column_id=main.parent_column_id LEFT JOIN sys.indexes ON sys.indexes.object_id=index_columns_temp.object_id AND sys.indexes.index_id=index_columns_temp.index_id LEFT JOIN sys.index_columns ON sys.index_columns.object_id=sys.indexes.object_id AND sys.index_columns.index_id=sys.indexes.index_id LEFT JOIN sys.columns ON sys.columns.object_id=sys.index_columns.object_id AND sys.columns.column_id=sys.index_columns.column_id {tablesWhere} ORDER BY id, index_id, index_order;"))
                {
                    int columnFKIDOrdinal = resultReader.GetOrdinal("id");
                    int columnIndexIDOrdinal = resultReader.GetOrdinal("index_id");
                    int columnIndexOrderOrdinal = resultReader.GetOrdinal("index_order");
                    int columnIndexColumnNameOrdinal = resultReader.GetOrdinal("index_column_name");
                    int lastFK_id = -1;
                    int lastIndexID = -1;
                    int lastIndexOrder = -1;
                    IList<string> stringColumns = new List<string>();
                    ForeignKey FK = null;
                    while (resultReader.Read())
                    {
                        int FK_id = resultReader.GetInt32(columnFKIDOrdinal);
                        if (lastFK_id != FK_id && lastFK_id > -1)
                        {
                            if (stringColumns.Count > 0)
                            {
                                AddMissingForeignKey(FK, keys, stringColumns);
                            }
                        }
                        if (lastFK_id != FK_id)
                        {
                            stringColumns = new List<string>(allKeys[FK_id].Columns.Select(c => c.Name));
                            FK = allKeys[FK_id];
                            lastIndexID = -1;
                            lastIndexOrder = -1;
                        }
                        lastFK_id = FK_id;
                        if (resultReader.IsDBNull(columnIndexIDOrdinal))
                        {
                            continue;
                        }
                        if (resultReader.GetInt32(columnIndexOrderOrdinal) == 1 && stringColumns.Contains(resultReader.GetString(columnIndexColumnNameOrdinal)))
                        {
                            stringColumns.Remove(resultReader.GetString(columnIndexColumnNameOrdinal));
                            lastIndexID = resultReader.GetInt32(columnIndexIDOrdinal);
                            lastIndexOrder = 1;
                        }
                        else if (resultReader.GetInt32(columnIndexIDOrdinal) == lastIndexID && resultReader.GetInt32(columnIndexOrderOrdinal) == lastIndexOrder + 1)
                        {
                            stringColumns.Remove(resultReader.GetString(columnIndexColumnNameOrdinal));
                        }
                        
                    }
                    if (FK != null && stringColumns.Count > 0)
                    {
                        AddMissingForeignKey(FK, keys, stringColumns);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("List of foreign keys without indexes cannot be loaded", exc);
            }
            return keys;
        }

        public bool HasTableIdentityColumn(DatabaseObject table)
        {
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect($"SELECT sys.identity_columns.object_id FROM sys.identity_columns WHERE sys.identity_columns.object_id = {table.ID};"))
                {
                    while (resultReader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("Cannot load identity columns", exc);
            }
        }

        public IList<string> GetCollationList()
        {
            try
            {
                using (SqlDataReader resultReader = _database.ExecuteSelect("SELECT name FROM sys.fn_helpcollations();"))
                {
                    IList<string> result = new List<string>();
                    int columnNameOrdinal = resultReader.GetOrdinal("name");
                    while (resultReader.Read())
                    {
                        result.Add(resultReader.GetString(columnNameOrdinal));
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw new DatabaseException("List of collations cannot be loaded", exc);
            }
        }

        public class ValueInsertGetter : IEnumerable<string>, IDisposable
        {
            private readonly ValueInsertEnumerator _enumerator;
            private bool _disposed;

            public ValueInsertGetter(string select)
            {
                _enumerator = new ValueInsertEnumerator(select);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _enumerator?.Dispose();
                    }
                    _disposed = true;
                }
            }

            private class ValueInsertEnumerator : IEnumerator<string>
            {
                private bool _disposed;
                private readonly SqlDataReader _resultReader;

                public ValueInsertEnumerator(string select)
                {
                    try
                    {
                        _resultReader = Database.Instance.ExecuteSelect(select);
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc);
                        throw new DatabaseException("Values for inserts cannot be obtained", exc);
                    }
                }

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                private void Dispose(bool disposing)
                {
                    if (!_disposed)
                    {
                        if (disposing)
                        {
                            _resultReader?.Dispose();
                        }
                        _disposed = true;
                    }
                }

                public bool MoveNext()
                {
                    try
                    {
                        if (_resultReader.Read())
                        {
                            IList<string> values = new List<string>(_resultReader.FieldCount);
                            for (int i = 0; i < _resultReader.FieldCount; i++)
                            {
                                values.Add(GetInsertRepresentationOfSqlValue(_resultReader, i));
                            }
                            Current = "(" + string.Join(", ", values) + ")";
                            return true;
                        }
                        return false;
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc);
                        throw new DatabaseException("Values for inserts cannot be obtained", exc);
                    }
                }

                public void Reset()
                {
                    throw new NotSupportedException();
                }

                public string Current { get; private set; }

                object IEnumerator.Current => Current;
            }
        }

        private void AddMissingForeignKey(ForeignKey FK, IList<ForeignKeyMissingIndexes> keys, IList<string> missingIndexColumns)
        {
            IList<TableColumn> columns = new List<TableColumn>(missingIndexColumns.Count);
            foreach (string column in missingIndexColumns)
            {
                columns.Add(new TableColumn(column, null, false, null, -1, false, true)); // Only need to know name of column
            }
            keys.Add(new ForeignKeyMissingIndexes(FK, columns));
        }

        private long GetIntValue(SqlDataReader resultReader, int columnOrdinal)
        {
            string dataType = resultReader.GetDataTypeName(columnOrdinal);
            if (dataType == DataTypeConstants.INT)
            {
                return resultReader.GetInt32(columnOrdinal);
            }
            else if (dataType == DataTypeConstants.BIGINT)
            {
                return resultReader.GetInt64(columnOrdinal);
            }
            else if (dataType == DataTypeConstants.SMALLINT)
            {
                return resultReader.GetInt16(columnOrdinal);
            }
            else
            {
                return resultReader.GetByte(columnOrdinal);
            }
        }

        private static string GetInsertRepresentationOfSqlValue(SqlDataReader resultReader, int field)
        {
            if (!resultReader.IsDBNull(field))
            {
                if (resultReader.GetDataTypeName(field) == DataTypeConstants.DATETIME || resultReader.GetDataTypeName(field) == DataTypeConstants.DATE)
                {
                    return $"CAST(N'{GetStringRepresentationOfSqlValue(resultReader, field, false)}' AS {resultReader.GetDataTypeName(field)})";
                }
                else if (DataTypeConstants.DataTypeIsBinary(resultReader.GetDataTypeName(field)))
                {
                    return $"CONVERT({resultReader.GetDataTypeName(field)}, '{GetStringRepresentationOfSqlValue(resultReader, field, false)}', 1)";
                }
                else if (!DataTypeConstants.DataTypeIsUnicodeCharPrefix(resultReader.GetDataTypeName(field)))
                {
                    return "'" + GetStringRepresentationOfSqlValue(resultReader, field, false).Replace("'", "''") + "'";
                }
                else
                {
                    return "N'" + GetStringRepresentationOfSqlValue(resultReader, field, false).Replace("'", "''") + "'";
                }
            }
            else
            {
                return GetStringRepresentationOfSqlValue(resultReader, field, false);
            }
        }

        private static string GetStringRepresentationOfSqlValue(SqlDataReader resultReader, int field, bool nullAsHyphen)
        {
            if (!resultReader.IsDBNull(field))
            {
                if (resultReader.GetDataTypeName(field) == DataTypeConstants.DATETIME)
                {
                    return resultReader.GetDateTime(field).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                }
                else if (resultReader.GetDataTypeName(field) == DataTypeConstants.DATE)
                {
                    return resultReader.GetDateTime(field).ToString("yyyy-MM-dd");
                }
                else if (DataTypeConstants.DataTypeIsBinary(resultReader.GetDataTypeName(field)))
                {
                    byte[] bytes = (byte[])resultReader.GetValue(field);
                    return $"0x{BitConverter.ToString(bytes).Replace("-", string.Empty)}";
                }
                else if (DataTypeConstants.DataTypeIsFloatingPoint(resultReader.GetDataTypeName(field)))
                {
                    return resultReader.GetValue(field).ToString().Replace(",", "."); // conversion to floatinf point (fixes Czech environment)
                }
                else if (!DataTypeConstants.DataTypeIsUnicodeCharPrefix(resultReader.GetDataTypeName(field)))
                {
                    return resultReader.GetValue(field).ToString();
                }
                else
                {
                    return resultReader.GetValue(field).ToString();
                }
            }
            else if (nullAsHyphen)
            {
                return "-";
            }
            else
            {
                return "NULL";
            }
        }
    }
}

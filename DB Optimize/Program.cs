using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DB_Optimize.DB;
using DB_Optimize.Dependency;
using DB_Optimize.Optimization;

namespace DB_Optimize
{
    class Program
    {
        private const string HELP_COMMAND = "help";
        private const string HELP_DESCRIPTION = "lists this overview of commands";
        private const string ARCHIVE_ENTRIES_COMMAND = "archive_entries";
        private const string ARCHIVE_TABLE_DESCRIPTION = "archives entries with all dependencies";
        private const string CHANGE_COLLATION_COMMAND = "change_collation";
        private const string CHANGE_COLLATION_DESCRIPTION = "changes database collation to given collation, WARNING: it is recommended to create backup of your database before running this command";
        private const string COLUMNS_OPTIMAL_TYPE_COMMAND = "columns_optimal_type";
        private const string COLUMNS_OPTIMAL_TYPE_DESCRIPTION = "shows suggestions of optimal data type of columns";
        private const string DELETE_ENTRIES_COMMAND = "delete_entries";
        private const string DELETE_ENTRIES_DESCRIPTION = "deletes entries respecting all dependencies";
        private const string FK_BAD_TYPE_COMMAND = "fk_bad_type";
        private const string FK_BAD_TYPE_DESCRIPTION = "shows suggestions of foreign keys data type";
        private const string FK_DELETE_ACTIONS_COMMAND = "fk_delete_actions";
        private const string FK_DELETE_ACTIONS_DESCRIPTION = "evaluates foreign keys and their delete actions";
        private const string FK_MISSING_INDEX_COMMAND = "fk_missing_index";
        private const string FK_MISSING_INDEX_DESCRIPTION = "shows missing indices on foreign keys";
        private const string FK_NOT_USED_COMMAND = "fk_used";
        private const string FK_NOT_USED_DESCRIPTION = "checks foreign keys if they are used and shows them";
        private const string INDEX_STATISTICS_COMMAND = "index_statistics";
        private const string INDEX_STATISTICS_DESCRIPTION = "shows recommendations of indices based on server statistics for given tables";
        private const string DEPENDENCY_COMMAND = "table_dependency";
        private const string DEPENDENCY_DESCRIPTION = "shows table dependencies";
        private const string DEPENDENCY_EXPORT_COMMAND = "table_dependency_export";
        private const string DEPENDENCY_EXPORT_DESCRIPTION = "exports table dependencies to given file in format dot";
        private const string QUIT_COMMAND = "quit";
        private const string QUIT_DESCRIPTION = "terminates this tool";
        private const string ALL_TABLES_SIGNAL = "-all";
        private const string UNKNOWN_COMMAND_ERROR = "Error: Unknown command, show supported commands with help keyword";
        private const string NO_TABLE_SELECTED = "Error: No table specified, please append table names or use " + ALL_TABLES_SIGNAL + " for all Tables";
        private const string HELP_ARG_TABLES = "tables - list of tables separated by spaces (or -all for list all tables), you can use [ and ]";
        private const string SEPARATOR = "____________________________";
        private const string DELETE_ACTION_NO_ACTION = "NO_ACTION";
        private const string DELETE_ACTION_CASCADE = "CASCADE";
        private const string DELETE_ACTION_SET_NULL = "SET_NULL";
        private const string DELETE_ACTION_SET_DEFAULT = "SET_DEFAULT";
        private static readonly string[] SUPPORTED_DELETE_ACTIONS = { DELETE_ACTION_NO_ACTION, DELETE_ACTION_CASCADE, DELETE_ACTION_SET_NULL, DELETE_ACTION_SET_DEFAULT };
        private static readonly string SUPPORTED_DELETE_ACTION_VALUES_TEXT = $"supported values are: {string.Join(", ", SUPPORTED_DELETE_ACTIONS)}";
        private static readonly IDictionary<string, ForeignKey.DeleteActions> DELETE_ACTION_CONVERTOR = new Dictionary<string, ForeignKey.DeleteActions> {{DELETE_ACTION_NO_ACTION, ForeignKey.DeleteActions.NoAction}, { DELETE_ACTION_CASCADE, ForeignKey.DeleteActions.Cascade }, { DELETE_ACTION_SET_NULL, ForeignKey.DeleteActions.SetNull }, { DELETE_ACTION_SET_DEFAULT, ForeignKey.DeleteActions.SetDefault } };

        private static DatabaseOperations _databaseOperations;
        private static TableDependencyGraph _dependencies;
        private static Optimizer _optimizer;
        private static readonly IDictionary<string, DatabaseObject> _tableByName = new Dictionary<string, DatabaseObject>();

        static void Main(string[] args)
        {
            TextReader input = Console.In;
            if (args.Length > 2)
            {
                Console.WriteLine("Error: You must provide zero params or up to 2 params (1: connection string do database, 2: filename containg commands (it is mandatory use connection string parameter))");
                input.Read();
                return;
            }
            string connectionString;
            bool fileCommands = false;
            if (args.Length == 1)
            {
                connectionString = args[0];
            }
            else if (args.Length >= 2)
            {
                connectionString = args[0];
                string commandFile = args[1];
                fileCommands = true;
                if (File.Exists(commandFile))
                {
                    input = File.OpenText(commandFile);
                }
                else
                {
                    Console.WriteLine($"Error: File {commandFile} cannot be open - it does not exists");
                    input.Read();
                    return;
                }
            }
            else
            {
                connectionString = GetConnectionString();
            }
            Console.WriteLine("Connecting to database....");
            if (Database.Instance.Connect(connectionString))
            {
                Console.WriteLine();
                Console.WriteLine("Connection was successful.");
                Console.WriteLine();
                _databaseOperations = new DatabaseOperations(Database.Instance);
                _optimizer = Optimizer.Instance;
                try
                {
                    foreach (DatabaseObject dbObject in _databaseOperations.GetTables())
                    {
                        _tableByName.Add(dbObject.NameWithSchema.ToLower(), dbObject);
                    }
                    _dependencies = new TableDependencyGraph();
                }
                catch (TableDependencyException exc)
                {
                    Debug.WriteLine(exc);
                    Console.WriteLine("Error: Cannot get table dependencies graph");
                    return;
                }
                catch (DatabaseException exc)
                {
                    Debug.WriteLine(exc);
                    Console.WriteLine("Error: Cannot get tables");
                    return;
                }
                Console.WriteLine("Table dependency graph has been built.");
                if (!fileCommands)
                {
                    ListCommands();
                }
                ProcessCommands(input, fileCommands);
                if (fileCommands)
                {
                    input.Close();
                    Console.WriteLine("All commands have been processed");
                }
            }
            else
            {
                Console.WriteLine("Error: Connection failed. Details: {0}", Database.Instance.ErrorMessage);
                input.Read();
            }
        }

        private static void ListCommands()
        {
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine($"{HELP_COMMAND} -> {HELP_DESCRIPTION}");
            Console.WriteLine($"{ARCHIVE_ENTRIES_COMMAND} -> {ARCHIVE_TABLE_DESCRIPTION}");
            Console.WriteLine($"{CHANGE_COLLATION_COMMAND} -> {CHANGE_COLLATION_DESCRIPTION}");
            Console.WriteLine($"{COLUMNS_OPTIMAL_TYPE_COMMAND} -> {COLUMNS_OPTIMAL_TYPE_DESCRIPTION}");
            Console.WriteLine($"{DELETE_ENTRIES_COMMAND} -> {DELETE_ENTRIES_DESCRIPTION}");
            Console.WriteLine($"{FK_BAD_TYPE_COMMAND} -> {FK_BAD_TYPE_DESCRIPTION}");
            Console.WriteLine($"{FK_DELETE_ACTIONS_COMMAND} -> {FK_DELETE_ACTIONS_DESCRIPTION}");
            Console.WriteLine($"{FK_MISSING_INDEX_COMMAND} -> {FK_MISSING_INDEX_DESCRIPTION}");
            Console.WriteLine($"{FK_NOT_USED_COMMAND} -> {FK_NOT_USED_DESCRIPTION}");
            Console.WriteLine($"{INDEX_STATISTICS_COMMAND} -> {INDEX_STATISTICS_DESCRIPTION}");
            Console.WriteLine($"{DEPENDENCY_COMMAND} -> {DEPENDENCY_DESCRIPTION}");
            Console.WriteLine($"{DEPENDENCY_EXPORT_COMMAND} -> {DEPENDENCY_EXPORT_DESCRIPTION}");
            Console.WriteLine($"{QUIT_COMMAND} -> {QUIT_DESCRIPTION}");
            Console.WriteLine();
        }

        private static void ProcessCommands(TextReader input, bool fromFile)
        {
            Regex commandNameRegex = new Regex(@"^(?<command>\w+)(?: (?<args>(?:.)+))?$");
            string command = input.ReadLine();
            while (command != null && command.ToLower() != QUIT_COMMAND)
            {
                if (fromFile && command.Trim().Length > 0 && !command.StartsWith("//"))
                {
                    Console.WriteLine($"Processing command: {command}");
                }
                else if (fromFile)
                {
                    command = input.ReadLine();
                    continue;
                }
                Match commandMatch = commandNameRegex.Match(command);
                switch (commandMatch.Groups["command"].Value.ToLower())
                {
                    case HELP_COMMAND:
                        ProcessHelpCommand(commandMatch.Groups["args"].Value);
                        break;
                    case ARCHIVE_ENTRIES_COMMAND:
                        ProcessArchiveCommand(commandMatch.Groups["args"].Value);
                        break;
                    case CHANGE_COLLATION_COMMAND:
                        ProcessChangeCollationCommand(commandMatch.Groups["args"].Value);
                        break;
                    case COLUMNS_OPTIMAL_TYPE_COMMAND:
                        ProcessColumnsOptimalTypeCommand(commandMatch.Groups["args"].Value);
                        break;
                    case DELETE_ENTRIES_COMMAND:
                        ProcessDeleteEntries(commandMatch.Groups["args"].Value);
                        break;
                    case FK_BAD_TYPE_COMMAND:
                        ProcessFKBadTypeCommand(commandMatch.Groups["args"].Value);
                        break;
                    case FK_DELETE_ACTIONS_COMMAND:
                        ProcessFKDeleteActionsCommand(commandMatch.Groups["args"].Value);
                        break;
                    case FK_MISSING_INDEX_COMMAND:
                        ProcessFKMissingIndexCommand(commandMatch.Groups["args"].Value);
                        break;
                    case FK_NOT_USED_COMMAND:
                        ProcessFKNotUsedCommand(commandMatch.Groups["args"].Value);
                        break;
                    case INDEX_STATISTICS_COMMAND:
                        ProcessIndexStatisticsCommand(commandMatch.Groups["args"].Value);
                        break;
                    case DEPENDENCY_COMMAND:
                        ProcessDependencyCommand(commandMatch.Groups["args"].Value);
                        break;
                    case DEPENDENCY_EXPORT_COMMAND:
                        ProcessDependencyExportCommand(commandMatch.Groups["args"].Value);
                        break;
                    case "":
                        if (command.Trim().Length > 0 && !command.StartsWith("//"))
                        {
                            Console.WriteLine(UNKNOWN_COMMAND_ERROR);
                        }
                        break;
                    default:
                        Console.WriteLine(UNKNOWN_COMMAND_ERROR);
                        break;
                }
                command = input.ReadLine();
                if (fromFile)
                {
                    Console.WriteLine();
                }
            }
        }

        private static IList<DatabaseObject> GetTablesFromArgs(string args, out IList<string> errors)
        {
            Regex tablesRegex = new Regex(@"(?<tables>(?:(?:\[(?:\S| )+\])|\S|\.)+)");
            IList<DatabaseObject> result = new List<DatabaseObject>();
            errors = new List<string>();
            foreach (Match match in tablesRegex.Matches(args))
            {
                DatabaseObject dbObject;
                string tablename = match.Value.Replace("[", "").Replace("]", "").ToLower();
                if (tablename == ALL_TABLES_SIGNAL)
                {
                    return _tableByName.Values.ToList();
                }
                else if (_tableByName.TryGetValue(tablename, out dbObject))
                {
                    result.Add(dbObject);
                }
                else
                {
                    errors.Add($"Error: Table {tablename} does not exist.");
                }
            }
            return result;
        }

        private static void ProcessHelpCommand(string args)
        {
            if (args == "")
            {
                ListCommands();
                Console.WriteLine();
                Console.WriteLine($"Command line arguments: \"{Process.GetCurrentProcess().MainModule.FileName}\" [<connection_string> [<commands_filename>]]");
                Console.WriteLine("connection_string - optional argument, it is a connection string which will be used to connect to database");
                Console.WriteLine("commands_filename - optional argument (but must be used connection_string), it is a path to file with commands");
                Console.WriteLine($"Example: \"{Process.GetCurrentProcess().MainModule.FileName}\" \"data source=(localdb)\\MSSQLLocalDB;database=AdventureWorks;\" commands.txt");
            }
            else
            {
                switch (args)
                {
                    case DEPENDENCY_COMMAND:
                        Console.WriteLine(DEPENDENCY_DESCRIPTION);
                        Console.WriteLine($"Syntax: {DEPENDENCY_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {DEPENDENCY_COMMAND} Person.Address Person.Person");
                        break;
                    case DEPENDENCY_EXPORT_COMMAND:
                        Console.WriteLine(DEPENDENCY_EXPORT_DESCRIPTION);
                        Console.WriteLine($"Syntax: {DEPENDENCY_EXPORT_COMMAND} <filename>");
                        Console.WriteLine("filename - path to file, where you want to store dot export of table dependencies");
                        Console.WriteLine($"Example: {DEPENDENCY_EXPORT_COMMAND} export.dot");
                        break;
                    case ARCHIVE_ENTRIES_COMMAND:
                        Console.WriteLine(ARCHIVE_TABLE_DESCRIPTION);
                        Console.WriteLine($"Syntax: {ARCHIVE_ENTRIES_COMMAND} [-is] [-all] [-ct] <filename> <table> [-n <oldtables=newtables>] [-c <conditions>]");
                        Console.WriteLine("-is - optional argument to use Insert with Select");
                        Console.WriteLine("-all - optional argument to save all values from dependent tables");
                        Console.WriteLine("-ct - optional argument to include create table query");
                        Console.WriteLine("filename - path to file, where you want to store dot export of table dependencies");
                        Console.WriteLine("table - table from which you want to archive entries");
                        Console.WriteLine("-n <oldtables=newtables> - optional argument, you can specify renaming archived tables");
                        Console.WriteLine("-c <conditions> - optional argument, conditions can limit values, which will be archived");
                        Console.WriteLine($"Example: {ARCHIVE_ENTRIES_COMMAND} -is -all -ct archive.sql Person.Address -n Person.Address=Person.Address_old -c Person.Address.AddressLine2 IS NOT NULL");
                        break;
                    case CHANGE_COLLATION_COMMAND:
                        Console.WriteLine(CHANGE_COLLATION_DESCRIPTION);
                        Console.WriteLine($"Syntax: {CHANGE_COLLATION_COMMAND} <collation>");
                        Console.WriteLine("collation - new database collation");
                        Console.WriteLine($"Example: {CHANGE_COLLATION_COMMAND} Traditional_Spanish_ci_ai");
                        break;
                    case COLUMNS_OPTIMAL_TYPE_COMMAND:
                        Console.WriteLine(COLUMNS_OPTIMAL_TYPE_DESCRIPTION);
                        Console.WriteLine($"Syntax: {COLUMNS_OPTIMAL_TYPE_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {COLUMNS_OPTIMAL_TYPE_COMMAND} Person.Address Person.Person");
                        break;
                    case DELETE_ENTRIES_COMMAND:
                        Console.WriteLine(DELETE_ENTRIES_DESCRIPTION);
                        Console.WriteLine($"Syntax: {DELETE_ENTRIES_COMMAND} <table> [<default_delete_action>] [-d <foreign_key=delete_action>] [-c <conditions>]");
                        Console.WriteLine("table - table from which you want to delete entries");
                        Console.WriteLine($"default_delete_action - sets default delete actions for all foreign keys, can be overriden with param -d, {SUPPORTED_DELETE_ACTION_VALUES_TEXT}");
                        Console.WriteLine($"-d <foreign_key=delete_action> - optional argument, you can specify delete action ({SUPPORTED_DELETE_ACTION_VALUES_TEXT}) for given foreign key");
                        Console.WriteLine("-c <conditions> - optional argument, conditions can limit values, which will be deleted");
                        Console.WriteLine($"Example: {DELETE_ENTRIES_COMMAND} Person.Address SET_NULL -d FK_SalesOrderHeader_Address_BillToAddressID=CASCADE -c Person.Address.AddressLine2 IS NOT NULL");
                        break;
                    case FK_BAD_TYPE_COMMAND:
                        Console.WriteLine(FK_BAD_TYPE_DESCRIPTION);
                        Console.WriteLine($"Syntax: {FK_BAD_TYPE_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {FK_BAD_TYPE_COMMAND} Person.Address Person.Person");
                        break;
                    case FK_DELETE_ACTIONS_COMMAND:
                        Console.WriteLine(FK_DELETE_ACTIONS_DESCRIPTION);
                        Console.WriteLine($"Syntax: {FK_DELETE_ACTIONS_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {FK_DELETE_ACTIONS_COMMAND} Person.Address Person.Person");
                        break;
                    case FK_MISSING_INDEX_COMMAND:
                        Console.WriteLine(FK_MISSING_INDEX_DESCRIPTION);
                        Console.WriteLine($"Syntax: {FK_MISSING_INDEX_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {FK_MISSING_INDEX_COMMAND} Person.Address Person.Person");
                        break;
                    case FK_NOT_USED_COMMAND:
                        Console.WriteLine(FK_NOT_USED_DESCRIPTION);
                        Console.WriteLine($"Syntax: {FK_NOT_USED_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {FK_NOT_USED_COMMAND} Person.Address Person.Person");
                        break;
                    case INDEX_STATISTICS_COMMAND:
                        Console.WriteLine(INDEX_STATISTICS_DESCRIPTION);
                        Console.WriteLine($"Syntax: {INDEX_STATISTICS_COMMAND} <tables>");
                        Console.WriteLine(HELP_ARG_TABLES);
                        Console.WriteLine($"Example: {INDEX_STATISTICS_COMMAND} Person.Address Person.Person");
                        break;
                    case QUIT_COMMAND:
                        Console.WriteLine(QUIT_DESCRIPTION);
                        break;
                    case HELP_COMMAND:
                        Console.WriteLine(HELP_DESCRIPTION);
                        break;
                    default:
                        Console.WriteLine($"Error: Unknown command {args}");
                        break;
                }
            }
        }

        private static void ProcessArchiveCommand(string args)
        {
            Regex archiveRegex = new Regex(@"^(?:(?:(?<is>-is)?|(?<all>-all)?|(?<ct>-ct)?) )*(?<filename>(?:\S+|""(?:\S| )+"")) (?<table>(?:(?:\[(?:\S| )+\])|\S|\.)+)(?: (?:-n (?<n>(?: ?(?<oldTable>(?:(?:\[(?:\S| )+\])|\S|\.)+)\=(?<newTable>(?:(?:\[(?:\S| )+\])|\S|\.)+))+)))?(?: (?:-c (?<c>.+)))?$");
            Match archiveMatch = archiveRegex.Match(args);
            DatabaseObject archiveTable;
            if (!archiveMatch.Success)
            {
                Console.WriteLine($"Error: Invalid syntax for command {ARCHIVE_ENTRIES_COMMAND}, syntax information can be showed by typing: help {ARCHIVE_ENTRIES_COMMAND}");
                return;
            }
            else if (!_tableByName.TryGetValue(archiveMatch.Groups["table"].Value.Replace("[", "").Replace("]", "").ToLower(), out archiveTable))
            {
                Console.WriteLine($"Error: Table {archiveMatch.Groups["table"].Value} not found");
                return;
            }
            try
            {
                using (StreamWriter fileStream = new StreamWriter(archiveMatch.Groups["filename"].Value))
                {
                    ISet<DependencyNode> dependentTables = _dependencies.GetAncestors(archiveTable);
                    dependentTables.Add(_dependencies[archiveTable]);
                    bool createTable = false;
                    bool useInsertIntoSelect = false;
                    bool saveAll = false;
                    string conditions = "";
                    IDictionary<string, string> nameConvertor = new Dictionary<string, string>(dependentTables.Count);
                    foreach (DependencyNode table in dependentTables)
                    {
                        nameConvertor.Add(table.DbObject.NameWithSchema.ToLower(), table.DbObject.NameWithSchemaBrackets);
                    }
                    if (archiveMatch.Groups["n"].Success)
                    {
                        for (int i = 0; i < archiveMatch.Groups["newTable"].Captures.Count; i++)
                        {
                            nameConvertor[archiveMatch.Groups["oldTable"].Captures[i].Value.Replace("[", "").Replace("]", "").ToLower()] = archiveMatch.Groups["newTable"].Captures[i].Value;
                        }
                    }
                    if (archiveMatch.Groups["ct"].Success)
                    {
                        createTable = true;
                    }
                    if (archiveMatch.Groups["c"].Success)
                    {
                        conditions = archiveMatch.Groups["c"].Value;
                    }
                    if (archiveMatch.Groups["is"].Success)
                    {
                        useInsertIntoSelect = true;
                    }
                    if (archiveMatch.Groups["all"].Success)
                    {
                        saveAll = true;
                    }
                    Console.WriteLine("Archiving in progress...");
                    if (_optimizer.ArchiveTable(archiveTable, nameConvertor, createTable, conditions, useInsertIntoSelect, saveAll, fileStream))
                    {
                        Console.WriteLine("Archive has been created and saved");
                    }
                    else
                    {
                        Console.WriteLine("Nothing to archive");
                    }
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine($"Error: cannot get data - {exc.InnerException.Message}");
                return;
            }
            catch (IOException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine($"Error: problem data file - {exc.Message}");
                return;
            }
        }

        private static void ProcessDeleteEntries(string args)
        {
            Regex deleteRegex = new Regex(@"^(?<table>(?:(?:\[(?:\S| )+\])|\S|\.)+)(?: (?<defaultDeleteAction>" + string.Join("|", SUPPORTED_DELETE_ACTIONS) + @"))?(?: (?:-d (?<d>(?: ?(?<foreignKey>(?:(?:\[(?:\S| )+\])|\S|\.)+)\=(?<deleteAction>" + string.Join("|", SUPPORTED_DELETE_ACTIONS) + @"))+)))?(?: (?:-c (?<c>.+)))?$");
            Match deleteMatch = deleteRegex.Match(args);
            DatabaseObject deleteTable;
            if (!deleteMatch.Success)
            {
                Console.WriteLine($"Error: Invalid syntax for command {DELETE_ENTRIES_COMMAND}, syntax information can be showed by typing: help {DELETE_ENTRIES_COMMAND}");
                return;
            }
            else if (!_tableByName.TryGetValue(deleteMatch.Groups["table"].Value.Replace("[", "").Replace("]", "").ToLower(), out deleteTable))
            {
                Console.WriteLine($"Error: Table {deleteMatch.Groups["table"].Value} not found");
                return;
            }
            string conditions = "";
            ForeignKey.DeleteActions? defaultDeleteAction = null;
            if (deleteMatch.Groups["c"].Success)
            {
                conditions = deleteMatch.Groups["c"].Value;
            }
            if (deleteMatch.Groups["defaultDeleteAction"].Success)
            {
                if (DELETE_ACTION_CONVERTOR.ContainsKey(deleteMatch.Groups["defaultDeleteAction"].Value.ToUpper()))
                {
                    defaultDeleteAction = DELETE_ACTION_CONVERTOR[deleteMatch.Groups["defaultDeleteAction"].Value.ToUpper()];
                }
                else
                {
                    Console.WriteLine($"Error: Unsupported default delete action, {SUPPORTED_DELETE_ACTION_VALUES_TEXT}");
                    return;
                }
            }
            IDictionary<string, ForeignKey.DeleteActions> overrideDeleteActions = new Dictionary<string, ForeignKey.DeleteActions>();
            if (deleteMatch.Groups["foreignKey"].Success)
            {
                for (int i = 0; i < deleteMatch.Groups["foreignKey"].Captures.Count; i++)
                {
                    ForeignKey.DeleteActions deleteAction;
                    if (DELETE_ACTION_CONVERTOR.TryGetValue(deleteMatch.Groups["deleteAction"].Captures[i].Value.ToUpper(), out deleteAction))
                    {
                        overrideDeleteActions.Add(deleteMatch.Groups["foreignKey"].Captures[i].Value.ToLower(), deleteAction);
                    }
                    else
                    {
                        Console.WriteLine($"Error: Unsupported default delete action {deleteMatch.Groups["defaultDeleteAction"].Value} at foreign key {deleteMatch.Groups["foreignKey"].Captures[i].Value}, {SUPPORTED_DELETE_ACTION_VALUES_TEXT}");
                        return;
                    }
                }
            }
            ISet<DependencyEdge> edges = _dependencies.GetDescendantEdges(deleteTable);
            IDictionary<ForeignKey, ForeignKey.DeleteActions> foreignKeyDeleteActions = new Dictionary<ForeignKey, ForeignKey.DeleteActions>(edges.Count);
            foreach (DependencyEdge edge in edges)
            {
                ForeignKey.DeleteActions? overrideDeleteAction;
                if (overrideDeleteActions.ContainsKey(edge.ForeignKey.Name.ToLower()))
                {
                    overrideDeleteAction = overrideDeleteActions[edge.ForeignKey.Name.ToLower()];
                }
                else
                {
                    overrideDeleteAction = defaultDeleteAction;
                }
                if (overrideDeleteAction == null)
                {
                    foreignKeyDeleteActions.Add(edge.ForeignKey, edge.ForeignKey.DeleteAction);
                }
                else if (overrideDeleteAction.Value == ForeignKey.DeleteActions.SetDefault && edge.ForeignKey.HasDefaultValue)
                {
                    foreignKeyDeleteActions.Add(edge.ForeignKey, ForeignKey.DeleteActions.SetDefault);
                }
                else if (overrideDeleteAction.Value == ForeignKey.DeleteActions.SetNull && edge.ForeignKey.CanBeNull)
                {
                    foreignKeyDeleteActions.Add(edge.ForeignKey, ForeignKey.DeleteActions.SetNull);
                }
                else
                {
                    foreignKeyDeleteActions.Add(edge.ForeignKey, overrideDeleteAction.Value);
                }
            }
            try
            {
                Console.WriteLine("Delete procedure is running");
                string deleteQuery = _optimizer.DeleteEntriesFromTable(deleteTable, conditions, foreignKeyDeleteActions);
                Console.WriteLine(deleteQuery);
                Database.Instance.ExecuteNonResultQuery(deleteQuery);
                Console.WriteLine("Delete procedure was completed");
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine($"Error: cannot get data - {exc.InnerException.Message}");
                return;
            }
            catch (DeleteDependencyException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine($"Error: cannot get data - {exc.Message}");
                return;
            }
            catch (SqlException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine("Error: delete query failed");
                return;
            }
        }

        private static void ProcessChangeCollationCommand(string args)
        {
            IList<string> supportedCollations = _databaseOperations.GetCollationList().Select(c => c.ToLower()).ToList();
            string collation = args;
            if (collation == "")
            {
                Console.WriteLine("Error: Missing collation argument");
            }
            else if (supportedCollations.Contains(collation.ToLower()))
            {
                Console.WriteLine("Change of collation is started, it can take minutes, please wait");
                try
                {
                    _optimizer.ChangeCollation(collation);
                    Console.WriteLine("Change of collation was succesfully executed");
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                    Console.WriteLine($"Error: Change of collation failed with error - {exc.Message}");
                }
            }
            else
            {
                Console.WriteLine("Error: Unsupported collation, supported collations are:");
                foreach (string supportedCollation in supportedCollations)
                {
                    Console.WriteLine(supportedCollation);
                }
            }
        }

        private static void ProcessColumnsOptimalTypeCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return;
            }
            try
            {
                foreach (DatabaseObject table in tables)
                {
                    Console.WriteLine($"Table: {table.NameWithSchemaBrackets}");
                    IList<TableColumn> columns = _databaseOperations.GetTableColumns(table);
                    foreach (TableColumn column in columns)
                    {
                        Console.WriteLine($"Column: {column.Name}");
                        foreach (IList<string> infoPair in _optimizer.EvaluateColumnDataType(column, table))
                        {
                            if (infoPair.Count > 1)
                            {
                                Console.WriteLine(string.Join(" ", infoPair));
                            }
                            else
                            {
                                Console.WriteLine(infoPair[0]);
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine(SEPARATOR);
                    Console.WriteLine();
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine("Error: Statistics cannot be loaded");
            }
        }

        private static void ProcessIndexStatisticsCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return;
            }
            try
            {
                DataTable values = _databaseOperations.MissingIndexesFromStatistics(tables);
                if (values.Rows.Count > 0)
                {
                    foreach (DataRow row in values.Rows)
                    {
                        foreach (DataColumn column in values.Columns)
                        {
                            Console.WriteLine($"{column.ColumnName}: {row[column]}");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No recommendations.");
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine("Error: Statistics cannot be loaded");
            }
        }

        private static void ProcessFKBadTypeCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return;
            }
            try
            {
                ISet<DatabaseObject> tablesWithoutForeignKeys = new HashSet<DatabaseObject>(tables);
                IList<ForeignKey> foreignKeys = new List<ForeignKey>();
                foreach (DatabaseObject table in tables)
                {
                    foreach (DependencyEdge edge in Optimizer.Instance.DependencyGraph[table].ParentEdges)
                    {

                        foreignKeys.Add(edge.ForeignKey);
                    }
                }
                IDictionary<ForeignKey, int> foreignKeySizes = _databaseOperations.GetForeignKeySize(foreignKeys);
                foreach (ForeignKey foreignKey in foreignKeys)
                {
                    Console.WriteLine($"Table: {foreignKey.Table.NameWithSchemaBrackets}");
                    Console.WriteLine($"Foreign key: {foreignKey.Name}");
                    tablesWithoutForeignKeys.Remove(foreignKey.Table);
                    int size = foreignKeySizes[foreignKey];
                    foreach (IList<string> infoPair in _optimizer.EvaluateForeignKeyDataType(foreignKey, size))
                    {
                        if (infoPair.Count > 1)
                        {
                            Console.WriteLine(string.Join(" ", infoPair));
                        }
                        else
                        {
                            Console.WriteLine(infoPair[0]);
                        }
                    }
                    Console.WriteLine();
                }
                if (foreignKeys.Count == 0)
                {
                    Console.WriteLine("All tables are without foreign keys");
                    return;
                }
                else if (tablesWithoutForeignKeys.Count > 0)
                {
                    Console.WriteLine($"These tables are without foreign keys: {string.Join(", ", tablesWithoutForeignKeys)}");
                    return;
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine("Error: Foreign key sizes cannot be loaded");
                return;
            }
        }

        private static void ProcessFKDeleteActionsCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return;
            }
            foreach (DatabaseObject table in tables)
            {
                Console.WriteLine($"Table: {table.NameWithSchemaBrackets}");
                if (_dependencies[table].Children.Count == 0)
                {
                    Console.WriteLine("No foreign keys");
                }
                else
                {
                    bool OK;
                    string explanation;
                    StringBuilder hierarchyFK = new StringBuilder();
                    IList<Tuple<DependencyEdge, string>> keys = Optimizer.Instance.CheckDeleteCascade(_dependencies[table], out OK, out explanation);
                    foreach (Tuple<DependencyEdge, string> key in keys)
                    {
                        hierarchyFK.AppendLine($"{new string(' ', key.Item2.Count(ch => ch == '.'))}{key.Item2}: {key.Item1.Name} - {ForeignKey.DeleteActionToString(key.Item1.ForeignKey.DeleteAction)}");
                    }
                    if (OK)
                    {
                        Console.Write("OK");
                    }
                    else
                    {
                        Console.Write("NOK");
                    }
                    if (explanation.Length > 0)
                    {
                        Console.WriteLine($" - {explanation}");
                    }
                    if (hierarchyFK.Length > 0)
                    {
                        Console.Write(hierarchyFK);
                    }
                }
                Console.WriteLine(SEPARATOR);
                Console.WriteLine();
            }
        }

        private static void ProcessFKMissingIndexCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return;
            }
            try
            {
                IList<ForeignKeyMissingIndexes> keys = _databaseOperations.GetForeignKeysWithoutIndexes(tables);
                if (keys.Count > 0)
                {
                    keys = keys.OrderBy(k => k.Table.NameWithSchema).ToList();
                    foreach (ForeignKeyMissingIndexes key in keys)
                    {
                        Console.WriteLine($"Table: {key.Table.NameWithSchemaBrackets}");
                        Console.WriteLine($"Name: {key.Name}");
                        Console.WriteLine($"Columns: {key.ColumnsText}");
                        Console.WriteLine($"Missing columns in index: {key.MissingIndexColumnsText}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Every foreign key has index");
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine("Error: Foreign keys without indexes cannot be loaded");
                return;
            }
        }

        private static void ProcessFKNotUsedCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                return;
            }
            try
            {
                ISet<DatabaseObject> tablesWithoutForeignKeys = new HashSet<DatabaseObject>(tables);
                foreach (DatabaseObject table in tables)
                {
                    foreach (DependencyEdge edge in _optimizer.DependencyGraph[table].ChildEdges)
                    {
                        Console.WriteLine($"Table: {edge.Parent.DbObject.NameWithSchemaBrackets}");
                        Console.WriteLine($"Foreign key: {edge.ForeignKey.Name}");
                        tablesWithoutForeignKeys.Remove(edge.Parent.DbObject);
                        DataTable values;
                        try
                        {
                            values = _databaseOperations.ValuesNotUsedInForeignKeys(edge);
                        }
                        catch (DatabaseException exc)
                        {
                            Debug.WriteLine(exc);
                            Console.WriteLine("Error: Values cannot be loaded");
                            return;
                        }
                        if (values.Rows.Count > 0)
                        {
                            Console.WriteLine("Data:");
                            foreach (DataRow row in values.Rows)
                            {
                                foreach (DataColumn column in values.Columns)
                                {
                                    Console.WriteLine($"{column.ColumnName}: {row[column]}");
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Every value of foreign key is used");
                        }
                        Console.WriteLine(SEPARATOR);
                        Console.WriteLine();
                    }
                }
                if (tablesWithoutForeignKeys.Count == tables.Count)
                {
                    Console.WriteLine("All tables are without foreign keys");
                    return;
                }
                else if (tablesWithoutForeignKeys.Count > 0)
                {
                    Console.WriteLine($"These tables are without foreign keys: {string.Join(", ", tablesWithoutForeignKeys)}");
                    return;
                }
            }
            catch (DatabaseException exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine("Error: Foreign keys without indexes cannot be loaded");
                return;
            }
        }

        private static void ProcessDependencyCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine(NO_TABLE_SELECTED);
                return;
            }
            IList<string> errors;
            IList<DatabaseObject> tables = GetTablesFromArgs(args, out errors);
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            else if (tables != null)
            {
                foreach (DatabaseObject table in tables)
                {
                    _dependencies.PrintDependencies(table);
                    Console.WriteLine();
                }
            }
            else
            {
                _dependencies.PrintAllDependencies();
            }
            Console.WriteLine();
        }

        private static void ProcessDependencyExportCommand(string args)
        {
            if (args == "")
            {
                Console.WriteLine("Error: No file specified, export must be saved to a file");
                return;
            }
            try
            {
                using (StreamWriter file = new StreamWriter(args))
                {
                    file.Write(_dependencies.ToDotFormat());
                    Console.WriteLine("Export saved");
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                Console.WriteLine($"Error: Export cannot be saved - {exc.Message}");
            }
            Console.WriteLine();
        }

        private static string GetConnectionString(bool ignoreStoredConnectionString = false)
        {
            if (ignoreStoredConnectionString || Database.StoredConnectionString == "")
            {
                string connectionString;
                Console.Write("Do you want to connect with connection string? (y/n) ");
                if (Console.In.ReadLine() == "y")
                {
                    Console.Write("Connection string: ");
                    connectionString = Console.In.ReadLine();
                }
                else
                {
                    Console.WriteLine("Fill values, which are needed for succesful connection. If you do not want to fill some value, leave it empty (just press enter).");
                    Console.Write("Data source: ");
                    string dataSource = Console.In.ReadLine();
                    Console.Write("Database: ");
                    string database = Console.In.ReadLine();
                    Console.Write("User: ");
                    string user = Console.In.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.In.ReadLine();
                    connectionString = Database.Instance.GenerateConnectionString(dataSource, database, user, password);

                }
                Console.Write(connectionString.ToLower().Contains("password=")
                        ? "Do you want to store connection string for later usage? Connection string contains password, use it on your own risk. (y/n) "
                        : "Do you want to store connection string for later usage? (y/n) ");
                if (Console.In.ReadLine() == "y")
                {
                    Database.StoredConnectionString = connectionString;
                }
                return connectionString;
            }
            Console.Write("Do you want to use stored connection string? (y/n) ");
            if (Console.In.ReadLine() == "y")
            {
                return Database.StoredConnectionString;
            }
            Console.Write("Do you want to remove stored connection string? (y/n) ");
            if (Console.In.ReadLine() == "y")
            {
                Database.StoredConnectionString = "";
            }
            return GetConnectionString(true);
        }
    }
}

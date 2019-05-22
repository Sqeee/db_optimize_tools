using System.Collections.Generic;
using System.Linq;

namespace DB_Optimize.DB
{
    public class ForeignKey
    {
        public enum DeleteActions
        {
            NoAction = 0,
            Cascade,
            SetNull,
            SetDefault
        }

        [System.ComponentModel.Browsable(false)]
        public int ID { get; }

        public DatabaseObject Table { get; }

        public string Name { get; }

        [System.ComponentModel.Browsable(false)]
        public IList<TableColumn> Columns { get; }

        public string ColumnsText => GenerateColumnsText(Columns);

        [System.ComponentModel.Browsable(false)]
        public DeleteActions DeleteAction { get; }

        [System.ComponentModel.Browsable(false)]
        public bool CanBeNull => Columns.All(tc => tc.CanBeNull);

        [System.ComponentModel.Browsable(false)]
        public bool HasDefaultValue => Columns.All(tc => tc.DefaultValue != null || tc.CanBeNull);

        public ForeignKey(int id, DatabaseObject table, string name, DeleteActions deleteAction, IList<TableColumn> columns)
        {
            ID = id;
            Table = table;
            Name = name;
            DeleteAction = deleteAction;
            Columns = columns;
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
            return ((ForeignKey)obj).ID == ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            return Name;
        }

        public static string DeleteActionToString(DeleteActions deleteAction)
        {
            switch (deleteAction)
            {
                case DeleteActions.NoAction:
                    return "No action";
                case DeleteActions.Cascade:
                    return "Cascade";
                case DeleteActions.SetDefault:
                    return "Set default value";
                case DeleteActions.SetNull:
                    return "Set null";
                default:
                    return "Unknown type";
            }
        }

        protected string GenerateColumnsText(IList<TableColumn> columns)
        {
            if (columns.Count == 1)
            {
                return columns[0].Name;
            }
            return columns.Count > 1 ? $"({string.Join(",", columns)})" : "-";
        }
    }
}

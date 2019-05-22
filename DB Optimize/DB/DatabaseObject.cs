namespace DB_Optimize.DB
{
    public class DatabaseObject
    {
        public enum DatabaseObjectType
        {
            Table,
            View,
            Function,
            Constraint,
            Index
        }

        public int ID { get; }

        public string Schema { get; }

        public string Name { get; }

        public string NameWithSchema => $"{Schema}.{Name}";

        public string NameWithSchemaBrackets => $"[{Schema}].[{Name}]";

        public DatabaseObjectType Type { get; }

        public DatabaseObject(int id, string schema, string name, DatabaseObjectType type)
        {
            ID = id;
            Schema = schema;
            Name = name;
            Type = type;
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
            return ((DatabaseObject)obj).ID == ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            return NameWithSchema;
        }
    }
}

namespace DB_Optimize.DB
{
    public class Index
    {
        public string Name { get; }

        public string TypeName { get; }

        public Index(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;
        }
    }
}

using System.Collections.Generic;
using DB_Optimize.DB;

namespace DB_Optimize.Dependency
{
    public class DependencyEdge
    {
        public string Name => ForeignKey.Name;

        public DependencyNode Parent { get; }

        public DependencyNode Child { get; }

        public ForeignKey ForeignKey { get; }

        public IList<DependencyColumn> Columns { get; }

        public DependencyEdge(ForeignKey foreignKey, DependencyNode parent, TableColumn parentColumn, DependencyNode child, TableColumn childColumn)
        {
            ForeignKey = foreignKey;
            Parent = parent;
            Child = child;
            Columns = new List<DependencyColumn> { new DependencyColumn(parentColumn, childColumn) };
        }

        public void AddDependencyColumn(TableColumn parentColumn, TableColumn childColumn)
        {
            Columns.Add(new DependencyColumn(parentColumn, childColumn));
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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
            return ((DependencyEdge)obj).Name.Equals(Name);
        }

        public override string ToString()
        {
            return $"{Parent} -> {Child}";
        }
    }
}

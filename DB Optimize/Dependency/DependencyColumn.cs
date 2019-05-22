using DB_Optimize.DB;

namespace DB_Optimize.Dependency
{
    public class DependencyColumn
    {
        public TableColumn ParentColumn { get; }

        public TableColumn ChildColumn { get; }

        public DependencyColumn(TableColumn parentColumn, TableColumn childColumn)
        {
            ParentColumn = parentColumn;
            ChildColumn = childColumn;
        }
    }
}

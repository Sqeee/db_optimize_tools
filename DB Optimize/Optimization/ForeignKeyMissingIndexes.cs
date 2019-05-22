
using System.Collections.Generic;
using DB_Optimize.DB;

namespace DB_Optimize.Optimization
{
    public class ForeignKeyMissingIndexes : ForeignKey
    {
        [System.ComponentModel.Browsable(false)]
        public IList<TableColumn> MissingIndexColumns { get; }

        public string MissingIndexColumnsText => GenerateColumnsText(MissingIndexColumns);

        public ForeignKeyMissingIndexes(ForeignKey FK, IList<TableColumn> missingIndexColumns) : base(FK.ID, FK.Table, FK.Name, FK.DeleteAction, FK.Columns)
        {
            MissingIndexColumns = missingIndexColumns;
        }

        public ForeignKeyMissingIndexes(int id, DatabaseObject table, string name, IList<TableColumn> columns, IList<TableColumn> missingIndexColumns) : base(id, table, name, DeleteActions.NoAction, columns)
        {
            MissingIndexColumns = missingIndexColumns;
        }
    }
}

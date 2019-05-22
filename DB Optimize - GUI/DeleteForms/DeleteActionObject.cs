using DB_Optimize.DB;

namespace DB_Optimize.GUI.DeleteForms
{
    class DeleteActionObject
    {
        public ForeignKey.DeleteActions? DeleteAction { get; }

        public bool IsOriginalAction { get; }

        public DeleteActionObject(ForeignKey.DeleteActions? action, bool isOriginalAction)
        {
            DeleteAction = action;
            IsOriginalAction = isOriginalAction;
        }

        public override string ToString()
        {
            if (IsOriginalAction)
            {
                return $"Original action{(DeleteAction != null ? " - " : "")}{DeleteActonText()}";
            }
            return DeleteActonText();
        }

        private string DeleteActonText()
        {
            switch (DeleteAction)
            {
                case null:
                    return "";
                default:
                    return ForeignKey.DeleteActionToString(DeleteAction.Value);
            }
        }
    }
}

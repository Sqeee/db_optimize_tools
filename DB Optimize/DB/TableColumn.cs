using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB_Optimize.DB
{
    public class TableColumn
    {
        public string Name { get; }
        public string DefaultValue { get;  }
        public bool CanBeNull { get; }
        public string Type { get; }
        public string TypeWithLength { get; }
        public int Length { get; }
        public int MaxLength { get; }
        public bool IsUserType { get; }
        public bool IsUnicode { get; }
        public bool IsForeignKey { get; }

        public TableColumn(string name, string defaultValue, bool canBeNull, string type, int length, bool isUserType, bool isForeignKey)
        {
            Name = name;
            DefaultValue = defaultValue;
            CanBeNull = canBeNull;
            Type = type;
            MaxLength = -1;
            if (DataTypeIsChar() || DataTypeIsBinary())
            {
                MaxLength = DataTypeConstants.MAX_LENGTH_IN_BYTES;
                if (DataTypeConstants.DataTypeIsUnicodeChar(type)) // Need 2 bytes per char
                {
                    IsUnicode = true;
                    length /= 2;
                    MaxLength /= 2;
                }
                if (length <= 0)
                {
                    TypeWithLength = $"{type}(MAX)";
                }
                else
                {
                    TypeWithLength = $"{type}({length})";
                }
            }
            else
            {
                TypeWithLength = type;
            }
            Length = length;
            IsUserType = isUserType;
            IsForeignKey = isForeignKey;
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
            return ((TableColumn)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public bool DataTypeIsChar()
        {
            return DataTypeConstants.DataTypeIsChar(Type);
        }

        public bool DataTypeIsBinary()
        {
            return DataTypeConstants.DataTypeIsBinary(Type);
        }

        public bool DataTypeIsInteger()
        {
            return DataTypeConstants.DataTypeIsInteger(Type);
        }
    }
}

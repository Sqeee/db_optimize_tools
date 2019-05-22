using System.Linq;

namespace DB_Optimize.DB
{
    public static class DataTypeConstants
    {
        public const string INT = "int";
        public const string BIGINT = "bigint";
        public const string SMALLINT = "smallint";
        public const string TINYINT = "tinyint";
        public const string DATE = "date";
        public const string DATETIME = "datetime";
        public const string CHAR = "char";
        public const string VARCHAR = "varchar";
        public const string NCHAR = "nchar";
        public const string NVARCHAR = "nvarchar";
        public const string TEXT = "text";
        public const string NTEXT = "ntext";
        public const string BINARY = "binary";
        public const string VARBINARY = "varbinary";
        public const string HIERARCHYID = "hierarchyid";
        public const string DECIMAL = "decimal";
        public const string NUMERIC = "numeric";
        public const string FLOAT = "float";
        public const string REAL = "real";
        public const string MONEY = "money";
        public const string SMALL_MONEY = "smallmoney";

        public const int MIN_LENGTH_IN_BYTES = 1;
        public const int MAX_LENGTH_IN_BYTES = 8000;
        public const int TINYINT_MIN_VALUE = 0;
        public const int TINYINT_MAX_VALUE = 255;
        public const int SMALLINT_MIN_VALUE = -32768;
        public const int SMALLINT_MAX_VALUE = 32767;
        public const int INT_MIN_VALUE = -2147483648;
        public const int INT_MAX_VALUE = 2147483647;
        public const long BIGINT_MIN_VALUE = -9223372036854775808;
        public const long BIGINT_MAX_VALUE = 9223372036854775807;

        private static readonly string[] UNICODE_CHARACTER_TYPES = { NCHAR, NVARCHAR, NTEXT };
        private static readonly string[] BINARY_TYPES = {BINARY, VARBINARY};
        private static readonly string[] INT_TYPES = {TINYINT, SMALLINT, INT, BIGINT};
        private static readonly string[] CHAR_TYPES = {NCHAR, NVARCHAR, CHAR, VARCHAR};
        private static readonly string[] FLOATING_POINT_TYPES = {DECIMAL, NUMERIC, FLOAT, REAL, MONEY, SMALL_MONEY};

        public static bool DataTypeIsChar(string type)
        {
            return CHAR_TYPES.Any(t => t == type);
        }

        public static bool DataTypeIsBinary(string type)
        {
            return BINARY_TYPES.Any(t => t == type);
        }

        public static bool DataTypeIsInteger(string type)
        {
            return INT_TYPES.Any(t => t == type);
        }

        public static bool DataTypeIsFloatingPoint(string type)
        {
            return FLOATING_POINT_TYPES.Any(t => t == type);
        }

        public static bool DataTypeIsUnicodeChar(string type)
        {
            return UNICODE_CHARACTER_TYPES.Any(t => t == type);
        }

        public static bool DataTypeIsUnicodeCharPrefix(string type)
        {
            return UNICODE_CHARACTER_TYPES.Any(type.Contains);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace DB_Optimize.DB
{
    public class Database
    {
        private const string PROPERTY_CONNECTION_STRING = "ConnectionString";
        private string _collation = null;

        public string Collation
        {
            get
            {
                if (!_isConnected)
                {
                    return null;
                }
                if (_collation == null)
                {
                    try
                    {
                        using (SqlDataReader resultReader = ExecuteSelect($"SELECT sys.databases.collation_name FROM sys.databases WHERE sys.databases.name='{DatabaseName}';"))
                        {
                            int columnCollationNameOrdinal = resultReader.GetOrdinal("collation_name");
                            while (resultReader.Read())
                            {
                                _collation = resultReader.GetString(columnCollationNameOrdinal);
                            }
                            return _collation;
                        }
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc);
                        throw new DatabaseException("List of collations cannot be loaded", exc);
                    }
                }
                else
                {
                    return _collation;
                }
            }
        }

        public static Database Instance { get; } = new Database();

        public static string StoredConnectionString
        {
            get
            {
                return Properties.Settings.Default[PROPERTY_CONNECTION_STRING].ToString() == "" ? "" : DataProtection.Decrypt(Properties.Settings.Default[PROPERTY_CONNECTION_STRING].ToString());
            }
            set
            {
                Properties.Settings.Default[PROPERTY_CONNECTION_STRING] = DataProtection.Encrypt(value);
                Properties.Settings.Default.Save();
            }
        }

        public string DatabaseName => Connection.Database;

        public SqlConnection Connection { get; } = new SqlConnection();

        private bool _isConnected;
        private string _errorMessage;

        private Database()
        {
        }

        public string ErrorMessage => _errorMessage;

        public bool IsConnected => _isConnected;

        public SqlDataReader ExecuteSelect(string selectQuery)
        {
            if (!_isConnected)
            {
                return null;
            }
            SqlCommand command = new SqlCommand(selectQuery, Connection);
            return command.ExecuteReader();
        }

        public int? ExecuteScalarSelect(string selectQuery)
        {
            if (!_isConnected)
            {
                return null;
            }
            SqlCommand command = new SqlCommand(selectQuery, Connection);
            return (int?)command.ExecuteScalar();
        }

        public int ExecuteNonResultQuery(string query)
        {
            if (!_isConnected)
            {
                return -1;
            }
            SqlCommand command = new SqlCommand(query, Connection);
            command.CommandTimeout = 0;
            return command.ExecuteNonQuery();
        }

        public bool Connect(string dataSource, string database, string userId, string password)
        {
            return Connect(GenerateConnectionString(dataSource, database, userId, password));
        }

        public bool Connect(string connectionString)
        {
            Disconnect();
            _collation = null;
            Connection.ConnectionString = connectionString;
            try
            {
                Connection.Open();
                _isConnected = true;
                _errorMessage = "";
            }
            catch (InvalidOperationException exc)
            {
                _errorMessage = exc.Message;
                Debug.WriteLine(exc);
            }
            catch (SqlException exc)
            {
                _errorMessage = exc.Message;
                Debug.WriteLine(exc);
            }
            return _isConnected;
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                Connection.Close();
                _isConnected = false;
            }
        }

        public string GenerateConnectionString(string dataSource, string database, string userId, string password)
        {
            StringBuilder connectionString = new StringBuilder();
            connectionString.Append($"data source={dataSource};");
            connectionString.Append($"database={database};");
            if (userId != "")
            {
                connectionString.Append($"user id={userId};");
            }
            if (password != "")
            {
                connectionString.Append($"password={password};");
            }
            return connectionString.ToString();
        }
    }
}

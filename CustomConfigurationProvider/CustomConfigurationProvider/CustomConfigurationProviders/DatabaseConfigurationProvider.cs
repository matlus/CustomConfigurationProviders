using System;
using System.Data;
using System.Data.SqlClient;

namespace CustomConfigurationProviders
{
    internal sealed class DatabaseConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider, IDisposable
    {
        private bool _disposed;
        private readonly IDbConnection _dbConnection;

        public DatabaseConfigurationProvider(string connectionString)
        {
            _dbConnection = SqlClientFactory.Instance.CreateConnection();
            _dbConnection.ConnectionString = connectionString;
        }

        public override void Load()
        {
            _dbConnection.Open();
            IDataReader dbDataReader = null;
            try
            {
                var dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "SELECT [Section], [Key], [Value] FROM ConfigurationSource";
                dbDataReader = dbCommand.ExecuteReader();

                while (dbDataReader.Read())
                {
                    Data.Add($"{(string)dbDataReader[0]}:{(string)dbDataReader[1]}", (string)dbDataReader[2]);
                }

                dbDataReader.Close();
            }
            finally
            {
                if (dbDataReader != null)
                {
                    dbDataReader.Dispose();
                }

                _dbConnection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key parameter is in the form section:key.
        /// The "section" part could be the "AppSettings" section for example</param>
        /// <param name="value">The value is the value of the key</param>
        public override void Set(string key, string value)
        {
            _dbConnection.Open();
            IDbTransaction dbTransaction = null;
            try
            {
                var dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "INSERT INTO ConfigurationSource ([Section], [Key], [Value]) VALUES(@Section, @Key, @Value)";

                var (sectionName, keyName) = EnsureKeyformatIsCorrect(key);
                AddCommandParameters(dbCommand, sectionName, keyName, value);

                dbTransaction = _dbConnection.BeginTransaction();
                dbCommand.Transaction = dbTransaction;
                dbCommand.ExecuteNonQuery();
                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
            finally
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Dispose();
                }

                _dbConnection.Close();
            }
        }

        private static (string sectionName, string keyName) EnsureKeyformatIsCorrect(string key)
        {
            var keyParts = key.Split(':', StringSplitOptions.RemoveEmptyEntries);

            if (keyParts.Length != 2)
            {
                throw new ArgumentException("The parameter named key, Must be in the form: section:key.", key);
            }

            return (sectionName: keyParts[0], keyName: keyParts[1]);
        }

        private static void AddCommandParameters(IDbCommand dbCommand, string section, string key, string value)
        {
            var sectionParameter = dbCommand.CreateParameter();
            sectionParameter.DbType = DbType.String;
            sectionParameter.ParameterName = "@Section";
            sectionParameter.Value = section;
            dbCommand.Parameters.Add(sectionParameter);

            var keyParameter = dbCommand.CreateParameter();
            keyParameter.DbType = DbType.String;
            keyParameter.ParameterName = "@Key";
            keyParameter.Value = key;
            dbCommand.Parameters.Add(keyParameter);

            var valueParameter = dbCommand.CreateParameter();
            valueParameter.DbType = DbType.String;
            valueParameter.ParameterName = "@Value";
            valueParameter.Value = value;
            dbCommand.Parameters.Add(valueParameter);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _dbConnection.Dispose();
            }

            _disposed = true;
        }
    }
}

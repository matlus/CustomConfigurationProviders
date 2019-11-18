using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProviders
{
    internal sealed class DatabaseConfigurationSource : IConfigurationSource
    {
        private readonly string _connectionString;

        public DatabaseConfigurationSource(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DatabaseConfigurationProvider(_connectionString);
        }
    }
}

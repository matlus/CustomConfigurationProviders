using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConfigurationProvider.Tests")]

namespace CustomConfigurationProviders
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        static void Main(string[] args)
        {
            /*
             *   The commented code below is used only to populate the Configuration Store.
             *   One can manually add data records to the required table as well
             */
            ////const string appSettingsSection = "AppSettings";
            ////using (var databaseConfigurationProvider = new DatabaseConfigurationProvider(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=ConfigurationStore;Integrated Security=True;TrustServerCertificate=True;"))
            ////{
            ////    databaseConfigurationProvider.Set($"{appSettingsSection}:EmailTemplatesPath", "Templates\\Emails");
            ////    databaseConfigurationProvider.Set($"{appSettingsSection}:PaymentGatewayServiceUrl", "http://www.matlus.com/paymentService");
            ////    databaseConfigurationProvider.Set($"{appSettingsSection}:FiscalYearStart", "20/15/2000");
            ////    databaseConfigurationProvider.Set($"{appSettingsSection}:NotifyOnUpload", "true");
            ////    databaseConfigurationProvider.Set($"{appSettingsSection}:MyDb", "Data Source=(localdb)\\ProjectsV12;Initial Catalog=Ooblx;Integrated Security=True");
            ////}

            var configurationProvider = new ConfigurationProvider();
            Console.WriteLine(configurationProvider.EmailTemplatesPath);
            Console.WriteLine(configurationProvider.PaymentGatewayServiceUrl);
            Console.WriteLine(configurationProvider.FiscalYearStart.ToLongDateString());
            Console.WriteLine(configurationProvider.NotifyOnUpload);

            var dbConnectionInformation = configurationProvider.DbConnectionInformation;
            Console.WriteLine(dbConnectionInformation.ConnectionStringName);
            Console.WriteLine(dbConnectionInformation.ConnectionString);
            Console.WriteLine(dbConnectionInformation.ProviderName);
        }
    }
}

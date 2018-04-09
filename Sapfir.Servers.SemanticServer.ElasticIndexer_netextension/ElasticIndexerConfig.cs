using System;
using System.Collections.Generic;
using System.Globalization;
using ConfigLibrary;

namespace Sapfir.Servers.SemanticServer.ElasticIndexer_netextension
{
    internal class ElasticSearchConfig
    {
        public static string ServerAddress = string.Empty;

        public static int ServerPort;
        public static CultureInfo CultureInfo = new CultureInfo("en-US");


        private static readonly Dictionary<string, Action<string>> parametersDictionary = new Dictionary
            <string, Action<string>>
            {
                {"server.port", SetServerPort},
                {"server.address", SetServerAddress}
            };

        private static void SetServerAddress(string serverAddress)
        {
            ServerAddress = serverAddress;
        }

        private static void SetServerPort(string serverPort)
        {
            int.TryParse(serverPort, out ServerPort);
        }


        public static void Read()
        {
            //config file must be in  directory whis the program
            const string fileName = "ElasticIndexer.properties";
            ConfirReader.ReadConfigurationFile(fileName, parametersDictionary);
        }
    }
}
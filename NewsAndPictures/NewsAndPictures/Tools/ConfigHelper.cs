using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace NewsAndPictures.Tools
{
    public class ConfigHelper
    {
        private static string ZONE_CONNECTION_NAME = "sqlZoneConnString";
        private static string SSZGEN_CONNECTION_NAME = "sqlSSZConnString";

        public static string getConfigByName(string keyName)
        {
            string value = null;
            if (ConfigurationManager.AppSettings[keyName] != null)
            {
                value = ConfigurationManager.AppSettings[keyName];
            }
            return value;
        }

        public static string getZoneSQLConnection()
        {
            string value = null;
            if (ConfigurationManager.ConnectionStrings[ZONE_CONNECTION_NAME] != null && !string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[ZONE_CONNECTION_NAME].ConnectionString))
            {
                value = ConfigurationManager.ConnectionStrings[ZONE_CONNECTION_NAME].ConnectionString;
            }
            return value;
        }

        public static string getSSZGeneralSQLConnection()
        {
            string value = null;
            if (ConfigurationManager.ConnectionStrings[SSZGEN_CONNECTION_NAME] != null && !string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[SSZGEN_CONNECTION_NAME].ConnectionString))
            {
                value = ConfigurationManager.ConnectionStrings[SSZGEN_CONNECTION_NAME].ConnectionString;
            }
            return value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAndPictures.Tools
{
    public class DBTools
    {
        public static int? convertToInt(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }

            return (int)value;
        }

        public static bool? convertToBool(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }

            return (bool)value;
        }

        public static string convertToString(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }

            return (string)value;
        }

        public static object convertDBNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }
    }
}
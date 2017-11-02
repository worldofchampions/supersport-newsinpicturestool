using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;

namespace NewsAndPictures.DataObejcts
{
    public class Credit
    {
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public static List<Credit> getCredits()
        {
            List<Credit> credits = new List<Credit>();
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select Id, Name from zonecredits order by Name asc";

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Credit credit = new Credit();
                            credit.Id = (int)rdr["Id"];
                            credit.Name = DBTools.convertToString(rdr["Name"]);
                            credits.Add(credit);
                        }
                    }
                }
            }
            return credits;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using NewsAndPictures.Tools;

namespace NewsAndPictures.DataObejcts
{
    public class Sport
    {
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public static List<Sport> getSports()
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select ID, SportName from zonesports where SportName!='All' order by SportName asc";
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Sport> sports = new List<Sport>();
                        while (rdr.Read())
                        {
                            Sport sport = new Sport();
                            sport.Id = (int)rdr["ID"];
                            sport.Name = DBTools.convertToString(rdr["SportName"]);
                            sports.Add(sport);
                        }
                        return sports;
                    }
                }
            }
        }
    }
}
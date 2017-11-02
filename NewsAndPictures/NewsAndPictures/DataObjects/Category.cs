using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;

namespace NewsAndPictures.DataObejcts
{
    public class Category
    {
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public static List<Category> getZoneCategories(int SportId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select ID, Name from ZoneCategories where Sport = @SportId order by Name asc";
                    cmd.Parameters.Add("@SportId", SqlDbType.Int).Value = SportId;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Category> categories = new List<Category>();
                        while (rdr.Read())
                        {
                            Category category = new Category();
                            category.Id = (int)rdr["ID"];
                            category.Name = DBTools.convertToString(rdr["Name"]);
                            categories.Add(category);
                        }

                        return categories;
                    }
                }
            }
        }

        public static List<Category> geSSZGenCategories(int sportId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getSSZGeneralSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select ID, Name from broadBandVideoCategories 
                    where Sport = @Sport 
                    order by Name asc";

                    cmd.Parameters.Add("@Sport", SqlDbType.Int).Value = sportId;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Category> categories = new List<Category>();
                        while (rdr.Read())
                        {
                            Category category = new Category();
                            category.Id = (int)rdr["ID"];
                            category.Name = DBTools.convertToString(rdr["Name"]);
                            categories.Add(category);
                        }

                        return categories;
                    }
                }
            }
        }
    }
}
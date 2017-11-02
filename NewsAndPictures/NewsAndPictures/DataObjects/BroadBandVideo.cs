using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;

namespace NewsAndPictures.DataObejcts
{
    public class BroadBandVideo
    {
        public int Id
        { get; set; }

        public string Name
        { get; set; }

        public string Description
        { get; set; }

        public string GroupedSelectId
        { get; set; }


        public static List<BroadBandVideo> getVideoByCategoryId(int categoryid)
        {
            using(SqlConnection cn = new SqlConnection(ConfigHelper.getSSZGeneralSQLConnection()))
            {
                cn.Open();
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select id, CAST(id AS nvarchar(12)) + ' ' + name as name, [description], CAST(id AS nvarchar(12)) +'|'+[description] as groupedId from broadBandVideo where category = @category order by created desc";
                    cmd.Parameters.Add("@category", SqlDbType.Int).Value = categoryid;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<BroadBandVideo> videos = new List<BroadBandVideo>();
                        while (rdr.Read())
                        {
                            BroadBandVideo video = new BroadBandVideo();
                            video.Id = (int)rdr["id"];
                            video.Name = DBTools.convertToString(rdr["name"]);
                            video.Description = DBTools.convertToString(rdr["description"]);
                            video.GroupedSelectId = DBTools.convertToString(rdr["groupedId"]);
                            videos.Add(video);
                        }

                        return videos;
                    }
                }
            }
        }
    }
}
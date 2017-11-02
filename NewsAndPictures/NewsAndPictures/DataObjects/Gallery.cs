using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;

namespace NewsAndPictures.DataObejcts
{
    public class Gallery
    {
        public int Id
        { get; set; }

        public int SportId
        { get; set; }

        public int CategoryId
        { get; set; }

        public int? ThumbImageId
        { get; set; }

        public int? MainContentId
        { get; set; }

        public int? BackgroundImageId
        { get; set; }

        public string ThumbImageName
        { get; set; }

        public string MainImageName
        { get; set; }

        public string BackgroundImageName
        { get; set; }

        public int? VideoId
        { get; set; }

        public string Title
        { get; set; }

        public string Synopsis
        { get; set; }

        public string Script
        { get; set; }

        public string Tag
        { get; set; }

        public bool Active
        { get; set; }

        public string FriendlyName
        { get; set; }

        public bool Featured
        { get; set; }

        public int? GalleryRank
        { get; set; }

        public DateTime DateCreated
        { get; set; }

        private static object locker = new object();

        public static Gallery GetGalleryById(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                    @"select g.id, 
	                        g.sportId, 
	                        g.categoryId, 
                            g.npGalleryContentThumbnailId, 
                            g.npGalleryContentMainContentId,
                            g.npGalleryContentBackgroundId,
	                        gc1.ImageName as thumb, 
	                        gc2.ImageName as mainImage, 
	                        gc2.videoId, 
                            gc3.ImageName as BackgroundImage,
	                        g.title, 
	                        g.synopsis, 
	                        g.active, 
	                        g.friendlyName, 
                            g.tag,
                            g.script,
	                        g.featured, 
	                        g.galleryRank,
                            g.dateCreated
                    from npGallery g 
                    left outer join npGalleryContent gc1 on gc1.id = g.npGalleryContentThumbnailId 
                    left outer join npGalleryContent gc2 on gc2.id = g.npGalleryContentMainContentId  
                    left outer join npGalleryContent gc3 on gc3.id = g.npGalleryContentBackgroundId
                    where g.id = @GalleryId 
                    order by g.galleryRank desc";
                    cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = id;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        Gallery gallery = null;
                        if (rdr.Read())
                        {
                            gallery = new Gallery();
                            gallery.Id = (int)rdr["id"];
                            gallery.SportId = (int)rdr["sportId"];
                            gallery.CategoryId = (int)rdr["categoryId"];
                            gallery.ThumbImageId = DBTools.convertToInt(rdr["npGalleryContentThumbnailId"]);
                            gallery.MainContentId = DBTools.convertToInt(rdr["npGalleryContentMainContentId"]);
                            gallery.ThumbImageName = DBTools.convertToString(rdr["thumb"]);
                            gallery.MainImageName = DBTools.convertToString(rdr["mainImage"]);
                            gallery.VideoId = DBTools.convertToInt(rdr["videoId"]);
                            gallery.Title = DBTools.convertToString(rdr["title"]);
                            gallery.Synopsis = DBTools.convertToString(rdr["synopsis"]);
                            gallery.Active = (bool)rdr["active"];
                            gallery.FriendlyName = DBTools.convertToString(rdr["friendlyName"]);
                            gallery.Featured = (bool)rdr["featured"];
                            gallery.GalleryRank = DBTools.convertToInt(rdr["galleryRank"]);
                            gallery.Script = DBTools.convertToString(rdr["script"]);
                            gallery.Tag = DBTools.convertToString(rdr["tag"]);
                            gallery.BackgroundImageId = DBTools.convertToInt(rdr["npGalleryContentBackgroundId"]);
                            gallery.BackgroundImageName = DBTools.convertToString(rdr["BackgroundImage"]);
                        }
                        return gallery;
                    }
                }
            }
        }

        public static List<Gallery> GetAllGalleries()
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select top 500 g.id, 
	                        g.sportId, 
	                        g.categoryId, 
                            g.npGalleryContentThumbnailId, 
                            g.npGalleryContentMainContentId,
                            g.npGalleryContentBackgroundId,
	                        gc1.ImageName as thumb, 
	                        gc2.ImageName as mainImage, 
	                        gc2.videoId, 
                            gc3.ImageName as BackgroundImage,
	                        g.title, 
	                        g.synopsis, 
	                        g.active, 
	                        g.friendlyName, 
                            g.tag,
                            g.script,
	                        g.featured, 
	                        g.galleryRank,
                            g.dateCreated
                    from npGallery g 
                    left outer join npGalleryContent gc1 on gc1.id = g.npGalleryContentThumbnailId 
                    left outer join npGalleryContent gc2 on gc2.id = g.npGalleryContentMainContentId  
                    left outer join npGalleryContent gc3 on gc3.id = g.npGalleryContentBackgroundId
                    order by (CASE WHEN g.galleryRank IS NULL then 1 ELSE 0 END) asc, g.galleryRank asc";

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Gallery> galleries = new List<Gallery>();
                        while (rdr.Read())
                        {
                            Gallery gallery = new Gallery();
                            gallery = new Gallery();
                            gallery.Id = (int)rdr["id"];
                            gallery.SportId = (int)rdr["sportId"];
                            gallery.CategoryId = (int)rdr["categoryId"];
                            gallery.ThumbImageId = DBTools.convertToInt(rdr["npGalleryContentThumbnailId"]);
                            gallery.MainContentId = DBTools.convertToInt(rdr["npGalleryContentMainContentId"]);
                            gallery.ThumbImageName = DBTools.convertToString(rdr["thumb"]);
                            gallery.MainImageName = DBTools.convertToString(rdr["mainImage"]);
                            gallery.VideoId = DBTools.convertToInt(rdr["videoId"]);
                            gallery.Title = DBTools.convertToString(rdr["title"]);
                            gallery.Synopsis = DBTools.convertToString(rdr["synopsis"]);
                            gallery.Active = (bool)rdr["active"];
                            gallery.FriendlyName = DBTools.convertToString(rdr["friendlyName"]);
                            gallery.Featured = (bool)rdr["featured"];
                            gallery.GalleryRank = DBTools.convertToInt(rdr["galleryRank"]);
                            gallery.DateCreated = (DateTime)rdr["dateCreated"];
                            gallery.Script = DBTools.convertToString(rdr["script"]);
                            gallery.Tag = DBTools.convertToString(rdr["tag"]);
                            gallery.BackgroundImageId = DBTools.convertToInt(rdr["npGalleryContentBackgroundId"]);
                            gallery.BackgroundImageName = DBTools.convertToString(rdr["BackgroundImage"]);

                            galleries.Add(gallery);
                        }
                        return galleries;
                    }
                }
            }
        }

        public static void UpdateGallery(Gallery item)
        {
            lock (locker)
            {
                item.GalleryRank = getGalleryRank(item.Id);
                if (item.Active == false && item.GalleryRank != null)
                {
                    item.GalleryRank = null;
                    UpdateGalleryRank(GetLastGalleryRank(), item.Id);
                }
                if (item.Active == true && item.GalleryRank == null)
                {
                    item.GalleryRank = GetLastGalleryRank();
                }

                using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"update npGallery set 
                                            sportId = @sportId, 
	                                        categoryId = @categoryId, 
                                            npGalleryContentThumbnailId = @npGalleryContentThumbnailId, 
                                            npGalleryContentMainContentId = @npGalleryContentMainContentId, 
	                                        title = @title, 
	                                        synopsis = @synopsis, 
	                                        active = @active, 
	                                        friendlyName = @friendlyName, 
	                                        featured = @featured, 
                                            dateModified = @dateModified,
                                            script = @script,
                                            tag = @tag,
                                            npGalleryContentBackgroundId = @npGalleryContentBackgroundId,
                                            galleryRank = @galleryRank                                      
                                            where id = @id";
                        cmd.Parameters.Add("@sportId", SqlDbType.Int).Value = DBTools.convertDBNull(item.SportId);
                        cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = DBTools.convertDBNull(item.CategoryId);
                        cmd.Parameters.Add("@npGalleryContentThumbnailId ", SqlDbType.Int).Value = DBTools.convertDBNull(item.ThumbImageId);
                        cmd.Parameters.Add("@npGalleryContentMainContentId", SqlDbType.Int).Value = DBTools.convertDBNull(item.MainContentId);
                        cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Title);
                        cmd.Parameters.Add("@synopsis", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Synopsis);
                        cmd.Parameters.Add("@active", SqlDbType.Bit).Value = DBTools.convertDBNull(item.Active);
                        cmd.Parameters.Add("@friendlyName", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.FriendlyName);
                        cmd.Parameters.Add("@featured", SqlDbType.Bit).Value = DBTools.convertDBNull(item.Featured);
                        cmd.Parameters.Add("@dateModified", SqlDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = DBTools.convertDBNull(item.Id);
                        cmd.Parameters.Add("@tag", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Tag);
                        cmd.Parameters.Add("@script", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Script);
                        cmd.Parameters.Add("@npGalleryContentBackgroundId", SqlDbType.Int).Value = DBTools.convertDBNull(item.BackgroundImageId);
                        cmd.Parameters.Add("@galleryRank", SqlDbType.Int).Value = DBTools.convertDBNull(item.GalleryRank);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static int AddGallery(Gallery item, int userId)
        {
            lock (locker)
            {
                using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"insert into npGallery
                                            (sportId, 
	                                        categoryId, 
                                            npGalleryContentThumbnailId, 
                                            npGalleryContentMainContentId,
                                            npGalleryContentBackgroundId, 
	                                        title, 
	                                        synopsis, 
	                                        active, 
	                                        friendlyName, 
                                            tag,
                                            script,
	                                        featured, 
	                                        galleryRank,
                                            dateModified,
                                            createdByUserId)
                                            values
                                            (@sportId, 
                                            @categoryId,
                                            @npGalleryContentThumbnailId, 
                                            @npGalleryContentMainContentId, 
                                            @npGalleryContentBackgroundId,
                                            @title, 
                                            @synopsis, 
                                            @active, 
                                            @friendlyName,
                                            @tag,
                                            @script,
                                            @featured,
                                            @galleryRank,                                             
                                            @dateModified, 
                                            @createdByUserId);
                                            Select SCOPE_IDENTITY()";
                        cmd.Parameters.Add("@sportId", SqlDbType.Int).Value = DBTools.convertDBNull(item.SportId);
                        cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = DBTools.convertDBNull(item.CategoryId);
                        cmd.Parameters.Add("@npGalleryContentThumbnailId ", SqlDbType.Int).Value = DBTools.convertDBNull(item.ThumbImageId);
                        cmd.Parameters.Add("@npGalleryContentMainContentId", SqlDbType.Int).Value = DBTools.convertDBNull(item.MainContentId);
                        cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Title);
                        cmd.Parameters.Add("@synopsis", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Synopsis);
                        cmd.Parameters.Add("@active", SqlDbType.Bit).Value = DBTools.convertDBNull(item.Active);
                        cmd.Parameters.Add("@friendlyName", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.FriendlyName);
                        cmd.Parameters.Add("@tag", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Tag);
                        cmd.Parameters.Add("@script", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Script);
                        cmd.Parameters.Add("@featured", SqlDbType.Bit).Value = DBTools.convertDBNull(item.Featured);
                        if (item.Active == false)
                        {
                            cmd.Parameters.Add("@galleryRank", SqlDbType.Int).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@galleryRank", SqlDbType.Int).Value = GetLastGalleryRank();
                        }
                        cmd.Parameters.Add("@dateModified", SqlDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("@createdByUserId", SqlDbType.Int).Value = DBTools.convertDBNull(userId);
                        cmd.Parameters.Add("@npGalleryContentBackgroundId", SqlDbType.Int).Value = DBTools.convertDBNull(item.BackgroundImageId);

                        return int.Parse(cmd.ExecuteScalar().ToString());
                    }
                }
            }
        }

        private static int GetLastGalleryRank()
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select top 1 galleryRank from npGallery order by galleryRank desc";
                    int? galleryRank = (int?)cmd.ExecuteScalar();

                    if (galleryRank.HasValue)
                    {
                        return galleryRank.Value + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }

        private static int? getGalleryRank(int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select galleryRank from npGallery 
                    where Id = @GalleryId";
                    cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = galleryId;

                    object rankReturned = cmd.ExecuteScalar();
                    if (rankReturned == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return (int?)rankReturned;
                    }
                }
            }
        }

        public static void UpdateGalleryRank(int newRank, int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select galleryRank from npGallery where id = @id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = galleryId;
                    int oldRank = (int)cmd.ExecuteScalar();
                    cmd.Parameters.Clear();


                    //from high rank to low rank
                    if (oldRank < newRank)
                    {
                        cmd.CommandText = "NPGALLERY_UpdateRankFromHighToLowRank";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@newvalue", newRank).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@oldvalue", oldRank).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@galleryItemId", galleryId).Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }

                    //from low rank to high
                    if (oldRank > newRank)
                    {
                        cmd.CommandText = "NPGALLERY_UpdateRankFromLowToHighRank";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@newvalue", newRank).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@oldvalue", oldRank).Direction = ParameterDirection.Input;
                        cmd.Parameters.AddWithValue("@galleryItemId", galleryId).Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<int> GetAllGalleryRanks()
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select distinct galleryRank from npGallery";
                    List<int> ranks = new List<int>();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr["galleryRank"] != DBNull.Value)
                            {
                                ranks.Add((int)rdr["galleryRank"]);
                            }
                        }
                    }

                    return ranks;
                }
            }
        }
    }
}
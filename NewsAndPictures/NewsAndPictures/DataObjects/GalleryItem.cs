using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;

namespace NewsAndPictures.DataObejcts
{
    public class GalleryItem
    {
        public int Id
        { get; set; }

        public int GalleryId
        { get; set; }

        public int? ThumbnailId
        { get; set; }

        public int? MainContentId
        { get; set; }

        public string ThumbnailName
        { get; set; }

        public string MainImageName
        { get; set; }

        public int? VideoId
        { get; set; }

        public string Title
        { get; set; }

        public string Synopsis
        { get; set; }

        public bool Active
        { get; set; }

        public int? ItemRank
        { get; set; }

        public DateTime DateCreated
        { get; set; }

        public string credit
        { get; set; }

        private static object locker = new object();

        public static GalleryItem getGalleryItemById(int id)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select gi.id, 
                                            gi.npGalleryId,
                                            gi.npGalleryContentThumbnailId, 
                                            gi.npGalleryContentMainContentId,
	                                        gc1.ImageName as thumb, 
	                                        gc2.ImageName as mainImage, 
	                                        gc2.videoId, 
	                                        gi.title, 
	                                        gi.synopsis, 
	                                        gi.active, 
	                                        gi.itemRank,
                                            gi.dateCreated,
                                            gi.credit
                                    from npGalleryItem gi 
                                    left outer join npGalleryContent gc1 on gc1.id = gi.npGalleryContentThumbnailId 
                                    left outer join npGalleryContent gc2 on gc2.id = gi.npGalleryContentMainContentId  
                                    where gi.id = @GalleryItemId";
                    cmd.Parameters.Add("@GalleryItemId", SqlDbType.Int).Value = id;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        GalleryItem galleryItem = null;
                        if (rdr.Read())
                        {
                            galleryItem = new GalleryItem();

                            galleryItem.Id = (int)rdr["id"];
                            galleryItem.GalleryId = (int)rdr["npGalleryId"];
                            galleryItem.ThumbnailId = DBTools.convertToInt(rdr["npGalleryContentThumbnailId"]);
                            galleryItem.MainContentId = DBTools.convertToInt(rdr["npGalleryContentMainContentId"]);
                            galleryItem.ThumbnailName = DBTools.convertToString(rdr["thumb"]);
                            galleryItem.MainImageName = DBTools.convertToString(rdr["mainImage"]);
                            galleryItem.VideoId = DBTools.convertToInt(rdr["videoId"]);
                            galleryItem.Title = DBTools.convertToString(rdr["title"]);
                            galleryItem.Synopsis = DBTools.convertToString(rdr["synopsis"]);
                            galleryItem.Active = (bool)rdr["active"];
                            galleryItem.ItemRank = DBTools.convertToInt(rdr["itemRank"]);
                            galleryItem.DateCreated = (DateTime)rdr["dateCreated"];
                            galleryItem.credit = DBTools.convertToString(rdr["credit"]);
                        }

                        return galleryItem;
                    }
                }
            }
        }

        public static List<GalleryItem> getGalleryItemsForGallery(int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select gi.id, 
                                            gi.npGalleryId,
                                            gi.npGalleryContentThumbnailId, 
                                            gi.npGalleryContentMainContentId,
	                                        gc1.ImageName as thumb, 
	                                        gc2.ImageName as mainImage, 
	                                        gc2.videoId, 
	                                        gi.title, 
	                                        gi.synopsis, 
	                                        gi.active, 
	                                        gi.itemRank,
                                            gi.dateCreated,
                                            gi.credit
                                    from npGalleryItem gi 
                                    left outer join npGalleryContent gc1 on gc1.id = gi.npGalleryContentThumbnailId 
                                    left outer join npGalleryContent gc2 on gc2.id = gi.npGalleryContentMainContentId  
                                    where gi.npGalleryId = @GalleryId 
                                    order by (CASE WHEN gi.itemRank IS NULL then 1 ELSE 0 END) asc, gi.itemRank asc";
                    cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = galleryId;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<GalleryItem> galleryItems = new List<GalleryItem>();
                        while (rdr.Read())
                        {
                            GalleryItem galleryItem = new GalleryItem();

                            galleryItem.Id = (int)rdr["id"];
                            galleryItem.GalleryId = (int)rdr["npGalleryId"];
                            galleryItem.ThumbnailId = DBTools.convertToInt(rdr["npGalleryContentThumbnailId"]);
                            galleryItem.MainContentId = DBTools.convertToInt(rdr["npGalleryContentMainContentId"]);
                            galleryItem.ThumbnailName = DBTools.convertToString(rdr["thumb"]);
                            galleryItem.MainImageName = DBTools.convertToString(rdr["mainImage"]);
                            galleryItem.VideoId = DBTools.convertToInt(rdr["videoId"]);
                            galleryItem.Title = DBTools.convertToString(rdr["title"]);
                            galleryItem.Synopsis = DBTools.convertToString(rdr["synopsis"]);
                            galleryItem.Active = (bool)rdr["active"];
                            galleryItem.ItemRank = DBTools.convertToInt(rdr["itemRank"]);
                            galleryItem.DateCreated = (DateTime)rdr["dateCreated"];
                            galleryItem.credit = DBTools.convertToString(rdr["credit"]);

                            galleryItems.Add(galleryItem);
                        }

                        return galleryItems;
                    }
                }
            }
        }

        public static void updateGalleryItem(GalleryItem item)
        {
            lock (locker)
            {
                item.ItemRank = getGalleryItemRank(item.Id);
                if (item.Active == false && item.ItemRank != null)
                {
                    item.ItemRank = null;
                    updateItemRank(getLastGalleryItemRank(item.GalleryId), item.Id, item.GalleryId);
                }
                if (item.Active == true && item.ItemRank == null)
                {
                    item.ItemRank = getLastGalleryItemRank(item.GalleryId);
                }

                using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"update npGalleryItem set 
                                            npGalleryContentThumbnailId = @npGalleryContentThumbnailId, 
                                            npGalleryContentMainContentId = @npGalleryContentMainContentId,  
	                                        title = @title, 
	                                        synopsis = @synopsis, 
	                                        active = @active, 
                                            dateModified = @dateModified,
                                            itemRank = @itemRank,
                                            credit = @credit
                                            where id = @id";
                        cmd.Parameters.Add("@npGalleryContentThumbnailId ", SqlDbType.Int).Value = DBTools.convertDBNull(item.ThumbnailId);
                        cmd.Parameters.Add("@npGalleryContentMainContentId", SqlDbType.Int).Value = DBTools.convertDBNull(item.MainContentId);
                        cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Title);
                        cmd.Parameters.Add("@synopsis", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Synopsis);
                        cmd.Parameters.Add("@active", SqlDbType.Bit).Value = DBTools.convertDBNull(item.Active);
                        cmd.Parameters.Add("@dateModified", SqlDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("@itemRank", SqlDbType.Int).Value = DBTools.convertDBNull(item.ItemRank);
                        cmd.Parameters.Add("@credit", SqlDbType.VarChar).Value = DBTools.convertDBNull(item.credit);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = item.Id;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static int addGalleryItem(GalleryItem item, int userId)
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
                        cmd.CommandText = @"insert into npGalleryItem  
                                            (npGalleryContentThumbnailId, 
                                            npGalleryContentMainContentId, 
                                            npGalleryId, 
                                            title, 
                                            synopsis, 
                                            itemRank, 
                                            active, 
                                            dateModified, 
                                            createdByUserId,
                                            credit)
                                            values
                                            (@npGalleryContentThumbnailId, 
                                            @npGalleryContentMainContentId, 
                                            @npGalleryId, 
                                            @title, 
                                            @synopsis, 
                                            @itemRank, 
                                            @active, 
                                            @dateModified, 
                                            @createdByUserId,
                                            @credit);
                                            Select SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";

                        cmd.Parameters.Add("@npGalleryContentThumbnailId ", SqlDbType.Int).Value = DBTools.convertDBNull(item.ThumbnailId);
                        cmd.Parameters.Add("@npGalleryContentMainContentId", SqlDbType.Int).Value = DBTools.convertDBNull(item.MainContentId);
                        cmd.Parameters.Add("@npGalleryId", SqlDbType.Int).Value = DBTools.convertDBNull(item.GalleryId);
                        cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Title);
                        cmd.Parameters.Add("@synopsis", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.Synopsis);
                        cmd.Parameters.Add("@active", SqlDbType.Bit).Value = DBTools.convertDBNull(item.Active);
                        cmd.Parameters.Add("@dateModified", SqlDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("@createdByUserId", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@credit", SqlDbType.VarChar).Value = DBTools.convertDBNull(item.credit);
                        if (item.Active == false)
                        {
                            cmd.Parameters.Add("@itemRank", SqlDbType.Int).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@itemRank", SqlDbType.Int).Value = getLastGalleryItemRank(item.GalleryId);
                        }

                        return int.Parse(cmd.ExecuteScalar().ToString());
                    }
                }
            }
        }

        private static int getLastGalleryItemRank(int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select top 1 itemRank from npGalleryItem 
                    where npGalleryId = @GalleryId 
                    order by itemRank desc";
                    cmd.Parameters.Add("@GalleryId", SqlDbType.Int).Value = galleryId;
                    int? intemRank = (int?)cmd.ExecuteScalar();

                    if (intemRank.HasValue)
                    {
                        return intemRank.Value + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }

        private static int? getGalleryItemRank(int galleryItemId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select itemRank from npGalleryItem 
                    where Id = @GalleryItemId";
                    cmd.Parameters.Add("@GalleryItemId", SqlDbType.Int).Value = galleryItemId;

                    return (int?)cmd.ExecuteScalar();
                }
            }
        }

        public static void updateItemRank(int newRank, int galleryItemId, int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select itemRank from npGalleryItem where id = @id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = galleryItemId;
                    int oldRank = (int)cmd.ExecuteScalar();
                    cmd.Parameters.Clear();

                    if (oldRank == newRank || newRank < 1)
                    {
                        return;
                    }

                    if (newRank > oldRank)
                    {
                        cmd.CommandText = "update npGalleryItem set itemRank = itemRank -1 where itemRank <= @newItemRank and itemRank > @oldRank and npGalleryId = @npGalleryId";
                        cmd.Parameters.Add("@newItemRank", SqlDbType.Int).Value = newRank;
                        cmd.Parameters.Add("@oldRank", SqlDbType.Int).Value = oldRank;
                        cmd.Parameters.Add("@npGalleryId", SqlDbType.Int).Value = galleryId;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    else
                    {
                        cmd.CommandText = "update npGalleryItem set itemRank = itemRank + 1 where itemRank >= @newItemRank and itemRank < @oldRank and npGalleryId = @npGalleryId";
                        cmd.Parameters.Add("@newItemRank", SqlDbType.Int).Value = newRank;
                        cmd.Parameters.Add("@oldRank", SqlDbType.Int).Value = oldRank;
                        cmd.Parameters.Add("@npGalleryId", SqlDbType.Int).Value = galleryId;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    cmd.CommandText = "update npGalleryItem set itemRank = @itemRank where id = @id";
                    cmd.Parameters.Add("@itemRank", SqlDbType.Int).Value = newRank;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = galleryItemId;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        public static List<int> GetAllGalleryItemRanks(int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select distinct itemRank from npGalleryItem where npGalleryId = @npGalleryId";
                    cmd.Parameters.Add("@npGalleryId", SqlDbType.Int).Value = galleryId;
                    List<int> ranks = new List<int>();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr["itemRank"] != DBNull.Value)
                            {
                                ranks.Add((int)rdr["itemRank"]);
                            }
                        }
                    }

                    return ranks;
                }
            }
        }
    }
}
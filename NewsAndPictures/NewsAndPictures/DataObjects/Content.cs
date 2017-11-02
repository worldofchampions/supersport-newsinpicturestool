using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using NewsAndPictures.Tools;
using System.Net;
using System.IO;

namespace NewsAndPictures
{
    public class Content
    {
        public int Id
        { get; set; }

        public string ContentType
        { get; set; }

        public string ContainingFolderName
        { get; set; }

        public string Name
        { get; set; }

        public string Description
        { get; set; }

        public int? videoId
        { get; set; }

        public static List<Content> getImagesForGallery(int galleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select id, ContainingFolderName, ImageName
                                            from npGalleryContent
                                            where ContainingFolderName = @ContainingFolder 
                                            and npGalleryContentTypeId = (select id from npGalleryContentType where typeName='image') 
                                            order by ImageName asc";
                    cmd.Parameters.Add("@ContainingFolder", SqlDbType.NVarChar).Value = galleryId.ToString();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Content> images = new List<Content>();
                        while (rdr.Read())
                        {
                            Content image = new Content();
                            image.Id = (int)rdr["id"];
                            image.ContainingFolderName = DBTools.convertToString(rdr["ContainingFolderName"]);
                            image.Name = DBTools.convertToString(rdr["ImageName"]);
                            image.ContentType = "image";
                            images.Add(image);
                        }

                        return images;
                    }
                }
            }
        }

        public static Content getContentById(int contentId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select gc.id, gc.ContainingFolderName, gc.ImageName, gc.videoId, gct.typeName
                                            from npGalleryContent gc
                                            inner join npGalleryContentType gct on gc.npGalleryContentTypeId = gct.id
                                            where gc.id = @id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = contentId.ToString();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        Content image = null;
                        if (rdr.Read())
                        {
                            image = new Content();
                            image.Id = (int)rdr["id"];
                            image.ContainingFolderName = DBTools.convertToString(rdr["ContainingFolderName"]);
                            image.Name = DBTools.convertToString(rdr["ImageName"]);
                            image.ContentType = DBTools.convertToString(rdr["typeName"]);
                            image.videoId = DBTools.convertToInt(rdr["videoId"]);
                        }

                        return image;
                    }
                }
            }
        }

        public static void deleteContentById(int contentId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Delete from npGalleryContent where id = @id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = contentId;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Content getVideoContentDataByContentId(int id)
        {
            Content video = new Content();

            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select videoid from npGalleryContent where id = @id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    video.Id = id;
                    video.videoId = (int)cmd.ExecuteScalar();
                }
            }

            using (SqlConnection cn = new SqlConnection(ConfigHelper.getSSZGeneralSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select name, description
                        from broadBandVideo
                        where id = @id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = video.videoId;
                    

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            video.Name = DBTools.convertToString(rdr["name"]);
                            video.Description = DBTools.convertToString(rdr["description"]);
                            video.ContentType = "video";
                        }
                    }

                    return video;
                }
            }
        }

        public static void updateGalleryContent(Content item)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"update npGalleryContent set 
                                            npGalleryContentTypeId = @npGalleryContentTypeId, 
                                            ContainingFolderName = @ContainingFolderName, 
                                            ImageName = @ImageName, 
                                            videoId = @videoId                                           
                                            where id = @id";
                    cmd.Parameters.Add("@npGalleryContentTypeId", SqlDbType.Int).Value = getContentTypeId(item.ContentType);
                    cmd.Parameters.Add("@ContainingFolderName", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.ContentType == "image" ? item.ContainingFolderName: null);
                    cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.ContentType == "image" ? item.Name : null);
                    cmd.Parameters.Add("@videoId", SqlDbType.Int).Value = DBTools.convertDBNull(item.ContentType.Equals("video", StringComparison.OrdinalIgnoreCase) ? item.videoId : null);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = item.Id;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int addGalleryContent(Content item)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"insert into npGalleryContent
                                            (npGalleryContentTypeId,  
                                            ContainingFolderName, 
                                            ImageName, 
                                            videoId)
                                            values
                                            (@npGalleryItemTypeId,
                                             @ContainingFolderName,
                                             @ImageName,
                                             @videoId);
                                            Select SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";

                    cmd.Parameters.Add("@npGalleryItemTypeId ", SqlDbType.Int).Value = getContentTypeId(item.ContentType);
                    cmd.Parameters.Add("@ContainingFolderName", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.ContentType.Equals("image", StringComparison.OrdinalIgnoreCase) ? item.ContainingFolderName : null);
                    cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = DBTools.convertDBNull(item.ContentType.Equals("image", StringComparison.OrdinalIgnoreCase) ? item.Name : null);
                    cmd.Parameters.Add("@videoId", SqlDbType.Int).Value = DBTools.convertDBNull(item.ContentType.Equals("video",StringComparison.OrdinalIgnoreCase) ? item.videoId : null);

                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
        }

        private static int getContentTypeId(string contentType)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"Select id from npGalleryContentType where typeName=@typeName";
                    cmd.Parameters.Add("@typeName", SqlDbType.NVarChar).Value = contentType;
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static bool ContentExists(string filename, int GalleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select ImageName from npGalleryContent where ImageName = @ImageName and ContainingFolderName = @Folder";
                    cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = filename;
                    cmd.Parameters.Add("@Folder", SqlDbType.NVarChar).Value = GalleryId;

                    if (cmd.ExecuteScalar() != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static int GetContentIdByLocation(string filename, int GalleryId)
        {
            using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select id from npGalleryContent where ImageName = @ImageName and ContainingFolderName = @Folder";
                    cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = filename;
                    cmd.Parameters.Add("@Folder", SqlDbType.NVarChar).Value = GalleryId;

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static int SaveAndFtpImage(byte[] image, string filename, int GalleryId)
        {
            Content imageContent = new Content();

            imageContent.ContentType = "image";
            imageContent.ContainingFolderName = GalleryId.ToString();
            imageContent.Name = filename;
            imageContent.Id = Content.addGalleryContent(imageContent);

            try
            {
                string uploadDirectory = string.Format("ftp://{0}/{1}", ConfigHelper.getConfigByName("CDNFtpAddress"), ConfigHelper.getConfigByName("CDNDirectory"));
                NetworkCredential credentials = new NetworkCredential(ConfigHelper.getConfigByName("CDNFtpUsername"), ConfigHelper.getConfigByName("CDNFtpPassword"));
                FtpWebRequest ftpCheckFolderExists = (FtpWebRequest)FtpWebRequest.Create(new Uri(uploadDirectory));
                ftpCheckFolderExists.Credentials = credentials;
                ftpCheckFolderExists.KeepAlive = false;
                ftpCheckFolderExists.Proxy = null;
                ftpCheckFolderExists.Timeout = 2000;
                ftpCheckFolderExists.Method = WebRequestMethods.Ftp.ListDirectory;

                using (WebResponse directoryResponse = ftpCheckFolderExists.GetResponse())
                {
                    using (Stream DirectoryStream = directoryResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(DirectoryStream))
                        {
                            List<string> directories = new List<string>();
                            string directory = reader.ReadLine();
                            while (directory != null)
                            {
                                directories.Add(directory);
                                directory = reader.ReadLine();
                            }
                            if (!directories.Contains(GalleryId.ToString()))
                            {
                                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(string.Format("ftp://{0}/{1}/{2}", ConfigHelper.getConfigByName("CDNFtpAddress"), ConfigHelper.getConfigByName("CDNDirectory"), GalleryId));
                                request.Credentials = credentials;
                                request.UsePassive = true;
                                request.UseBinary = true;
                                request.KeepAlive = false;
                                request.Proxy = null;
                                request.Timeout = 2000;

                                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                                using (FtpWebResponse resp = (FtpWebResponse)request.GetResponse())
                                {
                                    if (resp.StatusCode != FtpStatusCode.PathnameCreated)
                                    {
                                        throw new Exception("Folder could not be created on CDN.");
                                    }
                                }
                            }
                        }
                    }
                }

                string uploadAddress = string.Format("ftp://{0}/{1}/{2}/{3}", ConfigHelper.getConfigByName("CDNFtpAddress"), ConfigHelper.getConfigByName("CDNDirectory"), GalleryId, filename);
                FtpWebRequest ftpUpload = (FtpWebRequest)FtpWebRequest.Create(new Uri(uploadAddress));
                ftpUpload.Credentials = new NetworkCredential(ConfigHelper.getConfigByName("CDNFtpUsername"), ConfigHelper.getConfigByName("CDNFtpPassword"));
                ftpUpload.KeepAlive = false;
                ftpUpload.Timeout = 10000;
                ftpUpload.Method = WebRequestMethods.Ftp.UploadFile;
                ftpUpload.UseBinary = true;
                ftpUpload.ContentLength = image.Length;
                ftpUpload.Proxy = null;

                using (Stream ftpStream = ftpUpload.GetRequestStream())
                {
                    ftpStream.Write(image, 0, image.Length);
                }
            }
            catch (Exception)
            {
                Content.deleteContentById(imageContent.Id);
                throw;
            }

            return imageContent.Id;
        }
    }
}
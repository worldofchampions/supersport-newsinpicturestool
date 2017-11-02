using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;
using System.Net;

namespace NewsAndPictures
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Get the data
                HttpPostedFile image_upload = Request.Files["Filedata"];
                string filename = Request.Form["Filename"];
                string galleryId = Request.Form["GalleryId"];

                using (BinaryReader br = new BinaryReader(image_upload.InputStream))
                {
                    byte[] bin = br.ReadBytes(image_upload.ContentLength);

                    if (!Content.ContentExists(Path.GetFileName(filename), int.Parse(galleryId)))
                    {
                        Content.SaveAndFtpImage(bin, filename, int.Parse(galleryId));
                    }
                }
                
                Response.StatusCode = 200;
            }
            catch(Exception ex)
            {
                // If any kind of error occurs return a 500 Internal Server error
                Response.StatusCode = 500;
                Response.Write(string.Format("An error occured: {0} {1} {2}", ex.Message, System.Environment.NewLine, ex.InnerException));
                Response.End();
            }
            finally
            {
                // Clean up
                Response.End();
            }
        }
    }
}
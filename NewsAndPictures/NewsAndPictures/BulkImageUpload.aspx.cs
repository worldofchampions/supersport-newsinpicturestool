using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsAndPictures
{
    public partial class BulkImageUpload : System.Web.UI.Page
    {
        protected int GalleryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["GalleryId"] == null || string.IsNullOrEmpty(Request.QueryString["GalleryId"]))
            {
                throw new Exception("Gallery Id was not passed to Bulk Upload.");
            }

            GalleryId = int.Parse(Request.QueryString["GalleryId"]);
            // Clear the user's session
            Session.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewsAndPictures.DataObejcts;

namespace NewsAndPictures
{
    public partial class VideoSelect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SportSelect.DataSource = Sport.getSports();
                SportSelect.DataValueField = "Id";
                SportSelect.DataTextField = "Name";
                SportSelect.DataBind();
                SportSelect.Items.Insert(0, new ListItem("Please Select Sport", ""));
            }
        }

        protected void Sport_IndexChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SportSelect.SelectedValue))
            {
                CategorySelect.DataSource = Category.geSSZGenCategories(int.Parse(SportSelect.SelectedValue));
                CategorySelect.DataValueField = "Id";
                CategorySelect.DataTextField = "Name";
                CategorySelect.DataBind();
                CategorySelect.Items.Insert(0, new ListItem("Please Select Category", ""));
            }
        }

        protected void Category_IndexChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CategorySelect.SelectedValue))
            {
                VideoSelector.DataSource = BroadBandVideo.getVideoByCategoryId(int.Parse(CategorySelect.SelectedValue));
                VideoSelector.DataValueField = "GroupedSelectId";
                VideoSelector.DataTextField = "Name";
                VideoSelector.DataBind();
                VideoSelector.Items.Insert(0, new ListItem("Please Select Video", ""));
            }
        }
    }
}
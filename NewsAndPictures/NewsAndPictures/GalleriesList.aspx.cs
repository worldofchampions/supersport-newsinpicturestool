using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;
using NewsAndPictures.DataObejcts;
using System.Web.UI.HtmlControls;

namespace NewsAndPictures
{
    public partial class GalleriesList : System.Web.UI.Page
    {
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);
            }
        }
        int numberofRecoreds = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/login.aspx");
            }
            if (Session["PageNumber"] == null)
                Session["PageNumber"] = 1;

            SetGridData(Convert.ToInt32(Session["PageNumber"]));

        }

        private void SetGridData(int pageIndex)
        {
            Session["PageIndex"] = pageIndex;
            int numberOfItemstoSkip = 0;
            var results = Gallery.GetAllGalleries();
            numberofRecoreds = results.Count;

            if (pageIndex == 1)
                numberOfItemstoSkip = 0;
            else
                numberOfItemstoSkip = (pageIndex - 1) * PageSize;

            GalleriesRepeater.DataSource = results.Skip(numberOfItemstoSkip).Take(PageSize).ToList();
            GalleriesRepeater.EnableViewState = false;
            GalleriesRepeater.DataBind();

            PopulatePager(pageIndex);
        }

        protected void Galleries_DataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList list = (DropDownList)e.Item.FindControl("GalleryRankDropDown");
            list.DataSource = Gallery.GetAllGalleryRanks();
            list.DataBind();
            list.Items.Add(new ListItem("", ""));
            list.EnableViewState = false;
            list.AutoPostBack = true;

            list.SelectedValue = ((Gallery)e.Item.DataItem).GalleryRank.ToString();
            // list.SelectedIndexChanged += GalleryRankChanged_Click;
        }

        protected void Gallerie_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect("~/AddUpdateGallery.aspx?GalleryId=" + e.CommandArgument);
            }
        }

        protected void AddGallery_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddUpdateGallery.aspx");
        }

        protected void GalleryRankChanged_Click(object sender, EventArgs e)
        {
            DropDownList galleryRank = (DropDownList)sender;
            RepeaterItem parentItem = (RepeaterItem)galleryRank.Parent.Parent;
            int newRank;
            if (((HtmlTableCell)parentItem.FindControl("GalleryActive")).InnerText.Contains("True") && int.TryParse(galleryRank.SelectedValue, out newRank))
            {
                Gallery.UpdateGalleryRank(newRank, int.Parse(((HiddenField)parentItem.FindControl("GalleryId")).Value));
                Response.Redirect("~/GalleriesList.aspx");
            }
            else
            {
                galleryRank.SelectedValue = "";
                Response.Redirect("~/GalleriesList.aspx");
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);

            Session["PageNumber"] = pageIndex;
            this.SetGridData(Convert.ToInt32(Session["PageNumber"]));
        }

        private void PopulatePager(int currentPage)
        {


            double dblPageCount = (double)((decimal)numberofRecoreds / Convert.ToDecimal(PageSize));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }
    }
}
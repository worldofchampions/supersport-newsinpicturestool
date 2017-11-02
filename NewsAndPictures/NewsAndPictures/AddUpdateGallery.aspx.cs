using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using NewsAndPictures.Tools;
using NewsAndPictures.DataObejcts;
using System.IO;
using System.Web.Security;
using System.Net;
using System.Web.UI.HtmlControls;

namespace NewsAndPictures
{
    public partial class AddUpdateGallery : NewsAndPictures
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["Saved"] != null)
                {
                    Infolbl.Visible = true;
                    Infolbl.Text = "Gallery has been saved correctly.";
                }
                if (Request.QueryString["GalleryId"] != null && !string.IsNullOrEmpty(Request.QueryString["GalleryId"]))
                {
                    CreateOrUpdateGalleryButton.Text = "Update Gallery";
                    int galleryId;
                    if (int.TryParse(Request.QueryString["GalleryId"], out galleryId))
                    {
                        GalleryIdHidden.Value = galleryId.ToString();
                        List<Content> galleryContent = Content.getImagesForGallery(galleryId);

                        MainImageSelect.DataSource = galleryContent;
                        MainImageSelect.DataValueField = "id";
                        MainImageSelect.DataTextField = "Name";
                        MainImageSelect.DataBind();
                        MainImageSelect.Items.Insert(0, new ListItem("None", ""));

                        ThumbnailSelect.DataSource = galleryContent;
                        ThumbnailSelect.DataValueField = "id";
                        ThumbnailSelect.DataTextField = "Name";
                        ThumbnailSelect.DataBind();
                        ThumbnailSelect.Items.Insert(0, new ListItem("None", ""));

                        BackgroundSelect.DataSource = galleryContent;
                        BackgroundSelect.DataValueField = "id";
                        BackgroundSelect.DataTextField = "Name";
                        BackgroundSelect.DataBind();
                        BackgroundSelect.Items.Insert(0, new ListItem("None", ""));

                        SportSelect.DataSource = Sport.getSports();
                        SportSelect.DataValueField = "ID";
                        SportSelect.DataTextField = "Name";
                        SportSelect.DataBind();
                        SportSelect.Items.Insert(0, new ListItem("None", ""));

                        Gallery gallery = Gallery.GetGalleryById(galleryId);

                        CategorySelect.DataSource = Category.getZoneCategories(gallery.SportId);
                        CategorySelect.DataValueField = "ID";
                        CategorySelect.DataTextField = "Name";
                        CategorySelect.DataBind();
                        CategorySelect.Items.Insert(0, new ListItem("None", ""));

                        MainImageSelect.SelectedValue = gallery.MainContentId == null ? "" : gallery.MainContentId.ToString();
                        ThumbnailSelect.SelectedValue = gallery.ThumbImageId == null ? "" : gallery.ThumbImageId.ToString();
                        BackgroundSelect.SelectedValue = gallery.BackgroundImageId == null ? "" : gallery.BackgroundImageId.ToString();
                        SportSelect.SelectedValue = gallery.SportId.ToString();
                        CategorySelect.SelectedValue = gallery.CategoryId.ToString();
                        TitleTxt.Text = gallery.Title;
                        SynopsisTxt.Text = gallery.Synopsis;
                        FriendlyNameTxt.Text = gallery.FriendlyName;
                        FeaturedChk.Checked = gallery.Featured;
                        ActiveChk.Checked = gallery.Active;
                        TagTxt.Text = gallery.Tag == null ? "" : gallery.Tag.ToString();
                        ScriptTxt.Text = gallery.Script == null ? "" : gallery.Script.ToString();

                        if (gallery.VideoId.HasValue)
                        {
                            Content videoDetails = Content.getVideoContentDataByContentId(gallery.MainContentId.Value);
                            if (videoDetails != null)
                            {
                                MainVideoName.Text = videoDetails.Name;
                            }
                        }

                        ItemsRepeater.DataSource = GalleryItem.getGalleryItemsForGallery(gallery.Id);
                        ItemsRepeater.DataBind();
                    }
                    else
                    {
                        throw new Exception("Gallery id passed is not a valid id.");
                    }
                }
                else
                {
                    SportSelect.DataSource = Sport.getSports();
                    SportSelect.DataValueField = "Id";
                    SportSelect.DataTextField = "Name";
                    SportSelect.DataBind();
                    SportSelect.Items.Insert(0, new ListItem("None", ""));

                    GalleryItemTableHolder.Visible = false;
                    AddItem.Visible = false;
                    BulkUpload.Visible = false;
                }
            }
        }

        protected void CreateUpdateGallery_Click(object sender, EventArgs e)
        {
            int galleryId;
            if (Request.QueryString["GalleryId"] == null || string.IsNullOrEmpty(Request.QueryString["GalleryId"]))
            {
                Gallery gallery = new Gallery();
                gallery.SportId = int.Parse(SportSelect.SelectedValue);
                gallery.CategoryId = int.Parse(CategorySelect.SelectedValue);
                gallery.Title = TitleTxt.Text;
                gallery.Synopsis = SynopsisTxt.Text;
                gallery.FriendlyName = FriendlyNameTxt.Text;
                gallery.Featured = FeaturedChk.Checked;
                gallery.Active = ActiveChk.Checked;
                gallery.Script = ScriptTxt.Text;
                gallery.Tag = TagTxt.Text;

                gallery.Id = Gallery.AddGallery(gallery, UserId);
                var defaultValue = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NewItemRank"]);
                if (gallery.Active)
                {
                    Gallery.UpdateGalleryRank(defaultValue, gallery.Id);
                }

                if (BackgroundUpload.HasFile)
                {
                    if (!Content.ContentExists(Path.GetFileName(BackgroundUpload.FileName), gallery.Id))
                    {
                        gallery.BackgroundImageId = Content.SaveAndFtpImage(BackgroundUpload.FileBytes, Path.GetFileName(BackgroundUpload.FileName), gallery.Id);
                    }
                    else
                    {
                        gallery.BackgroundImageId = Content.GetContentIdByLocation(Path.GetFileName(BackgroundUpload.FileName), gallery.Id);
                    }
                }
                else if (!string.IsNullOrEmpty(BackgroundSelect.SelectedValue))
                {
                    gallery.BackgroundImageId = int.Parse(BackgroundSelect.SelectedValue);
                }

                if (ThumbnailFileUpload.HasFile)
                {
                    if (!Content.ContentExists(Path.GetFileName(ThumbnailFileUpload.FileName), gallery.Id))
                    {
                        gallery.ThumbImageId = Content.SaveAndFtpImage(ThumbnailFileUpload.FileBytes, Path.GetFileName(ThumbnailFileUpload.FileName), gallery.Id);
                    }
                    else
                    {
                        gallery.ThumbImageId = Content.GetContentIdByLocation(Path.GetFileName(ThumbnailFileUpload.FileName), gallery.Id);
                    }
                }
                else if (!string.IsNullOrEmpty(ThumbnailSelect.SelectedValue))
                {
                    gallery.ThumbImageId = int.Parse(ThumbnailSelect.SelectedValue);
                }

                if (!string.IsNullOrEmpty(MainVideoName.Text) && (!string.IsNullOrEmpty(MainImageSelect.SelectedValue) || MainContentImageUpload.HasFile))
                {
                    Infolbl.Visible = true;
                    Infolbl.Text = "Cannot have both Video and Image content selected. Please remove one before being able to continue.";
                    return;
                }
                else
                {
                    if (MainContentImageUpload.HasFile)
                    {
                        if (!Content.ContentExists(Path.GetFileName(MainContentImageUpload.FileName), gallery.Id))
                        {
                            gallery.MainContentId = Content.SaveAndFtpImage(MainContentImageUpload.FileBytes, Path.GetFileName(MainContentImageUpload.FileName), gallery.Id);
                        }
                        else
                        {
                            gallery.MainContentId = Content.GetContentIdByLocation(Path.GetFileName(MainContentImageUpload.FileName), gallery.Id);
                        }
                    }
                    else if (!string.IsNullOrEmpty(MainImageSelect.SelectedValue))
                    {
                        gallery.MainContentId = int.Parse(MainImageSelect.SelectedValue);
                    }
                    else if (!string.IsNullOrEmpty(VideoIdHidden.Value) && !string.IsNullOrEmpty(MainVideoName.Text))
                    {
                        Content content = new Content();
                        content.ContentType = "Video";
                        content.videoId = int.Parse(VideoIdHidden.Value);
                        gallery.MainContentId = Content.addGalleryContent(content);
                    }
                }

                Gallery.UpdateGallery(gallery);
                Response.Redirect(string.Format("~/AddUpdateGallery.aspx?GalleryId={0}&Saved=1", gallery.Id));
            }
            else if (int.TryParse(Request.QueryString["GalleryId"], out galleryId))
            {
                Gallery gallery = Gallery.GetGalleryById(galleryId);
                gallery.SportId = int.Parse(SportSelect.SelectedValue);
                gallery.CategoryId = int.Parse(CategorySelect.SelectedValue);
                gallery.Title = TitleTxt.Text;
                gallery.Synopsis = SynopsisTxt.Text;
                gallery.FriendlyName = FriendlyNameTxt.Text;
                gallery.Featured = FeaturedChk.Checked;
                gallery.Active = ActiveChk.Checked;
                gallery.Tag = TagTxt.Text;
                gallery.Script = ScriptTxt.Text;

                if (BackgroundUpload.HasFile)
                {
                    if (!Content.ContentExists(Path.GetFileName(BackgroundUpload.FileName), galleryId))
                    {
                        gallery.BackgroundImageId = Content.SaveAndFtpImage(BackgroundUpload.FileBytes, Path.GetFileName(BackgroundUpload.FileName), gallery.Id);
                    }
                    else
                    {
                        gallery.BackgroundImageId = Content.GetContentIdByLocation(Path.GetFileName(BackgroundUpload.FileName), galleryId);
                    }
                }
                else if (!string.IsNullOrEmpty(BackgroundSelect.SelectedValue))
                {
                    gallery.BackgroundImageId = int.Parse(BackgroundSelect.SelectedValue);
                }

                if (ThumbnailFileUpload.HasFile)
                {
                    if (!Content.ContentExists(Path.GetFileName(ThumbnailFileUpload.FileName), galleryId))
                    {
                        gallery.ThumbImageId = Content.SaveAndFtpImage(ThumbnailFileUpload.FileBytes, Path.GetFileName(ThumbnailFileUpload.FileName), gallery.Id);
                    }
                    else
                    {
                        gallery.ThumbImageId = Content.GetContentIdByLocation(Path.GetFileName(ThumbnailFileUpload.FileName), galleryId);
                    }
                }
                else if (!string.IsNullOrEmpty(ThumbnailSelect.SelectedValue))
                {
                    gallery.ThumbImageId = int.Parse(ThumbnailSelect.SelectedValue);
                }

                if (!string.IsNullOrEmpty(MainVideoName.Text) && (!string.IsNullOrEmpty(MainImageSelect.SelectedValue) || MainContentImageUpload.HasFile))
                {
                    Infolbl.Visible = true;
                    Infolbl.Text = "Cannot have both Video and Image content selected. Please remove one before being able to continue.";
                    return;
                }
                else
                {
                    if (MainContentImageUpload.HasFile)
                    {
                        if (!Content.ContentExists(Path.GetFileName(MainContentImageUpload.FileName), galleryId))
                        {
                            gallery.MainContentId = Content.SaveAndFtpImage(MainContentImageUpload.FileBytes, Path.GetFileName(MainContentImageUpload.FileName), gallery.Id);
                        }
                        else
                        {
                            gallery.MainContentId = Content.GetContentIdByLocation(Path.GetFileName(MainContentImageUpload.FileName), galleryId);
                        }
                    }
                    else if (!string.IsNullOrEmpty(MainImageSelect.SelectedValue))
                    {
                        gallery.MainContentId = int.Parse(MainImageSelect.SelectedValue);
                    }
                    else if (!string.IsNullOrEmpty(VideoIdHidden.Value) && !string.IsNullOrEmpty(MainVideoName.Text))
                    {
                        if (gallery.MainContentId.HasValue)
                        {
                            Content content = Content.getContentById(gallery.MainContentId.Value);
                            content.ContentType = "Video";
                            content.videoId = int.Parse(VideoIdHidden.Value);
                            Content.updateGalleryContent(content);
                        }
                        else
                        {
                            Content content = new Content();
                            content.ContentType = "Video";
                            content.videoId = int.Parse(VideoIdHidden.Value);
                            gallery.MainContentId = Content.addGalleryContent(content);
                        }
                    }
                }

                Gallery.UpdateGallery(gallery);
                Response.Redirect(string.Format("~/AddUpdateGallery.aspx?GalleryId={0}&Saved=1", gallery.Id));
            }
        }

        protected void Items_Databound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList list = (DropDownList)e.Item.FindControl("ItemRankDropDown");
            list.DataSource = GalleryItem.GetAllGalleryItemRanks(((GalleryItem)e.Item.DataItem).GalleryId);
            list.DataBind();
            list.Items.Add(new ListItem("", ""));
            list.SelectedValue = ((GalleryItem)e.Item.DataItem).ItemRank.ToString();
        }

        protected void GalleryItem_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Infolbl.Visible = false;
                Response.Redirect("~/AddUpdateGalleryItem.aspx?ItemId=" + e.CommandArgument);
            }
        }

        protected void ItemRankChanged_Click(object sender, EventArgs e)
        {
            DropDownList itemRank = (DropDownList)sender;
            RepeaterItem parentItem = (RepeaterItem)itemRank.Parent.Parent;
            int newRank;
            if (((HtmlTableCell)parentItem.FindControl("ItemActive")).InnerText.Contains("True") && int.TryParse(itemRank.SelectedValue, out newRank))
            {
                GalleryItem.updateItemRank(newRank, int.Parse(((HiddenField)parentItem.FindControl("GalleryItemId")).Value), int.Parse(GalleryIdHidden.Value));
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                itemRank.SelectedValue = "";
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void AddItem_Click(object sender, EventArgs e)
        {
            Infolbl.Visible = false;
            Response.Redirect("~/addUpdateGalleryItem.aspx?GalleryId=" + GalleryIdHidden.Value);
        }

        protected void Return_Click(object sender, EventArgs e)
        {
            Infolbl.Visible = false;
            Response.Redirect("~/GalleriesList.aspx");
        }

        protected void SportSelect_IndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SportSelect.SelectedValue))
            {
                Infolbl.Visible = false;
                CategorySelect.DataSource = Category.getZoneCategories(int.Parse(SportSelect.SelectedValue));
                CategorySelect.DataValueField = "ID";
                CategorySelect.DataTextField = "Name";
                CategorySelect.DataBind();
                CategorySelect.Items.Insert(0, new ListItem("None", ""));
            }
        }
    }
}
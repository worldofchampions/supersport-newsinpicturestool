using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewsAndPictures.DataObejcts;
using System.Data.SqlClient;
using System.Data;
using NewsAndPictures.Tools;
using System.Net;
using System.IO;

namespace NewsAndPictures
{
    public partial class AddUpdateGalleryItem : NewsAndPictures
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
                    InfoLbl.Visible = true;
                    InfoLbl.Text = "Gallery Item saved correctly.";
                }
                if (Request.QueryString["ItemId"] != null || !string.IsNullOrEmpty(Request.QueryString["ItemId"]))
                {
                    CreateUpdateGalleryItemBtn.Text = "Update Item";

                    int itemId;
                    if (int.TryParse(Request.QueryString["ItemId"], out itemId))
                    {
                        ItemIdHidden.Value = itemId.ToString();
                        GalleryItem galleryItem = GalleryItem.getGalleryItemById(itemId);
                        
                        CreditSelect.DataSource = Credit.getCredits();
                        CreditSelect.DataValueField = "id";
                        CreditSelect.DataTextField = "Name";
                        CreditSelect.DataBind();
                        CreditSelect.Items.Insert(0, new ListItem("None", ""));

                        MainImageSelect.DataSource = Content.getImagesForGallery(galleryItem.GalleryId);
                        MainImageSelect.DataValueField = "id";
                        MainImageSelect.DataTextField = "Name";
                        MainImageSelect.DataBind();
                        MainImageSelect.Items.Insert(0, new ListItem("None", ""));

                        ThumbnailSelect.DataSource = Content.getImagesForGallery(galleryItem.GalleryId);
                        ThumbnailSelect.DataValueField = "id";
                        ThumbnailSelect.DataTextField = "Name";
                        ThumbnailSelect.DataBind();
                        ThumbnailSelect.Items.Insert(0, new ListItem("None", ""));                        

                        MainImageSelect.SelectedValue = galleryItem.MainContentId == null ? "" : galleryItem.MainContentId.ToString();
                        ThumbnailSelect.SelectedValue = galleryItem.ThumbnailId == null ? "" : galleryItem.ThumbnailId.ToString();
                        CreditSelect.Items.FindByText((galleryItem.credit == null ? "" : galleryItem.credit)).Selected = true;
                        TitleTxt.Text = galleryItem.Title;
                        SynopsisTxt.Text = galleryItem.Synopsis;
                        ActiveChk.Checked = galleryItem.Active;
                        GalleryIdHidden.Value = galleryItem.GalleryId.ToString();

                        if (galleryItem.VideoId.HasValue)
                        {
                            Content videoDetails = Content.getVideoContentDataByContentId(galleryItem.MainContentId.Value);
                            if (videoDetails != null)
                            {
                                MainVideoName.Text = videoDetails.Name;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Gallery item id passed is not a valid id.");
                    }
                }
                else if (Request.QueryString["GalleryId"] != null || !string.IsNullOrEmpty(Request.QueryString["GalleryId"]))
                {
                    int galleryId;
                    if (int.TryParse(Request.QueryString["GalleryId"], out galleryId))
                    {
                        GalleryIdHidden.Value = galleryId.ToString();

                        MainImageSelect.DataSource = Content.getImagesForGallery(galleryId);
                        MainImageSelect.DataValueField = "Id";
                        MainImageSelect.DataTextField = "Name";
                        MainImageSelect.DataBind();
                        MainImageSelect.Items.Insert(0, new ListItem("None", ""));

                        ThumbnailSelect.DataSource = Content.getImagesForGallery(galleryId);
                        ThumbnailSelect.DataValueField = "Id";
                        ThumbnailSelect.DataTextField = "Name";
                        ThumbnailSelect.DataBind();
                        ThumbnailSelect.Items.Insert(0, new ListItem("None", ""));

                        CreditSelect.DataSource = Credit.getCredits();
                        CreditSelect.DataValueField = "id";
                        CreditSelect.DataTextField = "Name";
                        CreditSelect.DataBind();
                        CreditSelect.Items.Insert(0, new ListItem("None", ""));

                    }
                    else
                    {
                        throw new Exception("Gallery id passed is not a valid id.");
                    }
                }
                else
                {
                    throw new Exception("A problem occurred due to the Query String being in the incorrect format.");
                }
            }
        }

        protected void CreateUpdate_Click(object sender, EventArgs e)
        {
            int galleryId;
            int galleryItemId;

            if (Request.QueryString["ItemId"] == null || string.IsNullOrEmpty(Request.QueryString["ItemId"]))
            {            
                galleryId = int.Parse(GalleryIdHidden.Value);
                GalleryItem galleryItem = new GalleryItem();
                galleryItem.Title = TitleTxt.Text;
                galleryItem.Synopsis = SynopsisTxt.Text;
                galleryItem.Active = ActiveChk.Checked;
                galleryItem.GalleryId = galleryId;
                galleryItem.Id = GalleryItem.addGalleryItem(galleryItem, UserId);
                galleryItem.credit = CreditSelect.SelectedItem.Text;

                if (ThumbnailFileUpload.HasFile)
                {
                    if (!Content.ContentExists(Path.GetFileName(ThumbnailFileUpload.FileName), galleryItem.GalleryId))
                    {
                        galleryItem.ThumbnailId = Content.SaveAndFtpImage(ThumbnailFileUpload.FileBytes, Path.GetFileName(ThumbnailFileUpload.FileName), galleryItem.GalleryId);
                    }
                    else
                    {
                        galleryItem.ThumbnailId = Content.GetContentIdByLocation(Path.GetFileName(ThumbnailFileUpload.FileName), galleryItem.GalleryId);
                    }                     
                }
                else if (!string.IsNullOrEmpty(ThumbnailSelect.SelectedValue))
                {
                    galleryItem.ThumbnailId = int.Parse(ThumbnailSelect.SelectedValue);
                }

                if (!string.IsNullOrEmpty(MainVideoName.Text) && (!string.IsNullOrEmpty(MainImageSelect.SelectedValue) || MainContentImageUpload.HasFile))
                {
                    InfoLbl.Visible = true;
                    InfoLbl.Text = "Cannot have both Video and Image content selected. Please remove one before being able to continue.";
                    return;
                }
                else
                {
                    if (MainContentImageUpload.HasFile)
                    {
                        if (!Content.ContentExists(Path.GetFileName(MainContentImageUpload.FileName), galleryItem.GalleryId))
                        {
                            galleryItem.MainContentId = Content.SaveAndFtpImage(MainContentImageUpload.FileBytes, Path.GetFileName(MainContentImageUpload.FileName), galleryItem.GalleryId);
                        }
                        else
                        {
                            galleryItem.MainContentId = Content.GetContentIdByLocation(Path.GetFileName(MainContentImageUpload.FileName), galleryItem.GalleryId);
                        }
                    }
                    else if (!string.IsNullOrEmpty(MainImageSelect.SelectedValue))
                    {
                        galleryItem.MainContentId = int.Parse(MainImageSelect.SelectedValue);
                    }
                    else if (!string.IsNullOrEmpty(VideoIdHidden.Value) && !string.IsNullOrEmpty(MainVideoName.Text))
                    {
                        Content content = new Content();
                        content.ContentType = "Video";
                        content.videoId = int.Parse(VideoIdHidden.Value);
                        galleryItem.MainContentId = Content.addGalleryContent(content);
                    }
                }

                GalleryItem.updateGalleryItem(galleryItem);
                Response.Redirect(string.Format("~/AddUpdateGallery.aspx?GalleryId={0}", GalleryIdHidden.Value));
            }
            else if (int.TryParse(Request.QueryString["ItemId"], out galleryItemId))
            {
                GalleryItem galleryItem = GalleryItem.getGalleryItemById(galleryItemId);
                galleryItem.Title = TitleTxt.Text;
                galleryItem.Synopsis = SynopsisTxt.Text;
                galleryItem.Active = ActiveChk.Checked;
                galleryItem.credit = CreditSelect.SelectedItem.Text;

                if (ThumbnailFileUpload.HasFile)
                {
                    if (!Content.ContentExists(Path.GetFileName(ThumbnailFileUpload.FileName), galleryItem.GalleryId))
                    {
                        galleryItem.ThumbnailId = Content.SaveAndFtpImage(ThumbnailFileUpload.FileBytes, Path.GetFileName(ThumbnailFileUpload.FileName), galleryItem.GalleryId);
                    }
                    else
                    {
                        galleryItem.ThumbnailId = Content.GetContentIdByLocation(Path.GetFileName(ThumbnailFileUpload.FileName), galleryItem.GalleryId);
                    }
                }
                else if (!string.IsNullOrEmpty(ThumbnailSelect.SelectedValue))
                {
                    galleryItem.ThumbnailId = int.Parse(ThumbnailSelect.SelectedValue);
                }

                if (!string.IsNullOrEmpty(MainVideoName.Text) && (!string.IsNullOrEmpty(MainImageSelect.SelectedValue) || MainContentImageUpload.HasFile))
                {
                    InfoLbl.Visible = true;
                    InfoLbl.Text = "Cannot have both Video and Image content selected. Please remove one before being able to continue.";
                    return;
                }
                else
                {
                    if (MainContentImageUpload.HasFile)
                    {
                        if (!Content.ContentExists(Path.GetFileName(MainContentImageUpload.FileName), galleryItem.GalleryId))
                        {
                            galleryItem.MainContentId = Content.SaveAndFtpImage(MainContentImageUpload.FileBytes, Path.GetFileName(MainContentImageUpload.FileName), galleryItem.GalleryId);
                        }
                        else
                        {
                            galleryItem.MainContentId = Content.GetContentIdByLocation(Path.GetFileName(MainContentImageUpload.FileName), galleryItem.GalleryId);
                        }
                    }
                    else if (!string.IsNullOrEmpty(MainImageSelect.SelectedValue))
                    {
                        galleryItem.MainContentId = int.Parse(MainImageSelect.SelectedValue);
                    }
                    else if (!string.IsNullOrEmpty(VideoIdHidden.Value) && !string.IsNullOrEmpty(MainVideoName.Text))
                    {
                        if (galleryItem.MainContentId.HasValue)
                        {
                            Content content = Content.getContentById(galleryItem.MainContentId.Value);
                            content.ContentType = "Video";
                            content.videoId = int.Parse(VideoIdHidden.Value);
                            Content.updateGalleryContent(content);
                        }
                        else
                        {
                            Content content = new Content();
                            content.ContentType = "Video";
                            content.videoId = int.Parse(VideoIdHidden.Value);
                            galleryItem.MainContentId = Content.addGalleryContent(content);
                        }
                    }
                }

                GalleryItem.updateGalleryItem(galleryItem);
                Response.Redirect(string.Format("~/AddUpdateGallery.aspx?GalleryId={0}", GalleryIdHidden.Value));
            }
            else
            {
                throw new Exception("Neither a Gallery ID nor Gallery Item Id passed to this page was correct.");
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddUpdateGallery.aspx?GalleryId=" + GalleryIdHidden.Value);
        }
    }
}
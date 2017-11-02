<%@ Page Title="Bulk Upload Content" Language="C#" AutoEventWireup="true"
    CodeBehind="BulkImageUpload.aspx.cs" inherits="NewsAndPictures.BulkImageUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Bulk Upload</title>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js"></script>
	<link href="Styles/swfupload.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="swfupload/swfupload.js?2"></script>
	<script type="text/javascript" src="Scripts/handlers.js?2"></script>
	<script type="text/javascript">
	    var swfu;
	    window.onload = function () {
	        swfu = new SWFUpload({
	            // Backend Settings
	            upload_url: "Upload.aspx",
	            post_params: {
	                "ASPSESSID": "<%=Session.SessionID %>",
	                "GalleryId": "<%=GalleryId %>"
	            },

	            // File Upload Settings
	            file_size_limit: "0",
	            file_types: "*.jpg; *.png",
	            file_types_description: "JPG, PNG Images",
	            file_upload_limit: "0",    // Zero means unlimited

	            // Event Handler Settings - these functions as defined in Handlers.js
	            //  The handlers are not part of SWFUpload but are part of my website and control how
	            //  my website reacts to the SWFUpload events.
	            file_queue_error_handler: fileQueueError,
	            file_dialog_complete_handler: fileDialogComplete,
	            upload_progress_handler: uploadProgress,
	            upload_error_handler: uploadError,
	            upload_success_handler: uploadSuccess,
	            upload_complete_handler: uploadComplete,

	            // Button settings
	            button_image_url: "images/XPButtonNoText_160x22.png",
	            button_placeholder_id: "spanButtonPlaceholder",
	            button_width: 160,
	            button_height: 22,
	            button_text: '<div class="button">Select Images </div>',
	            button_text_style: '.button { font-family: Helvetica, Arial, sans-serif; font-size: 14pt; } .buttonSmall { font-size: 10pt; }',
	            button_text_top_padding: 1,
	            button_text_left_padding: 5,

	            // Flash Settings
	            flash_url: "swfupload/swfupload.swf", // Relative to this file

	            custom_settings: {
	                upload_target: "divFileProgressContainer"
	            },

	            // Debug Settings
	            debug: false
	        });
	    }

	    window.onbeforeunload = function closing() {
	        window.opener.location = window.opener.location;
	    };

	    function SaveClick() {
	        window.opener.location = window.opener.location;
	        window.close();
	    };
	</script>
</head>
<body onbeforeunload="closing()">
    <form id="form1" runat="server">
	<div id="content">		
	    <div id="swfu_container" style="margin: 0px 10px;">
		    <div>
				<span id="spanButtonPlaceholder"></span>
		    </div>
		    <div id="divFileProgressContainer" style="height: 75px;"></div>
	    </div>
	</div>
    <button onclick="SaveClick()" >Save and return</button>
    </form>
</body>
</html>

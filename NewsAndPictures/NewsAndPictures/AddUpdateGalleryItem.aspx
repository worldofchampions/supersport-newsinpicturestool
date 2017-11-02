<%@ Page Title="Gallery Item" Language="C#" MasterPageFile="~/Site.master" 
AutoEventWireup="true" CodeBehind="AddUpdateGalleryItem.aspx.cs" Inherits="NewsAndPictures.AddUpdateGalleryItem" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript">
    function openVideoWindow() {
        var var1 = window.open("VideoSelect.aspx", 'Video_Select', 'width=700,height=450,top=5,left=5,toolbar=no,location=yes,directories=no,status=no,menubar=no,scrollbars=yes,resizeable=yes');
    }
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ValidationSummary ID="ValidationSummary" HeaderText = "Fields have not been supplied:"  runat="server"/> 
    <asp:HiddenField ID="ItemIdHidden" runat="server"/>
    <asp:HiddenField ID="GalleryIdHidden" runat="server"/>
    <table>
        <tr>
            <td>
                <asp:Label ID="InfoLbl" CssClass="red" Visible="false" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="VideoIdHidden" runat="server" ClientIDMode="Static"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="TitleTxt" Text="Title:" ID="Titlelbl" runat="server"></asp:Label> 
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="TitleTxt" Width="50%" ClientIDMode="Static"></asp:TextBox>
                <asp:RequiredFieldValidator ID="TitleValidator" ControlToValidate="TitleTxt" ErrorMessage="Please provide a title for the gallery." Text="*" runat="server">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="SynopsisTxt" Text="Synopsis:" ID="Synopsislbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="SynopsisTxt"  ClientIDMode="Static"  TextMode="multiline" Width="100%" Height="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="SynopsisValidator" ControlToValidate="SynopsisTxt" ErrorMessage="Please provide a synopsis for the gallery" Text="*" runat="server">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="CreditSelect" Text="Credit:" ID="Creditlbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="CreditSelect" Width="50%" ></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="ActiveChk" Text="Is Active" Checked="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="ThumbnailSelect" Text="Select or upload thumbnail (125x70):" ID="Thumbnaillbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ThumbnailSelect">   
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload runat="server" ID="ThumbnailFileUpload" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="MainImageSelect" Text="Select or upload the main content (940x513):" ID="MainContentlbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="MainImageSelect">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload runat="server" ID="MainContentImageUpload" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" Text="OR" runat="server" style="font-size:15px; font-weight:bold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" Text="Select Video" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="MainVideoName" Text="Video Name:" ID="VideoName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="MainVideoName" ClientIDMode="Static">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <button id="VideoBtn" onclick="openVideoWindow()" runat="server">Select Video</button>
            </td>
        </tr>
    </table>

    <asp:Button ID="CreateUpdateGalleryItemBtn" Text="Create Gallery Item" CausesValidation="true" OnClick="CreateUpdate_Click" runat="server" style="margin-right:30px; margin-top:20px" />
    <asp:Button ID="BackButton" Text="Return to Gallery" CausesValidation="false" OnClick="BackButton_Click" runat="server" style="margin-top:20px" />
 </asp:Content>

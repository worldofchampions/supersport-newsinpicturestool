<%@ Page Title="Gallery" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
CodeBehind="AddUpdateGallery.aspx.cs" Inherits="NewsAndPictures.AddUpdateGallery" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript">
    function openVideoWindow() {
        var var1 = window.open("VideoSelect.aspx", 'Video_Select', 'width=700,height=450,top=5,left=5,toolbar=no,location=yes,directories=no,status=no,menubar=no,scrollbars=yes,resizeable=yes');
    }

    function openBulkUploadWindow() {
        window.open("BulkImageUpload.aspx?GalleryId=" + $("#GalleryIdHidden").val(), 'Bulk_Image_Upload', 'width=700,height=450,top=5,left=5,toolbar=no,location=yes,directories=no,status=no,menubar=no,scrollbars=yes,resizeable=yes');
    }
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div>
    <asp:ValidationSummary ID="ValidationSummary" HeaderText = "Fields have not been supplied:"  runat="server"/> 
    <asp:HiddenField ID="GalleryIdHidden" runat="server" ClientIDMode="Static"/>
    <table>
        <tr>
            <td>
                <asp:Label ID="Infolbl" CssClass="red" Visible="false" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="VideoIdHidden" ClientIDMode="Static" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="SportSelect" Text="Sport:" ID="Sportlbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="SportSelect" AutoPostBack="true" OnSelectedIndexChanged="SportSelect_IndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="SportValidator" ControlToValidate="SportSelect" ErrorMessage="Please select a Sport" Text="*" runat="server">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>    
                <asp:Label AssociatedControlID="CategorySelect" Text="Category:" ID="Categorylbl" runat="server"></asp:Label> 
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="CategorySelect">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="CategoryValidator" ControlToValidate="CategorySelect" ErrorMessage="Please select a Category" Text="*" runat="server">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="TitleTxt" Text="Title:" ID="Titlelbl" runat="server"></asp:Label> 
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="TitleTxt" ClientIDMode="Static" Width="50%"></asp:TextBox>
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
                <asp:TextBox runat="server" ID="SynopsisTxt" ClientIDMode="Static" TextMode="multiline" Width="100%" Height="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="SynopsisValidator" ControlToValidate="SynopsisTxt" ErrorMessage="Please provide a synopsis for the gallery" Text="*" runat="server">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="FriendlyNameTxt" Text="Friendly name:" ID="FriendlyNamelbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="FriendlyNameTxt" Width="50%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="FriendlyNameValidator" ControlToValidate="FriendlyNameTxt" ErrorMessage="Please provide a friendly name for the gallery" Text="*" runat="server">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="TagTxt" Text="Tag:" ID="Taglbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="TagTxt" Width="50%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="ScriptTxt" Text="Script:" ID="Scriptlbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="ScriptTxt" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="FeaturedChk" Text="Is Featured" Checked="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="ActiveChk" Text="Is Active" Checked="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label Text="Select or upload the background image:" ID="Backgroundlbl" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="BackgroundSelect">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload runat="server" ID="BackgroundUpload" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="ThumbnailSelect" Text="Select or upload thumbnail (300x170):" ID="Thumbnaillbl" runat="server"></asp:Label>
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
                <asp:Label Text="Select or upload the main content (940x513):" ID="MainContentlbl" runat="server"></asp:Label>
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
                <asp:Label Text="OR" runat="server" style="font-size:15px; font-weight:bold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" Text="Select Video" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label AssociatedControlID="MainVideoName" Text="Video Name:" ID="VidoeNamelbl" runat="server"></asp:Label>
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
                <button id="VideoBtn" onclick="openVideoWindow()" runat="server" causesvalidation="false">Select Video</button>
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID= "GalleryItemTableHolder" runat="server">
        <div class="TableBackGround padding5" >
            <h2>Gallery Items</h2>
            <Table id="GalleryItemTable" width="100%" cellspacing="0px">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>Item rank</th>
                        <th>Gallery Item Title</th>
                        <th>Active</th>
                        <th>Created</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                      <asp:Repeater runat="server" ID="ItemsRepeater" OnItemDataBound="Items_Databound" OnItemCommand="GalleryItem_ItemCommand">
                        <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="GalleryItemId" runat="server" value=<%#Eval("id")%>>
                                        </asp:HiddenField>
                                    </td>
                                    <td id="ItemRank" runat="server">
                                        <asp:DropDownList ID="ItemRankDropDown" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ItemRankChanged_Click"></asp:DropDownList>
                                    </td>
                                    <td id="ItemTitle" runat="server">
                                        <%#Eval("title")%>
                                    </td>
                                    <td id="ItemActive" runat="server">
                                        <%#Eval("active")%>
                                    </td>
                                    <td id="Created" runat="server">
                                        <%#((DateTime)Eval("dateCreated")).ToShortDateString()%>
                                    </td>
                                    <td id="editBtnCell" runat="server">
                                        <asp:LinkButton ID="EditButton" Text="Edit Gallery Item" runat="server" CommandName="Edit" CommandArgument=<%#Eval("id")%>>                                
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </Table>
        </div>
    </asp:PlaceHolder>
    <asp:Button Text="Create Gallery" ID="CreateOrUpdateGalleryButton" CausesValidation="true" OnClick="CreateUpdateGallery_Click" runat="server" style="margin-right:30px; margin-top:20px"/>
    <asp:Button Text="Images Bulk Upload" ID="BulkUpload" CausesValidation="true" OnClientClick="openBulkUploadWindow()" runat="server" style="margin-right:30px; margin-top:20px"/>
    <asp:Button Text="Add Gallery Item" ID="AddItem" CausesValidation="true" OnClick="AddItem_Click" runat="server" style="margin-right:30px; margin-top:20px"/>
    <asp:Button Text="Return to Gallery List" ID="BtnReturn" CausesValidation="false" OnClick="Return_Click" runat="server" style="margin-top:20px"/>
</div>
</asp:Content>

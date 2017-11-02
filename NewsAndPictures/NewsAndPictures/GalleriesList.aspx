<%@ Page Title="Galleries List" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="GalleriesList.aspx.cs" Inherits="NewsAndPictures.GalleriesList" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="TableBackGround padding5">
        <h2>Galleries</h2>
        <table width="100%" cellspacing="0px">
            <tr>
                <th></th>
                <th>Rank</th>
                <th>Gallery Name</th>
                <th>Title</th>
                <th>Active</th>
                <th>Date Created</th>
                <th></th>
            </tr>
            <asp:Repeater EnableViewState="false" runat="server" ID="GalleriesRepeater" OnItemDataBound="Galleries_DataBound" OnItemCommand="Gallerie_ItemCommand">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="GalleryId" runat="server" Value='<%#Eval("id")%>'></asp:HiddenField>
                        </td>
                        <td id="GalleryRank" runat="server">
                            <asp:DropDownList ID="GalleryRankDropDown"  AutoPostBack="true" runat="server" OnSelectedIndexChanged="GalleryRankChanged_Click"></asp:DropDownList>
                        </td>
                        <td id="GalleryName" runat="server">
                            <%#Eval("friendlyName")%>
                        </td>
                        <td id="GalleryTitle" runat="server">
                            <%#Eval("title")%>
                        </td>
                        <td id="GalleryActive" runat="server">
                            <%#Eval("active")%>
                        </td>
                        <td runat="server">
                            <%#((DateTime)Eval("dateCreated")).ToShortDateString()%>
                        </td>
                        <td runat="server">
                            <asp:LinkButton ID="EditButton" Text="Edit Gallery" runat="server" CommandName="Edit" CommandArgument='<%#Eval("id")%>'>                                
                            </asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="padding5"></div>
    <div class="pages">
        <asp:Repeater ID="rptPager" runat="server">
            <ItemTemplate>
                <asp:LinkButton  ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                    CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                    OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="bottom">
        <asp:Button Text="Add Gallery" runat="server" OnClick="AddGallery_Click" />
    </div>
</asp:Content>

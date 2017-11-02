<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoSelect.aspx.cs" Inherits="NewsAndPictures.VideoSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add video</title>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
        function videoBtn_click() {
            $("#VideoIdHidden", window.opener.document).val($("#VideoSelector option:selected").val().split('|')[0]);
            $("#TitleTxt", window.opener.document).val($("#VideoSelector option:selected").text());
            $("#MainVideoName", window.opener.document).val($("#VideoSelector option:selected").text());
            $("#SynopsisTxt", window.opener.document).val($("#VideoSelector option:selected").val().split('|')[1]);            
            this.close();
        }

        function cancelBtn_Click() {
            this.close();
        }

        function previewBtn_Click() {
            var previewURL = ("http://www.supersport.com/video/play.aspx?id=" + $("#VideoSelector option:selected").val().split('|')[0]);
            var var1 = window.open(previewURL, 'Video_Preview', 'width=900,height=750,top=5,left=5,toolbar=no,location=yes,directories=no,status=no,menubar=no,scrollbars=yes,resizeable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label Text="Please select a video below after selecting a Sport and Category." runat="server"></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:Label AssociatedControlID="SportSelect" Text="Sport:" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="SportSelect" AutoPostBack="true" Width="300px" OnSelectedIndexChanged="Sport_IndexChange"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Categorylbl" AssociatedControlID="CategorySelect" Text="Category:" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="CategorySelect" AutoPostBack="true" Width="300px" OnSelectedIndexChanged="Category_IndexChange"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Videolbl" AssociatedControlID="VideoSelector" Text="Video:" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList runat="server" Width="300px" ID="VideoSelector"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <button type="button" ID="SelectVideoBtn" onclick="videoBtn_click()" style="margin-top:20px; margin-right:20px">Select Video</button>
                    <button type="button" ID="CancelBtn" onclick="cancelBtn_Click()" style="margin-top:20px; margin-right:20px">Cancel</button>
                    <button type="button" ID="PreviewBtn" onclick="previewBtn_Click()" style="margin-top:20px; margin-right:20px">Preview Video</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

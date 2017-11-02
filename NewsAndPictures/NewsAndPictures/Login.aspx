<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" inherits="NewsAndPictures.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="loginBox">
        <h1 class="center padding5">Login</h1>
        <table>
            <tr>
                <td>
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="An error has occurred:" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label AssociatedControlID="UsernameTxt" ID="UsernameLbl" text="Username:" runat="server" >
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="UsernameTxt" CausesValidation="true" runat="server" >
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="UsernameValidator" ControlToValidate="UsernameTxt" Text="*" ErrorMessage="Please Type in your username" runat="server"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label AssociatedControlID="PasswordTxt" ID="PasswordLbl" text="Password:" runat="server" CssClass="fieldWidth">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="PasswordTxt" TextMode="password" CausesValidation="true" runat="server">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordValidator" ControlToValidate="PasswordTxt" Text="*" ErrorMessage="Please Type in your password" runat="server"></asp:RequiredFieldValidator>
                </td>
            </tr>        
        </table>
        <div class="padding5"></div>
        <asp:Button ID="LoginButton" runat="server" OnClick="Login_Click" Text="Login"/>
    </div>
</asp:Content>

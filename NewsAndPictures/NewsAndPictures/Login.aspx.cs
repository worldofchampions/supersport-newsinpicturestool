using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using NewsAndPictures.Tools;
using System.Data;
using System.Web.Security;

namespace NewsAndPictures
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/GalleriesList.aspx");
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }
            if (IsPostBack)
            {
                using (SqlConnection cn = new SqlConnection(ConfigHelper.getZoneSQLConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"Select id, password, active from zoneusers where username = @username";
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = UsernameTxt.Text;

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                string password = DBTools.convertToString(rdr["password"]);
                                if (DBTools.convertToBool(rdr["active"]).HasValue && DBTools.convertToBool(rdr["active"]).Value == true)
                                {
                                    if (password != PasswordTxt.Text)
                                    {
                                        PasswordValidator.ErrorMessage = "Incorrect password";
                                        PasswordValidator.IsValid = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    UsernameValidator.ErrorMessage = "User is no longer active";
                                    UsernameValidator.IsValid = false;
                                    return;
                                }
                            }
                            else
                            {
                                UsernameValidator.ErrorMessage = "Username does not exist";
                                UsernameValidator.IsValid = false;
                                return;
                            }

                            string userdata =  ((int)rdr["id"]).ToString();
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                                           UsernameTxt.Text,
                                                           DateTime.Now,
                                                           DateTime.Now.AddDays(30),
                                                           false,
                                                           userdata,
                                                           FormsAuthentication.FormsCookiePath);

                            // Encrypt the ticket.
                            string encTicket = FormsAuthentication.Encrypt(ticket);

                            // Create the cookie.
                            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                            //Correct username and password supplied so continue on to the actual CMS
                            Response.Redirect("~/GalleriesList.aspx");
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace NewsAndPictures
{
    public class NewsAndPictures : System.Web.UI.Page
    {
        private int? userId = null;
        public int UserId
        {
            get
            {
                if(!userId.HasValue)
                {
                    FormsIdentity id = (FormsIdentity)User.Identity;
                    userId = int.Parse(id.Ticket.UserData);
                }

                return userId.Value;
            }
        }

    }
}
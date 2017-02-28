using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

        if (User.Identity.IsAuthenticated) {          
            Response.Redirect("~/Search.aspx");
        }
    }

    public void OnFormSubmit(object sender, EventArgs e) {
        if (Membership.ValidateUser(userId.Value, password.Value)) {
            //if (chkRememberMe.Checked) {
            //    // Ticket expires = 31 days
            //    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(userId.Value, true, 44640);
            //    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            //    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //    cookie.Expires = authTicket.Expiration;
            //    HttpContext.Current.Response.Cookies.Set(cookie);
            //} else {
                FormsAuthentication.SetAuthCookie(userId.Value, false);
            //}
            var returnUrl = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl)) {
                returnUrl = "Search.aspx";
            }
            Response.Redirect(returnUrl);
            //FormsAuthentication.RedirectFromLoginPage(userId.Value, chkRememberMe.Checked);
        }
    }
}
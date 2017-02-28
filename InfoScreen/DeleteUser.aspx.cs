using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeleteUser : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        if (User.Identity.IsAuthenticated) {
            if (!Roles.IsUserInRole("Admin")) {
                Response.Redirect("AccessDeniedPage.aspx");
            } else {
                string userId = null;
                userId = Request.QueryString["userId"];
                if (userId != null && userId != HttpContext.Current.User.Identity.Name) {
                    bool isDeleted = Membership.DeleteUser(userId, true);
                    if (isDeleted) {
                        Session["userDeletionStatus"] = "success";
                    }
                }
                Response.Redirect("UserOverview.aspx");
            }
        } else {
            Response.Redirect("Login.aspx");
        }
    }
}
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class UserOverview : System.Web.UI.Page {

    public List<UserEntity> userList = new List<UserEntity>();

    protected void Page_Load(object sender, EventArgs e) {
        if (User.Identity.IsAuthenticated) {
            if (!Roles.IsUserInRole("Admin")) {
                Response.Redirect("AccessDeniedPage.aspx");
            } else {
                if (!IsPostBack) {
                    if ((string)Session["userDeletionStatus"] != null) {
                        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Brugeren blev slettet');", true);
                        Session.Remove("userDeletionStatus");
                    }
                }
                populateUserInfo();
            }
        } else {
            Response.Redirect("Login.aspx");
        }
    }


    private void populateUserInfo() {
        //DecryptePassword decryptePasswrod = new DecryptePassword();
        MembershipUserCollection users = Membership.GetAllUsers();
        foreach (MembershipUser membershipUser in users) {
            UserEntity user = new UserEntity();
            user.userId = membershipUser.UserName;
            string[] roles = Roles.GetRolesForUser(membershipUser.UserName);
            if (roles.Length > 0) {
                user.role = roles[0];
            }
            //user.password = decryptePasswrod.GetClearTextPassword(membershipUser.GetPassword());
            userList.Add(user);
        }
    }
}


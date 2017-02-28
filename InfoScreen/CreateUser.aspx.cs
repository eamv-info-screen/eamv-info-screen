using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class CreateUser : System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));

    protected void Page_Load(object sender, EventArgs e) {
        if (User.Identity.IsAuthenticated) {
            if (!Roles.IsUserInRole("Admin")) {
                Response.Redirect("AccessDeniedPage.aspx");
            } else {
                if (!Page.IsPostBack) {
                    populateRoles();
                }
            }
        } else {
            Response.Redirect("Login.aspx");
        }
    }

    protected void OnFormSubmit(object sender, EventArgs e) {
        if (IsValid) {
            MembershipCreateStatus createStatus;

            MembershipUser newUser = Membership.CreateUser(userId.Value, password.Value, null, null,
                                        null, true, out createStatus);

            switch (createStatus) {
                case MembershipCreateStatus.Success:
                    CreateUserRole(newUser, rolesSelect.Value);
                    RegisterSweetAlertScriptOnSuccess();

                    // Halt the thread for half a seconds, so we can show the sweetalert message.
                    System.Threading.Thread.Sleep(500);

                    //Response.Redirect("~/UserOverview.aspx"); //or delete this?
                    break;

                case MembershipCreateStatus.DuplicateUserName:
                    RegisterSweetAlertScriptOnFail();
                    break;
            }
        }
    }

    private void CreateUserRole(MembershipUser newUser, string role) {
        Roles.AddUserToRole(newUser.UserName, role);
    }

    private void populateRoles() {
        foreach (string role in Roles.GetAllRoles()) {
            rolesSelect.Items.Add(role);
        }
        rolesSelect.Items[1].Selected = true;
    }

    private void RegisterSweetAlertScriptOnSuccess() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Brugeren blev oprettet med succes');", true);
    }
    private void RegisterSweetAlertScriptOnFail() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Brugeren findes allerede i systemet');", true);
    }
}
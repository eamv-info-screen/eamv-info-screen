using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class DeleteDepartment :System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        int? departmentId = null;
        try {
            departmentId = int.Parse(Request.QueryString["departmentId"]);
        } catch (Exception exc) {
        }
        if (departmentId != null) {
            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            DepartmentsRepository departmentsRepository = new DepartmentsRepository();
            dataAccess.Open();
            departmentsRepository.DeleteDepartment(dataAccess, (int)departmentId);
            dataAccess.Close();

            Session["departmentDeletionStatus"] = "success";
        }
        Response.Redirect("DepartmentOverview.aspx");
    }
}
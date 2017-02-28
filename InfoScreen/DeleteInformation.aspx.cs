using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class DeleteInformation :System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        int? informationId = null;
        try {
            informationId = int.Parse(Request.QueryString["informationId"]);
        } catch (Exception exc) {
        }
        if (informationId != null) {
            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            InformationsRepository informationsRepository = new InformationsRepository();
            dataAccess.Open();
            informationsRepository.DeleteInformation(dataAccess, (int)informationId);
            dataAccess.Close();

            Session["informationDeletionStatus"] = "success";
        }
        Response.Redirect("InformationManagement.aspx");
    }
}
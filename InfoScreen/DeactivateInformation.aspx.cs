using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class DeactivateInformation :System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        int? informationId = null;
        bool informationIsCanceled = false;
        try {
            informationId = int.Parse(Request.QueryString["informationId"]);
            informationIsCanceled = Convert.ToBoolean(Request.QueryString["informationStatus"]);
        } catch (Exception exc) {
        }
        if (informationId != null) {
            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            InformationsRepository informationsRepository = new InformationsRepository();
            dataAccess.Open();
            // 0 : false // 1: true
            if (informationIsCanceled) {
                informationIsCanceled = false;
            } else {
                informationIsCanceled = true;
            }
            informationsRepository.Update_status(dataAccess, informationId, informationIsCanceled);

            dataAccess.Close();

        }
        Response.Redirect("InformationManagement.aspx");
    }
}
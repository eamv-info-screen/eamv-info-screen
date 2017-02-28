using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;
using Repositories;

public partial class DeleteEvent : System.Web.UI.Page {
    protected void Page_Load(object sender,EventArgs e) {
        int? eventId = null;
        try {
            eventId = int.Parse(Request.QueryString["eventId"]);
        } catch (Exception exc) {
        }
        if (eventId != null) {
            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            EventsRepository eventsRepository = new EventsRepository();
            dataAccess.Open();
            eventsRepository.Delete(dataAccess,(int)eventId);
            dataAccess.Close();

            Session["eventDeletionStatus"] = "success";
        }
        Response.Redirect("Search.aspx");
    }
}
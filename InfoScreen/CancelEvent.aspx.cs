using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class CancelEvent : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        int? eventId = null;
        bool eventIsCanceled = false;
        try {
            eventId = int.Parse(Request.QueryString["eventId"]);
            eventIsCanceled = Convert.ToBoolean(Request.QueryString["eventStatus"]);


        } catch (Exception exc) {
        }
        if (eventId != null) {
            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            EventsRepository eventsRepository = new EventsRepository();
            dataAccess.Open();
            // 0 : false // 1: true
            if (eventIsCanceled) {
                eventIsCanceled = false;
            } else {
                eventIsCanceled = true;
            }
            eventsRepository.Update_Status(dataAccess, eventId, eventIsCanceled);

            dataAccess.Close();

        }
        Response.Redirect("Search.aspx");
    }
}

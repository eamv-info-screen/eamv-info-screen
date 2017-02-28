using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class DeleteRoom :System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        int? roomId = null;
        try {
            roomId = int.Parse(Request.QueryString["roomId"]);
        } catch (Exception exc) {
        }
        if (roomId != null) {
            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            RoomsRepository roomsRepository = new RoomsRepository();
            dataAccess.Open();
            roomsRepository.DeleteRoom(dataAccess, (int)roomId);
            dataAccess.Close();

            Session["roomDeletionStatus"] = "success";
        }
        Response.Redirect("RoomOverview.aspx");
    }
}
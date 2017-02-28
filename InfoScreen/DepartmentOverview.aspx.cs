using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class RoomOverview :System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private EventsRepository eventRepository = new EventsRepository();
    public List<DepartmentEntity> departments = new List<DepartmentEntity>();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            dataAccess.Open();
            departments = eventRepository.FetchAllDepartments(dataAccess);
            dataAccess.Close();

            if ((string)Session["departmentDeletionStatus"] != null) {
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Afdelingen blev slettet');", true);
                Session.Remove("departmentDeletionStatus");
            }
        } else {
            
                dataAccess.Open();
                departments = eventRepository.FetchAllDepartments(dataAccess);
                dataAccess.Close();
            
        }
      
    }

}
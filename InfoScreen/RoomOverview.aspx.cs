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
    private RoomsRepository roomsRepository = new RoomsRepository();
    public List<RoomEntity> rooms = new List<RoomEntity>();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            dataAccess.Open();
            populateDepartmentsSelect(dataAccess, eventRepository);
            dataAccess.Close();

            if ((string)Session["roomDeletionStatus"] != null) {
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Rummet blev slettet');", true);
                Session.Remove("roomDeletionStatus");
            }
        } else {
            int index = int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value);
            if (index > -1) {
                dataAccess.Open();
                rooms = roomsRepository.FetchAllRooms(dataAccess, index);
                rooms.RemoveAt(0);
                dataAccess.Close();
            }
        }
    }


    private void populateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<Entities.DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));
        }
    }

}
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class AddRoom :System.Web.UI.Page {

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
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Rummet blev fjernet');", true);
                Session.Remove("roomDeletionStatus");
            }
        }
    }

    protected void AlreadyExistValidator(object sender, ServerValidateEventArgs e) {
        try {
            dataAccess.Open();
            if (roomsRepository.IsRoomExists(dataAccess, roomName.Value, departmentsSelect.Value) == false) {
                e.IsValid = true;
            } else {
                e.IsValid = false;
            }
        } catch {

        } finally {
            dataAccess.Close();
        }
    }

    protected void OnFormSubmit(object sender, EventArgs e) {
        if (IsValid) {
            try {
                dataAccess.Open();
                RoomEntity room = new RoomEntity();
                room.Identifier = roomName.Value;
                room.DepartmentId = int.Parse(departmentsSelect.Value);

                bool result = roomsRepository.InsertRoom(dataAccess, room);

                if (result == true) {
                    RegisterSweetAlertScriptOnSuccess();
                }
            } catch (SqlException exc) {
                dataAccess.Rollback();
            } finally {
                dataAccess.Close();
            }
        }
    }

    private void populateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<Entities.DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));
        }
        departmentsSelect.Items[0].Selected = true;
    }

    private void RegisterSweetAlertScriptOnSuccess() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Rummet blev oprettet med succes');", true);
    }

}
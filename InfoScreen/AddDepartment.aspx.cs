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

public partial class AddDepartment : System.Web.UI.Page {
    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private DepartmentsRepository departmentsRepository = new DepartmentsRepository();
    private EventsRepository eventsRepository = new EventsRepository();
    public List<DepartmentEntity> departments = new List<DepartmentEntity>();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
        } 
    }

    protected void AlreadyExistValidator(object sender, ServerValidateEventArgs e) {
        try {
            dataAccess.Open();
            if (departmentsRepository.IsDepartmentExists(dataAccess, departmentName.Value) == false) {
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

                bool result = departmentsRepository.InsertDepartment(dataAccess, departmentName.Value);
               

                if (result == true) {
                    RoomsRepository roomsRepository = new RoomsRepository();
                    RoomEntity roomEntity = new RoomEntity();
                    roomEntity.Identifier = "Ingen";
                    roomEntity.DepartmentId = (int)departmentsRepository.LastInsertedId;
                    roomsRepository.InsertRoom(dataAccess, roomEntity);

                    RegisterSweetAlertScriptOnSuccess();
                }

                dataAccess.StartTransaction();
                dataAccess.Commit();
              
            } catch (SqlException exc) {
                dataAccess.Rollback();
            } finally {
                dataAccess.Close();
            }
        }
    }


    private void RegisterSweetAlertScriptOnSuccess() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Afdelingen blev oprettet med succes');", true);
    }

}
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class InformationManagement :System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private InformationEntity informationsEntity = new InformationEntity();
    private InformationsRepository informationsRepository = new InformationsRepository();
    public List<InformationEntity> informations = new List<InformationEntity>();
    private EventsRepository eventRepository = new EventsRepository();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            dataAccess.Open();
            populateDepartmentsSelect(dataAccess, eventRepository);
            informations = informationsRepository.FetchAllInformations(dataAccess);
            dataAccess.Close();

            if ((string)Session["informationDeletionStatus"] != null) {
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Informationen blev fjernet');", true);
                Session.Remove("informationDeletionStatus");
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

    protected void OnFormSubmit(object sender, EventArgs e) {
        if (IsValid) {
            try {
                dataAccess.Open();

                InformationEntity informationEntity = new InformationEntity();
                informationEntity.Topic = topic.Value;
                informationEntity.Input = input.Value;
                informationEntity.DepartmentId = int.Parse(departmentsSelect.Value);

                bool result = informationsRepository.InsertInformation(dataAccess, informationEntity);

                if (result == true) {
                    RegisterSweetAlertScriptOnSuccess();
                    informations = informationsRepository.FetchAllInformations(dataAccess);
                }
            } catch {
            } finally {
                dataAccess.Close();
            }

            // Halt the thread for half a seconds, so we can show the sweetalert message.
            System.Threading.Thread.Sleep(500);
        }
    }

    private void RegisterSweetAlertScriptOnSuccess() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Informationen blev tilføjet med succes');", true);
    }

}
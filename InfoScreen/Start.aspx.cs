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

public partial class Start :System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private EventsRepository eventRepository = new EventsRepository();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            dataAccess.Open();
            populateDepartmentsSelect(dataAccess,eventRepository);
            dataAccess.Close();
        }
    }

    protected void OnFormSubmit(object sender, EventArgs e) {
        if (IsValid) {
            string queryWithParameter = "DailyEvents.aspx?departmentId=" + departmentsSelect.Items[departmentsSelect.SelectedIndex].Value;
            Response.Redirect(queryWithParameter);
        }
    }

    private void populateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<Entities.DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));
        }
        departmentsSelect.Items[0].Selected = true;
    }
}
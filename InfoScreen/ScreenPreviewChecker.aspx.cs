using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

public partial class ScreenPreviewChecker :System.Web.UI.Page {
    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private EventsRepository eventRepository = new EventsRepository();
    public List<EventEntity> events = new List<EventEntity>();
    private WeekNumber weekNumber = new WeekNumber();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            searchDate.SelectedDate = DateTime.Today;
            dataAccess.Open();
            populateDepartmentsSelect(dataAccess, eventRepository);
            dataAccess.Close();
        }

        dataAccess.Open();
        dataAccess.Close();
    }

    private void populateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));
        }
    }

    // REMEMBER : it works with only without POP_UP is not blocked
    protected void OnFormSubmit_Preview(object sender, EventArgs e) {
        string selectedDate = searchDate.SelectedDate.Year + "," + searchDate.SelectedDate.Month + "," + searchDate.SelectedDate.Day;
        string queryWithParameter = "ScreenPreview.aspx?date=" + selectedDate + "&departmentId=" + departmentsSelect.Items[departmentsSelect.SelectedIndex].Value;
        Page.ClientScript.RegisterStartupScript(
           this.GetType(), "OpenWindow", "window.open('" + queryWithParameter + "','_newtab');", true);
    }

    protected void CalendarDayRender(object sender, DayRenderEventArgs e) {
        DisableDays(e);

        int weeknum = weekNumber.GetISO8601WeekNumber(e.Day.Date);
        string jscriptStartDate = "<script type='text/javascript'> addWkColumn('" + searchDate.ClientID + "', " + weeknum + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddWeeknumbersStartDate", jscriptStartDate);
    }

    private void DisableDays(DayRenderEventArgs e) {
        if (e.Day.Date < DateTime.Now.Date || e.Day.IsOtherMonth) {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
        }
    }

}
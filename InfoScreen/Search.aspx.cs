using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Utils;

public partial class Search :System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private EventsRepository eventRepository = new EventsRepository();
    public List<EventEntity> events = new List<EventEntity>();
    private WeekNumber weekNumber = new WeekNumber();

    protected void Page_Load(object sender, EventArgs e) {

        if (!IsPostBack) {
            //searchDate.SelectedDate = DateTime.Today;
            dataAccess.Open();
            populateDepartmentsSelect(dataAccess, eventRepository);
            dataAccess.Close();

            if ((string)Session["eventDeletionStatus"] != null) {
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Aktiviteten blev slettet');", true);
                Session.Remove("eventDeletionStatus");
            }

        }

        dataAccess.Open();
        FetchAllEvents();
        dataAccess.Close();

    }

    protected void OnFormSubmit(object sender, EventArgs e) {
        if (IsValid) {
            dataAccess.Open();
            FetchEvents();
            dataAccess.Close();
        }
    }

    protected void OnFormClear(object sender, EventArgs e) {
        searchDate.SelectedDate = DateTime.MinValue;
        host.Value = string.Empty;
    }

    public void FetchAllEvents() {
        events = eventRepository.FetchAllEvents(dataAccess);
    }

    public void FetchEventsByDepartment() {     
        events = eventRepository.FetchEventsByDepartment(dataAccess,
            int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value));
    }

    private void FetchEventsByDepartmentAndDate() {
        events = eventRepository.FetchEventsByDepartmentAndDate(dataAccess, searchDate.SelectedDate,
            int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value));
    }

    private void FetchEventsByDepartmentAndHost() {
        events = eventRepository.FetchEventsByDepartmentAndHost(dataAccess,
               int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value), host.Value);
    }

    private void FetchEventsByDepartmentAndHostAndDate() {
        events = eventRepository.FetchEventsByDepartmentAndHostAndDate(dataAccess, searchDate.SelectedDate,
                   int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value), host.Value);
    }

    private void FetchEvents() {
        if (host.Value == string.Empty && searchDate.SelectedDate.Date == DateTime.MinValue) {
            FetchEventsByDepartment();
        } else if (host.Value == string.Empty) {
            FetchEventsByDepartmentAndDate();
        } else if (searchDate.SelectedDate.Date == DateTime.MinValue) {
            FetchEventsByDepartmentAndHost();
        } else {
            FetchEventsByDepartmentAndHostAndDate();
        }
    }
  
    private void populateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));

        }
    }

    protected void CalendarDayRender(object sender, DayRenderEventArgs e) {
        DisableDays(e);
        int weeknum = weekNumber.GetISO8601WeekNumber(e.Day.Date);
        string jscriptEndDate = "<script type='text/javascript'> addWkColumn('" + searchDate.ClientID + "', " + weeknum + ");</script>";
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "AddWeeknumbersTilDato", jscriptEndDate, false);
    }

    private void DisableDays(DayRenderEventArgs e) {
        DateTime pastdate = DateTime.Now.AddDays(-14);
        if (e.Day.Date < pastdate) {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
        }
    }
}
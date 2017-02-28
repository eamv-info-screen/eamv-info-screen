using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;
using Repositories;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Security;

public partial class AddEvent : System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private EventsRepository eventRepository = new EventsRepository();
    private WeekNumber weekNumber = new WeekNumber();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            startDate.SelectedDate = DateTime.Today;
            endDate.SelectedDate = DateTime.MinValue;
            // Fetch all departments and populate dep. select menu
            dataAccess.Open();
            populateDepartmentsSelect(dataAccess, eventRepository);
            PopulateRoomsSelect(dataAccess, eventRepository, int.Parse(departmentsSelect.Items[0].Value));
            dataAccess.Close();
        } else {
            List<int> rooms = GetSelectedRooms();

            dataAccess.Open();
            PopulateRoomsSelect(dataAccess, eventRepository, int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value));
            dataAccess.Close();

            SelectRooms(rooms);
        }

    }

    protected void Page_PreRender(object sender, EventArgs e) {
        if (IsPostBack) {
            // Date warning. If the chosen event date is too far in the furute (over an year ahead).
            if ((startDate.SelectedDate - DateTime.Now).TotalDays > 365) {
                startDatoWarning.Style.Add("visibility", "visible");
            } else {
                startDatoWarning.Style.Add("visibility", "hidden");
            }
        }
    }

    protected void EndTimeValidator(object sender, ServerValidateEventArgs e) {
        DateTime startTimeInput = DateTime.ParseExact(startTime.Value, "HH:mm", CultureInfo.InvariantCulture);
        DateTime endTimeInput = DateTime.ParseExact(endTime.Value, "HH:mm", CultureInfo.InvariantCulture);

        if (endTimeInput <= startTimeInput) {
            e.IsValid = false;
        } else {
            e.IsValid = true;
        }
    }

    protected void BeforeNowValidator(object sender, ServerValidateEventArgs e) {
        DateTime startTimeInput = DateTime.ParseExact(startTime.Value, "HH:mm", CultureInfo.InvariantCulture);
        DateTime startDateInput = new DateTime(startDate.SelectedDate.Year, startDate.SelectedDate.Month, startDate.SelectedDate.Day,
    startTimeInput.Hour, startTimeInput.Minute, startTimeInput.Second);

        if (startDateInput < DateTime.Now) {
            e.IsValid = false;
        } else {
            e.IsValid = true;
        }
    }

    protected void OnFormSubmit(object sender, EventArgs e) {
        if (IsValid) {
            try {
                dataAccess.Open();

                EventEntity eventEntity = new EventEntity();

                DateTime startTimeInput = DateTime.ParseExact(startTime.Value, "HH:mm", CultureInfo.InvariantCulture);
                DateTime startDateInput = new DateTime(startDate.SelectedDate.Year, startDate.SelectedDate.Month, startDate.SelectedDate.Day,
                    startTimeInput.Hour, startTimeInput.Minute, startTimeInput.Second);
                eventEntity.FromDate = startDateInput;

                DateTime endTimeInput = DateTime.ParseExact(endTime.Value, "HH:mm", CultureInfo.InvariantCulture);
                if (endDate.SelectedDate == null || endDate.SelectedDate == DateTime.MinValue) {
                    DateTime endDateInput = new DateTime(startDate.SelectedDate.Year, startDate.SelectedDate.Month, startDate.SelectedDate.Day,
                    endTimeInput.Hour, endTimeInput.Minute, endTimeInput.Second);
                    eventEntity.ToDate = endDateInput;
                } else {
                    DateTime endDateInput = new DateTime(endDate.SelectedDate.Year, endDate.SelectedDate.Month, endDate.SelectedDate.Day,
                    endTimeInput.Hour, endTimeInput.Minute, endTimeInput.Second);
                    eventEntity.ToDate = endDateInput;
                }


                eventEntity.Host = host.Value;
                eventEntity.Topic = topic.Value;

                bool result = false;
                if (EventShouldRepeats()) {
                    List<DateTime> dates = GetRepeateDates();
                    foreach (DateTime date in dates) {
                        eventEntity.FromDate = date;
                        eventEntity.ToDate = date;
                        result = eventRepository.Insert(dataAccess, eventEntity);
                        //Add all rooms
                        eventRepository.InsertEventRooms(dataAccess, GetSelectedRooms(), (int)eventRepository.LastInsertedId);
                    }
                } else {
                    result = eventRepository.Insert(dataAccess, eventEntity);
                    //Add all rooms
                    eventRepository.InsertEventRooms(dataAccess, GetSelectedRooms(), (int)eventRepository.LastInsertedId);
                }

                dataAccess.StartTransaction();
                dataAccess.Commit();

                if (result == true) {
                    // Sweetalert popup.
                    RegisterSweetAlertScriptOnSuccess();
                }
            } catch (SqlException exc) {
                dataAccess.Rollback();
            } finally {
                dataAccess.Close();
            }

            // Halt the thread for half a seconds, so we can show the sweetalert message.
            System.Threading.Thread.Sleep(500);
        }

    }

    private List<int> GetSelectedRooms() {
        List<int> rooms = new List<int>();

        for (int i = 0; i < roomsSelect.Items.Count; i++) {
            if (roomsSelect.Items[i].Selected) {
                rooms.Add(int.Parse(roomsSelect.Items[i].Value));
            }
        }
        return rooms;
    }

    private void SelectRooms(List<int> rooms) {
        foreach (int id in rooms) {
            ListItem item = roomsSelect.Items.FindByValue(id.ToString());
            if (item != null) {
                item.Selected = true;
            }
        }
    }

    private bool EventShouldRepeats() {
        List<int> repeatWeekDays = GetRepeatedWeekDays();
        if (repeatWeekDays.Count > 0) {
            return true;
        }
        return false;
    }

    private List<DateTime> GetRepeateDates() {
        List<DateTime> dates = new List<DateTime>();

        DateTime startTimeInput = DateTime.ParseExact(startTime.Value, "HH:mm", CultureInfo.InvariantCulture);
        DateTime startDateInput = new DateTime(startDate.SelectedDate.Year, startDate.SelectedDate.Month, startDate.SelectedDate.Day,
            startTimeInput.Hour, startTimeInput.Minute, startTimeInput.Second);

        DateTime endTimeInput = DateTime.ParseExact(endTime.Value, "HH:mm", CultureInfo.InvariantCulture);
        DateTime endDateInput = new DateTime(endDate.SelectedDate.Year, endDate.SelectedDate.Month, endDate.SelectedDate.Day,
            endTimeInput.Hour, endTimeInput.Minute, endTimeInput.Second);

        List<int> repeatWeekDays = GetRepeatedWeekDays();
        for (DateTime date = startDateInput; date <= endDateInput; date = date.AddDays(1)) {
            if (repeatWeekDays.Contains((int)date.DayOfWeek)) {
                dates.Add(date);
            }
        }
        return dates;
    }

    private List<int> GetRepeatedWeekDays() {
        List<int> repeatWeekDays = new List<int>();
        for (int i = 0; i <= eventRepetitionSelect.Items.Count - 1; i++) {
            if (eventRepetitionSelect.Items[i].Selected) {
                // If the user has chosen every day repetition, clear any previous entries and add all week day numbers.
                if (int.Parse(eventRepetitionSelect.Items[i].Value) == 0) {
                    repeatWeekDays.Clear();
                    repeatWeekDays.AddRange(new List<int>() { 1, 2, 3, 4, 5 });
                    break;
                }
                repeatWeekDays.Add(int.Parse(eventRepetitionSelect.Items[i].Value));
            }
        }
        return repeatWeekDays;
    }

    private void RegisterSweetAlertScriptOnSuccess() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert",
            "swal({title : 'Aktiviteten blev registreret med succes'}, function() {window.location = 'AddEvent.aspx';});", true);
    }

    private void populateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));
        }
        departmentsSelect.Items[0].Selected = true;
    }

    private void PopulateRoomsSelect(DataAccess dataAccess, EventsRepository repository, int departmentId) {
        List<RoomEntity> rooms = repository.FetchDepartmentRooms(dataAccess, departmentId);
        // Empty the select
        roomsSelect.Items.Clear();
        foreach (RoomEntity room in rooms) {
            roomsSelect.Items.Add(new ListItem(room.Identifier, room.Id.ToString()));
        }
    }

    private bool IsEventTarget(string Id) {
        if (Request.Params["__EVENTTARGET"] != null) {
            if (Request.Params["__EVENTTARGET"].ToString().Contains(Id)) {
                return true;
            }
        }
        return false;
    }

    protected void Calendar_DayRender1(object sender, DayRenderEventArgs e) {
        if (e.Day.Date < DateTime.Now.Date || e.Day.IsOtherMonth) {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
        }

        int weeknum = weekNumber.GetISO8601WeekNumber(e.Day.Date);
        string jscriptStartDate = "<script type='text/javascript'> addWkColumn('" + startDate.ClientID + "', " + weeknum + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddWeeknumbersStartDate", jscriptStartDate);

    }

    protected void Calendar_DayRender2(object sender, DayRenderEventArgs e) {
        if (e.Day.Date < startDate.SelectedDate || e.Day.IsOtherMonth) {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.LightGray;
        }
        int weeknum = weekNumber.GetISO8601WeekNumber(e.Day.Date);
        string jscriptEndDate = "<script type='text/javascript'> addWkColumn('" + endDate.ClientID + "', " + weeknum + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddWeeknumbersTilDato", jscriptEndDate);
    }


}


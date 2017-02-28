using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities;
using Utils;
using Repositories;
using System.Globalization;

public partial class EditEvent :System.Web.UI.Page {

    private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
    private EventsRepository eventRepository = new EventsRepository();
    public EventEntity eventEntity;
    private WeekNumber weekNumber = new WeekNumber();

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {

            int? eventId = null;
            try {
                eventId = int.Parse(Request.QueryString.Get("eventId"));
            } catch (Exception exc) {
                Response.Redirect("Search.aspx");
            }

            DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            try {
                dataAccess.Open();
                EventsRepository eventRepository = new EventsRepository();

                eventEntity = eventRepository.FetchById(dataAccess, (int)eventId);

                startDate.SelectedDate = eventEntity.FromDate.Date;
                startDate.VisibleDate = startDate.SelectedDate;

                if (eventEntity.IsCanceled == true) {
                    canceled.Visible = true;
                } else {
                    canceled.Visible = false;
                }

                // Fetch all departments and populate dep. select menu
                PopulateDepartmentsSelect(dataAccess, eventRepository);

                // Fetch all rooms which are corresponding to the chosen department and populate rooms select menu
                int departmentId = int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value);
                PopulateRoomsSelect(dataAccess, eventRepository, departmentId);
                SelectRooms(eventEntity.Rooms);
            } catch (Exception) {

            } finally {
                dataAccess.Close();
            }
        } else {
            eventEntity = CreateEventEntityFromForm();

            List<int> rooms = GetSelectedRooms();

            dataAccess.Open();
            PopulateRoomsSelect(dataAccess, eventRepository, int.Parse(departmentsSelect.Items[departmentsSelect.SelectedIndex].Value));
            dataAccess.Close();

            SelectRooms(rooms);
        }

        DataBind();
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
            EventEntity eventEntity = CreateEventEntityFromForm();
            this.UpdateEvent(eventEntity);
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

    private void UpdateEvent(EventEntity eventEntity) {
        DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));

        try {
            dataAccess.Open();

            EventsRepository eventRepository = new EventsRepository();

            bool result = eventRepository.Update(dataAccess, eventEntity);
            eventRepository.DeleteEventRooms(dataAccess, eventEntity.Id);
            eventRepository.InsertEventRooms(dataAccess, GetSelectedRooms(), eventEntity.Id);
            dataAccess.StartTransaction();
            dataAccess.Commit();

            if (result == true)
                RegisterSweetAlertScriptOnSuccess();
        } catch (Exception exc) {
            System.Diagnostics.Debug.WriteLine(exc);
            dataAccess.Rollback();
        } finally {
            dataAccess.Close();
        }
        // Halt the thread for half a seconds, so we can show the sweetalert message.
        System.Threading.Thread.Sleep(500);
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

    public EventEntity CreateEventEntityFromForm() {
        EventEntity eventEntity = new EventEntity();

        eventEntity.Id = int.Parse(eventID.Attributes["value"]);

        DateTime startTimeInput = DateTime.ParseExact(startTime.Attributes["value"], "HH:mm", CultureInfo.InvariantCulture);
        DateTime startDateInput = new DateTime(startDate.SelectedDate.Year, startDate.SelectedDate.Month, startDate.SelectedDate.Day,
            startTimeInput.Hour, startTimeInput.Minute, startTimeInput.Second);
        eventEntity.FromDate = startDateInput;

        DateTime endTimeInput = DateTime.ParseExact(endTime.Attributes["value"], "HH:mm", CultureInfo.InvariantCulture);
        DateTime endDateInput = new DateTime(startDate.SelectedDate.Year, startDate.SelectedDate.Month, startDate.SelectedDate.Day,
           endTimeInput.Hour, endTimeInput.Minute, endTimeInput.Second);
        eventEntity.ToDate = endDateInput;

        eventEntity.Host = host.Value;
        eventEntity.Topic = topic.Value;

        List<RoomEntity> rooms = new List<RoomEntity>();
        for (int i = 0; i < roomsSelect.Items.Count; i++) {
            if (roomsSelect.Items[i].Selected == true) {
                RoomEntity room = new RoomEntity();
                room.Id = int.Parse(roomsSelect.Items[i].Value);
                room.Identifier = roomsSelect.Items[i].Text;
                rooms.Add(room);
            }
        }
        eventEntity.Rooms = rooms;

        return eventEntity;
    }

    private void RegisterSweetAlertScriptOnSuccess() {
        // Register the sweetalert which will be called after successfully inserted event 
        ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal('Aktiviteten blev ændret med succes');", true);
    }

    private void PopulateDepartmentsSelect(DataAccess dataAccess, EventsRepository repository) {
        List<DepartmentEntity> departments = repository.FetchAllDepartments(dataAccess);
        foreach (DepartmentEntity department in departments) {
            departmentsSelect.Items.Add(new ListItem(department.Name, department.Id.ToString()));
        }
        // Find and select event's department.
        departmentsSelect.Items.FindByValue(eventEntity.Rooms[0].DepartmentId.ToString()).Selected = true;
    }

    private void SelectRooms(List<int> rooms) {
        foreach (int id in rooms) {
            ListItem item = roomsSelect.Items.FindByValue(id.ToString());
            if (item != null) {
                item.Selected = true;
            }
        }
    }

    private void SelectRooms(List<RoomEntity> rooms) {
        foreach (RoomEntity room in rooms) {
            ListItem item = roomsSelect.Items.FindByValue(room.Id.ToString());
            if (item != null) {
                item.Selected = true;
            }
        }
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
        string jscriptEndDate = "<script type='text/javascript'> addWkColumn('" + startDate.ClientID + "', " + weeknum + ");</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddWeeknumbersTilDato", jscriptEndDate);

    }
}
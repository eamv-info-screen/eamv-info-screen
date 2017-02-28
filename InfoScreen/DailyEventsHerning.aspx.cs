using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Utils;
using Repositories;
using System.Text;
using FluentScheduler;

public partial class DailyEventsHerning :System.Web.UI.Page {

    private List<EventEntity> events = new List<EventEntity>();
    public List<InformationEntity> informations = new List<InformationEntity>();
    public string informationToInfoscreen = "";

    protected void Page_Load(object sender, EventArgs e) {
        
        JobManager.Initialize(new AutoRegistry());
        FetchDailyEvents();
        FillTable(events);

        DataBind();
    }
    private static void FetchDailyEventsds() {}
    private void FetchDailyEvents() {
        DataAccess dataAccess = null;
        try {
            dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
            dataAccess.Open();

            EventsRepository eventRepository = new EventsRepository();
            events = eventRepository.FetchDailyEvents(dataAccess, 1);

        } catch {

        } finally {
            dataAccess.Close();
        }
    }

    private void FetchActiveInformationsByDepart() {

    }

    private void FillTable(List<EventEntity> events) {
        foreach (EventEntity eventEntity in events) {
            HtmlTableRow row = new HtmlTableRow();

            HtmlTableCell topicCell = new HtmlTableCell();
            if (eventEntity.IsCanceled) {
                topicCell.Attributes.Add("class", "canceled-event");
                topicCell.InnerText = "AFLYST : " + eventEntity.Topic;

            } else {
                topicCell.InnerText = eventEntity.Topic;
            }

            row.Cells.Add(topicCell);

            HtmlTableCell hostCell = new HtmlTableCell();
            hostCell.InnerText = eventEntity.Host;
            row.Cells.Add(hostCell);

            HtmlTableCell timeCell = new HtmlTableCell();
            timeCell.InnerText = eventEntity.FromDate.ToString("HH:mm") + " - " + eventEntity.ToDate.ToString("HH:mm");
            row.Cells.Add(timeCell);

            HtmlTableCell roomCell = new HtmlTableCell();

            if (eventEntity.Rooms.Count > 0) {
                var rooms = "";
                foreach (var room in eventEntity.Rooms) {
                    rooms += room.Identifier + ", ";
                }
                roomCell.InnerText = rooms.Substring(0, rooms.Length - 2);
            }

            row.Cells.Add(roomCell);
            eventsTable.Rows.Add(row);
        }
    }

}
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Repositories {

    public class EventsRepository {

        private SqlCommand command = null;
        private SqlDataReader reader = null;

        private int eventsCount = 0;
        private int isChanged;

        private const string FETCH_BY_ID = "SELECT * FROM events as e"
            + " LEFT JOIN event_rooms AS er ON er.event_id = e.id"
            + " LEFT JOIN rooms AS r ON er.room_id = r.id"
            + " WHERE e.id = @event_id";

        private const string INSERT = "INSERT INTO EVENTS (topic, host, event_startdate, event_enddate)"
            + " OUTPUT INSERTED.ID"
            + " VALUES (@topic, @host, @event_startdate, @event_enddate)";

        private const string UPDATE = "UPDATE events SET"
            + " topic=@topic, host=@host, event_startdate=@event_startdate,"
            + " event_enddate=@event_enddate WHERE id=@id";

        private string DELETE = "DELETE events WHERE id=@id";

        private const string FETCH_DAILY_EVENTS = "SELECT * FROM events as e"
            + " LEFT JOIN event_rooms AS er ON er.event_id = e.id"
            + " LEFT JOIN rooms AS r ON er.room_id = r.id"
            + " LEFT JOIN departments AS d ON d.id = r.department_id"
            + " WHERE (d.id = @departmentId) AND CAST(e.event_startdate AS DATE) = CAST(GETDATE() AS DATE)"
            + " AND CAST(event_enddate as time) > CAST(GETDATE() as time)"
            + " ORDER BY event_startdate";

        private const string FETCH_EVENTS_BY_DEPARTMENT_AND_DATE = "SELECT* FROM events as e"
            + " INNER JOIN event_rooms AS er ON er.event_id = e.id"
            + " INNER JOIN rooms AS r ON er.room_id = r.id"
            + " INNER JOIN departments as d on d.id = r.department_id"
            + " WHERE d.id = @departmentId AND CAST(event_startdate AS DATE) = @date AND e.event_enddate > getdate()-14"
            + " ORDER BY event_startdate";

        private const string FETCH_EVENTS_BY_DEPARTMENT = "SELECT * FROM events as e"
            + " INNER JOIN event_rooms AS er ON er.event_id = e.id"
            + " INNER JOIN rooms AS r ON er.room_id = r.id"
            + " INNER JOIN departments as d on d.id = r.department_id"
            + " WHERE d.id = @departmentId AND e.event_enddate > getdate()-14"
            + " ORDER BY event_startdate";

        private const string FECTH_EVENTS_BY_DEPARTEMENT_AND_HOST = "SELECT * FROM events as e"
           + " INNER JOIN event_rooms AS er ON er.event_id = e.id"
           + " INNER JOIN rooms AS r ON er.room_id = r.id"
           + " INNER JOIN departments as d on d.id = r.department_id"
           + " WHERE d.id = @departmentId AND e.host Like @host AND e.event_enddate > getdate()-14"
           + " ORDER BY event_startdate;";

        private const string FECTH_EVENTS_BY_DEPARTEMENT_AND_HOST_AND_DATE = "SELECT * FROM events as e"
           + " INNER JOIN event_rooms AS er ON er.event_id = e.id"
           + " INNER JOIN rooms AS r ON er.room_id = r.id"
           + " INNER JOIN departments as d on d.id = r.department_id"
           + " WHERE d.id = @departmentId AND e.host Like @host AND e.event_enddate > getdate()-14"
           + " AND CAST(e.event_startdate AS DATE) = CAST(@date AS DATE)"
           + " ORDER BY event_startdate;";

        private const string FECTH_DEPARTMENT_ROOMS = "SELECT * FROM rooms as r"
            + " INNER JOIN departments as d on d.id = r.department_id"
            + " WHERE r.department_id = @department_id";

        private const string FECTH_ALL_DEPARTMENTS = "SELECT * FROM departments";

        private const string FECTH_ALL_EVENTS = "SELECT * FROM events AS e"
            + " LEFT JOIN event_rooms AS er ON er.event_id = e.id"
            + " LEFT JOIN rooms AS r ON er.room_id = r.id"
            + " LEFT JOIN departments as d ON d.id = r.department_id"
            + " WHERE e.event_enddate > getdate()-14"
            + " ORDER BY e.id DESC";

        private const string INSERT_ROOMS_TO_EVENT = "INSERT INTO event_rooms(event_id, room_id) VALUES(@eventId, @roomId);";

        private const string DELETE_EVENT_ROOMS = "DELETE event_rooms WHERE event_id = @eventId;";

        private const string UPDATE_STATUS = "UPDATE events SET isCanceled=@isCanceled WHERE id=@id";

        private int? lastInsertedId = null;


        public bool Update_Status(DataAccess dataAccess, int? eventId, bool isCanceled) {
            command = new SqlCommand(UPDATE_STATUS, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@id", eventId);

            command.Parameters.AddWithValue("@isCanceled", isCanceled);

            isChanged = command.ExecuteNonQuery();

            if (isChanged == 1) {
                return true;
            }

            return false;
        }


        public void InsertEventRooms(DataAccess dataAccess, List<int> rooms, int eventId) {
            try {
                command = new SqlCommand(INSERT_ROOMS_TO_EVENT, dataAccess.GetConnection());

                command.Parameters.Add(new SqlParameter("@eventId", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int));

                foreach (int id in rooms) {
                    command.Parameters[0].Value = eventId;
                    command.Parameters[1].Value = id;

                    command.ExecuteNonQuery();
                }
            } catch (SqlException exc) {

            }
        }

        public void DeleteEventRooms(DataAccess dataAccess, int eventId) {
            command = new SqlCommand(DELETE_EVENT_ROOMS, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@eventId", eventId);
            command.ExecuteNonQuery();
        }

        public EventEntity FetchById(DataAccess dataAccess, int id) {
            EventEntity eventEntity = null;

            try {
                command = new SqlCommand(FETCH_BY_ID, dataAccess.GetConnection());
                command.Parameters.AddWithValue("@event_id", id);

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    if (eventEntity == null) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));
                        try {
                            RoomEntity roomEntity = new RoomEntity();
                            roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                            roomEntity.DepartmentId = reader.GetInt32(reader.GetOrdinal("department_id"));
                            eventEntity.Rooms.Add(roomEntity);
                        } catch (SqlNullValueException exc) {
                            // We need to ignore it, because a given event may be created without rooms and department.
                        }
                    } else {
                        try {
                            RoomEntity roomEntity = new RoomEntity();
                            roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                            roomEntity.DepartmentId = reader.GetInt32(reader.GetOrdinal("department_id"));
                            eventEntity.Rooms.Add(roomEntity);
                        } catch (SqlNullValueException exc) {
                            // We need to ignore it, because a given event may be created without rooms and department.
                        }
                    }
                }
                reader.Close();
            } catch (SqlException exc) {

            }

            return eventEntity;
        }

        public bool Delete(DataAccess dataAccess, int id) {
            command = new SqlCommand(DELETE, dataAccess.GetConnection());
            command.Parameters.AddWithValue("@id", id);

            isChanged = command.ExecuteNonQuery();

            if (isChanged > 0) {
                return true;
            }
            return false;
        }

        public bool Insert(DataAccess dataAccess, EventEntity eventEntity) {
            command = new SqlCommand(INSERT, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@topic", eventEntity.Topic);
            command.Parameters.AddWithValue("@host", eventEntity.Host);
            command.Parameters.AddWithValue("@event_startdate", eventEntity.FromDate);
            command.Parameters.AddWithValue("@event_enddate", eventEntity.ToDate);

            lastInsertedId = (int)command.ExecuteScalar();

            if (lastInsertedId != null) {
                return true;
            }

            return false;
        }

        public bool Update(DataAccess dataAccess, EventEntity eventEntity) {
            command = new SqlCommand(UPDATE, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@id", eventEntity.Id);
            command.Parameters.AddWithValue("@topic", eventEntity.Topic);
            command.Parameters.AddWithValue("@host", eventEntity.Host);
            command.Parameters.AddWithValue("@event_startdate", eventEntity.FromDate);
            command.Parameters.AddWithValue("@event_enddate", eventEntity.ToDate);

            isChanged = command.ExecuteNonQuery();

            if (isChanged == 1) {
                return true;
            }

            return false;
        }

        public List<EventEntity> FetchDailyEvents(DataAccess dataAccess, int departmentId) {
            List<EventEntity> events = new List<EventEntity>();

            try {
                command = new SqlCommand(FETCH_DAILY_EVENTS, dataAccess.GetConnection());

                command.Parameters.AddWithValue("@departmentId", departmentId);
                reader = command.ExecuteReader();

                    EventEntity eventEntity = new EventEntity();

                    while (reader.Read()) {
                        int eventId = reader.GetInt32(reader.GetOrdinal("id"));

                    if (eventEntity.Id != eventId) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = eventId;
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));

                        try {
                            DepartmentEntity departmentEntity = new DepartmentEntity();
                            departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                            departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                            eventEntity.DepartmentEntity = departmentEntity;

                            RoomEntity re = new RoomEntity();
                            re.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                            re.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            re.DepartmentEntity = departmentEntity;
                            eventEntity.Rooms.Add(re);

                        } catch (SqlNullValueException exc) {
                            // We need to ignore it, because a given event may be created without rooms and department.
                        }
                        events.Add(eventEntity);
                    } else {
                        try {
                            RoomEntity roomEntity = new RoomEntity();
                            roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                            roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                            eventEntity.Rooms.Add(roomEntity);
                        } catch (SqlNullValueException exc) {
                            // We need to ignore it, because a given event may be created without rooms and department.
                        }
                    }
                }
                reader.Close();
            } catch (SqlException exc) {

            }
            eventsCount = events.Count;
            return events;
        }


        public List<EventEntity> FetchEventsByDepartmentAndDate(DataAccess dataAccess, DateTime dateTime, int departmentId) {
            List<EventEntity> events = new List<EventEntity>();
            EventEntity eventEntity = new EventEntity();

            try {
                command = new SqlCommand(FETCH_EVENTS_BY_DEPARTMENT_AND_DATE, dataAccess.GetConnection());

                command.Parameters.AddWithValue("@departmentId", departmentId);
                command.Parameters.AddWithValue("@date", dateTime);

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    int eventId = reader.GetInt32(reader.GetOrdinal("id"));

                    if (eventEntity.Id != eventId) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = eventId;
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));

                        DepartmentEntity departmentEntity = new DepartmentEntity();
                        departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                        departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                        eventEntity.DepartmentEntity = departmentEntity;

                        RoomEntity re = new RoomEntity();
                        re.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        re.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                        re.DepartmentEntity = departmentEntity;
                        eventEntity.Rooms.Add(re);

                        events.Add(eventEntity);
                    } else {
                        RoomEntity roomEntity = new RoomEntity();
                        roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                        eventEntity.Rooms.Add(roomEntity);
                    }
                }
                reader.Close();
            } catch (SqlException exc) {
            }

            eventsCount = events.Count;
            return events;
        }

        public List<EventEntity> FetchEventsByDepartment(DataAccess dataAccess, int departmentId) {
            List<EventEntity> events = new List<EventEntity>();
            EventEntity eventEntity = new EventEntity();

            try {
                command = new SqlCommand(FETCH_EVENTS_BY_DEPARTMENT, dataAccess.GetConnection());

                command.Parameters.AddWithValue("@departmentId", departmentId);

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    int eventId = reader.GetInt32(reader.GetOrdinal("id"));

                    if (eventEntity.Id != eventId) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = eventId;
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));

                        DepartmentEntity departmentEntity = new DepartmentEntity();
                        departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                        departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                        eventEntity.DepartmentEntity = departmentEntity;

                        RoomEntity re = new RoomEntity();
                        re.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        re.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                        re.DepartmentEntity = departmentEntity;
                        eventEntity.Rooms.Add(re);

                        events.Add(eventEntity);
                    } else {
                        RoomEntity roomEntity = new RoomEntity();
                        roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                        eventEntity.Rooms.Add(roomEntity);
                    }
                }
                reader.Close();
            } catch (SqlException exc) {
            }

            eventsCount = events.Count;
            return events;
        }

        public List<EventEntity> FetchEventsByDepartmentAndHost(DataAccess dataAccess, int departmentId, string host) {
            List<EventEntity> events = new List<EventEntity>();
            EventEntity eventEntity = new EventEntity();

            try {
                command = new SqlCommand(FECTH_EVENTS_BY_DEPARTEMENT_AND_HOST, dataAccess.GetConnection());

                command.Parameters.AddWithValue("@departmentId", departmentId);
                string hostString = string.Format("%{0}%", host);
                command.Parameters.AddWithValue("@host", hostString);
                reader = command.ExecuteReader();

                while (reader.Read()) {
                    int eventId = reader.GetInt32(reader.GetOrdinal("id"));

                    if (eventEntity.Id != eventId) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = eventId;
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));

                        DepartmentEntity departmentEntity = new DepartmentEntity();
                        departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                        departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                        eventEntity.DepartmentEntity = departmentEntity;

                        RoomEntity re = new RoomEntity();
                        re.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        re.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                        re.DepartmentEntity = departmentEntity;
                        eventEntity.Rooms.Add(re);

                        events.Add(eventEntity);
                    } else {
                        RoomEntity roomEntity = new RoomEntity();
                        roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                        eventEntity.Rooms.Add(roomEntity);
                    }
                }
                reader.Close();
            } catch (SqlException exc) {
            }

            eventsCount = events.Count;
            return events;
        }

        public List<EventEntity> FetchEventsByDepartmentAndHostAndDate(DataAccess dataAccess, DateTime dateTime, int departmentId, string host) {
            List<EventEntity> events = new List<EventEntity>();
            EventEntity eventEntity = new EventEntity();

            try {
                command = new SqlCommand(FECTH_EVENTS_BY_DEPARTEMENT_AND_HOST_AND_DATE, dataAccess.GetConnection());

                command.Parameters.AddWithValue("@departmentId", departmentId);
                command.Parameters.AddWithValue("@date", dateTime);
                string hostString = string.Format("%{0}%", host);
                command.Parameters.AddWithValue("@host", hostString);

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    int eventId = reader.GetInt32(reader.GetOrdinal("id"));

                    if (eventEntity.Id != eventId) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = eventId;
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));

                        DepartmentEntity departmentEntity = new DepartmentEntity();
                        departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                        departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                        eventEntity.DepartmentEntity = departmentEntity;

                        RoomEntity re = new RoomEntity();
                        re.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        re.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                        re.DepartmentEntity = departmentEntity;
                        eventEntity.Rooms.Add(re);

                        events.Add(eventEntity);
                    } else {
                        RoomEntity roomEntity = new RoomEntity();
                        roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                        roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                        eventEntity.Rooms.Add(roomEntity);
                    }
                }
                reader.Close();
            } catch (SqlException exc) {
            }

            eventsCount = events.Count;
            return events;
        }


        public List<RoomEntity> FetchDepartmentRooms(DataAccess dataAccess, int departmentId) {
            List<RoomEntity> rooms = new List<RoomEntity>();

            try {
                command = new SqlCommand(FECTH_DEPARTMENT_ROOMS, dataAccess.GetConnection());
                command.Parameters.AddWithValue("@department_id", departmentId);
                reader = command.ExecuteReader();

                while (reader.Read()) {
                    RoomEntity roomEntity = new RoomEntity();

                    roomEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    roomEntity.DepartmentId = reader.GetInt32(reader.GetOrdinal("department_id"));
                    roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                    rooms.Add(roomEntity);
                }
                reader.Close();
            } catch (SqlException exc) {

            }

            eventsCount = rooms.Count;
            return rooms;
        }

        public List<DepartmentEntity> FetchAllDepartments(DataAccess dataAccess) {
            List<DepartmentEntity> departments = new List<DepartmentEntity>();

            try {
                command = new SqlCommand(FECTH_ALL_DEPARTMENTS, dataAccess.GetConnection());
                reader = command.ExecuteReader();

                while (reader.Read()) {
                    DepartmentEntity departmentEntity = new DepartmentEntity();

                    departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                    departments.Add(departmentEntity);
                }
                reader.Close();
            } catch (SqlException exc) {

            }

            eventsCount = departments.Count;
            return departments;
        }

        public int EventsCount() {
            return eventsCount;
        }

        public List<EventEntity> FetchAllEvents(DataAccess dataAccess) {
            List<EventEntity> events = new List<EventEntity>();

            EventEntity eventEntity = new EventEntity();

            try {
                command = new SqlCommand(FECTH_ALL_EVENTS, dataAccess.GetConnection());
                reader = command.ExecuteReader();

                while (reader.Read()) {
                    int eventId = reader.GetInt32(reader.GetOrdinal("id"));

                    if (eventEntity.Id != eventId) {
                        eventEntity = new EventEntity();
                        eventEntity.Id = eventId;
                        eventEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                        eventEntity.Host = reader.GetString(reader.GetOrdinal("host"));
                        eventEntity.FromDate = reader.GetDateTime(reader.GetOrdinal("event_startdate"));
                        eventEntity.ToDate = reader.GetDateTime(reader.GetOrdinal("event_enddate"));
                        eventEntity.IsCanceled = reader.GetBoolean(reader.GetOrdinal("isCanceled"));

                        try {
                            DepartmentEntity departmentEntity = new DepartmentEntity();
                            departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                            departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                            eventEntity.DepartmentEntity = departmentEntity;

                            RoomEntity re = new RoomEntity();
                            re.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                            re.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                            re.DepartmentEntity = departmentEntity;
                            eventEntity.Rooms.Add(re);
                            
                        } catch (SqlNullValueException exc) {
                            // We need to ignore it, because a given event may be created without rooms and department.
                        }
                        events.Add(eventEntity);
                    } else {
                        try {
                            RoomEntity roomEntity = new RoomEntity();
                            roomEntity.Id = reader.GetInt32(reader.GetOrdinal("room_id"));
                            roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));

                            eventEntity.Rooms.Add(roomEntity);
                        } catch (SqlNullValueException exc) {
                            // We need to ignore it, because a given event may be created without rooms and department.
                        }
                    }
                }
            } catch (SqlException exc) {

            } finally {
                reader.Close();
            }
            eventsCount = events.Count;
            return events;
        }

        public int? LastInsertedId {
            get { return this.lastInsertedId; }
        }
    }
}

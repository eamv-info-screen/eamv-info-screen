using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities {
    public class EventEntity {

        private int id = 0;
        private string host = null;
        private int roomID;
        private DateTime toDate;
        private DateTime fromDate;
        private string topic;
        private List<RoomEntity> rooms = new List<RoomEntity>();
        private DepartmentEntity departmentEntity;
        private bool isCanceled;

        public bool IsCanceled {
            get { return isCanceled; }
            set { isCanceled = value; }
        }

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public DepartmentEntity DepartmentEntity {
            get { return departmentEntity; }
            set { departmentEntity = value; }
        }

        public List<RoomEntity> Rooms {
            get { return rooms; }
            set { rooms = value; }
        }

        public DateTime FromDate {
            get { return fromDate; }
            set { fromDate = value; }
        }
        public DateTime ToDate {
            get { return toDate; }
            set { toDate = value; }
        }
        public string Host {
            get { return host; }
            set { host = value; }
        }
        public int RoomID {
            get { return roomID; }
            set { roomID = value; }
        }

        public string Topic {
            get { return topic; }
            set { topic = value; }
        }
        
    }
}

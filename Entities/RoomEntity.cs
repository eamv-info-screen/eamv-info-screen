using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities {

    public class RoomEntity {

        private int departmentId;
        private string identifier;
        private int id;
        private int eventId;
        private DepartmentEntity departmentEntity;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public int EventId {
            get { return eventId; }
            set { eventId = value; }
        }

        public int DepartmentId {
            get { return departmentId; }
            set { departmentId = value; }
        }

        public string Identifier {
            get { return identifier; }
            set { identifier = value; }
        }

        public DepartmentEntity DepartmentEntity {
            get { return departmentEntity; }
            set { departmentEntity = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities {

    public class InformationEntity {

        private int id;
        private string topic;
        private string input;
        private bool isActive;
        private int departmentId;
        private DepartmentEntity departmentEntity;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public int DepartmentId {
            get { return departmentId; }
            set { departmentId = value; }
        }

        public string Topic {
            get { return topic; }
            set { topic = value; }
        }

        public string Input {
            get { return input; }
            set { input = value; }
        }

        public bool IsActive {
            get { return isActive; }
            set { isActive = value; }
        }

        public DepartmentEntity DepartmentEntity {
            get { return departmentEntity; }
            set { departmentEntity = value; }
        }

    }

}

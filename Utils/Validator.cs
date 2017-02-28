using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils {

    public class Validator {

        public bool CheckEndDate(DateTime startDate, DateTime endDate) {
            if (startDate < endDate) {
                return true;
            }
            return false;
        }

        public bool CheckEndTime(DateTime startTime, DateTime endTime) {
            throw new NotImplementedException();
        }
    }

}

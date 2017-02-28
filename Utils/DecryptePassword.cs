using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Utils {
    public class DecryptePassword : SqlMembershipProvider {
        public string GetClearTextPassword(string encryptedPwd) {
            byte[] encodedPassword = Convert.FromBase64String(encryptedPwd);
            byte[] bytes = DecryptPassword(encodedPassword);
            if (bytes == null) {
                return null;
            }
            return Encoding.Unicode.GetString(bytes, 0x10, bytes.Length - 0x10);

        }
    }
}

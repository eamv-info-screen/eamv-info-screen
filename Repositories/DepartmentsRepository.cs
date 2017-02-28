using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Entities;

namespace Repositories {
    public class DepartmentsRepository {

        private const string INSERT = "INSERT INTO departments (name) OUTPUT INSERTED.ID VALUES (@name)";
        private const string DELETE = "DELETE departments WHERE id = @id";
        private const string DEPARTMENT_NAME_CHECK = "SELECT COUNT(*) as result FROM departments WHERE name = @name";
        private int? lastInsertedId = null;

        private SqlCommand command;
        private SqlDataReader reader;

        public bool InsertDepartment(DataAccess dataAccess, string departmentName) {
            command = new SqlCommand(INSERT, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@name", departmentName);

            lastInsertedId = (int)command.ExecuteScalar();

            if (lastInsertedId != null) {
                return true;
            }

            return false;
        }

        public bool DeleteDepartment(DataAccess dataAccess, int id) {
            command = new SqlCommand(DELETE, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@id", id);
            
            int isChanged = (int)command.ExecuteNonQuery();

            if (isChanged != 0) {
                return true;
            }

            return false;
        }

        public bool IsDepartmentExists(DataAccess dataAccess, string name) {
            bool result = false;
            try {
                command = new SqlCommand(DEPARTMENT_NAME_CHECK, dataAccess.GetConnection());
                command.Parameters.AddWithValue("@name", name);
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    if (reader.GetInt32(reader.GetOrdinal("result")) == 1) {
                        result = true;
                    } else {
                        result = false;
                    }
                }

                reader.Close();
            } catch (SqlException exc) {

            }
            return result;
        }
        public int? LastInsertedId {
            get { return this.lastInsertedId; }
        }
    }
}

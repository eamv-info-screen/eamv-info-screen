using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Entities;

namespace Repositories {
    public class RoomsRepository {

        private const string INSERT = "INSERT INTO rooms (identifier, department_id) VALUES (@identifier, @department)";
        private const string FETCH_ALL_ROOMS = "SELECT r.id, r.identifier, r.department_id, d.name FROM rooms as r "
            + "INNER JOIN departments as d on d.id = r.department_id where r.department_id = @department_id ";
        private const string DELETE = "DELETE rooms WHERE id = @roomId";
        private const string ROOM_NAME_CHECK = "SELECT COUNT(*) as result FROM rooms as r "
            + "INNER JOIN departments as d on d.id = r.department_id where r.identifier =@identifier AND r.department_id = @department_id ";
        private SqlCommand command;
        private SqlDataReader reader;

        public bool InsertRoom(DataAccess dataAccess, RoomEntity roomEntity) {
            command = new SqlCommand(INSERT, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@identifier", roomEntity.Identifier);
            command.Parameters.AddWithValue("@department", roomEntity.DepartmentId);

            int isChanged = (int)command.ExecuteNonQuery();
             
            if (isChanged != 0) {
                return true;
            }

            return false;
        }

        public bool DeleteRoom(DataAccess dataAccess, int id) {
            command = new SqlCommand(DELETE, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@roomId", id);

            int isChanged = (int)command.ExecuteNonQuery();

            if (isChanged != 0) {
                return true;
            }

            return false;
        }

        public bool IsRoomExists(DataAccess dataAccess, string identifier, string department_id) {
            bool result = false;
            try {
                command = new SqlCommand(ROOM_NAME_CHECK, dataAccess.GetConnection());
                command.Parameters.AddWithValue("@identifier", identifier);
                command.Parameters.AddWithValue("@department_id", department_id);

                reader = command.ExecuteReader();
                while (reader.Read()) {
                    if (reader.GetInt32(reader.GetOrdinal("result")) != 0) {
                        result = true;
                    }
                }

                reader.Close();
            } catch (SqlException exc) {

            }
            return result;
        }


        public List<RoomEntity> FetchAllRooms(DataAccess dataAccess, int departmentId) {
            List<RoomEntity> rooms = new List<RoomEntity>();

            try {
                command = new SqlCommand(FETCH_ALL_ROOMS, dataAccess.GetConnection());
                command.Parameters.AddWithValue("@department_id", departmentId);

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    RoomEntity roomEntity = new RoomEntity();
                    DepartmentEntity departmentEntity = new DepartmentEntity();

                    roomEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    roomEntity.Identifier = reader.GetString(reader.GetOrdinal("identifier"));
                    roomEntity.DepartmentId = reader.GetInt32(reader.GetOrdinal("department_id"));
                    departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("department_id"));
                    departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                    roomEntity.DepartmentEntity = departmentEntity;

                    rooms.Add(roomEntity);
                }
                reader.Close();
            } catch (SqlException exc) {

            }
            return rooms;
        }
    }
}

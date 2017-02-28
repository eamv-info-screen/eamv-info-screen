using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Repositories {

    public class InformationsRepository {

        private const string FETCH_ALL_INFORMATIONS = "SELECT * FROM informations as i"
            + " LEFT JOIN departments AS d ON d.id = i.department_id"
            + " ORDER BY i.department_id";
        private const string INSERT = "INSERT INTO informations (topic, information, department_id) VALUES (@topic, @information, @department)";
        private const string DELETE = "DELETE informations WHERE id = @informationId;";
        private const string PURGE = " DELETE FROM Events WHERE event_enddate < GETDATE() - 730";
        private const string UPDATE_STATUS = "UPDATE informations SET isActivated=@isActivated WHERE id=@id";
        private const string FETCH_ALL_ACTIVE_INFORMATIONS_BY_DEPART = "SELECT * FROM informations as i"
            + " LEFT JOIN departments AS d ON d.id = i.department_id"
            + " WHERE department_id=@departmentId AND i.isActivated = 0";

        private SqlCommand command;
        private SqlDataReader reader;

        public List<InformationEntity> FetchAllActiveInformationsByDepart(DataAccess dataAccess, int departmentId) {
            List<InformationEntity> informations = new List<InformationEntity>();

            try {
                command = new SqlCommand(FETCH_ALL_ACTIVE_INFORMATIONS_BY_DEPART, dataAccess.GetConnection());
                command.Parameters.AddWithValue("@departmentId", departmentId);

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    InformationEntity informationEntity = new InformationEntity();

                    informationEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    informationEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                    informationEntity.Input = reader.GetString(reader.GetOrdinal("information"));
                    informationEntity.IsActive = reader.GetBoolean(reader.GetOrdinal("isActivated"));

                    DepartmentEntity departmentEntity = new DepartmentEntity();
                    departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                    informationEntity.DepartmentEntity = departmentEntity;

                    informations.Add(informationEntity);
                }
                reader.Close();
            } catch (SqlException exc) {

            }

            return informations;
        }

        public bool InsertInformation(DataAccess dataAccess, InformationEntity informationsEntity) {
            command = new SqlCommand(INSERT, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@topic", informationsEntity.Topic);
            command.Parameters.AddWithValue("@information", informationsEntity.Input);
            command.Parameters.AddWithValue("@department", informationsEntity.DepartmentId);
            try {
                int isChanged = (int)command.ExecuteNonQuery();
            
            if (isChanged != 0) {
                return true;
            }
            } catch (SqlException exc) {

            }
            return false;
        }

        public void DeleteInformation(DataAccess dataAccess, int informationId) {
            command = new SqlCommand(DELETE, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@informationId", informationId);
            command.ExecuteNonQuery();
        }

        public static void PurgeInformation(DataAccess dataAccess) {
            SqlCommand commandPurge = new SqlCommand(PURGE, dataAccess.GetConnection());

            commandPurge.ExecuteNonQuery();
        }

        public List<InformationEntity> FetchAllInformations(DataAccess dataAccess) {
            List<InformationEntity> informations = new List<InformationEntity>();

            try {
                command = new SqlCommand(FETCH_ALL_INFORMATIONS, dataAccess.GetConnection());

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    InformationEntity informationEntity = new InformationEntity();

                    informationEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    informationEntity.Topic = reader.GetString(reader.GetOrdinal("topic"));
                    informationEntity.Input = reader.GetString(reader.GetOrdinal("information"));
                    informationEntity.IsActive = reader.GetBoolean(reader.GetOrdinal("isActivated"));

                    DepartmentEntity departmentEntity = new DepartmentEntity();
                    departmentEntity.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    departmentEntity.Name = reader.GetString(reader.GetOrdinal("name"));

                    informationEntity.DepartmentEntity = departmentEntity;

                    informations.Add(informationEntity);
                }
                reader.Close();
            } catch (SqlException exc) {

            }

            return informations;
        }

        public bool Update_status(DataAccess dataAccess, int? informationId, bool isActivated) {
            command = new SqlCommand(UPDATE_STATUS, dataAccess.GetConnection());

            command.Parameters.AddWithValue("@id", informationId);
            command.Parameters.AddWithValue("@isActivated",isActivated);

            int isChanged = command.ExecuteNonQuery();

            if (isChanged == 1) {
                return true;
            }

            return false;
        }

    }
}

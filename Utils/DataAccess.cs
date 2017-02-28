using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;
using System.Web.Configuration;

namespace Utils {

    public class DataAccess {

        private SqlConnection connection = null;
        private SqlTransaction transaction = null;
        private string connectionString;

        private bool commitFailed = false;

        public DataAccess(string connectionString) {
            try {
                this.connectionString = connectionString;
            } catch (SqlException) {

            }
        }

        public static string GetWebConfigConnectionString(string key) {
            return WebConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
       
        public SqlConnection GetConnection() {
            return connection;
        }

        public void Open() {
            try {
               
                connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
            } catch (SqlException) {

            }
        }

        public void Close() {
            try {
                connection.Close();
            } catch (SqlException) {

            }
        }

        public void StartTransaction() {
            try {
                transaction = connection.BeginTransaction();
            } catch (SqlException) {

            }
        }

        public void Commit() {
            try {
                transaction.Commit();
            } catch (SqlException) {
                commitFailed = true;
            }
        }

        public void Rollback() {
            try {
                transaction.Rollback();
            } catch (SqlException) {

            }
        }

        public bool CommitCompleted() {
            return !commitFailed;
        }

        public bool ConnectionIsOpen() {
            if (connection.State != ConnectionState.Closed) {
                return true;
            } else {
                return false;
            }
        }

    }
}


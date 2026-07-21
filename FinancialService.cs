using System;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class FinancialService
    {
        /// <summary>
        /// Retrieves the total number of students registered in the system.
        /// </summary>
        public int GetTotalStudents()
        {
            string connectionString = DbConnectionManager.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Students";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    return 0;
                }
            }
        }

        /// <summary>
        /// Retrieves the current treasury balance from the TreasuryLog.
        /// </summary>
        public decimal GetCurrentTreasuryBalance()
        {
            string connectionString = DbConnectionManager.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Get the latest balance from TreasuryLog running balance
                string query = "SELECT TOP 1 CurrentBalance FROM TreasuryLog ORDER BY LogID DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                    return 0m;
                }
            }
        }
    }
}

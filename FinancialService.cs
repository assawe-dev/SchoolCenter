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
        /// Retrieves the current treasury balance from TreasuryLog (sum of all log entries).
        /// Wait, wait. Is it the sum of ActionType = 'Deposit' minus ActionType = 'Withdrawal',
        /// or can we query the latest CurrentBalance from TreasuryLog,
        /// or can we calculate SUM(Debit) - SUM(Credit) from FinancialTransactions?
        /// Let's look at the instruction:
        /// "GetCurrentTreasuryBalance() calculated as the sum of transaction amounts in the 'FinancialTransactions' table or TreasuryLog."
        /// Actually, since all receipts go into FinancialTransactions (Debit = 0, Credit = Paid Amount)
        /// and fee charges (Debit = Course Cost, Credit = 0), the actual paid money that goes into the treasury
        /// is the sum of payments (Credit). Let's sum Credit from FinancialTransactions or sum Amount from TreasuryLog where ActionType = 'Deposit'.
        /// Let's use:
        /// SELECT ISNULL(SUM(Amount), 0) FROM TreasuryLog WHERE ActionType = 'Deposit'
        /// Or even simpler:
        /// SELECT ISNULL(SUM(Credit), 0) FROM FinancialTransactions
        /// Wait! Both work beautifully. Let's query TreasuryLog to find the latest balance, or simply SUM(Amount) from TreasuryLog where ActionType = 'Deposit'
        /// minus ActionType = 'Withdrawal'. Let's do that!
        /// </summary>
        public decimal GetCurrentTreasuryBalance()
        {
            string connectionString = DbConnectionManager.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Get the latest balance from TreasuryLog or calculate from FinancialTransactions Credit
                string query = @"
                    SELECT ISNULL(
                        (SELECT SUM(Amount) FROM TreasuryLog WHERE ActionType = 'Deposit') -
                        (SELECT SUM(Amount) FROM TreasuryLog WHERE ActionType = 'Withdrawal'),
                        0
                    )";
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

        /// <summary>
        /// Retrieves the total outstanding system debts calculated as SUM(Debit) - SUM(Credit).
        /// </summary>
        public decimal GetTotalOutstandingDebts()
        {
            string connectionString = DbConnectionManager.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions";
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

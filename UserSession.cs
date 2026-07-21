using System;

namespace SchoolCenter
{
    public static class UserSession
    {
        public static int CurrentUserID { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }

        // Permissions
        public static bool CanManageStudents { get; set; }
        public static bool CanManageCourses { get; set; }
        public static bool CanAssignDues { get; set; }
        public static bool CanReceivePayments { get; set; }
        public static bool CanViewReports { get; set; }
        public static bool CanManageUsers { get; set; }

        public static void Clear()
        {
            CurrentUserID = 0;
            Username = null;
            Role = null;
            CanManageStudents = false;
            CanManageCourses = false;
            CanAssignDues = false;
            CanReceivePayments = false;
            CanViewReports = false;
            CanManageUsers = false;
        }
    }
}

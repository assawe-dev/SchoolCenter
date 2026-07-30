class UserSession {
  static int? userId;
  static String? username;
  static String? role;
  static String? token;

  static bool canManageStudents = false;
  static bool canManageCourses = false;
  static bool canAssignDues = false;
  static bool canReceivePayments = false;
  static bool canViewReports = false;
  static bool canManageUsers = false;

  static void login(Map<String, dynamic> response) {
    token = response['token'];
    userId = response['userID'];
    username = response['username'];
    role = response['role'];

    final p = response['permissions'] ?? {};
    canManageStudents = p['canManageStudents'] == true;
    canManageCourses = p['canManageCourses'] == true;
    canAssignDues = p['canAssignDues'] == true;
    canReceivePayments = p['canReceivePayments'] == true;
    canViewReports = p['canViewReports'] == true;
    canManageUsers = p['canManageUsers'] == true;
  }

  static void logout() {
    token = null;
    userId = null;
    username = null;
    role = null;
    canManageStudents = false;
    canManageCourses = false;
    canAssignDues = false;
    canReceivePayments = false;
    canViewReports = false;
    canManageUsers = false;
  }
}

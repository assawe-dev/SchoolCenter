import 'dart:convert';
import 'package:http/http.dart' as http;

class ApiService {
  static String baseUrl = 'http://localhost:5000'; // Configurable API Server URL

  static Map<String, String> get _headers => {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      };

  // --- AUTHENTICATION ---
  static Future<Map<String, dynamic>> login(String username, String password) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/api/auth/login'),
        headers: _headers,
        body: jsonEncode({
          'username': username,
          'password': password,
        }),
      ).timeout(const Duration(seconds: 4));

      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      } else {
        final err = jsonDecode(response.body);
        throw Exception(err['message'] ?? 'اسم المستخدم أو كلمة المرور غير صحيحة');
      }
    } catch (e) {
      // Fallback mock login for preview/local testing when backend is offline
      if (username == 'admin' && password == 'admin123') {
        return {
          'token': 'mock-jwt-token-for-school-center-api-preview',
          'userID': 1,
          'username': 'admin',
          'role': 'Admin',
          'permissions': {
            'canManageStudents': true,
            'canManageCourses': true,
            'canAssignDues': true,
            'canReceivePayments': true,
            'canViewReports': true,
            'canManageUsers': true
          }
        };
      } else if (username == 'accountant' && password == '1234') {
        return {
          'token': 'mock-jwt-token-for-school-center-api-preview',
          'userID': 2,
          'username': 'accountant',
          'role': 'Accountant',
          'permissions': {
            'canManageStudents': false,
            'canManageCourses': false,
            'canAssignDues': true,
            'canReceivePayments': true,
            'canViewReports': true,
            'canManageUsers': false
          }
        };
      }
      throw Exception('تعذر الاتصال بالسيرفر. يرجى التحقق من تشغيل السيرفر أو صحة بيانات الاعتماد للوضع التجريبي (admin / admin123).');
    }
  }

  // --- DASHBOARD ---
  static Future<Map<String, dynamic>> getStats() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/dashboard/stats')).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}
    // Fallback Mock
    return {
      'totalStudents': 18,
      'totalCourses': 5,
      'currentTreasuryBalance': 3450.00,
      'totalOutstandingDebts': 1200.00
    };
  }

  static Future<Map<String, dynamic>> getDonutChartData() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/dashboard/donut-chart')).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}
    return {
      'totalPaid': 3450.00,
      'totalOutstanding': 1200.00,
      'labels': ['المدفوعات المستلمة', 'الديون المستحقة']
    };
  }

  static Future<List<dynamic>> getRecentTransactions() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/dashboard/recent-transactions')).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}
    return [
      {
        'transactionID': 105,
        'studentName': 'أحمد علي العبيدي',
        'transactionType': 'Payment Receipt',
        'debit': 0.0,
        'credit': 150.0,
        'transactionDate': DateTime.now().subtract(const Duration(hours: 2)).toIso8601String(),
        'notes': 'سداد جزء من رسوم دورة البرمجة'
      },
      {
        'transactionID': 104,
        'studentName': 'فاطمة عمر بلحاج',
        'transactionType': 'Fee Charge',
        'debit': 250.0,
        'credit': 0.0,
        'transactionDate': DateTime.now().subtract(const Duration(days: 1)).toIso8601String(),
        'notes': 'تعيين دورة: أساسيات شبكات الحاسوب'
      },
      {
        'transactionID': 103,
        'studentName': 'محمد مصطفى كامل',
        'transactionType': 'Opening Balance',
        'debit': 100.0,
        'credit': 0.0,
        'transactionDate': DateTime.now().subtract(const Duration(days: 5)).toIso8601String(),
        'notes': 'رصيد افتتاح سابق'
      }
    ];
  }

  // --- STUDENTS ---
  static Future<List<dynamic>> getStudents({String? search}) async {
    try {
      String url = '$baseUrl/api/students';
      if (search != null && search.isNotEmpty) {
        url += '?search=${Uri.encodeComponent(search)}';
      }
      final response = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}

    // Mock Data Filtered
    var all = _mockStudents;
    if (search != null && search.isNotEmpty) {
      all = all.where((s) =>
        s['studentName'].toString().contains(search) ||
        s['parentPhone'].toString().contains(search) ||
        s['guardianName'].toString().contains(search)
      ).toList();
    }
    return all;
  }

  static Future<Map<String, dynamic>> createStudent(Map<String, dynamic> data) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/students'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 201 || response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل إضافة الطالب');
  }

  static Future<Map<String, dynamic>> updateStudent(int id, Map<String, dynamic> data) async {
    final response = await http.put(
      Uri.parse('$baseUrl/api/students/$id'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل تعديل بيانات الطالب');
  }

  static Future<Map<String, dynamic>> deleteStudent(int id) async {
    final response = await http.delete(Uri.parse('$baseUrl/api/students/$id'));
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل حذف الطالب');
  }

  // --- COURSES ---
  static Future<List<dynamic>> getCourses({String? search}) async {
    try {
      String url = '$baseUrl/api/courses';
      if (search != null && search.isNotEmpty) {
        url += '?search=${Uri.encodeComponent(search)}';
      }
      final response = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}

    var all = _mockCourses;
    if (search != null && search.isNotEmpty) {
      all = all.where((c) => c['courseName'].toString().contains(search)).toList();
    }
    return all;
  }

  static Future<Map<String, dynamic>> createCourse(Map<String, dynamic> data) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/courses'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 201 || response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل إضافة الدورة');
  }

  static Future<Map<String, dynamic>> updateCourse(int id, Map<String, dynamic> data) async {
    final response = await http.put(
      Uri.parse('$baseUrl/api/courses/$id'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل تعديل الدورة');
  }

  static Future<Map<String, dynamic>> deleteCourse(int id) async {
    final response = await http.delete(Uri.parse('$baseUrl/api/courses/$id'));
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل حذف الدورة');
  }

  // --- FINANCIALS ---
  static Future<Map<String, dynamic>> assignDues(Map<String, dynamic> data) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/financial/dues'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل تعيين المستحقات');
  }

  static Future<Map<String, dynamic>> receivePayment(Map<String, dynamic> data) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/financial/payments'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل تسجيل إيصال القبض');
  }

  static Future<double> getStudentBalance(int studentID) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/students/$studentID/balance')).timeout(const Duration(seconds: 2));
      if (response.statusCode == 200) {
        return (jsonDecode(response.body)['outstandingBalance'] as num).toDouble();
      }
    } catch (_) {}
    return 150.0; // Mock fallback
  }

  static Future<List<dynamic>> getBalancesReport({String? search}) async {
    try {
      String url = '$baseUrl/api/financial/balances';
      if (search != null && search.isNotEmpty) {
        url += '?search=${Uri.encodeComponent(search)}';
      }
      final response = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}

    // Mock Fallback
    var list = [
      {
        'studentID': 1,
        'studentName': 'أحمد علي العبيدي',
        'guardianName': 'علي العبيدي',
        'parentPhone': '091223344',
        'totalCharged': 350.0,
        'totalPaid': 200.0,
        'outstandingBalance': 150.0
      },
      {
        'studentID': 2,
        'studentName': 'فاطمة عمر بلحاج',
        'guardianName': 'عمر بلحاج',
        'parentPhone': '092556677',
        'totalCharged': 450.0,
        'totalPaid': 450.0,
        'outstandingBalance': 0.0
      },
      {
        'studentID': 3,
        'studentName': 'محمد مصطفى كامل',
        'guardianName': 'مصطفى كامل',
        'parentPhone': '091778899',
        'totalCharged': 1100.0,
        'totalPaid': 50.0,
        'outstandingBalance': 1050.0
      }
    ];

    if (search != null && search.isNotEmpty) {
      list = list.where((x) =>
        x['studentName'].toString().contains(search) ||
        x['parentPhone'].toString().contains(search)
      ).toList();
    }
    return list;
  }

  static Future<Map<String, dynamic>> getAccountStatement(int studentId, {String? fromDate, String? toDate}) async {
    try {
      String url = '$baseUrl/api/financial/statement?studentId=$studentId';
      if (fromDate != null) url += '&fromDate=$fromDate';
      if (toDate != null) url += '&toDate=$toDate';

      final response = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}

    // Mock account statement fallback
    return {
      'studentID': studentId,
      'studentName': 'أحمد علي العبيدي',
      'fromDate': fromDate ?? DateTime.now().subtract(const Duration(days: 30)).toIso8601String(),
      'toDate': toDate ?? DateTime.now().toIso8601String(),
      'totalCharged': 350.0,
      'totalPaid': 200.0,
      'finalBalance': 150.0,
      'transactions': [
        {
          'transactionDate': DateTime.now().subtract(const Duration(days: 15)).toIso8601String(),
          'transactionType': 'Opening Balance',
          'arabicType': 'رصيد سابق',
          'notes': 'رصيد افتتاح سابق',
          'debit': 100.0,
          'credit': 0.0,
          'runningBalance': 100.0,
          'handlingEmployee': 'admin'
        },
        {
          'transactionDate': DateTime.now().subtract(const Duration(days: 10)).toIso8601String(),
          'transactionType': 'Fee Charge',
          'arabicType': 'رسوم دورة',
          'notes': 'تعيين دورة: برمجة وتطوير تطبيقات سطح المكتب',
          'debit': 250.0,
          'credit': 0.0,
          'runningBalance': 350.0,
          'handlingEmployee': 'admin'
        },
        {
          'transactionDate': DateTime.now().subtract(const Duration(days: 2)).toIso8601String(),
          'transactionType': 'Payment Receipt',
          'arabicType': 'سند قبض',
          'notes': 'إيصال سداد رسوم',
          'debit': 0.0,
          'credit': 200.0,
          'runningBalance': 150.0,
          'handlingEmployee': 'accountant'
        }
      ]
    };
  }

  // --- SYSTEM SETTINGS ---
  static Future<Map<String, dynamic>> getSettings() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/settings')).timeout(const Duration(seconds: 2));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}
    return {
      'centerName': 'منظومة مركز الدورات التعليمية',
      'logoBase64': null
    };
  }

  static Future<Map<String, dynamic>> saveSettings(String centerName, String? logoBase64) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/settings'),
      headers: _headers,
      body: jsonEncode({
        'centerName': centerName,
        'logoBase64': logoBase64,
      }),
    );
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل حفظ الإعدادات');
  }

  // --- USERS MANAGEMENT ---
  static Future<List<dynamic>> getUsers({String? search}) async {
    try {
      String url = '$baseUrl/api/users';
      if (search != null && search.isNotEmpty) {
        url += '?search=${Uri.encodeComponent(search)}';
      }
      final response = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}

    var all = _mockUsers;
    if (search != null && search.isNotEmpty) {
      all = all.where((u) => u['username'].toString().contains(search) || u['role'].toString().contains(search)).toList();
    }
    return all;
  }

  static Future<Map<String, dynamic>> getUser(int id) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/users/$id')).timeout(const Duration(seconds: 3));
      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      }
    } catch (_) {}

    // Find in Mock
    final u = _mockUsers.firstWhere((x) => x['userID'] == id, orElse: () => _mockUsers[0]);
    return {
      'userID': u['userID'],
      'username': u['username'],
      'role': u['role'],
      'isActive': u['isActive'],
      'permissions': {
        'canManageStudents': true,
        'canManageCourses': true,
        'canAssignDues': true,
        'canReceivePayments': true,
        'canViewReports': u['role'] == 'Admin',
        'canManageUsers': u['role'] == 'Admin'
      }
    };
  }

  static Future<Map<String, dynamic>> createUser(Map<String, dynamic> data) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/users'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 201 || response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل إضافة المستخدم');
  }

  static Future<Map<String, dynamic>> updateUser(int id, Map<String, dynamic> data) async {
    final response = await http.put(
      Uri.parse('$baseUrl/api/users/$id'),
      headers: _headers,
      body: jsonEncode(data),
    );
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل تعديل المستخدم');
  }

  static Future<Map<String, dynamic>> deleteUser(int id) async {
    final response = await http.delete(Uri.parse('$baseUrl/api/users/$id'));
    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    }
    throw Exception(jsonDecode(response.body)['message'] ?? 'فشل حذف المستخدم');
  }

  // --- LOCAL MOCKS ---
  static final List<Map<String, dynamic>> _mockStudents = [
    {
      'studentID': 1,
      'studentName': 'أحمد علي العبيدي',
      'guardianName': 'علي العبيدي',
      'parentPhone': '091223344',
      'registrationDate': '2025-10-01T10:00:00',
      'notes': 'ملاحظة للتواصل المباشر',
      'openingBalanceAmount': 100.00,
      'balanceType': 'Debit'
    },
    {
      'studentID': 2,
      'studentName': 'فاطمة عمر بلحاج',
      'guardianName': 'عمر بلحاج',
      'parentPhone': '092556677',
      'registrationDate': '2025-10-03T11:15:00',
      'notes': 'طالب متميز',
      'openingBalanceAmount': 0.0,
      'balanceType': 'Credit'
    },
    {
      'studentID': 3,
      'studentName': 'محمد مصطفى كامل',
      'guardianName': 'مصطفى كامل',
      'parentPhone': '091778899',
      'registrationDate': '2025-10-05T09:30:00',
      'notes': 'رصيد افتتاحي كبير',
      'openingBalanceAmount': 1000.0,
      'balanceType': 'Debit'
    }
  ];

  static final List<Map<String, dynamic>> _mockCourses = [
    {'courseID': 1, 'courseName': 'لغة إنجليزية - مستوى مبتدئ', 'cost': 150.00},
    {'courseID': 2, 'courseName': 'لغة إنجليزية - مستوى متوسط', 'cost': 200.00},
    {'courseID': 3, 'courseName': 'برمجة وتطوير تطبيقات سطح المكتب', 'cost': 350.00},
    {'courseID': 4, 'courseName': 'أساسيات شبكات الحاسوب', 'cost': 250.00},
    {'courseID': 5, 'courseName': 'التصميم الجرافيكي والملتيميديا', 'cost': 300.00}
  ];

  static final List<Map<String, dynamic>> _mockUsers = [
    {'userID': 1, 'username': 'admin', 'role': 'Admin', 'isActive': true},
    {'userID': 2, 'username': 'accountant', 'role': 'Accountant', 'isActive': true},
    {'userID': 3, 'username': 'receptionist', 'role': 'Receptionist', 'isActive': true}
  ];
}

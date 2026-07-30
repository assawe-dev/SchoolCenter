import 'dart:convert';
import 'package:flutter/material.dart';
import '../models/user_session.dart';
import '../services/api_service.dart';

// Import all screens (we will create them next)
import 'dashboard_screen.dart';
import 'students_screen.dart';
import 'courses_screen.dart';
import 'dues_screen.dart';
import 'payments_screen.dart';
import 'balance_report_screen.dart';
import 'users_screen.dart';
import 'settings_screen.dart';

class MainNavigationLayout extends StatefulWidget {
  const MainNavigationLayout({super.key});

  @override
  State<MainNavigationLayout> createState() => _MainNavigationLayoutState();
}

class _MainNavigationLayoutState extends State<MainNavigationLayout> {
  int _currentIndex = 0;
  String _centerName = 'منظومة مركز الدورات التعليمية';
  String? _logoBase64;

  @override
  void initState() {
    super.initState();
    _loadCenterSettings();
  }

  Future<void> _loadCenterSettings() async {
    try {
      final settings = await ApiService.getSettings();
      if (mounted) {
        setState(() {
          _centerName = settings['centerName'] ?? _centerName;
          _logoBase64 = settings['logoBase64'];
        });
      }
    } catch (_) {}
  }

  // Generate menu items based on permissions
  List<Map<String, dynamic>> _getMenuItems() {
    return [
      {
        'title': 'لوحة التحكم',
        'icon': Icons.dashboard_outlined,
        'index': 0,
        'widget': const DashboardScreen(),
        'allowed': true,
      },
      {
        'title': 'إدارة الطلاب',
        'icon': Icons.people_alt_outlined,
        'index': 1,
        'widget': const StudentsScreen(),
        'allowed': UserSession.canManageStudents,
      },
      {
        'title': 'إدارة الدورات',
        'icon': Icons.school_outlined,
        'index': 2,
        'widget': const CoursesScreen(),
        'allowed': UserSession.canManageCourses,
      },
      {
        'title': 'تعيين المستحقات',
        'icon': Icons.assignment_outlined,
        'index': 3,
        'widget': const DuesScreen(),
        'allowed': UserSession.canAssignDues,
      },
      {
        'title': 'سندات القبض',
        'icon': Icons.payment_outlined,
        'index': 4,
        'widget': const PaymentsScreen(),
        'allowed': UserSession.canReceivePayments,
      },
      {
        'title': 'تقرير الأرصدة',
        'icon': Icons.assessment_outlined,
        'index': 5,
        'widget': const BalanceReportScreen(),
        'allowed': UserSession.canViewReports,
      },
      {
        'title': 'إدارة المستخدمين',
        'icon': Icons.manage_accounts_outlined,
        'index': 6,
        'widget': const UsersScreen(),
        'allowed': UserSession.canManageUsers,
      },
      {
        'title': 'الإعدادات',
        'icon': Icons.settings_outlined,
        'index': 7,
        'widget': const SettingsScreen(onSettingsSaved: null), // We can pass a callback
        'allowed': UserSession.role == 'Admin',
      },
    ];
  }

  @override
  Widget build(BuildContext context) {
    final menuItems = _getMenuItems().where((item) => item['allowed'] == true).toList();
    final isDesktop = MediaQuery.of(context).size.width > 950;

    // Check if current index is still valid in filtered menu items, if not reset to 0
    var activeItem = menuItems.firstWhere(
      (item) => item['index'] == _currentIndex,
      orElse: () => menuItems.first,
    );
    _currentIndex = activeItem['index'];

    Widget currentScreen = activeItem['widget'];
    // Special setup for Settings screen to refresh layout on change
    if (activeItem['index'] == 7) {
      currentScreen = SettingsScreen(onSettingsSaved: _loadCenterSettings);
    }

    return Scaffold(
      drawer: !isDesktop
          ? Drawer(
              backgroundColor: const Color(0xFF0F172A),
              child: _buildSidebar(menuItems),
            )
          : null,
      appBar: !isDesktop
          ? AppBar(
              title: Text(
                _centerName,
                style: const TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, fontSize: 16),
              ),
              backgroundColor: Colors.white,
              elevation: 0,
              bottom: PreferredSize(
                preferredSize: const Size.fromHeight(1),
                child: Container(color: const Color(0xFFE2E8F0), height: 1),
              ),
            )
          : null,
      body: Row(
        children: [
          // Desktop Fixed Sidebar
          if (isDesktop)
            Container(
              width: 260,
              color: const Color(0xFF0F172A), // Dark Slate Navy
              child: _buildSidebar(menuItems),
            ),

          // Main Content Area
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                // Top Header (Desktop only)
                if (isDesktop) _buildTopHeader(),

                // Dynamic Screen View
                Expanded(
                  child: AnimatedSwitcher(
                    duration: const Duration(milliseconds: 250),
                    child: currentScreen,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTopHeader() {
    return Container(
      height: 70,
      decoration: const BoxDecoration(
        color: Colors.white,
        border: Border(
          bottom: BorderSide(color: Color(0xFFE2E8F0), width: 1),
        ),
      ),
      padding: const EdgeInsets.symmetric(horizontal: 24),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          // Organization branding details
          Row(
            children: [
              _logoBase64 != null
                  ? Container(
                      width: 40,
                      height: 40,
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(8),
                        border: Border.all(color: const Color(0xFFE2E8F0)),
                        image: DecorationImage(
                          image: MemoryImage(base64Decode(_logoBase64!)),
                          fit: BoxFit.cover,
                        ),
                      ),
                    )
                  : Container(
                      padding: const EdgeInsets.all(8),
                      decoration: BoxDecoration(
                        color: const Color(0xFFEFF6FF),
                        borderRadius: BorderRadius.circular(8),
                      ),
                      child: const Text('🏫', style: TextStyle(fontSize: 20)),
                    ),
              const SizedBox(width: 12),
              Text(
                _centerName,
                style: const TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: Color(0xFF0F172A),
                  fontFamily: 'Cairo',
                ),
              ),
            ],
          ),

          // User info and Server state
          Row(
            children: [
              // Connection badge
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                decoration: BoxDecoration(
                  color: const Color(0xFFDEF7EC),
                  borderRadius: BorderRadius.circular(20),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Container(
                      width: 6,
                      height: 6,
                      decoration: const BoxDecoration(
                        color: Color(0xFF03543F),
                        shape: BoxShape.circle,
                      ),
                    ),
                    const SizedBox(width: 6),
                    const Text(
                      'متصل بالسيرفر',
                      style: TextStyle(
                        color: Color(0xFF03543F),
                        fontSize: 11,
                        fontWeight: FontWeight.bold,
                        fontFamily: 'Cairo',
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(width: 16),
              const VerticalDivider(width: 1, indent: 20, endIndent: 20),
              const SizedBox(width: 16),

              // User Info
              Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  Text(
                    UserSession.username ?? 'المستخدم',
                    style: const TextStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF0F172A),
                      fontFamily: 'Cairo',
                    ),
                  ),
                  Text(
                    _translateRole(UserSession.role),
                    style: const TextStyle(
                      fontSize: 11,
                      color: Color(0xFF64748B),
                      fontFamily: 'Cairo',
                    ),
                  ),
                ],
              ),
              const SizedBox(width: 12),
              CircleAvatar(
                backgroundColor: const Color(0xFF3799EB).withOpacity(0.1),
                radius: 20,
                child: Text(
                  (UserSession.username ?? 'U').substring(0, 1).toUpperCase(),
                  style: const TextStyle(
                    color: Color(0xFF2563EB),
                    fontWeight: FontWeight.bold,
                    fontFamily: 'Cairo',
                  ),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  String _translateRole(String? role) {
    if (role == 'Admin') return 'المدير العام';
    if (role == 'Accountant') return 'المحاسب المالي';
    if (role == 'Receptionist') return 'موظف الاستقبال';
    return role ?? 'موظف';
  }

  Widget _buildSidebar(List<Map<String, dynamic>> menuItems) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        // Top Header/Logo area of sidebar
        Container(
          padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 32),
          child: Column(
            children: [
              Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: const Color(0xFF1E293B),
                  borderRadius: BorderRadius.circular(16),
                  border: Border.all(color: const Color(0xFF334155), width: 1),
                ),
                child: const Text('🏫', style: TextStyle(fontSize: 36)),
              ),
              const SizedBox(height: 16),
              const Text(
                'نظام السيطرة المدرسية',
                textAlign: TextAlign.center,
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                  fontFamily: 'Cairo',
                ),
              ),
              const SizedBox(height: 4),
              const Text(
                'الإصدار الذكي v2.0',
                style: TextStyle(
                  color: Color(0xFF64748B),
                  fontSize: 12,
                  fontFamily: 'Cairo',
                ),
              ),
            ],
          ),
        ),

        // Navigation Menu list
        Expanded(
          child: ListView.builder(
            padding: const EdgeInsets.symmetric(horizontal: 16),
            itemCount: menuItems.length,
            itemBuilder: (context, index) {
              final item = menuItems[index];
              final isSelected = item['index'] == _currentIndex;

              return Padding(
                padding: const EdgeInsets.only(bottom: 6.0),
                child: InkWell(
                  onTap: () {
                    setState(() {
                      _currentIndex = item['index'];
                    });
                    // Close drawer if on mobile
                    if (Navigator.canPop(context)) {
                      Navigator.pop(context);
                    }
                  },
                  borderRadius: BorderRadius.circular(8),
                  child: AnimatedContainer(
                    duration: const Duration(milliseconds: 200),
                    padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                    decoration: BoxDecoration(
                      color: isSelected ? const Color(0xFF2563EB) : Colors.transparent,
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: Row(
                      children: [
                        Icon(
                          item['icon'] as IconData,
                          color: isSelected ? Colors.white : const Color(0xFF94A3B8),
                          size: 20,
                        ),
                        const SizedBox(width: 14),
                        Text(
                          item['title'] as String,
                          style: TextStyle(
                            color: isSelected ? Colors.white : const Color(0xFFF1F5F9),
                            fontSize: 14,
                            fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                            fontFamily: 'Cairo',
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              );
            },
          ),
        ),

        // Logout Button Area at bottom
        Padding(
          padding: const EdgeInsets.all(20),
          child: OutlinedButton.icon(
            onPressed: () {
              UserSession.logout();
              Navigator.of(context).pushReplacementNamed('/login');
            },
            icon: const Icon(Icons.logout_outlined, color: Color(0xFFEF4444), size: 18),
            label: const Text(
              'تسجيل الخروج',
              style: TextStyle(
                color: Color(0xFFEF4444),
                fontWeight: FontWeight.bold,
                fontFamily: 'Cairo',
              ),
            ),
            style: OutlinedButton.styleFrom(
              side: const BorderSide(color: Color(0xFFEF4444), width: 1.2),
              padding: const EdgeInsets.symmetric(vertical: 14),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(8),
              ),
            ),
          ),
        ),
      ],
    );
  }
}

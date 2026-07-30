import 'package:flutter/material.dart';
import '../services/api_service.dart';

class UsersScreen extends StatefulWidget {
  const UsersScreen({super.key});

  @override
  State<UsersScreen> createState() => _UsersScreenState();
}

class _UsersScreenState extends State<UsersScreen> {
  final _searchController = TextEditingController();
  bool _isLoading = true;
  List<dynamic> _usersList = [];

  @override
  void initState() {
    super.initState();
    _loadUsers();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  Future<void> _loadUsers() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final data = await ApiService.getUsers(search: _searchController.text.trim());
      if (mounted) {
        setState(() {
          _usersList = data;
          _isLoading = false;
        });
      }
    } catch (_) {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }

  void _showUserDialog({int? userId}) async {
    final isEdit = userId != null;

    // Default initializers
    final usernameController = TextEditingController();
    final passwordController = TextEditingController();
    String role = 'Receptionist';
    bool isActive = true;

    bool pStudents = true;
    bool pCourses = true;
    bool pDues = true;
    bool pPayments = true;
    bool pReports = false;
    bool pUsers = false;

    if (isEdit) {
      setState(() {
        _isLoading = true;
      });
      try {
        final userData = await ApiService.getUser(userId);
        usernameController.text = userData['username'] ?? '';
        role = userData['role'] ?? 'Receptionist';
        isActive = userData['isActive'] == true;

        final p = userData['permissions'] ?? {};
        pStudents = p['canManageStudents'] == true;
        pCourses = p['canManageCourses'] == true;
        pDues = p['canAssignDues'] == true;
        pPayments = p['canReceivePayments'] == true;
        pReports = p['canViewReports'] == true;
        pUsers = p['canManageUsers'] == true;
      } catch (_) {}
      setState(() {
        _isLoading = false;
      });
    }

    if (!mounted) return;

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (ctx) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
          title: Text(
            isEdit ? 'تعديل بيانات وصلاحيات المستخدم' : 'إضافة حساب مستخدم جديد وصلاحياته',
            textAlign: TextAlign.right,
            style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
          ),
          content: SizedBox(
            width: 550,
            child: SingleChildScrollView(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // Username
                  const Text('اسم المستخدم *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: usernameController,
                    decoration: InputDecoration(
                      hintText: 'مثال: mohamed_ali',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Password
                  Text(
                    isEdit ? 'كلمة المرور الجديدة (اتركها فارغة لعدم التعديل)' : 'كلمة المرور *',
                    style: const TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: passwordController,
                    obscureText: true,
                    decoration: InputDecoration(
                      hintText: isEdit ? 'أدخل كلمة مرور جديدة فقط في حال رغبت بتعديلها' : 'أدخل كلمة المرور السرية للحساب',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Role Dropdown
                  const Text('الدور الوظيفي والمسؤولية *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  DropdownButtonFormField<String>(
                    value: role,
                    onChanged: (val) {
                      if (val != null) {
                        setDialogState(() {
                          role = val;
                          if (role == 'Admin') {
                            // Turn all permissions on for Admin
                            pStudents = true;
                            pCourses = true;
                            pDues = true;
                            pPayments = true;
                            pReports = true;
                            pUsers = true;
                          }
                        });
                      }
                    },
                    decoration: InputDecoration(
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                    items: const [
                      DropdownMenuItem(value: 'Admin', child: Text('مدير عام النظام (Admin)', style: TextStyle(fontFamily: 'Cairo', fontSize: 13))),
                      DropdownMenuItem(value: 'Accountant', child: Text('محاسب مالي (Accountant)', style: TextStyle(fontFamily: 'Cairo', fontSize: 13))),
                      DropdownMenuItem(value: 'Receptionist', child: Text('موظف استقبال (Receptionist)', style: TextStyle(fontFamily: 'Cairo', fontSize: 13))),
                    ],
                  ),
                  const SizedBox(height: 16),

                  // Active status Checkbox
                  Row(
                    children: [
                      Checkbox(
                        value: isActive,
                        activeColor: const Color(0xFF2563EB),
                        onChanged: (val) {
                          if (val != null) setDialogState(() => isActive = val);
                        },
                      ),
                      const Text('هل الحساب نشط ويمكنه تسجيل الدخول؟', style: TextStyle(fontFamily: 'Cairo', fontSize: 13)),
                    ],
                  ),
                  const Divider(height: 32),

                  // Permissions Checkboxes Group
                  const Text(
                    'الصلاحيات التفصيلية (Granular Permissions)',
                    style: TextStyle(fontFamily: 'Cairo', fontSize: 14, fontWeight: FontWeight.bold, color: Color(0xFF1E3A8A)),
                  ),
                  const SizedBox(height: 12),
                  _buildPermCheckbox('إدارة بيانات وقوائم الطلاب الدراسية', pStudents, (v) => setDialogState(() => pStudents = v), setDialogState),
                  _buildPermCheckbox('إدارة الدورات التعليمية والأسعار الكلية', pCourses, (v) => setDialogState(() => pCourses = v), setDialogState),
                  _buildPermCheckbox('تعيين المستحقات المالية ورسوم التسجيل للطلاب', pDues, (v) => setDialogState(() => pDues = v), setDialogState),
                  _buildPermCheckbox('تحرير سندات القبض وتتبع صندوق الخزينة', pPayments, (v) => setDialogState(() => pPayments = v), setDialogState),
                  _buildPermCheckbox('عرض تقارير الأرصدة الشاملة والتصدير لـ Excel', pReports, (v) => setDialogState(() => pReports = v), setDialogState),
                  _buildPermCheckbox('إدارة حسابات المستخدمين وصلاحياتهم (صلاحية حساسة)', pUsers, (v) => setDialogState(() => pUsers = v), setDialogState),
                ],
              ),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(ctx),
              child: const Text('إلغاء', style: TextStyle(fontFamily: 'Cairo')),
            ),
            ElevatedButton(
              onPressed: () async {
                if (usernameController.text.trim().isEmpty || (!isEdit && passwordController.text.isEmpty)) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('يرجى ملء الحقول المطلوبة الكلية', style: TextStyle(fontFamily: 'Cairo'))),
                  );
                  return;
                }

                final payload = {
                  'username': usernameController.text.trim(),
                  'password': passwordController.text.isEmpty ? null : passwordController.text,
                  'role': role,
                  'isActive': isActive,
                  'permissions': {
                    'canManageStudents': pStudents,
                    'canManageCourses': pCourses,
                    'canAssignDues': pDues,
                    'canReceivePayments': pPayments,
                    'canViewReports': pReports,
                    'canManageUsers': pUsers,
                  }
                };

                try {
                  if (isEdit) {
                    await ApiService.updateUser(userId, payload);
                  } else {
                    await ApiService.createUser(payload);
                  }
                  Navigator.pop(ctx);
                  _loadUsers();
                } catch (e) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
                  );
                }
              },
              style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFF2563EB)),
              child: Text(
                isEdit ? 'تعديل الصلاحيات' : 'إنشاء الحساب وصلاحياته',
                style: const TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPermCheckbox(String title, bool val, ValueChanged<bool> onChanged, StateSetter setDialogState) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 4.0),
      child: Row(
        children: [
          SizedBox(
            width: 24,
            height: 24,
            child: Checkbox(
              value: val,
              activeColor: const Color(0xFF2563EB),
              onChanged: (v) {
                if (v != null) onChanged(v);
              },
            ),
          ),
          const SizedBox(width: 8),
          Text(title, style: const TextStyle(fontFamily: 'Cairo', fontSize: 13)),
        ],
      ),
    );
  }

  void _deleteUser(int id, String name) {
    if (name == 'admin') {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('لا يمكن حذف الحساب الأساسي للنظام (admin) لأمان لوحة التحكم', style: TextStyle(fontFamily: 'Cairo')), backgroundColor: Color(0xFFEF4444)),
      );
      return;
    }

    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('حذف حساب مستخدم ⚠️', textAlign: TextAlign.right, style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold)),
        content: Text(
          'هل أنت متأكد من حذف الحساب "$name" نهائياً؟ لن يتمكن هذا الموظف من تسجيل الدخول للنظام مجدداً.',
          textAlign: TextAlign.right,
          style: const TextStyle(fontFamily: 'Cairo', height: 1.5),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('إلغاء', style: TextStyle(fontFamily: 'Cairo')),
          ),
          ElevatedButton(
            onPressed: () async {
              try {
                await ApiService.deleteUser(id);
                Navigator.pop(ctx);
                _loadUsers();
              } catch (e) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
                );
              }
            },
            style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFFEF4444)),
            child: const Text('حذف الحساب', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'إدارة المستخدمين والموظفين والصلاحيات',
                      style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold, color: Color(0xFF0F172A), fontFamily: 'Cairo'),
                    ),
                    SizedBox(height: 4),
                    Text(
                      'إضافة حسابات الموظفين وتخصيص صلاحيات الوصول التفصيلية لكل شاشة لضمان حماية وسرية البيانات.',
                      style: TextStyle(fontSize: 13, color: Color(0xFF64748B), fontFamily: 'Cairo'),
                    ),
                  ],
                ),
                ElevatedButton.icon(
                  onPressed: () => _showUserDialog(),
                  icon: const Icon(Icons.person_add_alt_1, color: Colors.white),
                  label: const Text('إضافة حساب جديد 👤', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF2563EB),
                    padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 14),
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 24),

            // Search input field
            Container(
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(12),
                border: Border.all(color: const Color(0xFFE2E8F0)),
              ),
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
              child: Row(
                children: [
                  const Icon(Icons.search, color: Color(0xFF64748B)),
                  const SizedBox(width: 12),
                  Expanded(
                    child: TextField(
                      controller: _searchController,
                      onChanged: (_) => _loadUsers(),
                      decoration: const InputDecoration(
                        hintText: 'بحث سريع باسم المستخدم أو الدور الوظيفي للموظف...',
                        border: InputBorder.none,
                        hintStyle: TextStyle(fontSize: 13, color: Color(0xFF94A3B8)),
                      ),
                    ),
                  ),
                  if (_searchController.text.isNotEmpty)
                    IconButton(
                      onPressed: () {
                        _searchController.clear();
                        _loadUsers();
                      },
                      icon: const Icon(Icons.clear, size: 18),
                    ),
                ],
              ),
            ),
            const SizedBox(height: 24),

            // Users Table Grid
            Expanded(
              child: Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(16),
                  border: Border.all(color: const Color(0xFFE2E8F0)),
                ),
                child: _isLoading
                    ? const Center(child: CircularProgressIndicator(color: Color(0xFF2563EB)))
                    : _usersList.isEmpty
                        ? const Center(
                            child: Text(
                              'لا توجد سجلات مستخدمين مطابقة للبحث',
                              style: TextStyle(fontFamily: 'Cairo', color: Color(0xFF64748B)),
                            ),
                          )
                        : ClipRRect(
                            borderRadius: BorderRadius.circular(16),
                            child: SingleChildScrollView(
                              scrollDirection: Axis.vertical,
                              child: DataTable(
                                headingRowColor: WidgetStateProperty.all(const Color(0xFFF8FAFC)),
                                headingTextStyle: const TextStyle(
                                  color: Color(0xFF334155),
                                  fontFamily: 'Cairo',
                                  fontWeight: FontWeight.bold,
                                  fontSize: 13,
                                ),
                                columns: const [
                                  DataColumn(label: Text('المعرف الآلي')),
                                  DataColumn(label: Text('اسم المستخدم')),
                                  DataColumn(label: Text('الدور الوظيفي')),
                                  DataColumn(label: Text('حالة الحساب')),
                                  DataColumn(label: Text('خيارات العمل')),
                                ],
                                rows: _usersList.map((row) {
                                  final isActive = row['isActive'] == true;
                                  return DataRow(
                                    cells: [
                                      DataCell(Text('#${row['userID']}', style: const TextStyle(fontFamily: 'Cairo'))),
                                      DataCell(Text(
                                        row['username'] ?? '',
                                        style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(
                                        row['role'] == 'Admin'
                                            ? 'مدير النظام'
                                            : (row['role'] == 'Accountant' ? 'محاسب مالي' : 'موظف استقبال'),
                                        style: const TextStyle(fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Container(
                                        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                                        decoration: BoxDecoration(
                                          color: isActive ? const Color(0xFFDEF7EC) : const Color(0xFFFDE8E8),
                                          borderRadius: BorderRadius.circular(12),
                                        ),
                                        child: Text(
                                          isActive ? 'نشط ومصرح له' : 'معطل ومحظور',
                                          style: TextStyle(
                                            color: isActive ? const Color(0xFF03543F) : const Color(0xFF9B1C1C),
                                            fontSize: 11,
                                            fontWeight: FontWeight.bold,
                                            fontFamily: 'Cairo',
                                          ),
                                        ),
                                      )),
                                      DataCell(Row(
                                        mainAxisSize: MainAxisSize.min,
                                        children: [
                                          IconButton(
                                            icon: const Icon(Icons.security, color: Color(0xFF2563EB), size: 20),
                                            tooltip: 'تعديل البيانات والصلاحيات',
                                            onPressed: () => _showUserDialog(userId: row['userID']),
                                          ),
                                          IconButton(
                                            icon: const Icon(Icons.delete_outline, color: Color(0xFFEF4444), size: 20),
                                            tooltip: 'حذف الحساب نهائياً',
                                            onPressed: () => _deleteUser(row['userID'], row['username']),
                                          ),
                                        ],
                                      )),
                                    ],
                                  );
                                }).toList(),
                              ),
                            ),
                          ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

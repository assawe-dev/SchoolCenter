import 'package:flutter/material.dart';
import '../services/api_service.dart';

class CoursesScreen extends StatefulWidget {
  const CoursesScreen({super.key});

  @override
  State<CoursesScreen> createState() => _CoursesScreenState();
}

class _CoursesScreenState extends State<CoursesScreen> {
  final _searchController = TextEditingController();
  bool _isLoading = true;
  List<dynamic> _coursesList = [];

  @override
  void initState() {
    super.initState();
    _loadCourses();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  Future<void> _loadCourses() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final data = await ApiService.getCourses(search: _searchController.text.trim());
      if (mounted) {
        setState(() {
          _coursesList = data;
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

  void _showCourseDialog({Map<String, dynamic>? course}) {
    final isEdit = course != null;
    final nameController = TextEditingController(text: isEdit ? course['courseName'] : '');
    final costController = TextEditingController(text: isEdit ? (course['cost']?.toString() ?? '0') : '0');

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        title: Text(
          isEdit ? 'تعديل بيانات الدورة' : 'إضافة دورة تعليمية جديدة',
          textAlign: TextAlign.right,
          style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // Course Name
            const Text('اسم الدورة التدريبية *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
            const SizedBox(height: 6),
            TextFormField(
              controller: nameController,
              decoration: InputDecoration(
                hintText: 'مثال: لغة إنجليزية - مستوى مبتدئ',
                hintStyle: const TextStyle(fontSize: 12),
                border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
              ),
            ),
            const SizedBox(height: 16),

            // Cost
            const Text('تكلفة الدورة التدريبية (د.ل) *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
            const SizedBox(height: 6),
            TextFormField(
              controller: costController,
              keyboardType: const TextInputType.numberWithOptions(decimal: true),
              decoration: InputDecoration(
                hintText: 'أدخل التكلفة الكلية للدورة',
                hintStyle: const TextStyle(fontSize: 12),
                border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('إلغاء', style: TextStyle(fontFamily: 'Cairo')),
          ),
          ElevatedButton(
            onPressed: () async {
              if (nameController.text.trim().isEmpty) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('يرجى إدخال اسم الدورة التدريبية', style: TextStyle(fontFamily: 'Cairo'))),
                );
                return;
              }

              double cost = double.tryParse(costController.text.trim()) ?? 0.0;
              final payload = {
                'courseName': nameController.text.trim(),
                'cost': cost,
              };

              try {
                if (isEdit) {
                  await ApiService.updateCourse(course['courseID'], payload);
                } else {
                  await ApiService.createCourse(payload);
                }
                Navigator.pop(ctx);
                _loadCourses();
              } catch (e) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
                );
              }
            },
            style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFF2563EB)),
            child: Text(
              isEdit ? 'تعديل وحفظ' : 'إضافة الدورة',
              style: const TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white),
            ),
          ),
        ],
      ),
    );
  }

  void _deleteCourse(int id, String name) {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('تأكيد الحذف ⚠️', textAlign: TextAlign.right, style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold)),
        content: Text(
          'هل أنت متأكد من حذف الدورة "$name" نهائياً من قاعدة البيانات؟',
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
                await ApiService.deleteCourse(id);
                Navigator.pop(ctx);
                _loadCourses();
              } catch (e) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
                );
              }
            },
            style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFFEF4444)),
            child: const Text('حذف', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
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
                      'إدارة الدورات التدريبية والبرامج',
                      style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold, color: Color(0xFF0F172A), fontFamily: 'Cairo'),
                    ),
                    SizedBox(height: 4),
                    Text(
                      'تعريف المناهج والدورات وتكلفة الاشتراك الافتراضية لكل برنامج تعليمي.',
                      style: TextStyle(fontSize: 13, color: Color(0xFF64748B), fontFamily: 'Cairo'),
                    ),
                  ],
                ),
                ElevatedButton.icon(
                  onPressed: () => _showCourseDialog(),
                  icon: const Icon(Icons.add, color: Colors.white),
                  label: const Text('إضافة دورة جديدة 📚', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF2563EB),
                    padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 14),
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 24),

            // Search
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
                      onChanged: (_) => _loadCourses(),
                      decoration: const InputDecoration(
                        hintText: 'بحث سريع باسم الدورة التدريبية...',
                        border: InputBorder.none,
                        hintStyle: TextStyle(fontSize: 13, color: Color(0xFF94A3B8)),
                      ),
                    ),
                  ),
                  if (_searchController.text.isNotEmpty)
                    IconButton(
                      onPressed: () {
                        _searchController.clear();
                        _loadCourses();
                      },
                      icon: const Icon(Icons.clear, size: 18),
                    ),
                ],
              ),
            ),
            const SizedBox(height: 24),

            // Table Grid
            Expanded(
              child: Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(16),
                  border: Border.all(color: const Color(0xFFE2E8F0)),
                ),
                child: _isLoading
                    ? const Center(child: CircularProgressIndicator(color: Color(0xFF2563EB)))
                    : _coursesList.isEmpty
                        ? const Center(
                            child: Text(
                              'لا توجد سجلات دورات مطابقة للبحث',
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
                                  DataColumn(label: Text('رقم الدورة الآلي')),
                                  DataColumn(label: Text('اسم الدورة التدريبية')),
                                  DataColumn(label: Text('تكلفة الدورة')),
                                  DataColumn(label: Text('العمليات')),
                                ],
                                rows: _coursesList.map((row) {
                                  return DataRow(
                                    cells: [
                                      DataCell(Text('#${row['courseID']}', style: const TextStyle(fontFamily: 'Cairo'))),
                                      DataCell(Text(
                                        row['courseName'] ?? '',
                                        style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(
                                        '${(row['cost'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل',
                                        style: const TextStyle(fontFamily: 'Cairo', color: Color(0xFF2563EB), fontWeight: FontWeight.bold),
                                      )),
                                      DataCell(Row(
                                        mainAxisSize: MainAxisSize.min,
                                        children: [
                                          IconButton(
                                            icon: const Icon(Icons.edit_outlined, color: Color(0xFF10B981), size: 20),
                                            onPressed: () => _showCourseDialog(course: row),
                                          ),
                                          IconButton(
                                            icon: const Icon(Icons.delete_outline, color: Color(0xFFEF4444), size: 20),
                                            onPressed: () => _deleteCourse(row['courseID'], row['courseName']),
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

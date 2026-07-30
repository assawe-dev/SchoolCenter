import 'package:flutter/material.dart';
import '../services/api_service.dart';

class DuesScreen extends StatefulWidget {
  const DuesScreen({super.key});

  @override
  State<DuesScreen> createState() => _DuesScreenState();
}

class _DuesScreenState extends State<DuesScreen> {
  final _formKey = GlobalKey<FormState>();
  final _amountController = TextEditingController();
  final _notesController = TextEditingController();

  bool _isLoading = true;
  bool _isSaving = false;

  List<dynamic> _studentsList = [];
  List<dynamic> _coursesList = [];

  int? _selectedStudentID;
  int? _selectedCourseID;

  @override
  void initState() {
    super.initState();
    _loadDuesData();
  }

  @override
  void dispose() {
    _amountController.dispose();
    _notesController.dispose();
    super.dispose();
  }

  Future<void> _loadDuesData() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final students = await ApiService.getStudents();
      final courses = await ApiService.getCourses();

      if (mounted) {
        setState(() {
          _studentsList = students;
          _coursesList = courses;
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

  void _onCourseChanged(int? courseId) {
    if (courseId == null) return;
    _selectedCourseID = courseId;

    final course = _coursesList.firstWhere((c) => c['courseID'] == courseId, orElse: () => null);
    if (course != null) {
      setState(() {
        _amountController.text = (course['cost'] as num).toStringAsFixed(2);
        _notesController.text = 'تعيين دورة: ${course['courseName']}';
      });
    }
  }

  Future<void> _submitDues() async {
    if (_formKey.currentState!.validate()) {
      if (_selectedStudentID == null || _selectedCourseID == null) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('يرجى تحديد الطالب والدورة التدريبية أولاً', style: TextStyle(fontFamily: 'Cairo'))),
        );
        return;
      }

      setState(() {
        _isSaving = true;
      });

      final payload = {
        'studentID': _selectedStudentID,
        'courseID': _selectedCourseID,
        'customAmount': double.tryParse(_amountController.text) ?? 0.0,
        'notes': _notesController.text.trim(),
        'userID': 1, // Default user
      };

      try {
        await ApiService.assignDues(payload);
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('تم تعيين المستحقات المالية وحفظ المعاملة بنجاح!', style: TextStyle(fontFamily: 'Cairo')),
              backgroundColor: Color(0xFF10B981),
            ),
          );
          // Clear and reset form
          setState(() {
            _selectedStudentID = null;
            _selectedCourseID = null;
            _amountController.clear();
            _notesController.clear();
          });
        }
      } catch (e) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
          );
        }
      } finally {
        if (mounted) {
          setState(() {
            _isSaving = false;
          });
        }
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _isLoading
          ? const Center(child: CircularProgressIndicator(color: Color(0xFF2563EB)))
          : Center(
              child: SingleChildScrollView(
                padding: const EdgeInsets.all(32.0),
                child: Container(
                  constraints: const BoxConstraints(maxWidth: 650),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(16),
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black.withOpacity(0.04),
                        blurRadius: 10,
                        offset: const Offset(0, 4),
                      ),
                    ],
                    border: Border.all(color: const Color(0xFFE2E8F0)),
                  ),
                  padding: const EdgeInsets.all(32),
                  child: Form(
                    key: _formKey,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        // Header
                        Center(
                          child: Container(
                            padding: const EdgeInsets.all(16),
                            decoration: const BoxDecoration(
                              color: Color(0xFFEFF6FF),
                              shape: BoxShape.circle,
                            ),
                            child: const Icon(Icons.assignment_outlined, size: 40, color: Color(0xFF2563EB)),
                          ),
                        ),
                        const SizedBox(height: 16),
                        const Center(
                          child: Text(
                            'تعيين المستحقات ورسوم الدورات للطلاب',
                            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold, color: Color(0xFF0F172A), fontFamily: 'Cairo'),
                          ),
                        ),
                        const SizedBox(height: 8),
                        const Center(
                          child: Text(
                            'تسجيل تكلفة الدورة ذهنياً أو يدوياً كدين مستحق على حساب الطالب في النظام.',
                            textAlign: TextAlign.center,
                            style: TextStyle(fontSize: 13, color: Color(0xFF64748B), fontFamily: 'Cairo'),
                          ),
                        ),
                        const SizedBox(height: 32),

                        // Select Student dropdown
                        const Text('اختر الطالب المسجل *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        DropdownButtonFormField<int>(
                          value: _selectedStudentID,
                          onChanged: (val) => setState(() => _selectedStudentID = val),
                          decoration: InputDecoration(
                            hintText: 'اختر الطالب من القائمة',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                          items: _studentsList.map((s) {
                            return DropdownMenuItem<int>(
                              value: s['studentID'] as int,
                              child: Text(s['studentName'] ?? '', style: const TextStyle(fontFamily: 'Cairo', fontSize: 13)),
                            );
                          }).toList(),
                          validator: (value) => value == null ? 'يرجى تحديد الطالب' : null,
                        ),
                        const SizedBox(height: 20),

                        // Select Course dropdown
                        const Text('اختر البرنامج / الدورة التدريبية *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        DropdownButtonFormField<int>(
                          value: _selectedCourseID,
                          onChanged: _onCourseChanged,
                          decoration: InputDecoration(
                            hintText: 'اختر الدورة لتحميل سعرها الافتراضي',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                          items: _coursesList.map((c) {
                            return DropdownMenuItem<int>(
                              value: c['courseID'] as int,
                              child: Text('${c['courseName']} (${c['cost']} د.ل)', style: const TextStyle(fontFamily: 'Cairo', fontSize: 13)),
                            );
                          }).toList(),
                          validator: (value) => value == null ? 'يرجى تحديد الدورة' : null,
                        ),
                        const SizedBox(height: 20),

                        // Custom Amount Field
                        const Text('المبلغ المستحق (د.ل) *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        TextFormField(
                          controller: _amountController,
                          keyboardType: const TextInputType.numberWithOptions(decimal: true),
                          decoration: InputDecoration(
                            hintText: '0.00',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                          validator: (value) {
                            if (value == null || value.trim().isEmpty) return 'يرجى إدخال قيمة المبلغ';
                            if (double.tryParse(value) == null || double.parse(value) <= 0) return 'المبلغ يجب أن يكون أكبر من الصفر';
                            return null;
                          },
                        ),
                        const SizedBox(height: 20),

                        // Notes Field
                        const Text('البيان / الملاحظات', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        TextFormField(
                          controller: _notesController,
                          maxLines: 2,
                          decoration: InputDecoration(
                            hintText: 'البيان التلقائي لتعيين الدورة أو ملاحظة مخصصة...',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                        ),
                        const SizedBox(height: 32),

                        // Submit Button
                        ElevatedButton(
                          onPressed: _isSaving ? null : _submitDues,
                          style: ElevatedButton.styleFrom(
                            backgroundColor: const Color(0xFF2563EB),
                            padding: const EdgeInsets.symmetric(vertical: 16),
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                            elevation: 0,
                          ),
                          child: _isSaving
                              ? const SizedBox(
                                  height: 20,
                                  width: 20,
                                  child: CircularProgressIndicator(strokeWidth: 2, valueColor: AlwaysStoppedAnimation<Color>(Colors.white)),
                                )
                              : const Text(
                                  'حفظ وتعميد المستحقات المالية',
                                  style: TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo', color: Colors.white, fontSize: 15),
                                ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),
    );
  }
}

import 'package:flutter/material.dart';
import '../services/api_service.dart';

class PaymentsScreen extends StatefulWidget {
  const PaymentsScreen({super.key});

  @override
  State<PaymentsScreen> createState() => _PaymentsScreenState();
}

class _PaymentsScreenState extends State<PaymentsScreen> {
  final _formKey = GlobalKey<FormState>();
  final _amountController = TextEditingController();
  final _notesController = TextEditingController();

  bool _isLoading = true;
  bool _isSaving = false;
  bool _isLoadingBalance = false;

  List<dynamic> _studentsList = [];
  int? _selectedStudentID;
  double? _currentStudentBalance;

  @override
  void initState() {
    super.initState();
    _loadPaymentsData();
  }

  @override
  void dispose() {
    _amountController.dispose();
    _notesController.dispose();
    super.dispose();
  }

  Future<void> _loadPaymentsData() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final students = await ApiService.getStudents();
      if (mounted) {
        setState(() {
          _studentsList = students;
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

  Future<void> _onStudentChanged(int? studentId) async {
    if (studentId == null) return;

    setState(() {
      _selectedStudentID = studentId;
      _isLoadingBalance = true;
      _currentStudentBalance = null;
    });

    try {
      final balance = await ApiService.getStudentBalance(studentId);
      if (mounted) {
        setState(() {
          _currentStudentBalance = balance;
          _isLoadingBalance = false;
          _notesController.text = 'دفعة من الرسوم الدراسية المترتبة';
        });
      }
    } catch (_) {
      if (mounted) {
        setState(() {
          _isLoadingBalance = false;
        });
      }
    }
  }

  Future<void> _submitPayment() async {
    if (_formKey.currentState!.validate()) {
      if (_selectedStudentID == null) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('يرجى اختيار طالب أولاً', style: TextStyle(fontFamily: 'Cairo'))),
        );
        return;
      }

      setState(() {
        _isSaving = true;
      });

      final payload = {
        'studentID': _selectedStudentID,
        'amount': double.tryParse(_amountController.text) ?? 0.0,
        'paymentDate': DateTime.now().toIso8601String(),
        'notes': _notesController.text.trim(),
        'userID': 1, // Default user
      };

      try {
        await ApiService.receivePayment(payload);
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('تم تحرير سند القبض وتحديث الخزينة والواردات بنجاح! 🎉', style: TextStyle(fontFamily: 'Cairo')),
              backgroundColor: Color(0xFF10B981),
            ),
          );

          // Reset Form
          setState(() {
            _selectedStudentID = null;
            _currentStudentBalance = null;
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
                              color: Color(0xFFECFDF5),
                              shape: BoxShape.circle,
                            ),
                            child: const Icon(Icons.payment, size: 40, color: Color(0xFF10B981)),
                          ),
                        ),
                        const SizedBox(height: 16),
                        const Center(
                          child: Text(
                            'تحرير سند قبض رسوم وإيداع خزينة',
                            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold, color: Color(0xFF0F172A), fontFamily: 'Cairo'),
                          ),
                        ),
                        const SizedBox(height: 8),
                        const Center(
                          child: Text(
                            'تسجيل مدفوعات الطلاب وتحديث الرصيد التراكمي وصندوق الخزينة آلياً.',
                            textAlign: TextAlign.center,
                            style: TextStyle(fontSize: 13, color: Color(0xFF64748B), fontFamily: 'Cairo'),
                          ),
                        ),
                        const SizedBox(height: 32),

                        // Select Student dropdown
                        const Text('اختر الطالب لتسجيل سداده *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        DropdownButtonFormField<int>(
                          value: _selectedStudentID,
                          onChanged: _onStudentChanged,
                          decoration: InputDecoration(
                            hintText: 'اختر الطالب من القائمة للتحقق من رصيده',
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

                        // Dynamic balance lookup display card
                        if (_isLoadingBalance)
                          const Padding(
                            padding: EdgeInsets.symmetric(vertical: 12.0),
                            child: Center(child: CircularProgressIndicator(strokeWidth: 2)),
                          )
                        else if (_currentStudentBalance != null) ...[
                          Container(
                            decoration: BoxDecoration(
                              color: _currentStudentBalance! > 0 ? const Color(0xFFFEF2F2) : const Color(0xFFECFDF5),
                              borderRadius: BorderRadius.circular(12),
                              border: Border.all(color: _currentStudentBalance! > 0 ? const Color(0xFFFCA5A5) : const Color(0xFFA7F3D0)),
                            ),
                            padding: const EdgeInsets.all(16),
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Row(
                                  children: [
                                    Icon(
                                      _currentStudentBalance! > 0 ? Icons.error_outline : Icons.check_circle_outline,
                                      color: _currentStudentBalance! > 0 ? const Color(0xFFEF4444) : const Color(0xFF10B981),
                                    ),
                                    const SizedBox(width: 12),
                                    Text(
                                      _currentStudentBalance! > 0 ? 'الرصيد المتبقي المستحق:' : 'الطالب مسدد بالكامل (لا توجد ديون):',
                                      style: TextStyle(
                                        fontFamily: 'Cairo',
                                        fontSize: 13,
                                        fontWeight: FontWeight.bold,
                                        color: _currentStudentBalance! > 0 ? const Color(0xFF991B1B) : const Color(0xFF065F46),
                                      ),
                                    ),
                                  ],
                                ),
                                Text(
                                  '${_currentStudentBalance!.toStringAsFixed(2)} د.ل',
                                  style: TextStyle(
                                    fontFamily: 'Cairo',
                                    fontSize: 18,
                                    fontWeight: FontWeight.bold,
                                    color: _currentStudentBalance! > 0 ? const Color(0xFF991B1B) : const Color(0xFF065F46),
                                  ),
                                ),
                              ],
                            ),
                          ),
                          const SizedBox(height: 20),
                        ],

                        // Payment Amount
                        const Text('مبلغ السداد المقبوض (د.ل) *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        TextFormField(
                          controller: _amountController,
                          keyboardType: const TextInputType.numberWithOptions(decimal: true),
                          decoration: InputDecoration(
                            hintText: 'أدخل قيمة المدفوع نقداً',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                          validator: (value) {
                            if (value == null || value.trim().isEmpty) return 'يرجى إدخال قيمة السداد';
                            if (double.tryParse(value) == null || double.parse(value) <= 0) return 'المبلغ يجب أن يكون أكبر من الصفر';
                            return null;
                          },
                        ),
                        const SizedBox(height: 20),

                        // Notes Field
                        const Text('البيان / ملاحظة السند', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        TextFormField(
                          controller: _notesController,
                          maxLines: 2,
                          decoration: InputDecoration(
                            hintText: 'مثال: دفعة من الرسوم الدراسية...',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                        ),
                        const SizedBox(height: 32),

                        // Submit Button
                        ElevatedButton(
                          onPressed: _isSaving ? null : _submitPayment,
                          style: ElevatedButton.styleFrom(
                            backgroundColor: const Color(0xFF10B981), // Green Accent
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
                                  'إصدار السند وإيداع الخزينة الكلية',
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

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../services/api_service.dart';

class StudentsScreen extends StatefulWidget {
  const StudentsScreen({super.key});

  @override
  State<StudentsScreen> createState() => _StudentsScreenState();
}

class _StudentsScreenState extends State<StudentsScreen> {
  final _searchController = TextEditingController();
  bool _isLoading = true;
  List<dynamic> _studentsList = [];

  @override
  void initState() {
    super.initState();
    _loadStudents();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  Future<void> _loadStudents() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final data = await ApiService.getStudents(search: _searchController.text.trim());
      if (mounted) {
        setState(() {
          _studentsList = data;
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

  void _showStudentDialog({Map<String, dynamic>? student}) {
    final isEdit = student != null;
    final nameController = TextEditingController(text: isEdit ? student['studentName'] : '');
    final guardianController = TextEditingController(text: isEdit ? student['guardianName'] : '');
    final phoneController = TextEditingController(text: isEdit ? student['parentPhone'] : '');
    final notesController = TextEditingController(text: isEdit ? student['notes'] : '');
    final amountController = TextEditingController(
      text: isEdit ? (student['openingBalanceAmount']?.toString() ?? '0') : '0',
    );
    String balanceType = isEdit ? (student['balanceType'] ?? 'Debit') : 'Debit';

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (ctx) => StatefulBuilder(
        builder: (context, setDialogState) => AlertDialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
          title: Text(
            isEdit ? 'تعديل بيانات الطالب ورصيده' : 'تسجيل طالب جديد ورصيد سابق',
            textAlign: TextAlign.right,
            style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
          ),
          content: SizedBox(
            width: 500,
            child: SingleChildScrollView(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // Student Name
                  const Text('اسم الطالب الثنائي/الثلاثي *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: nameController,
                    decoration: InputDecoration(
                      hintText: 'أدخل الاسم الكامل للطالب',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Guardian Name
                  const Text('اسم ولي الأمر', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: guardianController,
                    decoration: InputDecoration(
                      hintText: 'أدخل اسم ولي الأمر أو صلة القرابة',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Phone
                  const Text('رقم هاتف ولي الأمر *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: phoneController,
                    keyboardType: TextInputType.phone,
                    decoration: InputDecoration(
                      hintText: 'أدخل رقم الهاتف للتواصل (مثال: 0912345678)',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Opening Balance Amount
                  const Text('مبلغ الرصيد الافتتاحي السابق', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: amountController,
                    keyboardType: const TextInputType.numberWithOptions(decimal: true),
                    decoration: InputDecoration(
                      hintText: 'مثال: 150.00 (اكتب 0 في حال عدم وجود رصيد)',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Balance Type
                  const Text('طبيعة الرصيد السابق', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  Row(
                    children: [
                      Expanded(
                        child: RadioListTile<String>(
                          title: const Text('مطلوب منه / مدين', style: TextStyle(fontFamily: 'Cairo', fontSize: 13)),
                          value: 'Debit',
                          groupValue: balanceType,
                          activeColor: const Color(0xFF2563EB),
                          contentPadding: EdgeInsets.zero,
                          onChanged: (val) {
                            if (val != null) setDialogState(() => balanceType = val);
                          },
                        ),
                      ),
                      Expanded(
                        child: RadioListTile<String>(
                          title: const Text('دائن له / دفعة مقدمة', style: TextStyle(fontFamily: 'Cairo', fontSize: 13)),
                          value: 'Credit',
                          groupValue: balanceType,
                          activeColor: const Color(0xFF2563EB),
                          contentPadding: EdgeInsets.zero,
                          onChanged: (val) {
                            if (val != null) setDialogState(() => balanceType = val);
                          },
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 16),

                  // Notes
                  const Text('ملاحظات إضافية', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(height: 6),
                  TextFormField(
                    controller: notesController,
                    maxLines: 2,
                    decoration: InputDecoration(
                      hintText: 'أي تفاصيل أو ملاحظات عن حالة الطالب...',
                      hintStyle: const TextStyle(fontSize: 12),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
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
                if (nameController.text.trim().isEmpty || phoneController.text.trim().isEmpty) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('يرجى ملء الحقول المطلوبة (*)', style: TextStyle(fontFamily: 'Cairo'))),
                  );
                  return;
                }

                double opAmount = double.tryParse(amountController.text.trim()) ?? 0.0;

                final payload = {
                  'studentName': nameController.text.trim(),
                  'guardianName': guardianController.text.trim(),
                  'parentPhone': phoneController.text.trim(),
                  'notes': notesController.text.trim(),
                  'openingBalanceAmount': opAmount,
                  'balanceType': balanceType,
                };

                try {
                  if (isEdit) {
                    await ApiService.updateStudent(student['studentID'], payload);
                  } else {
                    await ApiService.createStudent(payload);
                  }
                  Navigator.pop(ctx);
                  _loadStudents();
                } catch (e) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
                  );
                }
              },
              style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFF2563EB)),
              child: Text(
                isEdit ? 'تعديل وحفظ' : 'تسجيل وإضافة',
                style: const TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white),
              ),
            ),
          ],
        ),
      ),
    );
  }

  void _deleteStudent(int id, String name) {
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('تأكيد الحذف ⚠️', textAlign: TextAlign.right, style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold)),
        content: Text(
          'هل أنت متأكد من حذف الطالب "$name" نهائياً؟ سيتم حذف جميع مستحقاته وسجلاته المالية وسندات القبض الخاصة به بشكل كامل من قاعدة البيانات!',
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
                await ApiService.deleteStudent(id);
                Navigator.pop(ctx);
                _loadStudents();
              } catch (e) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(e.toString(), style: const TextStyle(fontFamily: 'Cairo'))),
                );
              }
            },
            style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFFEF4444)),
            child: const Text('حذف نهائي', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
          ),
        ],
      ),
    );
  }

  void _showAccountStatement(int studentId, String name) {
    showDialog(
      context: context,
      builder: (ctx) => _AccountStatementDialog(studentId: studentId, studentName: name),
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
            // Top action bar
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'إدارة ملفات الطلاب والمنظومة',
                      style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold, color: Color(0xFF0F172A), fontFamily: 'Cairo'),
                    ),
                    SizedBox(height: 4),
                    Text(
                      'تسجيل الطلاب وتعديل الحسابات ومتابعة كشوفات الحسابات الفردية التراكمية.',
                      style: TextStyle(fontSize: 13, color: Color(0xFF64748B), fontFamily: 'Cairo'),
                    ),
                  ],
                ),
                ElevatedButton.icon(
                  onPressed: () => _showStudentDialog(),
                  icon: const Icon(Icons.add, color: Colors.white),
                  label: const Text('تسجيل طالب جديد 👤', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
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
                      onChanged: (_) => _loadStudents(),
                      decoration: const InputDecoration(
                        hintText: 'بحث سريع باسم الطالب، اسم ولي الأمر، أو رقم هاتف للتواصل...',
                        border: InputBorder.none,
                        hintStyle: TextStyle(fontSize: 13, color: Color(0xFF94A3B8)),
                      ),
                    ),
                  ),
                  if (_searchController.text.isNotEmpty)
                    IconButton(
                      onPressed: () {
                        _searchController.clear();
                        _loadStudents();
                      },
                      icon: const Icon(Icons.clear, size: 18),
                    ),
                ],
              ),
            ),
            const SizedBox(height: 24),

            // Grid content
            Expanded(
              child: Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(16),
                  border: Border.all(color: const Color(0xFFE2E8F0)),
                ),
                child: _isLoading
                    ? const Center(child: CircularProgressIndicator(color: Color(0xFF2563EB)))
                    : _studentsList.isEmpty
                        ? const Center(
                            child: Text(
                              'لا توجد سجلات طلاب مطابقة للبحث',
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
                                  DataColumn(label: Text('اسم الطالب')),
                                  DataColumn(label: Text('ولي الأمر')),
                                  DataColumn(label: Text('رقم الهاتف')),
                                  DataColumn(label: Text('تاريخ التسجيل')),
                                  DataColumn(label: Text('الرصيد السابق')),
                                  DataColumn(label: Text('خيارات العمل')),
                                ],
                                rows: _studentsList.map((row) {
                                  double opBal = (row['openingBalanceAmount'] as num?)?.toDouble() ?? 0.0;
                                  String balType = row['balanceType'] == 'Credit' ? 'دائن' : 'مدين';
                                  String opBalStr = opBal > 0 ? '${opBal.toStringAsFixed(2)} ($balType)' : 'لا يوجد';

                                  return DataRow(
                                    cells: [
                                      DataCell(Text(
                                        row['studentName'] ?? '',
                                        style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(row['guardianName'] ?? '', style: const TextStyle(fontFamily: 'Cairo'))),
                                      DataCell(Text(row['parentPhone'] ?? '', style: const TextStyle(fontFamily: 'Cairo'))),
                                      DataCell(Text(
                                        row['registrationDate'] != null
                                            ? row['registrationDate'].toString().substring(0, 10)
                                            : '',
                                        style: const TextStyle(fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(
                                        opBalStr,
                                        style: TextStyle(
                                          fontFamily: 'Cairo',
                                          color: opBal > 0
                                              ? (row['balanceType'] == 'Credit'
                                                  ? const Color(0xFF10B981)
                                                  : const Color(0xFFEF4444))
                                              : const Color(0xFF64748B),
                                        ),
                                      )),
                                      DataCell(Row(
                                        mainAxisSize: MainAxisSize.min,
                                        children: [
                                          IconButton(
                                            icon: const Icon(Icons.receipt_long, color: Color(0xFF2563EB), size: 20),
                                            tooltip: 'كشف الحساب التراكمي',
                                            onPressed: () => _showAccountStatement(row['studentID'], row['studentName']),
                                          ),
                                          IconButton(
                                            icon: const Icon(Icons.edit_outlined, color: Color(0xFF10B981), size: 20),
                                            tooltip: 'تعديل البيانات',
                                            onPressed: () => _showStudentDialog(student: row),
                                          ),
                                          IconButton(
                                            icon: const Icon(Icons.delete_outline, color: Color(0xFFEF4444), size: 20),
                                            tooltip: 'حذف',
                                            onPressed: () => _deleteStudent(row['studentID'], row['studentName']),
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

// Dialog class to display student account statements dynamically
class _AccountStatementDialog extends StatefulWidget {
  final int studentId;
  final String studentName;
  const _AccountStatementDialog({required this.studentId, required this.studentName});

  @override
  State<_AccountStatementDialog> createState() => _AccountStatementDialogState();
}

class _AccountStatementDialogState extends State<_AccountStatementDialog> {
  bool _isLoading = true;
  Map<String, dynamic> _statement = {};

  @override
  void initState() {
    super.initState();
    _loadStatement();
  }

  Future<void> _loadStatement() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final data = await ApiService.getAccountStatement(widget.studentId);
      if (mounted) {
        setState(() {
          _statement = data;
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

  void _exportCSV() {
    final list = _statement['transactions'] as List<dynamic>? ?? [];
    if (list.isEmpty) return;

    final sb = StringBuffer();
    sb.write('\uFEFF'); // BOM for Excel Arabic Support
    sb.writeln('تاريخ الحركة,نوع الحركة,البيان / الملاحظات,المطلوب / مدين,المدفوع / دائن,الرصيد المتبقي التراكمي,الموظف المسؤول');

    for (var row in list) {
      final date = row['transactionDate'].toString().replaceAll(',', ' ');
      final type = row['arabicType'].toString();
      final notes = row['notes'].toString().replaceAll(',', ' ');
      final debit = (row['debit'] as num).toStringAsFixed(2);
      final credit = (row['credit'] as num).toStringAsFixed(2);
      final balance = (row['runningBalance'] as num).toStringAsFixed(2);
      final emp = row['handlingEmployee'].toString();

      sb.writeln('$date,$type,$notes,$debit,$credit,$balance,$emp');
    }

    sb.writeln();
    sb.writeln('إجمالي المطلوب,,,,,${(_statement['totalCharged'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل');
    sb.writeln('إجمالي المدفوع,,,,,${(_statement['totalPaid'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل');
    sb.writeln('الرصيد المتبقي النهائي,,,,,${(_statement['finalBalance'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل');

    Clipboard.setData(ClipboardData(text: sb.toString()));
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(
        content: Text('تم نسخ كشف الحساب بتنسيق Excel بنجاح لعملية اللصق السريع!', style: TextStyle(fontFamily: 'Cairo')),
        backgroundColor: Color(0xFF10B981),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final list = _statement['transactions'] as List<dynamic>? ?? [];

    return AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      title: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(
            'كشف حساب الطالب: ${widget.studentName}',
            style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo', fontSize: 16),
          ),
          if (!_isLoading && list.isNotEmpty)
            ElevatedButton.icon(
              onPressed: _exportCSV,
              icon: const Icon(Icons.copy, size: 14, color: Colors.white),
              label: const Text('نسخ كشف الحساب', style: TextStyle(fontFamily: 'Cairo', fontSize: 12, color: Colors.white)),
              style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFF10B981)),
            ),
        ],
      ),
      content: SizedBox(
        width: 800,
        height: 500,
        child: _isLoading
            ? const Center(child: CircularProgressIndicator(color: Color(0xFF2563EB)))
            : Column(
                children: [
                  // Summary Header Box
                  Container(
                    padding: const EdgeInsets.all(16),
                    decoration: BoxDecoration(
                      color: const Color(0xFFF8FAFC),
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(color: const Color(0xFFE2E8F0)),
                    ),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceAround,
                      children: [
                        _buildSummaryItem('إجمالي المطلوب', '${(_statement['totalCharged'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل', const Color(0xFFEF4444)),
                        _buildSummaryItem('إجمالي المدفوع', '${(_statement['totalPaid'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل', const Color(0xFF10B981)),
                        _buildSummaryItem('الرصيد المتبقي النهائي', '${(_statement['finalBalance'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل', const Color(0xFF2563EB)),
                      ],
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Ledger Grid
                  Expanded(
                    child: Container(
                      decoration: BoxDecoration(
                        border: Border.all(color: const Color(0xFFE2E8F0)),
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: list.isEmpty
                          ? const Center(child: Text('لا توجد حركات مالية مسجلة لهذا الطالب', style: TextStyle(fontFamily: 'Cairo')))
                          : ClipRRect(
                              borderRadius: BorderRadius.circular(12),
                              child: SingleChildScrollView(
                                scrollDirection: Axis.vertical,
                                child: DataTable(
                                  headingRowColor: WidgetStateProperty.all(const Color(0xFF2563EB)),
                                  headingTextStyle: const TextStyle(
                                    color: Colors.white,
                                    fontFamily: 'Cairo',
                                    fontWeight: FontWeight.bold,
                                    fontSize: 12,
                                  ),
                                  columns: const [
                                    DataColumn(label: Text('التاريخ والوقت')),
                                    DataColumn(label: Text('نوع الحركة')),
                                    DataColumn(label: Text('البيان / الملاحظات')),
                                    DataColumn(label: Text('مطلوب / مدين')),
                                    DataColumn(label: Text('مدفوع / دائن')),
                                    DataColumn(label: Text('الرصيد التراكمي')),
                                    DataColumn(label: Text('الموظف')),
                                  ],
                                  rows: list.map((tx) {
                                    final date = tx['transactionDate'] != null
                                        ? tx['transactionDate'].toString().substring(0, 16).replaceAll('T', ' ')
                                        : '';
                                    return DataRow(
                                      cells: [
                                        DataCell(Text(date, style: const TextStyle(fontFamily: 'Cairo', fontSize: 11))),
                                        DataCell(Text(tx['arabicType'] ?? '', style: const TextStyle(fontFamily: 'Cairo', fontSize: 11, fontWeight: FontWeight.bold))),
                                        DataCell(Text(tx['notes'] ?? '', style: const TextStyle(fontFamily: 'Cairo', fontSize: 11))),
                                        DataCell(Text('${(tx['debit'] as num).toStringAsFixed(2)} د.ل', style: const TextStyle(fontFamily: 'Cairo', fontSize: 11))),
                                        DataCell(Text('${(tx['credit'] as num).toStringAsFixed(2)} د.ل', style: const TextStyle(fontFamily: 'Cairo', fontSize: 11, color: Color(0xFF10B981)))),
                                        DataCell(Text('${(tx['runningBalance'] as num).toStringAsFixed(2)} د.ل', style: const TextStyle(fontFamily: 'Cairo', fontSize: 11, fontWeight: FontWeight.bold))),
                                        DataCell(Text(tx['handlingEmployee'] ?? '-', style: const TextStyle(fontFamily: 'Cairo', fontSize: 11))),
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
      actions: [
        TextButton(
          onPressed: () => Navigator.pop(context),
          child: const Text('إغلاق كشف الحساب', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold)),
        ),
      ],
    );
  }

  Widget _buildSummaryItem(String title, String val, Color color) {
    return Column(
      children: [
        Text(title, style: const TextStyle(fontFamily: 'Cairo', fontSize: 12, color: Color(0xFF64748B))),
        const SizedBox(height: 4),
        Text(val, style: TextStyle(fontFamily: 'Cairo', fontSize: 16, fontWeight: FontWeight.bold, color: color)),
      ],
    );
  }
}

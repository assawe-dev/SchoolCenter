import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../services/api_service.dart';

class BalanceReportScreen extends StatefulWidget {
  const BalanceReportScreen({super.key});

  @override
  State<BalanceReportScreen> createState() => _BalanceReportScreenState();
}

class _BalanceReportScreenState extends State<BalanceReportScreen> {
  final _searchController = TextEditingController();
  bool _isLoading = true;
  List<dynamic> _balancesList = [];
  double _totalCompanyDebt = 0.0;

  @override
  void initState() {
    super.initState();
    _loadReport();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  Future<void> _loadReport() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final data = await ApiService.getBalancesReport(search: _searchController.text.trim());

      double totalDebt = 0.0;
      for (var row in data) {
        totalDebt += (row['outstandingBalance'] as num?)?.toDouble() ?? 0.0;
      }

      if (mounted) {
        setState(() {
          _balancesList = data;
          _totalCompanyDebt = totalDebt;
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
    if (_balancesList.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('لا توجد بيانات متاحة للتصدير', style: TextStyle(fontFamily: 'Cairo'))),
      );
      return;
    }

    final sb = StringBuffer();
    // Excel Arabic UTF-8 BOM
    sb.write('\uFEFF');
    sb.writeln('اسم الطالب,اسم ولي الأمر,رقم هاتف ولي الأمر,إجمالي المستحقات,إجمالي المدفوعات,الديون المتبقية');

    for (var r in _balancesList) {
      final name = r['studentName'].toString().replaceAll(',', ' ');
      final guardian = r['guardianName'].toString().replaceAll(',', ' ');
      final phone = r['parentPhone'].toString().replaceAll(',', ' ');
      final charged = (r['totalCharged'] as num?)?.toStringAsFixed(2) ?? '0.00';
      final paid = (r['totalPaid'] as num?)?.toStringAsFixed(2) ?? '0.00';
      final bal = (r['outstandingBalance'] as num?)?.toStringAsFixed(2) ?? '0.00';

      sb.writeln('$name,$guardian,$phone,$charged,$paid,$bal');
    }

    sb.writeln();
    sb.writeln('إجمالي الديون المستحقة الكلية,,,,${_totalCompanyDebt.toStringAsFixed(2)} د.ل');

    final csvText = sb.toString();

    // Show cross-platform Copy Dialog
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        title: const Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              'تصدير كشف الأرصدة والديون',
              style: TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo', fontSize: 16),
            ),
            Icon(Icons.insert_drive_file, color: Color(0xFF10B981)),
          ],
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            const Text(
              'تم توليد ملف Excel (CSV) متوافق تماماً مع الحروف العربية والترميز الصحيح. يمكنك نسخ محتوى التقرير بنقرة واحدة ولصقه مباشرة في ملف Excel الخاص بك.',
              style: TextStyle(fontFamily: 'Cairo', fontSize: 13, height: 1.5),
            ),
            const SizedBox(height: 16),
            Container(
              constraints: const BoxConstraints(maxHeight: 120),
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: const Color(0xFFF1F5F9),
                borderRadius: BorderRadius.circular(8),
              ),
              child: SingleChildScrollView(
                child: Text(
                  csvText,
                  style: const TextStyle(fontSize: 10, fontFamily: 'monospace'),
                ),
              ),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx),
            child: const Text('إلغاء', style: TextStyle(fontFamily: 'Cairo')),
          ),
          ElevatedButton.icon(
            onPressed: () {
              Clipboard.setData(ClipboardData(text: csvText));
              Navigator.pop(ctx);
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(
                  content: Text('تم نسخ بيانات التقرير بتنسيق Excel بنجاح!', style: TextStyle(fontFamily: 'Cairo')),
                  backgroundColor: Color(0xFF10B981),
                ),
              );
            },
            icon: const Icon(Icons.copy, size: 16, color: Colors.white),
            label: const Text('نسخ البيانات للتصدير', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
            style: ElevatedButton.styleFrom(backgroundColor: const Color(0xFF10B981)),
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
            // Header panel with export button
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'تقرير الأرصدة والديون الشامل',
                      style: TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFF0F172A),
                        fontFamily: 'Cairo',
                      ),
                    ),
                    SizedBox(height: 4),
                    Text(
                      'مراقبة وتتبع أرصدة الطلاب والمبالغ المتبقية والمسددة.',
                      style: TextStyle(
                        fontSize: 13,
                        color: Color(0xFF64748B),
                        fontFamily: 'Cairo',
                      ),
                    ),
                  ],
                ),
                ElevatedButton.icon(
                  onPressed: _exportCSV,
                  icon: const Icon(Icons.download, size: 18, color: Colors.white),
                  label: const Text('تصدير البيانات 📥', style: TextStyle(fontFamily: 'Cairo', fontWeight: FontWeight.bold, color: Colors.white)),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF10B981), // Green Accent
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
                      onChanged: (_) => _loadReport(),
                      decoration: const InputDecoration(
                        hintText: 'بحث سريع باسم الطالب أو رقم الهاتف...',
                        border: InputBorder.none,
                        hintStyle: TextStyle(fontSize: 13, color: Color(0xFF94A3B8)),
                      ),
                    ),
                  ),
                  if (_searchController.text.isNotEmpty)
                    IconButton(
                      onPressed: () {
                        _searchController.clear();
                        _loadReport();
                      },
                      icon: const Icon(Icons.clear, size: 18),
                    ),
                ],
              ),
            ),
            const SizedBox(height: 24),

            // Table Content Area
            Expanded(
              child: Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(16),
                  border: Border.all(color: const Color(0xFFE2E8F0)),
                ),
                child: _isLoading
                    ? const Center(child: CircularProgressIndicator(color: Color(0xFF2563EB)))
                    : _balancesList.isEmpty
                        ? const Center(
                            child: Text(
                              'لا توجد سجلات مطابقة للبحث',
                              style: TextStyle(fontFamily: 'Cairo', color: Color(0xFF64748B)),
                            ),
                          )
                        : ClipRRect(
                            borderRadius: BorderRadius.circular(16),
                            child: SingleChildScrollView(
                              scrollDirection: Axis.vertical,
                              child: DataTable(
                                headingRowColor: WidgetStateProperty.all(const Color(0xFF2563EB)),
                                headingTextStyle: const TextStyle(
                                  color: Colors.white,
                                  fontFamily: 'Cairo',
                                  fontWeight: FontWeight.bold,
                                  fontSize: 13,
                                ),
                                columns: const [
                                  DataColumn(label: Text('اسم الطالب')),
                                  DataColumn(label: Text('اسم ولي الأمر')),
                                  DataColumn(label: Text('رقم الهاتف')),
                                  DataColumn(label: Text('إجمالي المستحقات')),
                                  DataColumn(label: Text('إجمالي المدفوعات')),
                                  DataColumn(label: Text('الديون المتبقية')),
                                ],
                                rows: _balancesList.map((row) {
                                  double bal = (row['outstandingBalance'] as num?)?.toDouble() ?? 0.0;
                                  return DataRow(
                                    cells: [
                                      DataCell(Text(
                                        row['studentName'] ?? '',
                                        style: const TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(row['guardianName'] ?? '', style: const TextStyle(fontFamily: 'Cairo'))),
                                      DataCell(Text(row['parentPhone'] ?? '', style: const TextStyle(fontFamily: 'Cairo'))),
                                      DataCell(Text(
                                        '${((row['totalCharged'] ?? row['totalCharged']) as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل',
                                        style: const TextStyle(fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(
                                        '${((row['totalPaid'] ?? row['totalPaid']) as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل',
                                        style: const TextStyle(color: Color(0xFF10B981), fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
                                      )),
                                      DataCell(Text(
                                        '${bal.toStringAsFixed(2)} د.ل',
                                        style: TextStyle(
                                          color: bal > 0 ? const Color(0xFFEF4444) : const Color(0xFF10B981),
                                          fontWeight: FontWeight.bold,
                                          fontFamily: 'Cairo',
                                        ),
                                      )),
                                    ],
                                  );
                                }).toList(),
                              ),
                            ),
                          ),
              ),
            ),
            const SizedBox(height: 24),

            // Summary Panel
            Container(
              decoration: BoxDecoration(
                color: const Color(0xFFFEE2E2), // Muted red background
                borderRadius: BorderRadius.circular(16),
                border: Border.all(color: const Color(0xFFFCA5A5)),
              ),
              padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 16),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  const Row(
                    children: [
                      Icon(Icons.warning_amber_rounded, color: Color(0xFFDC2626)),
                      SizedBox(width: 12),
                      Text(
                        'إجمالي الديون المستحقة الكلية:',
                        style: TextStyle(
                          color: Color(0xFF991B1B),
                          fontWeight: FontWeight.bold,
                          fontSize: 16,
                          fontFamily: 'Cairo',
                        ),
                      ),
                    ],
                  ),
                  Text(
                    '${_totalCompanyDebt.toStringAsFixed(2)} د.ل',
                    style: const TextStyle(
                      color: Color(0xFF991B1B),
                      fontWeight: FontWeight.bold,
                      fontSize: 22,
                      fontFamily: 'Cairo',
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

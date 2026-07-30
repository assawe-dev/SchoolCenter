import 'package:flutter/material.dart';
import '../services/api_service.dart';

class DashboardScreen extends StatefulWidget {
  const DashboardScreen({super.key});

  @override
  State<DashboardScreen> createState() => _DashboardScreenState();
}

class _DashboardScreenState extends State<DashboardScreen> with SingleTickerProviderStateMixin {
  bool _isLoading = true;
  Map<String, dynamic> _stats = {};
  Map<String, dynamic> _chartData = {};
  List<dynamic> _recentTx = [];

  late AnimationController _chartAnimationController;
  late Animation<double> _chartAnimation;

  @override
  void initState() {
    super.initState();
    _chartAnimationController = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 1500),
    );
    _chartAnimation = Tween<double>(begin: 0, end: 1).animate(
      CurvedAnimation(parent: _chartAnimationController, curve: Curves.decelerate),
    );
    _loadDashboardData();
  }

  @override
  void dispose() {
    _chartAnimationController.dispose();
    super.dispose();
  }

  Future<void> _loadDashboardData() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final statsFuture = ApiService.getStats();
      final chartFuture = ApiService.getDonutChartData();
      final txFuture = ApiService.getRecentTransactions();

      final results = await Future.wait([statsFuture, chartFuture, txFuture]);

      if (mounted) {
        setState(() {
          _stats = results[0] as Map<String, dynamic>;
          _chartData = results[1] as Map<String, dynamic>;
          _recentTx = results[2] as List<dynamic>;
          _isLoading = false;
        });
        _chartAnimationController.forward(from: 0.0);
      }
    } catch (_) {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return const Scaffold(
        body: Center(
          child: CircularProgressIndicator(
            color: Color(0xFF2563EB),
          ),
        ),
      );
    }

    final double totalPaid = (_chartData['totalPaid'] as num?)?.toDouble() ?? 0.0;
    final double totalOutstanding = (_chartData['totalOutstanding'] as num?)?.toDouble() ?? 0.0;
    final double totalFinancials = totalPaid + totalOutstanding;
    final double paidPercentage = totalFinancials > 0 ? (totalPaid / totalFinancials) : 0.0;

    final isDesktop = MediaQuery.of(context).size.width > 1000;

    return Scaffold(
      body: RefreshIndicator(
        onRefresh: _loadDashboardData,
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(24.0),
          physics: const AlwaysScrollableScrollPhysics(),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              // Welcome Banner
              _buildWelcomeBanner(),
              const SizedBox(height: 24),

              // Stat Cards Grid
              _buildStatsGrid(isDesktop),
              const SizedBox(height: 24),

              // Split Layout: Donut Chart & Recent Operations Table
              if (isDesktop)
                Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Expanded(
                      flex: 3,
                      child: _buildRecentTransactionsCard(),
                    ),
                    const SizedBox(width: 24),
                    Expanded(
                      flex: 2,
                      child: _buildDonutChartCard(paidPercentage, totalPaid, totalOutstanding),
                    ),
                  ],
                )
              else ...[
                _buildDonutChartCard(paidPercentage, totalPaid, totalOutstanding),
                const SizedBox(height: 24),
                _buildRecentTransactionsCard(),
              ],
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildWelcomeBanner() {
    return Container(
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [Color(0xFF1E3A8A), Color(0xFF3B82F6)],
          begin: Alignment.topRight,
          end: Alignment.bottomLeft,
        ),
        borderRadius: BorderRadius.circular(16),
        boxShadow: [
          BoxShadow(
            color: const Color(0xFF2563EB).withOpacity(0.15),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      padding: const EdgeInsets.all(24),
      child: const Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'مرحباً بك في لوحة تحكم المنظومة 👋',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                    fontFamily: 'Cairo',
                  ),
                ),
                SizedBox(height: 8),
                Text(
                  'هنا تجد ملخصاً سرياً وشاملاً لأحدث العمليات المالية والإحصاءات لمركزك التعليمي في الوقت الفعلي.',
                  style: TextStyle(
                    color: Color(0xFFBFDBFE),
                    fontSize: 14,
                    height: 1.5,
                    fontFamily: 'Cairo',
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatsGrid(bool isDesktop) {
    final cards = [
      _buildStatCard(
        title: 'الطلاب المسجلين',
        value: '${_stats['totalStudents'] ?? 0}',
        subtitle: 'إجمالي المشتركين بالمنظومة',
        icon: Icons.people_alt,
        color: const Color(0xFF2563EB),
        bgColor: const Color(0xFFEFF6FF),
      ),
      _buildStatCard(
        title: 'الدورات الفعالة',
        value: '${_stats['totalCourses'] ?? 0}',
        subtitle: 'الدورات المتاحة للتسجيل',
        icon: Icons.school,
        color: const Color(0xFF8B5CF6),
        bgColor: const Color(0xFFF5F3FF),
      ),
      _buildStatCard(
        title: 'رصيد الخزينة الحالي',
        value: '${(_stats['currentTreasuryBalance'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل',
        subtitle: 'المقبوضات والتحصيلات النقدية',
        icon: Icons.account_balance_wallet,
        color: const Color(0xFF10B981),
        bgColor: const Color(0xFFECFDF5),
      ),
      _buildStatCard(
        title: 'إجمالي الديون المعلقة',
        value: '${(_stats['totalOutstandingDebts'] as num?)?.toStringAsFixed(2) ?? '0.00'} د.ل',
        subtitle: 'مستحقات الطلاب غير المحصلة',
        icon: Icons.assignment_late,
        color: const Color(0xFFEF4444),
        bgColor: const Color(0xFFFEF2F2),
      ),
    ];

    if (isDesktop) {
      return Row(
        children: cards.map((card) => Expanded(child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 6.0),
          child: card,
        ))).toList(),
      );
    } else {
      return GridView.count(
        crossAxisCount: 2,
        shrinkWrap: true,
        physics: const NeverScrollableScrollPhysics(),
        crossAxisSpacing: 12,
        mainAxisSpacing: 12,
        childAspectRatio: 1.4,
        children: cards,
      );
    }
  }

  Widget _buildStatCard({
    required String title,
    required String value,
    required String subtitle,
    required IconData icon,
    required Color color,
    required Color bgColor,
  }) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: const Color(0xFFE2E8F0), width: 1),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.02),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      padding: const EdgeInsets.all(20),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                title,
                style: const TextStyle(
                  fontSize: 14,
                  color: Color(0xFF64748B),
                  fontWeight: FontWeight.bold,
                  fontFamily: 'Cairo',
                ),
              ),
              Container(
                padding: const EdgeInsets.all(10),
                decoration: BoxDecoration(
                  color: bgColor,
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Icon(icon, color: color, size: 22),
              ),
            ],
          ),
          const SizedBox(height: 12),
          Text(
            value,
            style: const TextStyle(
              fontSize: 22,
              fontWeight: FontWeight.bold,
              color: Color(0xFF0F172A),
              fontFamily: 'Cairo',
            ),
          ),
          const SizedBox(height: 4),
          Text(
            subtitle,
            style: const TextStyle(
              fontSize: 11,
              color: Color(0xFF94A3B8),
              fontFamily: 'Cairo',
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildDonutChartCard(double paidPercentage, double paid, double outstanding) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: const Color(0xFFE2E8F0), width: 1),
      ),
      padding: const EdgeInsets.all(24),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          const Text(
            'نسبة التحصيل المالي',
            style: TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.bold,
              color: Color(0xFF0F172A),
              fontFamily: 'Cairo',
            ),
          ),
          const SizedBox(height: 32),

          // Custom Painted Circular Progress
          AnimatedBuilder(
            animation: _chartAnimation,
            builder: (context, child) {
              return Center(
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    SizedBox(
                      width: 170,
                      height: 170,
                      child: CircularProgressIndicator(
                        value: paidPercentage * _chartAnimation.value,
                        strokeWidth: 22,
                        backgroundColor: const Color(0xFFF1F5F9),
                        color: const Color(0xFF10B981), // Paid Color green
                        strokeCap: StrokeCap.round,
                      ),
                    ),
                    Column(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Text(
                          '${(paidPercentage * 100).toStringAsFixed(1)}%',
                          style: const TextStyle(
                            fontSize: 24,
                            fontWeight: FontWeight.bold,
                            color: Color(0xFF0F172A),
                            fontFamily: 'Cairo',
                          ),
                        ),
                        const Text(
                          'نسبة المسدد الكلي',
                          style: TextStyle(
                            fontSize: 12,
                            color: Color(0xFF64748B),
                            fontFamily: 'Cairo',
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              );
            },
          ),

          const SizedBox(height: 32),

          // Legend Indicators
          _buildLegendRow('المدفوعات المستلمة', paid, const Color(0xFF10B981)),
          const SizedBox(height: 12),
          _buildLegendRow('الديون والمستحقات المتبقية', outstanding, const Color(0xFFEF4444)),
        ],
      ),
    );
  }

  Widget _buildLegendRow(String label, double amount, Color color) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Row(
          children: [
            Container(
              width: 12,
              height: 12,
              decoration: BoxDecoration(
                color: color,
                shape: BoxShape.circle,
              ),
            ),
            const SizedBox(width: 8),
            Text(
              label,
              style: const TextStyle(
                fontSize: 13,
                color: Color(0xFF475569),
                fontFamily: 'Cairo',
              ),
            ),
          ],
        ),
        Text(
          '${amount.toStringAsFixed(2)} د.ل',
          style: const TextStyle(
            fontSize: 13,
            fontWeight: FontWeight.bold,
            color: Color(0xFF0F172A),
            fontFamily: 'Cairo',
          ),
        ),
      ],
    );
  }

  Widget _buildRecentTransactionsCard() {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: const Color(0xFFE2E8F0), width: 1),
      ),
      padding: const EdgeInsets.all(24),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text(
                'آخر العمليات المالية المباشرة',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: Color(0xFF0F172A),
                  fontFamily: 'Cairo',
                ),
              ),
              TextButton(
                onPressed: _loadDashboardData,
                child: const Row(
                  children: [
                    Icon(Icons.refresh, size: 16, color: Color(0xFF2563EB)),
                    SizedBox(width: 4),
                    Text('تحديث البيانات', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, color: Color(0xFF2563EB))),
                  ],
                ),
              ),
            ],
          ),
          const SizedBox(height: 16),

          // Table list
          if (_recentTx.isEmpty)
            const Padding(
              padding: EdgeInsets.symmetric(vertical: 48),
              child: Center(
                child: Text(
                  'لا توجد عمليات مالية مسجلة حالياً',
                  style: TextStyle(fontFamily: 'Cairo', color: Color(0xFF64748B)),
                ),
              ),
            )
          else
            ListView.separated(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              itemCount: _recentTx.length,
              separatorBuilder: (_, __) => const Divider(height: 1, color: Color(0xFFF1F5F9)),
              itemBuilder: (context, index) {
                final tx = _recentTx[index];
                final isPayment = tx['transactionType'] == 'Payment Receipt';
                final isOpening = tx['transactionType'] == 'Opening Balance';

                Color badgeBg;
                Color badgeText;
                String typeStr;

                if (isPayment) {
                  badgeBg = const Color(0xFFDEF7EC);
                  badgeText = const Color(0xFF03543F);
                  typeStr = 'سند قبض';
                } else if (isOpening) {
                  badgeBg = const Color(0xFFEFF6FF);
                  badgeText = const Color(0xFF1E40AF);
                  typeStr = 'رصيد سابق';
                } else {
                  badgeBg = const Color(0xFFFDE8E8);
                  badgeText = const Color(0xFF9B1C1C);
                  typeStr = 'مستحقات رسوم';
                }

                double val = isPayment ? (tx['credit'] as num).toDouble() : (tx['debit'] as num).toDouble();

                return Padding(
                  padding: const EdgeInsets.symmetric(vertical: 12.0),
                  child: Row(
                    children: [
                      // Pill badge
                      Container(
                        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                        decoration: BoxDecoration(
                          color: badgeBg,
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: Text(
                          typeStr,
                          style: TextStyle(
                            color: badgeText,
                            fontSize: 11,
                            fontWeight: FontWeight.bold,
                            fontFamily: 'Cairo',
                          ),
                        ),
                      ),
                      const SizedBox(width: 16),

                      // Student Info & Notes
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              tx['studentName'] ?? 'طالب مجهول',
                              style: const TextStyle(
                                fontSize: 14,
                                fontWeight: FontWeight.bold,
                                color: Color(0xFF0F172A),
                                fontFamily: 'Cairo',
                              ),
                            ),
                            const SizedBox(height: 4),
                            Text(
                              tx['notes'] ?? '',
                              maxLines: 1,
                              overflow: TextOverflow.ellipsis,
                              style: const TextStyle(
                                fontSize: 12,
                                color: Color(0xFF64748B),
                                fontFamily: 'Cairo',
                              ),
                            ),
                          ],
                        ),
                      ),

                      // Value & Date
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.end,
                        children: [
                          Text(
                            '${val.toStringAsFixed(2)} د.ل',
                            style: TextStyle(
                              fontSize: 14,
                              fontWeight: FontWeight.bold,
                              color: isPayment ? const Color(0xFF10B981) : const Color(0xFFEF4444),
                              fontFamily: 'Cairo',
                            ),
                          ),
                          const SizedBox(height: 4),
                          Text(
                            _formatDate(tx['transactionDate']),
                            style: const TextStyle(
                              fontSize: 11,
                              color: Color(0xFF94A3B8),
                              fontFamily: 'Cairo',
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                );
              },
            ),
        ],
      ),
    );
  }

  String _formatDate(dynamic dateStr) {
    if (dateStr == null) return '';
    try {
      final dt = DateTime.parse(dateStr.toString());
      return '${dt.year}/${dt.month.toString().padLeft(2, '0')}/${dt.day.toString().padLeft(2, '0')}';
    } catch (_) {
      return dateStr.toString();
    }
  }
}

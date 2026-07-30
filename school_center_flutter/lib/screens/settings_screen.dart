import 'dart:convert';
import 'package:flutter/material.dart';
import '../services/api_service.dart';

class SettingsScreen extends StatefulWidget {
  final VoidCallback? onSettingsSaved;
  const SettingsScreen({super.key, this.onSettingsSaved});

  @override
  State<SettingsScreen> createState() => _SettingsScreenState();
}

class _SettingsScreenState extends State<SettingsScreen> {
  final _formKey = GlobalKey<FormState>();
  final _nameController = TextEditingController();
  final _logoController = TextEditingController();

  bool _isLoading = true;
  bool _isSaving = false;
  String? _logoBase64;

  @override
  void initState() {
    super.initState();
    _loadSettings();
  }

  @override
  void dispose() {
    _nameController.dispose();
    _logoController.dispose();
    super.dispose();
  }

  Future<void> _loadSettings() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final data = await ApiService.getSettings();
      if (mounted) {
        setState(() {
          _nameController.text = data['centerName'] ?? '';
          _logoBase64 = data['logoBase64'];
          _logoController.text = data['logoBase64'] ?? '';
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

  Future<void> _saveSettings() async {
    if (_formKey.currentState!.validate()) {
      setState(() {
        _isSaving = true;
      });

      try {
        final logoVal = _logoController.text.trim();
        await ApiService.saveSettings(
          _nameController.text.trim(),
          logoVal.isEmpty ? null : logoVal,
        );

        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('تم حفظ وتعديل إعدادات الهوية بنجاح! 🏫', style: TextStyle(fontFamily: 'Cairo')),
              backgroundColor: Color(0xFF10B981),
            ),
          );

          if (widget.onSettingsSaved != null) {
            widget.onSettingsSaved!();
          }

          _loadSettings();
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
          : SingleChildScrollView(
              padding: const EdgeInsets.all(32.0),
              child: Center(
                child: Container(
                  constraints: const BoxConstraints(maxWidth: 700),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(16),
                    border: Border.all(color: const Color(0xFFE2E8F0)),
                  ),
                  padding: const EdgeInsets.all(32),
                  child: Form(
                    key: _formKey,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        // Title Header
                        const Row(
                          children: [
                            Icon(Icons.settings_suggest_outlined, size: 28, color: Color(0xFF2563EB)),
                            SizedBox(width: 12),
                            Text(
                              'إعدادات الملف التعريفي والهوية للمركز',
                              style: TextStyle(
                                fontSize: 20,
                                fontWeight: FontWeight.bold,
                                color: Color(0xFF0F172A),
                                fontFamily: 'Cairo',
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 8),
                        const Text(
                          'تخصيص الهوية البصرية واسم المنشأة ليعكس ذلك تلقائياً على واجهات البرنامج، الترويسات، وسندات القبض المطبوعة والملفات المصدّرة.',
                          style: TextStyle(
                            fontSize: 13,
                            color: Color(0xFF64748B),
                            fontFamily: 'Cairo',
                            height: 1.5,
                          ),
                        ),
                        const SizedBox(height: 32),

                        // Logo Branding Preview Box
                        const Text('معاينة شعار المركز الحالي', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 12),
                        Center(
                          child: Container(
                            width: 120,
                            height: 120,
                            decoration: BoxDecoration(
                              color: const Color(0xFFF8FAFC),
                              borderRadius: BorderRadius.circular(20),
                              border: Border.all(color: const Color(0xFFE2E8F0), width: 1.5),
                            ),
                            child: _logoBase64 != null
                                ? ClipRRect(
                                    borderRadius: BorderRadius.circular(18),
                                    child: Image.memory(
                                      base64Decode(_logoBase64!),
                                      fit: BoxFit.cover,
                                    ),
                                  )
                                : const Center(
                                    child: Text(
                                      '🏫',
                                      style: TextStyle(fontSize: 48),
                                    ),
                                  ),
                          ),
                        ),
                        const SizedBox(height: 24),

                        // Center Name Input
                        const Text('اسم المركز التعليمي / المدرسة الدراسية *', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        TextFormField(
                          controller: _nameController,
                          decoration: InputDecoration(
                            hintText: 'مثال: مركز التفوق للتدريب والاستشارات',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                          ),
                          validator: (value) {
                            if (value == null || value.trim().isEmpty) {
                              return 'يرجى إدخال اسم المركز التعليمي';
                            }
                            return null;
                          },
                        ),
                        const SizedBox(height: 20),

                        // Logo Base64 Input
                        const Text('كود الشعار (Base64 Logo Image String)', style: TextStyle(fontFamily: 'Cairo', fontSize: 13, fontWeight: FontWeight.bold)),
                        const SizedBox(height: 8),
                        TextFormField(
                          controller: _logoController,
                          maxLines: 4,
                          decoration: InputDecoration(
                            hintText: 'ألصق كود الصورة المشفرة Base64 هنا لتحديث الشعار، أو اتركها فارغة لعرض الشعار الافتراضي 🏫',
                            border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                            hintStyle: const TextStyle(fontSize: 12, height: 1.5),
                          ),
                        ),
                        const SizedBox(height: 12),

                        // Built-in presets for ease of testing
                        const Text('أو اختر شعاراً سريعاً للتجربة:', style: TextStyle(fontFamily: 'Cairo', fontSize: 12, color: Color(0xFF64748B))),
                        const SizedBox(height: 8),
                        Row(
                          children: [
                            _buildPresetButton('إفراغ الشعار', ''),
                            const SizedBox(width: 12),
                            _buildPresetButton('لوجو نجمة التجريبية ⭐', 'iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAB3RJTUUHAAUFBgYDBgYGBgAAAD1JREFUeNrt0AENAAAAwqD3T20ON6hAYcCAAQMCBAgQIECAAAECBAgQIECAAAECBAgQIECAAAECBAgQIEBgewEcdAABb69pyAAAAABJRU5ErkJggg=='),
                          ],
                        ),
                        const SizedBox(height: 32),

                        // Action Buttons
                        Row(
                          children: [
                            Expanded(
                              child: OutlinedButton(
                                onPressed: _loadSettings,
                                style: OutlinedButton.styleFrom(
                                  side: const BorderSide(color: Color(0xFFCBD5E1)),
                                  padding: const EdgeInsets.symmetric(vertical: 16),
                                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                                ),
                                child: const Text(
                                  'تراجع عن التعديل',
                                  style: TextStyle(color: Color(0xFF475569), fontWeight: FontWeight.bold, fontFamily: 'Cairo'),
                                ),
                              ),
                            ),
                            const SizedBox(width: 16),
                            Expanded(
                              child: ElevatedButton(
                                onPressed: _isSaving ? null : _saveSettings,
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
                                        'حفظ وتطبيق التغييرات',
                                        style: TextStyle(fontWeight: FontWeight.bold, fontFamily: 'Cairo', color: Colors.white),
                                      ),
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),
    );
  }

  Widget _buildPresetButton(String label, String base64) {
    return ElevatedButton(
      onPressed: () {
        setState(() {
          _logoController.text = base64;
          _logoBase64 = base64.isEmpty ? null : base64;
        });
      },
      style: ElevatedButton.styleFrom(
        backgroundColor: const Color(0xFFF1F5F9),
        foregroundColor: const Color(0xFF0F172A),
        elevation: 0,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
      ),
      child: Text(label, style: const TextStyle(fontFamily: 'Cairo', fontSize: 12)),
    );
  }
}

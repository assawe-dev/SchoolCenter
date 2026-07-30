import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'screens/login_screen.dart';
import 'screens/setup_screen.dart';
import 'screens/main_navigation_layout.dart';

void main() {
  runApp(const SchoolCenterApp());
}

class SchoolCenterApp extends StatelessWidget {
  const SchoolCenterApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'مركز المدرسة - نظام الحسابات والطلاب',
      debugShowCheckedModeBanner: false,
      locale: const Locale('ar', 'AE'), // Default to Arabic Right-To-Left
      supportedLocales: const [
        Locale('ar', 'AE'),
        Locale('en', 'US'),
      ],
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      theme: ThemeData(
        useMaterial3: true,
        primaryColor: const Color(0xFF2563EB), // Accent Blue
        scaffoldBackgroundColor: const Color(0xFFF8FAFC), // Modern SaaS Light background
        fontFamily: 'Cairo', // Enterprise-grade Cairo Font theme
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFF2563EB),
          primary: const Color(0xFF2563EB),
          secondary: const Color(0xFF0F172A), // Slate Navy sidebar color
          surface: const Color(0xFFF8FAFC),
        ),
      ),
      initialRoute: '/login',
      routes: {
        '/login': (context) => const LoginScreen(),
        '/setup': (context) => const SetupScreen(),
        '/dashboard': (context) => const MainNavigationLayout(),
      },
    );
  }
}

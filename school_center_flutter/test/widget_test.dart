import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:school_center_flutter/main.dart';

void main() {
  testWidgets('App loads and renders Login Screen successfully', (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(const SchoolCenterApp());

    // Verify that the login title or form is present.
    expect(find.text('تسجيل الدخول'), findsWidgets);
    expect(find.byType(TextFormField), findsNWidgets(2));
  });
}

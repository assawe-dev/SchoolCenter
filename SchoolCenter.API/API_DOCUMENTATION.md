# SchoolCenter.API - Endpoint Documentation

This document maps all RESTful JSON endpoints exposed by the new ASP.NET Core API backend. These endpoints correspond to the extracted logic from the WinForms desktop app and are ready to be integrated into the upcoming **Flutter Frontend**.

---

## Base URL
When running locally, the base URL is typically:
```
http://localhost:5000
```
*(or any port configured in `launchSettings.json` or your server instance)*

---

## 1. Authentication Module (`AuthController`)

### **Login**
Authenticates user credentials and retrieves detailed screen-level permissions.

* **URL:** `/api/auth/login`
* **Method:** `POST`
* **Headers:** `Content-Type: application/json`
* **Request Body:**
  ```json
  {
    "username": "admin",
    "password": "admin123"
  }
  ```
* **Response (Success `200 OK`):**
  ```json
  {
    "token": "mock-jwt-token-for-school-center-api-abc123xyz...",
    "userID": 1,
    "username": "admin",
    "role": "Admin",
    "permissions": {
      "canManageStudents": true,
      "canManageCourses": true,
      "canAssignDues": true,
      "canReceivePayments": true,
      "canViewReports": true,
      "canManageUsers": true
    }
  }
  ```
* **Response (Unauthorized `401 Unauthorized`):**
  ```json
  {
    "message": "اسم المستخدم أو كلمة المرور غير صحيحة"
  }
  ```

---

## 2. Dashboard Module (`DashboardController`)

### **Get System Stats**
Retrieves numbers for the 4 stat cards on the dashboard.

* **URL:** `/api/dashboard/stats`
* **Method:** `GET`
* **Response (Success `200 OK`):**
  ```json
  {
    "totalStudents": 25,
    "totalCourses": 5,
    "currentTreasuryBalance": 4500.00,
    "totalOutstandingDebts": 1250.00
  }
  ```

---

### **Get Donut Chart Data**
Retrieves distribution metrics for the main financial doughnut chart.

* **URL:** `/api/dashboard/donut-chart`
* **Method:** `GET`
* **Response (Success `200 OK`):**
  ```json
  {
    "totalPaid": 4500.00,
    "totalOutstanding": 1250.00,
    "labels": ["المدفوعات المستلمة", "الديون المستحقة"]
  }
  ```

---

### **Get Recent Transactions**
Retrieves the 10 most recent transactions across all students.

* **URL:** `/api/dashboard/recent-transactions`
* **Method:** `GET`
* **Response (Success `200 OK`):**
  ```json
  [
    {
      "transactionID": 15,
      "studentName": "أحمد علي",
      "transactionType": "Payment Receipt",
      "debit": 0.00,
      "credit": 150.00,
      "transactionDate": "2025-10-25T14:30:00",
      "notes": "إيصال سداد رسوم"
    },
    {
      "transactionID": 14,
      "studentName": "فاطمة محمد",
      "transactionType": "Fee Charge",
      "debit": 200.00,
      "credit": 0.00,
      "transactionDate": "2025-10-24T09:15:00",
      "notes": "تعيين دورة: لغة إنجليزية - مستوى متوسط"
    }
  ]
  ```

---

## 3. Students Module (`StudentsController`)

### **Get All Students**
Retrieves a list of all students. Supports full-text search by name/phone.

* **URL:** `/api/students`
* **Method:** `GET`
* **Query Parameters (Optional):**
  * `search`: A search keyword filtering by `StudentName`, `ParentPhone`, or `GuardianName`.
* **Response (Success `200 OK`):**
  ```json
  [
    {
      "studentID": 12,
      "studentName": "أحمد علي",
      "guardianName": "علي أحمد",
      "parentPhone": "0912345678",
      "registrationDate": "2025-10-01T10:00:00",
      "notes": "ملاحظة للتواصل المباشر",
      "openingBalanceAmount": 100.00,
      "balanceType": "Debit"
    }
  ]
  ```

---

### **Get Single Student**
Retrieves details of a single student by ID.

* **URL:** `/api/students/{id}`
* **Method:** `GET`
* **Response (Success `200 OK`):**
  ```json
  {
    "studentID": 12,
    "studentName": "أحمد علي",
    "guardianName": "علي أحمد",
    "parentPhone": "0912345678",
    "registrationDate": "2025-10-01T10:00:00",
    "notes": "ملاحظة للتواصل المباشر",
    "openingBalanceAmount": 100.00,
    "balanceType": "Debit"
  }
  ```

---

### **Get Student Balance**
Retrieves the outstanding net financial balance for a student.

* **URL:** `/api/students/{id}/balance`
* **Method:** `GET`
* **Response (Success `200 OK`):**
  ```json
  {
    "studentID": 12,
    "outstandingBalance": 100.00
  }
  ```

---

### **Create Student**
Adds a new student and handles optional opening balance registration.

* **URL:** `/api/students`
* **Method:** `POST`
* **Headers:** `Content-Type: application/json`
* **Request Body:**
  ```json
  {
    "studentName": "أحمد علي",
    "guardianName": "علي أحمد",
    "parentPhone": "0912345678",
    "notes": "ملاحظة للتواصل المباشر",
    "openingBalanceAmount": 100.00,
    "balanceType": "Debit"
  }
  ```
* **Response (Success `210 Created`):**
  ```json
  {
    "studentID": 12,
    "message": "تم إضافة الطالب بنجاح"
  }
  ```

---

### **Update Student**
Updates student registration info and adjusts their opening balance transaction inside a SQL transaction.

* **URL:** `/api/students/{id}`
* **Method:** `PUT`
* **Headers:** `Content-Type: application/json`
* **Request Body:**
  ```json
  {
    "studentName": "أحمد علي المعدل",
    "guardianName": "علي أحمد",
    "parentPhone": "0912345678",
    "notes": "ملاحظة معدلة",
    "openingBalanceAmount": 150.00,
    "balanceType": "Debit"
  }
  ```
* **Response (Success `200 OK`):**
  ```json
  {
    "message": "تم تعديل بيانات الطالب والرصيد السابق بنجاح"
  }
  ```

---

### **Delete Student**
Deletes a student and cascades deletes all their financial transactions and dues.

* **URL:** `/api/students/{id}`
* **Method:** `DELETE`
* **Response (Success `200 OK`):**
  ```json
  {
    "message": "تم حذف الطالب وحركاته المالية بنجاح"
  }
  ```

---

## 4. Financial Module (`FinancialController`)

### **Assign Dues (Fee Charges)**
Assigns course fee charges to a student (creates a `Fee Charge` transaction).

* **URL:** `/api/financial/dues`
* **Method:** `POST`
* **Headers:** `Content-Type: application/json`
* **Request Body:**
  ```json
  {
    "studentID": 12,
    "courseID": 3,
    "customAmount": 0.00,
    "notes": "رسوم دورة برمجية خاصة",
    "userID": 1
  }
  ```
  *(If `customAmount` is `0.00` or less, the system automatically fetches and charges the exact default course cost from the `Courses` table).*
* **Response (Success `200 OK`):**
  ```json
  {
    "transactionID": 42,
    "amount": 350.00,
    "message": "تم تعيين المستحقات المالية بنجاح"
  }
  ```

---

### **Receive Payment**
Records a payment receipt, registers the transaction, and appends a matching log in `TreasuryLog` under a safe SQL database transaction.

* **URL:** `/api/financial/payments`
* **Method:** `POST`
* **Headers:** `Content-Type: application/json`
* **Request Body:**
  ```json
  {
    "studentID": 12,
    "amount": 200.00,
    "paymentDate": "2025-10-25T14:30:00",
    "notes": "دفعة جزئية من الرسوم",
    "userID": 1
  }
  ```
* **Response (Success `200 OK`):**
  ```json
  {
    "transactionID": 43,
    "message": "تم تسجيل الإيصال المالي وتحديث الخزينة بنجاح"
  }
  ```

---

### **Get Student Account Statement (JSON / CSV Export)**
Retrieves a list of chronological ledger entries for a student, calculates running balances, and aggregates total metrics. Alternatively, exports directly as an Excel-compatible Arabic RTL CSV file with the correct UTF-8 BOM byte sequence.

* **URL:** `/api/financial/statement`
* **Method:** `GET`
* **Query Parameters:**
  * `studentId` (Required): ID of the student.
  * `fromDate` (Optional): Filter start date (`YYYY-MM-DD`). Defaults to the first day of the current month.
  * `toDate` (Optional): Filter end date (`YYYY-MM-DD`). Defaults to current date.
  * `export` (Optional): Pass `csv` to receive a file download instead of JSON.
* **Response (Success JSON `200 OK`):**
  ```json
  {
    "studentID": 12,
    "studentName": "أحمد علي",
    "fromDate": "2025-10-01T00:00:00",
    "toDate": "2025-10-25T23:59:59",
    "totalCharged": 450.00,
    "totalPaid": 200.00,
    "finalBalance": 250.00,
    "transactions": [
      {
        "transactionDate": "2025-10-01T10:00:00",
        "transactionType": "Opening Balance",
        "arabicType": "رصيد سابق",
        "notes": "رصيد افتتاح سابق",
        "debit": 100.00,
        "credit": 0.00,
        "runningBalance": 100.00,
        "handlingEmployee": "admin"
      },
      {
        "transactionDate": "2025-10-24T09:15:00",
        "transactionType": "Fee Charge",
        "arabicType": "رسوم دورة",
        "notes": "تعيين دورة: لغة إنجليزية - مستوى متوسط",
        "debit": 350.00,
        "credit": 0.00,
        "runningBalance": 450.00,
        "handlingEmployee": "admin"
      },
      {
        "transactionDate": "2025-10-25T14:30:00",
        "transactionType": "Payment Receipt",
        "arabicType": "سند قبض",
        "notes": "دفعة جزئية من الرسوم",
        "debit": 0.00,
        "credit": 200.00,
        "runningBalance": 250.00,
        "handlingEmployee": "admin"
      }
    ]
  }
  ```
* **Response (Success CSV Export `200 OK` with `export=csv`):**
  * Downloads a `.csv` file.
  * Stream starts with UTF-8 BOM bytes (`0xEF, 0xBB, 0xBF` / `\uFEFF`) so Microsoft Excel parses the Arabic text (like "رسوم دورة" and "سند قبض") correctly.

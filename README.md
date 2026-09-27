# MediVora API

> A RESTful healthcare management API for authentication, doctors, patients, appointments, doctor schedules, departments, roles, and appointment status management.

**API Version:** `1.0.0`  
**OpenAPI:** `3.1.1`  
**Base URL:** `https://localhost:44342/`

---

## Overview

MediVora API provides backend services for a healthcare/medical appointment platform. The API is organized around clear functional modules:

- Authentication & user identity
- Doctor management
- Doctor schedule templates
- Doctor schedules and slot generation
- Patient management
- Appointment booking and management
- Appointment statuses
- Department management
- Role management

The API definition currently exposes **57 endpoints** across these modules.

> **Source of truth:** This README is generated from the project's OpenAPI definition (`v1.json`). Endpoint names, paths, HTTP methods, request models, and parameters below reflect that definition.

---

## API Information

| Property | Value |
|:----------------------------------|:--------------------------------------------------------------------------|
| **API Name** | **MediVora** |
| **Version** | `1.0.0` |
| **OpenAPI Version** | `3.1.1` |
| **Base URL** | `https://localhost:44342/` |
| **Content Type** | `application/json` unless otherwise specified |
| **File Uploads** | `multipart/form-data` / `application/x-www-form-urlencoded` for doctor profile image endpoints |

---

## Quick Start

### 1. Clone the project

```bash
git clone <your-repository-url>
cd <your-project-folder>
```

### 2. Configure application settings

Copy the example configuration:

```text
appsettings.example.json
```

to:

```text
appsettings.json
```

Then configure your local SQL Server, Redis, JWT, and other environment-specific values.

**Do not commit `appsettings.json` when it contains credentials or secrets.**

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Build

```bash
dotnet build
```

### 5. Run

```bash
dotnet run
```

The OpenAPI definition specifies the development server as:

```text
https://localhost:44342/
```

---

# Authentication

## Login

```http
POST /api/Auth/login
```

Request body:

```json
{
  "email": "user@example.com",
  "password": "your-password"
}
```

## Register

```http
POST /api/Auth/register
```

Request body:

```json
{
  "roleID": 2,
  "firstName": "John",
  "lastName": "Doe",
  "username": "john.doe",
  "password": "your-password",
  "email": "john@example.com",
  "mobileNo": "9876543210"
}
```

## Current User

```http
GET /api/Auth/me
```

The OpenAPI document does not declare a `securitySchemes` section, so authentication/authorization requirements are not formally described in the supplied API specification. If the application uses JWT authorization, document the required `Authorization: Bearer <token>` behavior in the project once that security scheme is added to OpenAPI.

---

# Endpoint Reference

## 1. Authentication

| Method | Endpoint | Purpose |
|---|---|---|
| `POST` | `/api/Auth/login` | Authenticate a user |
| `POST` | `/api/Auth/register` | Register a user |
| `GET` | `/api/Auth/me` | Get current user information |

---

## 2. Departments

### Public/General

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/departments` | Get departments |
| `GET` | `/api/departments/{departmentId}` | Get a department by ID |

### Administration

| Method | Endpoint | Purpose |
|---|---|---|
| `POST` | `/api/admin/departments` | Create a department |
| `PUT` | `/api/admin/departments/{departmentId}` | Update a department |
| `PATCH` | `/api/admin/departments/{departmentId}` | Partially update a department |

### Department Request Model

```json
{
  "departmentCode": "CARD",
  "departmentName": "Cardiology",
  "description": "Cardiology department",
  "isActive": true
}
```

---

# 3. Doctors

## Doctor Search

```http
GET /api/doctors/search
```

Supported query parameters:

| Parameter | Type | Description |
|---|---|---|
| `DepartmentID` | integer | Filter by department |
| `DoctorName` | string | Filter by doctor name |
| `Specialization` | string | Filter by specialization |
| `Qualification` | string | Filter by qualification |
| `MinConsultationFee` | number | Minimum consultation fee |
| `MaxConsultationFee` | number | Maximum consultation fee |

Example:

```http
GET /api/doctors/search?DepartmentID=1&Specialization=Cardiology&MinConsultationFee=500&MaxConsultationFee=1500
```

## Administration

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/admin/doctors` | Get doctors |
| `POST` | `/api/admin/doctors` | Create a doctor |
| `GET` | `/api/admin/doctors/{id}` | Get doctor by ID |
| `PUT` | `/api/admin/doctors/{id}` | Update doctor |
| `PATCH` | `/api/admin/doctors/{id}` | Partially update doctor |

### Create Doctor

```http
POST /api/admin/doctors
```

The OpenAPI definition describes this request as form-based and supports a profile image upload.

Fields include:

```text
FirstName
MiddleName
LastName
Email
MobileNo
UserName
Password
DepartmentID
DoctorCode
Qualification
Specialization
MedicalRegistrationNo
ConsultationFee
IsActive
ProfileImagePath
ProfileImage
```

The `ProfileImage` field is represented as a binary file in the OpenAPI specification.

---

# 4. Doctor Schedule Templates

Schedule templates define recurring availability patterns for doctors.

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/admin/doctors/{doctorId}/schedule-templates` | Get doctor's schedule templates |
| `POST` | `/api/admin/doctors/{doctorId}/schedule-templates` | Create schedule template |
| `GET` | `/api/admin/doctors/{doctorId}/schedule-templates/{id}` | Get template |
| `PUT` | `/api/admin/doctors/{doctorId}/schedule-templates/{id}` | Update template |
| `DELETE` | `/api/admin/doctors/{doctorId}/schedule-templates/{id}` | Delete template |
| `PATCH` | `/api/admin/doctors/{doctorId}/schedule-templates/{id}/status` | Update template status |

### Schedule Template Request

```json
{
  "dayOfWeek": 1,
  "dayOfWeekName": "Monday",
  "startTime": "09:00:00",
  "endTime": "13:00:00",
  "slotDurationInMinutes": 30,
  "maxAppointmentsPerSlot": 2,
  "effectiveFromDate": "2026-09-01T00:00:00",
  "effectiveToDate": null,
  "isActive": true
}
```

---

# 5. Doctor Schedules

Doctor schedules represent actual appointment availability.

## Administration

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/admin/doctors/{doctorId}/schedules` | Get doctor schedules |
| `POST` | `/api/admin/doctors/{doctorId}/schedules` | Create schedule |
| `GET` | `/api/admin/doctors/{doctorId}/schedules/{id}` | Get schedule |
| `PUT` | `/api/admin/doctors/{doctorId}/schedules/{id}` | Update schedule |
| `DELETE` | `/api/admin/doctors/{doctorId}/schedules/{id}` | Delete schedule |
| `PATCH` | `/api/admin/doctors/{doctorId}/schedules/{id}/status` | Update schedule availability/status |
| `POST` | `/api/admin/doctors/{doctorId}/schedules/generate` | Generate schedules |

## Doctor Self-Service

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/doctors/me/schedules` | Get current doctor's schedules |
| `GET` | `/api/doctors/me/schedules/by-date-range` | Get schedules within a date range |

### Get Schedules by Date Range

```http
GET /api/doctors/me/schedules/by-date-range?fromDate=2026-09-01T00:00:00&toDate=2026-09-30T23:59:59
```

### Generate Doctor Schedules

```http
POST /api/admin/doctors/{doctorId}/schedules/generate
```

Request:

```json
{
  "fromDate": "2026-09-01T00:00:00",
  "toDate": "2026-09-30T23:59:59"
}
```

### Schedule Request Model

```json
{
  "scheduleDate": "2026-09-28T00:00:00",
  "startTime": "09:00:00",
  "endTime": "09:30:00",
  "maxAppointments": 2,
  "isAvailable": true,
  "remarks": "Morning consultation slot"
}
```

---

# 6. Appointments

Appointments are available for administration, doctors, and patients.

## Administration

### Create Appointment

```http
POST /api/admin/appointments
```

Request:

```json
{
  "doctorScheduleID": 101,
  "doctorID": 5,
  "patientID": 25,
  "appointmentType": "Consultation",
  "reason": "Regular consultation"
}
```

### Cancel Appointment

```http
PUT /api/admin/appointments/cancel
```

Request:

```json
{
  "appointmentID": 1001,
  "cancellationReason": "Doctor unavailable"
}
```

## Doctor Appointments

### Get Appointments

```http
GET /api/doctors/me/appointments
```

Query parameters:

| Parameter | Type |
|---|---|
| `ViewType` | string |
| `Date` | date-time |
| `AppointmentStatusID` | integer |

Example:

```http
GET /api/doctors/me/appointments?ViewType=Today&Date=2026-09-28T00:00:00&AppointmentStatusID=1
```

### Get Appointment Details

```http
GET /api/doctors/me/appointments/{AppointmentID}
```

## Patient Appointments

### Book Appointment

```http
POST /api/patients/me/appointments
```

Request:

```json
{
  "doctorScheduleID": 101,
  "doctorID": 5,
  "appointmentType": "Consultation",
  "reason": "Headache and general consultation"
}
```

### Get Patient Appointments

```http
GET /api/patients/me/appointments
```

Supported query parameters:

```text
ViewType
Date
AppointmentStatusID
```

### Get Appointment Details

```http
GET /api/patients/me/appointments/{AppointmentID}
```

### Cancel Patient Appointment

```http
PUT /api/patient/me/appointments/cancel
```

Request:

```json
{
  "appointmentID": 1001,
  "cancellationReason": "Unable to attend"
}
```

### Update Appointment Status

```http
PUT /api/appointments/status
```

Request:

```json
{
  "appointmentID": 1001,
  "appointmentStatusID": 2
}
```

---

# 7. Appointment Status

Appointment statuses can be managed through CRUD endpoints.

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/appointmentstatus` | Get appointment statuses |
| `POST` | `/api/appointmentstatus` | Create appointment status |
| `GET` | `/api/appointmentstatus/{AppointmentStatusID}` | Get status by ID |
| `PUT` | `/api/appointmentstatus/{AppointmentStatusID}` | Update status |
| `DELETE` | `/api/appointmentstatus/{AppointmentStatusID}` | Delete status |
| `GET` | `/api/appointmentstatus/dropdown` | Get status dropdown data |

### Request

```json
{
  "statusCode": "CONFIRMED",
  "statusName": "Confirmed",
  "isActive": true
}
```

---

# 8. Patients

## Administration

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/admin/patients` | Get patients |
| `POST` | `/api/admin/patients` | Create patient |
| `GET` | `/api/admin/patients/{id}` | Get patient |
| `PUT` | `/api/admin/patients/{id}` | Update patient |
| `PATCH` | `/api/admin/patients/{id}/status` | Update patient status |

### Patient Request

```json
{
  "roleID": 3,
  "userName": "john.doe",
  "password": "your-password",
  "firstName": "John",
  "middleName": "",
  "lastName": "Doe",
  "email": "john@example.com"
}
```

## Patient Self-Service

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/patients/me` | Get current patient |
| `PUT` | `/api/patient/profile` | Update patient profile |

### Patient Profile

The profile model supports:

```text
firstName
middleName
lastName
email
mobileNo
dateOfBirth
gender
bloodGroup
address
city
state
postalCode
emergencyContactName
emergencyContactNumber
```

---

# 9. Roles

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/Role` | Get roles |
| `POST` | `/api/Role` | Create role |
| `GET` | `/api/Role/{roleId}` | Get role |
| `PUT` | `/api/Role/{roleId}` | Update role |
| `DELETE` | `/api/Role/{roleId}` | Delete role |

### Role Request

```json
{
  "roleCode": "DOCTOR",
  "roleName": "Doctor",
  "roleDescription": "Healthcare professional",
  "isActive": true
}
```

---

# 10. Response Handling

The supplied OpenAPI document declares `200 OK` responses for the documented operations. Detailed response schemas are not consistently defined for the endpoints.

For production documentation, consider expanding the OpenAPI specification with explicit response models for:

- Success responses
- Validation errors
- Unauthorized responses (`401`)
- Forbidden responses (`403`)
- Not found responses (`404`)
- Conflict responses (`409`)
- Rate-limit responses (`429`)
- Server errors (`500`)

This will make the API easier for frontend and third-party developers to integrate.

---

# Request Examples

## cURL

### Login

```bash
curl -X POST "https://localhost:44342/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "your-password"
  }'
```

### Search Doctors

```bash
curl -X GET "https://localhost:44342/api/doctors/search?Specialization=Cardiology&MinConsultationFee=500&MaxConsultationFee=1500"
```

### Book Appointment

```bash
curl -X POST "https://localhost:44342/api/patients/me/appointments" \
  -H "Content-Type: application/json" \
  -d '{
    "doctorScheduleID": 101,
    "doctorID": 5,
    "appointmentType": "Consultation",
    "reason": "General consultation"
  }'
```

---

# HTTP Methods

MediVora follows conventional HTTP semantics:

| Method | Typical Usage |
|---|---|
| `GET` | Retrieve resources |
| `POST` | Create resources or execute generation/actions |
| `PUT` | Replace/update a resource |
| `PATCH` | Partially update a resource |
| `DELETE` | Delete a resource |

---

# API Design Highlights

### Modular endpoint organization

The API separates major healthcare domains into focused modules:

```text
Auth
├── Login
├── Register
└── Current User

Doctors
├── Search
├── Management
├── Schedule Templates
└── Schedules

Patients
├── Management
├── Profile
└── Appointments

Appointments
├── Booking
├── Cancellation
├── Status
└── Doctor/Patient Views
```

### Schedule Template → Schedule Generation

The API supports recurring doctor availability through schedule templates and provides a dedicated schedule-generation endpoint.

```text
Schedule Template
       ↓
Effective Date Range
       ↓
Generate Schedules
       ↓
Available Appointment Slots
       ↓
Patient Books Appointment
```

---

# Data Models

The OpenAPI definition includes models for:

- `LoginDTO`
- `RegisterDTO`
- `PatientAddDTO`
- `PatientEditDTO`
- `PatientAppointmentAddDTO`
- `AdminAppointmentAddDTO`
- `AppointmentCancelDTO`
- `AppointmentStatusAddEditDTO`
- `AppointmentStatusUpdateDTO`
- `DepartmentAddEditDTO`
- `DoctorScheduleAddEditDTO`
- `DoctorScheduleTemplateAddEditDTO`
- `GenerateDoctorScheduleDTO`
- `RoleAddEditDTO`
- `IFormFile`
- `WeatherForecast`

---

# Development Notes

## Local Development

The current OpenAPI server is configured as:

```text
https://localhost:44342/
```

If your local HTTPS certificate is not trusted, trust the ASP.NET Core development certificate:

```bash
dotnet dev-certs https --trust
```

## Configuration

Keep sensitive values outside source control.

Recommended approach:

```text
appsettings.example.json  → Git
appsettings.json          → Local only
```

For local secrets, ASP.NET Core User Secrets can also be used.

---

# Recommended Project Workflow

```text
1. Register / Login
        ↓
2. Obtain authenticated user context
        ↓
3. Manage departments / doctors
        ↓
4. Configure doctor schedule templates
        ↓
5. Generate doctor schedules
        ↓
6. Patients search doctors
        ↓
7. Patients view available schedules
        ↓
8. Patient books appointment
        ↓
9. Doctor manages appointment
        ↓
10. Appointment status is updated
```

---

# API Documentation

The project exposes an OpenAPI document containing the complete endpoint and schema definitions used to generate this reference.

For interactive API testing, use the project's configured OpenAPI/Swagger/Scalar UI if enabled by the application.

---

# Contributing

Contributions are welcome.

Recommended workflow:

```bash
git checkout -b feature/your-feature
git add .
git commit -m "Add: your feature"
git push origin feature/your-feature
```

Then create a pull request with:

- A clear description
- API endpoint changes
- Request/response changes
- Database changes, if applicable
- Testing information

---

# Security

Never commit:

- Database passwords
- JWT signing keys
- API keys
- Redis credentials
- Third-party service secrets
- Production connection strings

Use environment variables, User Secrets, Azure Key Vault, or another secure secret-management solution for production credentials.

---

# License

Add your project's license information here.

---

## MediVora

**Healthcare API • Doctor Management • Patient Management • Appointment Scheduling**

Built with modern REST API principles and designed to support scalable healthcare application workflows.

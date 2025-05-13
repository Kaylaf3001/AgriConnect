# 🌾 FarmConnect System - README

## 📄 Overview

**FarmConnect** is a role-based agricultural product management system. It provides secure, role-specific features for **Farmers** and **Employees** to simplify product listing, farmer management, and product filtering.

## 🛠️ 1. Setting Up the Project
### ✅ Prerequisites

Ensure the following tools are installed:

- [.NET 8.0 SDK or later](https://dotnet.microsoft.com/download)
- Visual Studio 2022 (or Visual Studio Code with C# extension)
- Git (optional, for cloning from GitHub)

### 📂 Open the Project

1. **Download the ZIP** or **Clone the Repo**  
   If using Git:
   ```bash
   git clone https://github.com/yourusername/FarmConnect.git

2. Open the .sln file in Visual Studio.

## ▶️ 2. Running the Application
### 💻 In Visual Studio
- Press F5 or click the Start button
- The application will launch in your default browser at https://localhost:xxxx

## 👥 3. User Roles and Functionalities
### 👩‍🌾 Farmer Role
- 🔐 Log in securely
- ➕ Add new products (name, category, production date)
- 📄 View a list of their own products

### 👨‍💼 Employee Role (Admin)
Employees can: 
- 🔐 Log in securely
- 👤 Add new farmer profiles (basic details)
- 📦 View all products from any farmer
- 🔍 Filter products by:
   - Date range
   - Product type
   - Farmer Name
 
## 🔐 4. Security & Authentication
- Secure login system using ASP.NET Identity
- Role-based access to ensure feature restrictions
- Only registered and authenticated users can access the system

## 🗃️ 5. Technologies Used
- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core (Code-First + Migrations)
- SQLite (for local database)
- ASP.NET Identity (for authentication and role management)

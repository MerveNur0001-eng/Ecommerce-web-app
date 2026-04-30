---

# 🛒 Ecommerce Web Application (ASP.NET Core)

A full-featured **layered architecture e-commerce web application** built with ASP.NET Core MVC.
This project includes both **admin panel and user frontend**, simulating a real-world e-commerce platform.

---

## 🚀 Project Overview

This system provides a complete online shopping experience:

* Product management (CRUD operations)
* Category & brand management
* User authentication & authorization
* Shopping cart system
* Favorites (wishlist)
* Order management
* Payment integration (iyzico virtual POS)
* Email notifications
* Password reset via email
* Slider management
* Multi-image product support

---

## 🏗️ Architecture

The project follows a **Layered Architecture**:

* 📦 **Ecommerce.Core** → Entities & Interfaces
* 📦 **Ecommerce.Data** → Data Access Layer (EF Core, Repository Pattern)
* 📦 **Ecommerce.Service** → Business Logic Layer
* 📦 **Ecommerce.WebUI** → Presentation Layer (MVC)

---

## 🔐 Security Features

* User authentication system
* Role-based authorization (Admin / User)
* Password hashing
* Forgot password via email
* Secure configuration via `appsettings`

---

## 💳 Payment Integration

* iyzico Virtual POS integration
* Secure payment flow
* Order creation after successful payment

---

## ✉️ Email System

* SMTP email service
* Password reset emails
* Order confirmation emails

---

## 🛍️ Features

### 🧑‍💼 Admin Panel

* Product management (add, update, delete)
* Category & brand management
* Order tracking system
* Slider management
* User management

### 🛒 User Side

* Product browsing
* Product detail page (multi-image support)
* Add to cart system
* Wishlist (favorites)
* Checkout system
* Order history
* Secure login & register

---

## 📚 Key Technologies

* ASP.NET Core MVC
* Entity Framework Core
* Repository Pattern (Generic Repository)
* Dependency Injection
* LINQ
* Session & Cookie Management
* SMTP Email Service
* REST-based architecture principles

---

## 🎓 Project Modules Covered

* Application architecture setup
* Data layer design
* Admin panel development
* Frontend development
* Authentication & authorization
* Favorites & account system
* Generic repository pattern
* Cart & order system
* Payment integration (iyzico)
* Code review & optimization

---

## 🖼️ Features Highlights

* Multiple images per product
* Category & brand system
* Wishlist functionality
* Shopping cart system
* Order tracking
* Email notifications
* Forgot password system
* Payment gateway integration

---

## ⚙️ Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/MerveNur0001-eng/Ecommerce-web-app.git
```

---

### 2. Open the solution

Open in Visual Studio:

```
Ecommerce.WebUI.sln
```

---

### 3. Configure appsettings

Rename:

```
appsettings.Example.json → appsettings.json
```

Then fill in your own values:

```json id="appcfg"
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_DATABASE_CONNECTION_STRING"
  },
  "EmailSettings": {
    "Email": "your-email",
    "Password": "your-email-password"
  },
  "ApiKeys": {
    "Iyzico": "your-iyzico-key"
  }
}
```

---

### 4. Database Setup

Run migrations:

```bash
dotnet ef database update
```

If needed:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### 5. Run the project

```bash
dotnet run
```

or run via Visual Studio (IIS Express / Kestrel)

---

### 6. Default Routes

* Admin Panel → `/Admin`
* User Interface → `/`

---

## ⚠️ Important Notes

- This project follows clean layered architecture principles.
- Sensitive information is excluded using `.gitignore`.
- To run the project, rename `appsettings.Example.json` to `appsettings.json` and configure your own credentials.
- 
---

## 👩‍💻 Author

Developed by **Merve**

---

## ⭐ Support

If you like this project, give it a ⭐ on GitHub!

```


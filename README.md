# 📄 DVLD Management System

The **DVLD Management System** is a Windows Forms application built using a 3-tier architecture to manage users, drivers, and driving license applications. Designed for maintainability and scalability, the system incorporates several advanced features to ensure a dynamic and secure user experience.

---

## 🔧 Key Features

- **3-Tier Architecture**: Clear separation between UI, business logic, and data access layers.
- **Event-Driven Workflows**: Provides a responsive, dynamic user interface based on real-time interactions.
- **T-SQL Triggers**: Automatically manages sensitive operations like password security at the database level.
- **Persistent Login**: Uses Windows Registry to securely retain user sessions between application runs.
- **User & Driver Management**: Full CRUD support for handling people, users, drivers, and license applications.

This project demonstrates the use of clean architectural patterns and real-world administrative logic, making it a solid foundation for more advanced desktop systems.

---

## 🚀 Getting Started

### Prerequisites

- [.NET Framework 4.8+](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio](https://visualstudio.microsoft.com/) (recommended)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/Rayan-q//Drivers-Vehicles-Licenses-Departemnt
2. Set up the database:

   Open SQL Server Management Studio.
   Execute the .sql scripts found in the Database folder to create tables and triggers.

3. Configure the connection string:

   Open App.config.

   Update the connection string to match your SQL Server instance.

4. Build and run the project:

   Open the solution in Visual Studio.

   Build the project and run it (F5).


📌 **Notes**
Run Visual Studio as Administrator to allow access to the Windows Registry.

Login credentials and session persistence are handled securely using hashing and Registry integration.

📫 **Contact**
For questions or suggestions, feel free to open an issue or contact me at rayan.wenters.com.

# AdFlow Project

AdFlow is a web application developed for managing digital marketing campaigns, with a focus on workflow organization, priority management, and performance tracking.

The system was designed to help teams and professionals maintain control over campaigns, improve productivity, and support data-driven decision-making.

---

## Objective

Provide a simple and efficient platform for managing digital marketing campaigns, enabling users to oversee the entire campaign lifecycle, from creation to performance analysis.

---

## Technologies Used

* ASP.NET Core (MVC)
* Entity Framework Core
* SQL Server
* ASP.NET Identity (Authentication and Authorization)
* Bootstrap (User Interface)
* C#

---

## Features

* Campaign management
* Priority assignment by workflow managers
* Role-based user management system
* Performance tracking
* Workflow organization
* Authentication and authorization system (Identity)

---

## Architecture

The project follows an architecture based on:

* Layered separation (Model, View, Controller)
* Entity Framework for data persistence
* ASP.NET Identity for user and role management

---

## User Management

The system uses ASP.NET Identity with:

* `UserManager` for user management
* `RoleManager` for permission and role management
* Custom `ApplicationUser` class

---

## Business Rules

* Campaign priority is defined exclusively by workflow managers
* Newly created campaigns remain pending until a priority is assigned
* The system enforces access control based on user roles

---

## Running the Project

1. Clone the repository:

```bash
git clone https://github.com/your-username/adflow.git
```

2. Configure the database connection in `appsettings.json`

3. Run the migrations:

```bash
update-database
```

4. Start the application:

```bash
dotnet run
```

---

## Author

Rodrigo Fontes

Software Developer | Digital Marketing Specialist

---

## License

This project was developed for educational purposes.

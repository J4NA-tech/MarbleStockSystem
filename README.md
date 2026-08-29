# Marble Stock Management System

A desktop-based marble inventory and sales management application developed as a course examination project using C# and .NET.

The application was designed to manage marble products, customers, inventory operations, and sales through a structured layered architecture.

## Features

* Marble inventory management
* Customer management
* Sales management
* Add, update, delete, and list operations
* Database integration
* Layered application architecture
* Separation of business logic, data access, and presentation layers

## Architecture

The project follows a layered architecture consisting of three main layers:

### MarbleStockSystem.PL

**Presentation Layer**

Contains the Windows Forms user interface and application entry point.

### MarbleStockSystem.BLL

**Business Logic Layer**

Contains business rules and service classes responsible for managing application operations.

### MarbleStockSystem.DAL

**Data Access Layer**

Handles database communication, entities, repositories, and Entity Framework configuration.

## Technologies

* C#
* .NET
* Windows Forms
* Entity Framework
* SQL Server
* Git & GitHub

## Development Approach

This project was developed as part of a course examination project using an **AI-assisted development approach (vibe coding)**.

AI tools were used during the development process to assist with code generation, debugging, implementation ideas, and problem solving. The project structure, functionality, integration, testing, and final implementation were reviewed and adapted throughout the development process.

## Database

The application uses SQL Server for storing and managing:

* Marble information
* Customer information
* Sales records

## Project Structure

```text
MarbleStockSystem
│
├── MarbleStockSystem.BLL
│   ├── Interfaces
│   └── Services
│
├── MarbleStockSystem.DAL
│   ├── Data
│   ├── Entities
│   └── Repositories
│
├── MarbleStockSystem.PL
│   ├── Forms
│   └── Program.cs
│
├── MarbleStockSystem.sln
└── README.md
```

## Project Purpose

This project was developed as an examination/course project to demonstrate practical knowledge of:

* Object-Oriented Programming
* Layered architecture
* Database management
* Entity Framework
* Repository pattern
* Business logic separation
* Windows Forms application development
* AI-assisted software development

## Author

**Jana El Samra**

Computer Engineering

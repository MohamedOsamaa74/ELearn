# ELearn

<!-- ![ELearn Logo](https://github.com/user-attachments/assets/cd7a4c5b-cf04-452d-880e-2f8f5ab1379e)-->

![Untitled-4-Bi-XWAN7](https://github.com/user-attachments/assets/07ebbd01-14d0-4180-a9ca-fd9bec170e8a)


## Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Contact](#contact)
- [Contributers](#Contributers)

## About the Project

ELearn is a comprehensive educational platform that can be devided to 3 subsystems:
- Learning Manegment System
- Quizzing Systme
- Communication System: Similar to a social media platform that contains both Community Forum and Chatting between system users
This project is developed as a graduation project for the Computer Science department at South Valley University.

## Features

- User Authentication and Authorization using ASP.NET Identity
- Course Management
  - Creation, updating, and deletion of courses
  - Enrollment of users in courses
- Quiz and Assignment Modules
  - Creation, updating, and deletion of quizzes and assignments
  - Submission and grading of assignments
- Real-time Notifications with SignalR
- Social Media-like Features
  - Posting announcements
  - Commenting on posts
- File Upload and Management
- Role-based Access Control

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the repository:**

   ```sh
   git clone https://github.com/MohamedOsamaa74/ELearn.git
   cd ELearn


2. **Setup the backend:**

  - Navigate to the backend project directory:
  
    ```sh
    cd ELearn.Api
    ```
  - Restore the NuGet packages:
  
    ```sh
    dotnet restore
    ```
  - Update the database:
  
    ```sh
    dotnet ef database update
    ```
  
### Running the Application
- Start the backend:

  ```sh
  dotnet run
  ```
## Usage
Once the application is running, you can interact with the API using tools like Postman, swagger or through a frontend client. The API endpoints allow you to manage users, courses, assignments, quizzes, and more.

## Project Structure
  ```plaintext
  ELearn/
  │
  ├── ELearn.Api/           # ASP.NET Core Web API project, Handles HTTP requests and responses.
  ├── ELearn.Application/   # Application layer, Contains the buisiness logic and helpers classes
  ├── ELearn.Domain/        # Domain entities
  ├── ELearn.InfraStructure/# Infrastructure and data access
  └── README.md             # Project documentation
  ```

## Technologies Used
Backend:
- **ASP.NET Core**
- **Entity Framework Core**
- **SQL Server**
- **SignalR**
- **ASP.NET Identity**
- **JWT Authentication**
- **Gmail SMTP**
- **CSV Helper**
- **FluentValidation**
- **AutoMapper**

## Contact
Mohamed Osama - moa.mohamedosama@gmail.com

## Contributers
This project exists thanks to all the people who contribute. A special thanks to our BackEnd Developers:
- *Mohamed Osama*
  - *GitHub:* [MohamedOsamaa74](https://github.com/MohamedOsamaa74)
  - *LinkedIn:* [Mohamed Osama](https://www.linkedin.com/in/mohamed-osama-306573200/)

- *Aya Abdelkhalk*
  - *GitHub:* [AyaAbdelkhalk](https://github.com/AyaAbdelkhalk)
  - *LinkedIn:* [Aya Abdelkhalk](https://www.linkedin.com/in/aya-abdelkhalk-0026aa21b/)

- *Doha Ezzat*
  - *GitHub:* [DohaEzzat](https://github.com/DohaEzzat)
  - *LinkedIn:* [Doha Ezzat](https://www.linkedin.com/in/doha-ezzat-373718251/)

- *Marwan Wannan*
  - *GitHub:* [Wannan24](https://github.com/wannan24)
  - *LinkedIn:* [Marwan Wannan](https://www.linkedin.com/in/marwan-wannan-11275a230/)


Project Link: https://github.com/MohamedOsamaa74/ELearn

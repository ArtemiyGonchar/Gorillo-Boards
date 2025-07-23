# Gorillo-Boards
Kanban boards

## 🚀 About Gorillo-Boards

Gorillo-Boards is a trello-like project. Gorrilo-Boards designed for easy work management for small teams. It has all needed functionality for comfort working.

## 🧠 Features
- 🔐 Authentication and user management
- 📋 Boards with customizable states and tickets, sub-statuses
- 👾 Role based boards
- 📎 Ticket archiving via Azure Blob storage
- ☁️ Deployed as microservices in Azure
- 💬 Communication via Azure Service Bus
- 📈 Charts
  
## 🏗️ Architecture
- 4 microservices (Identity, Boards, Workflow, Charts)
- Azure Service Bus for async communication
- Blob Storage for files
- Timer Trigger for archiving tickets
- Follows **three-layer architecture**: Controllers, Services, Repositories

## ⚙️ Technologies

- ASP.NET Core
- Azure (Service Bus, Blob Storage, Functions)
- Entity Framework Core
- C#
- SQL Server
- Swagger for API testing


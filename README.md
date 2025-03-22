# RabbitMQWeb.ExcelCreate Project

## Description
RabbitMQWeb.ExcelCreate is an ASP.NET Core 8 MVC project that allows users to create Excel files asynchronously using RabbitMQ. The project consists of two main components:

1. **RabbitMQWeb.ExcelCreate (MVC Project)** - The main web application where users can upload files and request Excel file creation.
2. **FileCreateWorkerService (Worker Service)** - A background worker service that listens to RabbitMQ messages, processes the Excel file creation, and sends the result back.

To keep users updated in real-time, **SignalR** is used for notifications, and **SweetAlert** is used for user-friendly alerts.

## Features
- Upload files via the **FilesController** (API Controller)
- Create Excel files asynchronously using **RabbitMQ**
- Process Excel file creation in **FileCreateWorkerService**
- Notify users in real-time with **SignalR**
- Display alerts using **SweetAlert**
- Secure and structured message handling via **RabbitMQ**

## Project Structure
### RabbitMQWeb.ExcelCreate (MVC Project)
- **Controllers**
  - `FilesController` (API Controller) → Handles file uploads and interacts with the worker service.
  - `ProductController` → Sends messages to RabbitMQ and retrieves processed files.
  - `HomeController`, `AccountController` (MVC Controllers)
- **Services**
  - `RabbitMqClientService` → Manages RabbitMQ connection.
  - `RabbitMQPublisher` → Publishes messages to RabbitMQ.
  - `CreateExcelMessage` → Handles Excel file creation messages.
- **Hubs**
  - `MyHub.cs` → SignalR hub for real-time notifications.
- **wwwroot/files/** → Stores the generated Excel files.

### FileCreateWorkerService (Worker Service)
- **Worker.cs** → Background worker that listens to RabbitMQ.
- **Services**
  - `RabbitMqClientService` → Manages RabbitMQ connection.
  - `CreateExcelMessage` → Processes messages and creates Excel files.
- **Models**
  - `Product.cs` → Represents product data.
- **Registrations**
  - `RabbitMqRegistration.cs` → Configures RabbitMQ.
- **appsettings.json** → Configuration settings.

## How It Works
1. The user requests an Excel file creation via `ProductController`.
2. A message is sent to RabbitMQ.
3. `FileCreateWorkerService` listens to RabbitMQ and processes the message.
4. The Excel file is created and stored in `wwwroot/files/`.
5. `FileCreateWorkerService` sends the result back to `FilesController`.
6. The user receives a notification via **SignalR** and a **SweetAlert** popup.

## Installation & Setup
1. Clone the repository.
   ```sh
   git clone https://github.com/ZiyaMammadli/RabbitMQWeb.ExcelCreate.git
   ```
2. Navigate to the project folder.
   ```sh
   cd RabbitMQWeb.ExcelCreate
   ```
3. Configure `appsettings.json` for RabbitMQ connection.
4. Run the **RabbitMQWeb.ExcelCreate** project.
5. Start the **FileCreateWorkerService** to listen for messages.
6. Open the web application and start using the file upload feature!

## Technologies Used
- **ASP.NET Core 8 MVC**
- **RabbitMQ** (Message Queue)
- **SignalR** (Real-time communication)
- **SweetAlert** (User-friendly notifications)
- **Worker Services** (Background processing)
- **Entity Framework Core** (Database operations)

## Contact

For questions or issues, please reach out to:

- Email: [ziyam040@gmail.com](mailto:ziyam040@gmail.com)
- GitHub: [Profile](https://github.com/ZiyaMammadli)

## License
This project is open-source and free to use under the MIT license.




# Microservices Project

 
Welcome to the Microservices Project! This project is aimed at showcasing the implementation of microservices architecture using .NET.

![Project Logo](https://dotnetresturant.blob.core.windows.net/manger/Screenshot%202024-02-26%20142703.png)

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Technologies Used](#technologies-used)
4. [Getting Started](#getting-started)
5. [Contributing](#contributing)

## Introduction

The project consists of seven microservices, each responsible for a specific domain or functionality. These microservices are designed to be loosely coupled and independently deployable.
## Features

Asynchronous and synchronous communication between microservices
Custom payment handling using message brokers
API routing and aggregation with API Gateway
Secure identity management with OAuth3 and OpenID
File storage using Azure Blob Storage

## Technologies Used

🛠 7 Microservices: Developed and deployed 7 microservices, each with its own SQL Server database, ensuring modularity and scalability.

🔒 Robust Security: Integrated OAuth3 and OpenID with the Duendo Identity Server for top-notch security and identity management.

💾 Efficient File Storage: Leveraged Azure Blob Storage to efficiently handle our file storage requirements, ensuring reliability and scalability.

🔄 Flexible Communication: Implemented both asynchronous and synchronous communication methods between microservices for enhanced flexibility and performance.

💳 Custom Payment Handling: Experimented with a custom message broker setup, utilizing Azure Service Bus and RabbitMQ for efficient payment handling.

🌐 API Gateway: Implemented an API Gateway using Ocelot for streamlined API routing and aggregation, enhancing overall system efficiency.

🏗 Scalable Architecture: Followed an N-Layer implementation pattern with the Repository Pattern to ensure a scalable and maintainable architecture.

💻 Programming Language: C# with .NET 8

## Getting Started

Include instructions on how to get started with your project. This may include:

To get started with this project, follow these steps:

Clone the repository to your local machine.
Set up the necessary environment variables for database connections, identity server configurations, and Azure services.
Run each microservice individually or deploy them to your preferred hosting platform.
Explore the source code to understand the architecture and implementation details.

## Contributing

Contributions are welcome! If you have any suggestions, bug fixes, or enhancements, feel free to open an issue or submit a pull request.


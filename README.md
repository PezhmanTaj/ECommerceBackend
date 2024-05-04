# E-Commerce Backend

This repository contains the backend code for a multi-seller e-commerce platform. The backend is structured into three layers: Data Access Layer (DAL), Business Logic Layer (BLL), and the API layer. It is designed to support multiple sellers with a robust, scalable architecture.

## Current Project Status
This project is in its early stages, with about 30% of the planned functionality currently implemented. Development is ongoing, and new features are being added regularly.

## Technologies Used
- **MongoDB**: Used for database operations, leveraging its flexibility and performance for handling large volumes of data.
- **JWT Authentication**: Secures the API using JSON Web Tokens.
- **AutoMapper**: Facilitates object-object mapping in .NET.
- **FluentValidation**: Used for robust validation mechanisms in the business logic layer.
- **NUnit**: Ensures quality and reliability through extensive unit tests.
- **AspNetCoreRateLimit**: Implements rate limiting to protect the API from abuse and manage traffic effectively.

## Architecture
The backend is divided into the following main components:
- **DAL**: Handles all interactions with the MongoDB database.
- **BLL**: Contains business rules and logic, data transformations, and service layer definitions.
- **API**: Serves as the interface between the frontend and the backend, handling client requests and responses.


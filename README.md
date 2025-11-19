# 🛒 Ecommece API

## 📘 Overview
Ecommece API is a modular, high-performance e-commerce backend built with **.NET 8**, featuring:
- Clean Architecture
- RabbitMQ for asynchronous order/payment events
- Redis caching for performance optimization
- Strategy & Factory patterns for flexible payment and shipping handling
- JWT-based authentication and authorization

---

## ⚙️ Tech Stack

| Layer | Technology |
|-------|-------------|
| **Backend Framework** | ASP.NET Core 8 |
| **Database** | SQL Server (EF Core ORM) |
| **Caching** | Redis |
| **Message Broker** | RabbitMQ |
| **Authentication** | JWT |
| **Patterns Used** | Strategy, Factory, Repository, Unit of Work |
| **Logging** | Serilog (planned) |

---

## 🏗️ Project Structure

```
Ecommece/
│
├── Ecommece.API/                # Presentation Layer (Controllers, Middleware, DI)
│   ├── Controllers/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
│
├── Ecommece.Core/               # Domain Layer (Entities, Interfaces, Business Logic)
│   ├── Models/
│   ├── Interfaces/
│   ├── Payments/                # Strategy Pattern for payments
│   ├── Shipping/                # Factory Pattern for shipping methods
│   └── Enums/
│
├── Ecommece.EF/                 # Infrastructure Layer (Data Access, Repositories, Services)
│   ├── Data/                    # DbContext
│   ├── Services/                # Implementations for Repos, Orders, etc.
│   └── Migrations/

```

---

## 💡 Key Features

### 🧩 1. Product Management
- Add, update, and retrieve products
- Cached with Redis for faster response

### 📦 2. Orders
- Orders are created with shipping & payment strategies
- Event published to RabbitMQ for async payment processing
- Redis caching for frequent order lookups

### 💰 3. Payment Strategy Pattern
Each payment type implements its own logic via a strategy:
```csharp
public interface IPaymentStrategy {
    Task<bool> ProcessPaymentAsync(decimal amount);
}
```

### 🚚 4. Shipping Factory Pattern
Shipping costs and logic are dynamically resolved:
```csharp
var shippingStrategy = ShippingFactory.GetShippingStrategy("DHL");
var cost = shippingStrategy.CalculateShippingCost(order.Subtotal);
```

### 🔐 5. JWT Authentication
- Secure endpoints using JWT tokens
- Token generated on login/registration
- Middleware validates token and roles

### 📨 6. Message Broker Integration
- RabbitMQ producer publishes events (e.g., `OrderCreated`)
- Consumer service (in progress) listens and updates order status

### ⚡ 7. Redis Caching
- Products and Orders cached for performance
- Cache invalidation on updates

---

## 🛠️ Setup Instructions

### 1️⃣ Prerequisites
Ensure you have installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Redis](https://redis.io/download)
- [RabbitMQ](https://www.rabbitmq.com/download.html)

### 2️⃣ Configure `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EcommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "super_secure_key_1234567890",
    "Issuer": "EcommerceAPI"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

### 3️⃣ Run Database Migrations
```bash
cd Ecommece.API
dotnet ef database update
```

### 4️⃣ Run the API
```bash
dotnet run
```

API will start on:  
➡️ `https://localhost:5001`  
➡️ `http://localhost:5000`

---

## 🧠 Future Improvements
- ✅ Implement background RabbitMQ consumer
- ✅ Add logging with Serilog
- ✅ Add health checks & Prometheus metrics
- ✅ Add integration tests

---

## 👨‍💻 Authors
**Eman Ahmed**  
Full-stack Developer (.NET | Angular | Dynamics 365)  

---

## 📄 License
This project is licensed under the MIT License.

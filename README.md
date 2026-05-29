# FIFA World Cup 2026 - Tracking Application

⚽ A comprehensive web application for tracking and managing the FIFA World Cup 2026 tournament.

## 🎯 Project Overview

This is a full-stack web application built to track the 2026 FIFA World Cup, featuring team management, match scheduling, live results, group standings, and knockout bracket visualization. The project demonstrates modern software development practices with clean architecture, RESTful API design, and responsive UI.

## 🏗️ Architecture

- **Backend:** .NET 8 Web API with Clean Architecture
- **Frontend:** React 18 with TypeScript and Material-UI
- **Database:** PostgreSQL
- **Containerization:** Docker & Docker Compose

## 📚 Documentation

Comprehensive planning documentation is available in the [`.workbench/`](.workbench/) directory:

- **[Technology Stack](.workbench/01-technology-stack.md)** - Selected technologies and justifications
- **[Architecture](.workbench/02-architecture.md)** - System architecture and design patterns
- **[Folder Structure](.workbench/03-folder-structure.md)** - Complete project organization
- **[Database Model](.workbench/04-database-model.md)** - Entity relationships and schema
- **[API Endpoints](.workbench/05-api-endpoints.md)** - Complete REST API specification
- **[Implementation Plan](.workbench/06-implementation-plan.md)** - Step-by-step development roadmap
- **[Business Rules](.workbench/07-business-rules.md)** - Validation and business logic rules

## ✨ Key Features

### Core Functionality
- ✅ **Team Management** - CRUD operations for national teams
- ✅ **Group Stage** - 12 groups with 4 teams each
- ✅ **Match Scheduling** - Complete tournament calendar
- ✅ **Live Results** - Real-time score updates
- ✅ **Standings** - Automatic calculation and ranking
- ✅ **Knockout Bracket** - Interactive tournament bracket
- ✅ **Dashboard** - Tournament statistics and highlights
- ✅ **Stadium Information** - Venue details and locations

### Business Logic
- Automatic points calculation (Win: 3, Draw: 1, Loss: 0)
- Goal difference and standings sorting
- Penalty shootout support for knockout matches
- Winner advancement through bracket
- Data validation and integrity checks

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- PostgreSQL 15+
- Docker Desktop (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WorldCup2026
   ```

2. **Backend Setup**
   ```bash
   cd backend
   dotnet restore
   dotnet ef database update
   dotnet run --project src/WorldCup2026.API
   ```

3. **Frontend Setup**
   ```bash
   cd frontend
   npm install
   npm run dev
   ```

4. **Docker Setup** (Alternative)
   ```bash
   docker-compose up -d
   ```

## 📊 Tournament Structure

- **48 Teams** divided into **12 Groups** (A-L)
- **Group Stage:** 72 matches (3 per team)
- **Knockout Stage:** 32 matches (Round of 32 → Final)
- **Total Matches:** 104
- **Duration:** June 11 - July 19, 2026
- **Host Countries:** USA 🇺🇸, Mexico 🇲🇽, Canada 🇨🇦

## 🛠️ Technology Stack

### Backend
- **.NET 8** - Modern, high-performance framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core** - ORM for database access
- **PostgreSQL** - Relational database
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation

### Frontend
- **React 18** - UI library
- **TypeScript** - Type-safe JavaScript
- **Vite** - Fast build tool
- **Material-UI (MUI)** - Component library
- **React Router v6** - Client-side routing
- **React Query** - Data fetching and caching
- **Axios** - HTTP client
- **Formik + Yup** - Form handling and validation

### DevOps
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **xUnit** - Backend testing
- **Vitest** - Frontend testing

## 📁 Project Structure

```
WorldCup2026/
├── .workbench/              # Planning documentation
├── backend/                 # .NET 8 Web API
│   ├── src/
│   │   ├── WorldCup2026.API/
│   │   ├── WorldCup2026.Application/
│   │   ├── WorldCup2026.Domain/
│   │   └── WorldCup2026.Infrastructure/
│   └── tests/
├── frontend/                # React application
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   └── hooks/
│   └── public/
├── docker/                  # Docker configuration
├── docker-compose.yml
└── README.md
```

## 🧪 Testing

### Backend Tests
```bash
cd backend
dotnet test
```

### Frontend Tests
```bash
cd frontend
npm test
```

## 🐳 Docker Deployment

Build and run all services:
```bash
docker-compose up -d
```

Access the application:
- **Frontend:** http://localhost:3000
- **Backend API:** http://localhost:5000
- **Swagger UI:** http://localhost:5000/swagger

## 📖 API Documentation

Once the backend is running, access the interactive API documentation:
- **Swagger UI:** http://localhost:5000/swagger
- **OpenAPI Spec:** http://localhost:5000/swagger/v1/swagger.json

## 🎓 Learning Outcomes

This project demonstrates:
- Clean Architecture principles
- Repository and Service patterns
- RESTful API design
- Entity Framework Core
- React with TypeScript
- Material-UI components
- Docker containerization
- Automated testing
- Comprehensive documentation

## 🔮 Future Enhancements

- User authentication and authorization
- Match predictions and leaderboards
- Player statistics and analytics
- Real-time updates with SignalR
- Mobile application
- Multi-language support
- Social features (comments, sharing)
- News and articles integration

## 📝 Development Status

- ✅ **Planning Phase:** Complete
- ⏳ **Implementation Phase:** Ready to start
- ⏳ **Testing Phase:** Pending
- ⏳ **Deployment Phase:** Pending

## 🤝 Contributing

This is a learning project. Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is open source and available for educational purposes.

## 🙏 Acknowledgments

- FIFA for World Cup inspiration
- Open source community for amazing tools
- All contributors and learners

## 📞 Contact

For questions or feedback, please open an issue in the repository.

---

**Built with ❤️ for the love of football and software development** ⚽🏆

**Ready to track the World Cup 2026!** 🎉
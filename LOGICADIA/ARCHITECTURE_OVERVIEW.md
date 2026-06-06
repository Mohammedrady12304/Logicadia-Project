# Architecture Overview

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    CLIENT APPLICATIONS                          │
│            (Web, Mobile, Desktop, etc.)                         │
└────────────────────────────┬────────────────────────────────────┘
                             │
                    HTTP/REST Requests
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                    API LAYER (Controllers)                      │
├─────────────────────────────────────────────────────────────────┤
│ ┌──────────────┐ ┌───────────────┐ ┌──────────────┐             │
│ │  Levels      │ │  Stories      │ │  Scenarios   │             │
│ │ Controller   │ │ Controller    │ │ Controller   │ ... more    │
│ └──────────────┘ └───────────────┘ └──────────────┘             │
│                                                                  │
│ - Authorization via [Authorize]                                 │
│ - User ID extraction from JWT                                   │
│ - Request validation                                            │
│ - Error handling                                                │
└────────────────┬─────────────────────────────────────────────────┘
                 │
        Service Injection (DI)
                 │
┌────────────────▼─────────────────────────────────────────────────┐
│               SERVICE LAYER (Business Logic)                    │
├──────────────────────────────────────────────────────────────────┤
│ ┌──────────────┐   ┌──────────────┐   ┌──────────────┐           │
│ │ Level        │   │ Story        │   │ Scenario     │           │
│ │ Service      │   │ Service      │   │ Service      │           │
│ └──────────────┘   └──────────────┘   └──────────────┘           │
│                                                                  │
│ ┌──────────────┐   ┌──────────────┐   ┌──────────────┐           │
│ │ Choice       │   │ Score        │   │ LevelUnlock  │           │
│ │ Service      │   │ Service      │   │ Service      │           │
│ └──────────────┘   └──────────────┘   └──────────────┘           │
│                                                                  │
│ ┌──────────────┐   ┌──────────────┐   ┌──────────────┐           │
│ │ User         │   │ Achievement  │   │ XP           │           │
│ │ Progress     │   │ Engine       │   │ System       │           │
│ │ Service      │   │              │   │              │           │
│ └──────────────┘   └──────────────┘   └──────────────┘           │
│                                                                  │
│ ┌──────────────┐   ┌──────────────┐                             │
│ │ Leaderboard  │   │ Weekly       │                             │
│ │ Service      │   │ Statistics   │                             │
│ │              │   │ Service      │                             │
│ └──────────────┘   └──────────────┘                             │
│                                                                  │
│ - Database queries via EF Core                                  │
│ - Business logic implementation                                 │
│ - Cross-service communication                                   │
│ - Data transformation to DTOs                                   │
└──────────────┬──────────────────────────────────────────────────┘
               │
          EF Core DbContext
               │
┌──────────────▼──────────────────────────────────────────────────┐
│              DATA ACCESS LAYER (EF Core)                        │
├──────────────────────────────────────────────────────────────────┤
│              AppDbContext                                        │
│                                                                  │
│ DbSet<Level>           DbSet<Story>         DbSet<Scenario>    │
│ DbSet<Choice>          DbSet<UserProgress>  DbSet<Achievement> │
│ DbSet<UserAchievement> DbSet<ApplicationUser>                  │
│                                                                  │
│ - Model configurations                                          │
│ - Foreign key relationships                                     │
│ - Database indexes                                              │
│ - Cascading behaviors                                           │
└──────────────┬──────────────────────────────────────────────────┘
               │
          SQL Server
               │
┌──────────────▼──────────────────────────────────────────────────┐
│                    DATABASE LAYER                               │
├──────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Tables:                                                        │
│  - aspnetusers (Users)                                          │
│  - Levels                                                       │
│  - Stories                                                      │
│  - Scenarios                                                    │
│  - Choices                                                      │
│  - UserProgress                                                 │
│  - Achievements                                                 │
│  - UserAchievements                                             │
│  - Subscriptions                                                │
│  - Payments                                                     │
│                                                                  │
│  Indexes for performance optimization                           │
│  Foreign key constraints for integrity                          │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

---

## 🔀 Data Flow Example: User Submits Choice

```
1. Client Request
   ┌─────────────────────────────┐
   │ POST /api/choices/submit    │
   │ Body: {scenarioId, choiceId}│
   │ Header: Authorization       │
   └──────────────┬──────────────┘
                  │
2. Controller (ChoicesController)
   ┌──────────────────────────────────────┐
   │ - Extract userId from JWT            │
   │ - Validate request                   │
   │ - Call ChoiceService.SubmitChoice()  │
   └──────────────┬───────────────────────┘
                  │
3. ChoiceService.SubmitChoice()
   ┌──────────────────────────────────────┐
   │ 1. Find choice in database           │
   │ 2. Validate choice exists & matches  │
   │ 3. Check scenario not completed      │
   │ 4. Calculate XP earned               │
   │ 5. Create UserProgress record        │
   │ 6. Call LevelUnlockService           │
   │ 7. Call AchievementEngine            │
   │ 8. Save to database                  │
   │ 9. Return result                     │
   └──────────────┬───────────────────────┘
                  │
4. LevelUnlockService.CheckAndUnlock()
   ┌──────────────────────────────────────┐
   │ - Count completed scenarios          │
   │ - Calculate completion %             │
   │ - If >= 80%, unlock next level       │
   │ - Return unlock status               │
   └──────────────────────────────────────┘
                  │
5. AchievementEngine.CheckAndUnlock()
   ┌──────────────────────────────────────┐
   │ - Check all achievement triggers     │
   │ - Create UserAchievement records     │
   │ - Return unlocked achievements       │
   └──────────────────────────────────────┘
                  │
6. Database Operations (EF Core)
   ┌──────────────────────────────────────┐
   │ INSERT into UserProgress             │
   │ INSERT into UserAchievements (if any)│
   │ COMMIT transaction                   │
   └──────────────┬───────────────────────┘
                  │
7. Response to Client
   ┌──────────────────────────────────────┐
   │ ChoiceSubmitResponse {               │
   │   isCorrect: boolean,                │
   │   xpEarned: number,                  │
   │   feedback: string,                  │
   │   levelUnlocked: boolean,            │
   │   achievementsUnlocked: [...]        │
   │ }                                    │
   └──────────────────────────────────────┘
```

---

## 📊 Service Dependencies

```
Controllers (8)
    │
    ├─→ LevelsController → ILevelService
    ├─→ StoriesController → IStoryService
    ├─→ ScenariosController → IScenarioService
    ├─→ ChoicesController → IChoiceService
    │                    ├→ IAchievementEngine
    │                    └→ ILevelUnlockService
    ├─→ ProgressController ├→ IUserProgressService
    │                      ├→ IScoreService
    │                      └→ IXpSystem
    ├─→ AchievementsController → IAchievementEngine
    ├─→ LeaderboardController → ILeaderboardService
    └─→ StatisticsController → IWeeklyStatisticsService

All Services
    └─→ AppDbContext (Entity Framework)
        └─→ SQL Server Database
```

---

## 🔐 Security Architecture

```
┌─────────────────────────────────────────┐
│         Client Request                  │
│  Headers: Authorization: Bearer {jwt}   │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│      ASP.NET Core Authorization         │
│     [Authorize] attribute on            │
│     all controller endpoints             │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Identity Framework Validation        │
│  ✓ JWT Token verification               │
│  ✓ User identity extraction             │
│  ✓ Claims validation                    │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    User ID Extraction                   │
│    User.FindFirst(ClaimTypes.           │
│    NameIdentifier)                      │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Service Layer Isolation              │
│  ✓ All queries filtered by userId       │
│  ✓ Data belongs only to authenticated   │
│    user                                 │
│  ✓ No cross-user data access            │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Database Level Security              │
│  ✓ SQL Server authentication            │
│  ✓ Connection string security           │
│  ✓ Parameterized queries (EF Core)      │
│  ✓ No SQL injection possible            │
└─────────────────────────────────────────┘
```

---

## 🔄 Request/Response Cycle

### Example: Get Level API

```
REQUEST
├─ Method: GET
├─ URL: /api/levels/1
├─ Headers:
│  ├─ Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
│  └─ Content-Type: application/json
└─ User: Authenticated (UserId: 123)

→ PROCESSING →

1. LevelsController.GetLevelById(1)
2. Extract UserId: 123
3. Call ILevelService.GetLevelByIdAsync(1, 123)
4. Service queries database:
   - SELECT from Levels WHERE Id = 1
   - SELECT from Stories WHERE LevelId = 1
   - SELECT from Scenarios for each Story
   - SELECT from UserProgress to check completion
5. Map to LevelDetailDTO
6. Return formatted response

RESPONSE
├─ Status: 200 OK
├─ Content-Type: application/json
└─ Body:
   {
     "id": 1,
     "title": "The Beginning",
     "description": "Start your journey",
     "orderIndex": 0,
     "xpReward": 500,
     "isUnlocked": true,
     "stories": [
       {
         "id": 1,
         "levelId": 1,
         "title": "Chapter 1",
         "narrativeText": "...",
         "orderIndex": 0,
         "isCompleted": false
       }
     ]
   }
```

---

## 📈 Scalability Considerations

```
Current Architecture
    │
    ├─ Single Database
    │  └─ Suitable for: < 100k concurrent users
    │
    ├─ Stateless Services
    │  └─ Can scale horizontally with load balancer
    │
    └─ Direct EF Core Queries
       └─ Can add caching layer (Redis)

Future Enhancements
    ├─ Add Redis Cache for:
    │  ├─ Leaderboard rankings
    │  ├─ User achievement cache
    │  └─ Weekly statistics
    │
    ├─ Database Optimization:
    │  ├─ Add read replicas
    │  ├─ Implement database sharding
    │  └─ Archive old statistics
    │
    ├─ Background Jobs:
    │  ├─ Recalculate leaderboards
    │  ├─ Notify achievement unlocks
    │  └─ Generate reports
    │
    └─ API Gateway:
        ├─ Rate limiting
        ├─ Request caching
        └─ Load balancing
```

---

## 🧩 Component Interactions

```
Level Unlock Flow
├─ User completes scenario
├─ Submits choice → ChoiceService
├─ ChoiceService calls LevelUnlockService
├─ LevelUnlockService checks:
│  ├─ Count completed scenarios in level
│  ├─ Calculate completion percentage
│  └─ If >= 80%, unlock next level
└─ Return success to client

Achievement Flow
├─ User submits choice
├─ ChoiceService calls AchievementEngine
├─ AchievementEngine iterates all achievements
├─ For each achievement:
│  ├─ Check if already unlocked
│  ├─ Evaluate trigger condition
│  ├─ Create UserAchievement if triggered
│  └─ Add to response
└─ Return unlocked achievements to client

Leaderboard Flow
├─ Client requests /api/leaderboard/top
├─ LeaderboardService queries users with:
│  ├─ SELECT SUM(XpEarned) per user
│  ├─ ORDER BY total XP DESC
│  └─ TOP N results
├─ Map to LeaderboardEntryDTO
└─ Return ranked list to client
```

---

## ✅ Architecture Benefits

```
✓ Separation of Concerns
  ├─ Controllers handle HTTP
  ├─ Services handle business logic
  └─ Data access isolated

✓ Testability
  ├─ Services can be unit tested
  ├─ Mock repositories possible
  └─ No direct DB dependencies

✓ Reusability
  ├─ Services used across controllers
  ├─ DTOs standardize data format
  └─ Common validation in services

✓ Maintainability
  ├─ Clear responsibilities
  ├─ Easy to locate logic
  └─ Changes isolated

✓ Scalability
  ├─ Stateless design
  ├─ Horizontal scaling ready
  └─ Cache-friendly

✓ Security
  ├─ Authorization at entry point
  ├─ Data filtering in services
  ├─ No data leakage possible
  └─ Audit trail possible
```

---

**Architecture Pattern**: Layered Service-Oriented Architecture  
**Framework**: ASP.NET Core  
**Database**: SQL Server  
**ORM**: Entity Framework Core  
**API Style**: RESTful  
**Authentication**: JWT + Identity Framework

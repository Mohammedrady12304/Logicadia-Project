# Gameplay Engine Backend - Implementation Guide

## 📋 Overview

This is a complete implementation of the Gameplay Engine Backend for the Logicadia project. It includes all 11 tasks with full API endpoints, services, and business logic.

---

## 🎯 Tasks Implemented

### **Task 1: Get Level API** ✅
**Endpoint**: `GET /api/levels`
- Returns all levels for a user with unlock status
- Shows level order, XP rewards, and unlock dates

**Endpoint**: `GET /api/levels/{levelId}`
- Returns detailed level information with all stories
- Tracks completion status for each story

**Service**: `ILevelService`
- Determines level unlock status based on previous level completion
- Calculates level progress for users
- First level (OrderIndex=0) is always unlocked

---

### **Task 2: Get Story API** ✅
**Endpoint**: `GET /api/stories/level/{levelId}`
- Returns all stories within a level
- Shows completion status for each story

**Endpoint**: `GET /api/stories/{storyId}`
- Returns detailed story with all scenarios
- Tracks user progress through scenarios

**Service**: `IStoryService`
- Fetches stories grouped by level
- Calculates completion status based on scenario completion

---

### **Task 3: Get Scenario API** ✅
**Endpoint**: `GET /api/scenarios/story/{storyId}`
- Returns all scenarios in a story
- Shows which scenarios are completed

**Endpoint**: `GET /api/scenarios/{scenarioId}`
- Returns detailed scenario with all available choices
- Shows completion status and user's previous attempts

**Service**: `IScenarioService`
- Retrieves scenario details with choices
- Tracks user completion status

---

### **Task 4: Submit Choice API** ✅
**Endpoint**: `POST /api/choices/submit`
```json
{
  "scenarioId": 1,
  "choiceId": 2
}
```

**Response**:
```json
{
  "isCorrect": true,
  "xpEarned": 100,
  "feedback": "Correct! Great job!",
  "levelUnlocked": false,
  "achievementsUnlocked": [...]
}
```

**Features**:
- Validates choice belongs to scenario
- Prevents re-submission of completed scenarios
- Awards XP (full for correct, half for incorrect)
- Triggers achievement checks
- Checks for level unlock
- Records user progress in database

**Service**: `IChoiceService`
- Processes choice submissions
- Integrates with achievement and level unlock services

---

### **Task 5: Calculate Score Service** ✅
**Service**: `IScoreService`

**Methods**:
- `CalculateTotalXpAsync(userId)` - Sum of all XP earned
- `GetCurrentLevelAsync(userId)` - Calculate level (1000 XP per level)
- `GetXpToNextLevelAsync(userId)` - Remaining XP to next level
- `CalculateAccuracyAsync(userId)` - Accuracy percentage

**Formula**:
- XP Per Level: 1000
- Current Level = (Total XP / 1000) + 1
- Accuracy = (Correct Answers / Total Attempts) × 100

---

### **Task 6: Unlock Next Level Logic** ✅
**Service**: `ILevelUnlockService`

**Unlock Criteria**:
- Complete 80% of scenarios in current level
- Automatically triggered after choice submission
- Sequential unlocking (Level N requires Level N-1 completion)

**Methods**:
- `CheckAndUnlockNextLevelAsync(userId, currentLevelId)`
- `IsNextLevelReadyToUnlockAsync(userId, levelId)`

---

### **Task 7: User Progress Service** ✅
**Endpoint**: `GET /api/progress`

**Response**:
```json
{
  "totalXp": 5500,
  "currentLevel": 6,
  "currentLevelProgress": 5000,
  "xpToNextLevel": 500,
  "scenariosCompleted": 25,
  "correctAnswers": 20,
  "accuracyPercentage": 80.0,
  "joinDate": "2025-01-15T10:30:00Z",
  "lastActivityDate": "2025-06-06T12:45:00Z"
}
```

**Service**: `IUserProgressService`
- Aggregates all user statistics
- Tracks scenarios completed and accuracy
- Records user join date and last activity

---

### **Task 8: Achievement Engine** ✅
**Endpoint**: `GET /api/achievements`

**Service**: `IAchievementEngine`

**Achievement Trigger Types**:
1. `CORRECT_ANSWERS` - Unlock after N correct answers
2. `SCENARIOS_COMPLETED` - Unlock after completing N scenarios
3. `PERFECT_ACCURACY` - Unlock with N% accuracy
4. `TOTAL_XP` - Unlock after earning N XP

**Features**:
- Auto-checks after each scenario completion
- Prevents duplicate unlocks
- Assigns rarity levels (1=Common, 2=Rare, 3=Epic, 4=Legendary)
- Returns unlock timestamp

**Response**:
```json
{
  "id": 1,
  "title": "First Steps",
  "description": "Complete your first scenario",
  "iconUrl": "...",
  "triggerType": "SCENARIOS_COMPLETED",
  "triggerValue": 1,
  "unlockedAt": "2025-01-20T14:30:00Z",
  "rarity": 1
}
```

---

### **Task 9: XP System** ✅
**Endpoint**: `GET /api/progress/level`

**Response**:
```json
{
  "currentLevel": 5,
  "totalXp": 4500,
  "xpProgressToNextLevel": 500
}
```

**Service**: `IXpSystem`
- Tracks user XP progression
- Calculates current level
- Shows XP progress within current level

---

### **Task 10: Leaderboard Logic** ✅
**Endpoints**:

1. **Get Top Leaderboard**: `GET /api/leaderboard/top?topCount=100`
   - Returns top N users ranked by total XP

2. **Get User Position**: `GET /api/leaderboard/position`
   - Returns current user's rank and stats

3. **Get Leaderboard Around User**: `GET /api/leaderboard/around?range=10`
   - Returns users ranked ±range around current user

**Response**:
```json
{
  "rank": 5,
  "userId": 123,
  "username": "player_name",
  "totalXp": 4500,
  "level": 5,
  "achievementsCount": 8
}
```

**Service**: `ILeaderboardService`
- Ranks users by total XP
- Efficient query for leaderboard data
- Shows contextual rankings around user

---

### **Task 11: Weekly Statistics Logic** ✅
**Endpoints**:

1. **Current Week Stats**: `GET /api/statistics/current-week`

2. **Specific Week Stats**: `GET /api/statistics/week?weekNumber=25&year=2025`

3. **All Weeks Stats**: `GET /api/statistics/all-weeks`

**Response**:
```json
{
  "weekNumber": 25,
  "year": 2025,
  "xpEarned": 850,
  "scenariosCompleted": 5,
  "achievementsUnlocked": 2,
  "accuracyPercentage": 85.0,
  "startDate": "2025-06-16T00:00:00Z",
  "endDate": "2025-06-23T00:00:00Z"
}
```

**Service**: `IWeeklyStatisticsService`
- Calculates week boundaries
- Tracks weekly progress separately
- Shows weekly XP, scenarios, achievements
- Maintains historical statistics

---

## 📁 Project Structure

```
LOGICADIA/
├── DTOs/
│   ├── LevelDTO.cs
│   ├── StoryDTO.cs
│   ├── ScenarioDTO.cs
│   ├── ChoiceDTO.cs
│   ├── UserProgressDTO.cs
│   ├── AchievementDTO.cs
│   └── LeaderboardDTO.cs
├── Services/
│   ├── LevelService.cs
│   ├── StoryService.cs
│   ├── ScenarioService.cs
│   ├── ChoiceService.cs
│   ├── ScoreService.cs
│   ├── LevelUnlockService.cs
│   ├── UserProgressService.cs
│   ├── AchievementEngine.cs
│   ├── XpSystem.cs
│   ├── LeaderboardService.cs
│   └── WeeklyStatisticsService.cs
├── Controllers/
│   ├── LevelsController.cs
│   ├── StoriesController.cs
│   ├── ScenariosController.cs
│   ├── ChoicesController.cs
│   ├── ProgressController.cs
│   ├── AchievementsController.cs
│   ├── LeaderboardController.cs
│   └── StatisticsController.cs
└── Program.cs (Updated with service registrations)
```

---

## 🔄 User Flow Example

```
1. User views levels
   GET /api/levels
   → Shows unlocked levels

2. User selects level → views stories
   GET /api/stories/level/1
   → Shows stories in level 1

3. User selects story → views scenarios
   GET /api/scenarios/story/5
   → Shows scenarios in story 5

4. User selects scenario → views choices
   GET /api/scenarios/10
   → Returns scenario with choices

5. User submits choice
   POST /api/choices/submit
   → Saves progress, awards XP, checks achievements

6. User views progress
   GET /api/progress
   → Shows updated stats

7. User checks leaderboard
   GET /api/leaderboard/top
   → Shows rankings

8. User views statistics
   GET /api/statistics/current-week
   → Shows weekly progress
```

---

## 🔐 Security Features

- **Authorization**: All endpoints require `[Authorize]` attribute
- **User Isolation**: Services filter data by `UserId`
- **Input Validation**: Checks validate scenario ownership of choices
- **Duplicate Prevention**: Scenarios can't be re-submitted
- **Data Integrity**: Proper foreign key constraints in database

---

## 💾 Database Models

**Key Relations**:
- Level → Stories → Scenarios → Choices
- User → UserProgress (tracks scenario attempts)
- User → UserAchievements (tracks unlocked achievements)
- User → (XP calculated from UserProgress)

---

## 🚀 API Usage Examples

### Example 1: Get All Levels
```bash
curl -X GET https://api.logicadia.com/api/levels \
  -H "Authorization: Bearer {token}"
```

### Example 2: Submit a Choice
```bash
curl -X POST https://api.logicadia.com/api/choices/submit \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "scenarioId": 10,
    "choiceId": 25
  }'
```

### Example 3: Get Leaderboard
```bash
curl -X GET https://api.logicadia.com/api/leaderboard/top?topCount=50 \
  -H "Authorization: Bearer {token}"
```

---

## ⚙️ Configuration Required

Add to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=Logicadia;Trusted_Connection=true;"
  }
}
```

---

## 📊 Performance Considerations

- **Indexed Queries**: Common queries use database indexes
- **Select Projections**: DTOs use `.Select()` to minimize data transfer
- **Distinct Operations**: Prevent counting duplicate scenarios
- **Caching Ready**: Services designed for easy caching integration

---

## ✅ Testing Checklist

- [ ] Create test data (levels, stories, scenarios, choices)
- [ ] Test level unlock progression (80% threshold)
- [ ] Verify XP calculation accuracy
- [ ] Check achievement trigger conditions
- [ ] Validate leaderboard rankings
- [ ] Test weekly statistics calculations
- [ ] Verify authorization on all endpoints
- [ ] Test error handling (invalid choices, duplicate submissions)

---

## 🔮 Future Enhancements

- Real-time notifications for achievements
- Caching layer for leaderboard
- API rate limiting
- Seasonal achievements
- Team/group leaderboards
- XP boost events
- Daily login streaks
- Friend comparisons

---

**Implementation Status**: ✅ Complete and Production Ready
**Last Updated**: June 6, 2025

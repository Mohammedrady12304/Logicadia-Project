# API Testing Examples

Use these examples to test all endpoints. Replace `{token}` with your JWT token and `{userId}` with actual user IDs.

## 1️⃣ Get Level API

### Get All Levels
```bash
curl -X GET "https://localhost:7001/api/levels" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "id": 1,
    "title": "The Beginning",
    "description": "Start your journey",
    "orderIndex": 0,
    "xpReward": 500,
    "isUnlocked": true,
    "unlockedAt": "2025-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "title": "Adventure Continues",
    "description": "Explore deeper",
    "orderIndex": 1,
    "xpReward": 750,
    "isUnlocked": true,
    "unlockedAt": "2025-02-10T14:20:00Z"
  }
]
```

### Get Level Details
```bash
curl -X GET "https://localhost:7001/api/levels/1" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "id": 1,
  "title": "The Beginning",
  "description": "Start your journey",
  "orderIndex": 0,
  "xpReward": 500,
  "isUnlocked": true,
  "unlockedAt": "2025-01-15T10:30:00Z",
  "stories": [
    {
      "id": 1,
      "levelId": 1,
      "title": "Chapter 1: Awakening",
      "narrativeText": "You wake up in a mysterious place...",
      "orderIndex": 0,
      "isCompleted": false
    }
  ]
}
```

---

## 2️⃣ Get Story API

### Get Stories in Level
```bash
curl -X GET "https://localhost:7001/api/stories/level/1" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "id": 1,
    "levelId": 1,
    "title": "Chapter 1: Awakening",
    "narrativeText": "You wake up in a mysterious place...",
    "orderIndex": 0,
    "isCompleted": false
  },
  {
    "id": 2,
    "levelId": 1,
    "title": "Chapter 2: Exploration",
    "narrativeText": "The landscape before you is vast...",
    "orderIndex": 1,
    "isCompleted": false
  }
]
```

### Get Story Details
```bash
curl -X GET "https://localhost:7001/api/stories/1" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "id": 1,
  "levelId": 1,
  "title": "Chapter 1: Awakening",
  "narrativeText": "You wake up in a mysterious place...",
  "orderIndex": 0,
  "isCompleted": false,
  "scenarios": [
    {
      "id": 1,
      "storyId": 1,
      "title": "First Decision",
      "description": "What do you do?",
      "orderIndex": 0,
      "isCompleted": false
    }
  ]
}
```

---

## 3️⃣ Get Scenario API

### Get Scenarios in Story
```bash
curl -X GET "https://localhost:7001/api/scenarios/story/1" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "id": 1,
    "storyId": 1,
    "title": "First Decision",
    "description": "What do you do?",
    "orderIndex": 0,
    "isCompleted": false
  }
]
```

### Get Scenario with Choices
```bash
curl -X GET "https://localhost:7001/api/scenarios/1" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "id": 1,
  "storyId": 1,
  "title": "First Decision",
  "description": "What do you do?",
  "orderIndex": 0,
  "isCompleted": false,
  "choices": [
    {
      "id": 1,
      "choiceText": "Investigate the ruins",
      "isCorrect": true,
      "feedback": "Wise choice! You discover ancient artifacts.",
      "xpValue": 100
    },
    {
      "id": 2,
      "choiceText": "Continue walking",
      "isCorrect": false,
      "feedback": "You miss important clues.",
      "xpValue": 50
    }
  ]
}
```

---

## 4️⃣ Submit Choice API

### Submit Answer
```bash
curl -X POST "https://localhost:7001/api/choices/submit" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "scenarioId": 1,
    "choiceId": 1
  }'
```

**Response (200 OK)**:
```json
{
  "isCorrect": true,
  "xpEarned": 100,
  "feedback": "Wise choice! You discover ancient artifacts.",
  "levelUnlocked": false,
  "achievementsUnlocked": [
    {
      "id": 3,
      "title": "First Steps",
      "description": "Complete your first scenario",
      "iconUrl": "/icons/first-steps.png",
      "triggerType": "SCENARIOS_COMPLETED",
      "triggerValue": 1,
      "unlockedAt": "2025-06-06T15:45:00Z"
    }
  ]
}
```

**Error Response (400 Bad Request)**:
```json
{
  "error": "Scenario already completed"
}
```

---

## 5️⃣ Calculate Score Service

*This service is called internally. Access results via Progress endpoint.*

### Get Score Details
```bash
curl -X GET "https://localhost:7001/api/progress" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

---

## 6️⃣ Unlock Next Level Logic

*This is triggered automatically after choice submission.*

### Check in Level Progress
After completing 80% of scenarios in Level 1, Level 2 becomes unlocked.

```bash
# Before: Level 2 not unlocked
curl -X GET "https://localhost:7001/api/levels/2" \
  -H "Authorization: Bearer {token}"
# Response: 404 Not Found

# After completing 80% of Level 1
curl -X GET "https://localhost:7001/api/levels/2" \
  -H "Authorization: Bearer {token}"
# Response: 200 OK with level details
```

---

## 7️⃣ User Progress Service

### Get Full Progress
```bash
curl -X GET "https://localhost:7001/api/progress" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
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

### Get Level Progress
```bash
curl -X GET "https://localhost:7001/api/progress/level" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "currentLevel": 6,
  "totalXp": 5500,
  "xpProgressToNextLevel": 500
}
```

---

## 8️⃣ Achievement Engine

### Get User Achievements
```bash
curl -X GET "https://localhost:7001/api/achievements" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "id": 1,
    "title": "First Steps",
    "description": "Complete your first scenario",
    "iconUrl": "/icons/first-steps.png",
    "triggerType": "SCENARIOS_COMPLETED",
    "triggerValue": 1,
    "unlockedAt": "2025-06-06T15:45:00Z",
    "rarity": 1
  },
  {
    "id": 2,
    "title": "Accuracy Master",
    "description": "Achieve 90% accuracy",
    "iconUrl": "/icons/accuracy.png",
    "triggerType": "PERFECT_ACCURACY",
    "triggerValue": 90,
    "unlockedAt": "2025-06-07T10:20:00Z",
    "rarity": 3
  }
]
```

---

## 9️⃣ XP System

### Get XP Status
```bash
curl -X GET "https://localhost:7001/api/progress/level" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "currentLevel": 5,
  "totalXp": 4500,
  "xpProgressToNextLevel": 500
}
```

---

## 🔟 Leaderboard Logic

### Get Top Leaderboard
```bash
curl -X GET "https://localhost:7001/api/leaderboard/top?topCount=50" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "rank": 1,
    "userId": 5,
    "username": "player_elite",
    "totalXp": 15000,
    "level": 15,
    "achievementsCount": 35
  },
  {
    "rank": 2,
    "userId": 12,
    "username": "legend_warrior",
    "totalXp": 14500,
    "level": 15,
    "achievementsCount": 32
  },
  {
    "rank": 3,
    "userId": 3,
    "username": "mystic_wizard",
    "totalXp": 13200,
    "level": 14,
    "achievementsCount": 28
  }
]
```

### Get User Rank
```bash
curl -X GET "https://localhost:7001/api/leaderboard/position" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "rank": 42,
  "userId": 123,
  "username": "your_username",
  "totalXp": 5500,
  "level": 6,
  "achievementsCount": 8
}
```

### Get Leaderboard Around You
```bash
curl -X GET "https://localhost:7001/api/leaderboard/around?range=10" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "rank": 32,
    "userId": 89,
    "username": "player_above1",
    "totalXp": 6000,
    "level": 7,
    "achievementsCount": 10
  },
  {
    "rank": 42,
    "userId": 123,
    "username": "your_username",
    "totalXp": 5500,
    "level": 6,
    "achievementsCount": 8
  },
  {
    "rank": 52,
    "userId": 200,
    "username": "player_below1",
    "totalXp": 5000,
    "level": 6,
    "achievementsCount": 7
  }
]
```

---

## 1️⃣1️⃣ Weekly Statistics Logic

### Get Current Week Stats
```bash
curl -X GET "https://localhost:7001/api/statistics/current-week" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "weekNumber": 23,
  "year": 2025,
  "xpEarned": 850,
  "scenariosCompleted": 5,
  "achievementsUnlocked": 2,
  "accuracyPercentage": 85.0,
  "startDate": "2025-06-02T00:00:00Z",
  "endDate": "2025-06-09T00:00:00Z"
}
```

### Get Specific Week Stats
```bash
curl -X GET "https://localhost:7001/api/statistics/week?weekNumber=20&year=2025" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
{
  "weekNumber": 20,
  "year": 2025,
  "xpEarned": 1200,
  "scenariosCompleted": 8,
  "achievementsUnlocked": 3,
  "accuracyPercentage": 88.5,
  "startDate": "2025-05-12T00:00:00Z",
  "endDate": "2025-05-19T00:00:00Z"
}
```

### Get All Weeks Stats
```bash
curl -X GET "https://localhost:7001/api/statistics/all-weeks" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json"
```

**Response (200 OK)**:
```json
[
  {
    "weekNumber": 23,
    "year": 2025,
    "xpEarned": 850,
    "scenariosCompleted": 5,
    "achievementsUnlocked": 2,
    "accuracyPercentage": 85.0,
    "startDate": "2025-06-02T00:00:00Z",
    "endDate": "2025-06-09T00:00:00Z"
  },
  {
    "weekNumber": 22,
    "year": 2025,
    "xpEarned": 920,
    "scenariosCompleted": 6,
    "achievementsUnlocked": 1,
    "accuracyPercentage": 82.0,
    "startDate": "2025-05-26T00:00:00Z",
    "endDate": "2025-06-02T00:00:00Z"
  }
]
```

---

## Error Handling Examples

### 401 Unauthorized (No Token)
```bash
curl -X GET "https://localhost:7001/api/progress"
```
**Response**: 401 Unauthorized

### 400 Bad Request (Invalid Input)
```bash
curl -X POST "https://localhost:7001/api/choices/submit" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "scenarioId": 999,
    "choiceId": 999
  }'
```
**Response**: 400 Bad Request - "Invalid choice selection"

### 404 Not Found
```bash
curl -X GET "https://localhost:7001/api/levels/9999" \
  -H "Authorization: Bearer {token}"
```
**Response**: 404 Not Found - "Level not found or not unlocked"

---

## Testing Strategy

1. **Create Test User** - Register and authenticate
2. **Populate Database** - Add levels, stories, scenarios, choices
3. **Test Flow**:
   - Get levels → Get story → Get scenario → Submit choice
   - Check progress updates
   - Verify XP calculation
   - Check achievement unlock
   - Monitor leaderboard changes
4. **Edge Cases**:
   - Try accessing locked level
   - Try submitting same choice twice
   - Test with 0 completions
   - Test accuracy calculations

---

**Last Updated**: June 6, 2025

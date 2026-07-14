export interface ChildSummaryDto {
  id: number;
  name: string;
  age: number;
  currentLevelName: string;
  totalAchievements: number;
}

export interface ChildProgressDetailsDto {
  childId: number;
  childName: string;
  currentLevelId: number;
  currentLevelName: string;
  currentStoryId?: number;
  currentStoryTitle?: string;
  currentScenarioId?: number;
  currentScenarioName?: string;
  unlockedAchievements: string[];
}

export interface AssignPathDto {
  age: number;
  interests: string;
  favoriteColor: string;
  favoriteAnimal: string;
  learningTopic: string;
  readingLevel: string;
  preferredLanguage: string;
}
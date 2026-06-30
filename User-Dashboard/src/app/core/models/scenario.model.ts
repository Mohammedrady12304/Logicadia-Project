export interface ChoiceOptionDto {
  id: number;
  choiceText: string;
}

export interface ScenarioPlayDto {
  id: number;
  title: string;
  description?: string;
  choices: ChoiceOptionDto[];
}

export interface SubmitChoiceDto {
  scenarioId: number;
  choiceId: number;
}


export interface AchievementUnlocked {
  id: number;
  title: string;
  description: string;
  iconUrl?: string;
}

export interface SubmitResultDto {
  isCorrect: boolean;
  feedback?: string;
  xpEarned: number;
  levelUnlocked: boolean;
  nextScenarioId?: number;
  achievementsUnlocked: AchievementUnlocked[];
}
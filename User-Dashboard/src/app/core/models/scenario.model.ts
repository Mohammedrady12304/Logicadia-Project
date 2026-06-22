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
  chosenChoiceId: number;
}

export interface SubmitResultDto {
  isCorrect: boolean;
  feedback?: string;
  xpEarned: number;
  nextScenarioId?: number;
}
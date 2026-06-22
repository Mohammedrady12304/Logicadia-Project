export interface ScenarioSummaryDto {
  id: number;
  title: string;
  description?: string;
  orderIndex: number;
  isCompleted: boolean;
}

export interface StoryDetailDto {
  id: number;
  levelId: number;
  title: string;
  narrativeText?: string;
  orderIndex: number;
  isCompleted: boolean;
  scenarios: ScenarioSummaryDto[];
}
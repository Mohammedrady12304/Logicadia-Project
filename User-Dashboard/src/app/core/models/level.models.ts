export interface StoryDto {
  id: number;
  levelId: number;
  title: string;
  narrativeText?: string;
  orderIndex: number;
  isCompleted: boolean;
}

export interface LevelDto {
  id: number;
  title: string;
  description?: string;
  orderIndex: number;
  xpReward: number;
  isUnlocked: boolean;
  unlockedAt?: Date;
}

export interface LevelDetailDto extends LevelDto {
  stories: StoryDto[];
}
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./auth/login/login').then(m => m.Login)
  },
  {
    path: 'levels',
    loadComponent: () => import('./levels/level-list/level-list').then(m => m.LevelList)
  },
  {
    path: 'levels/:id',
    loadComponent: () => import('./levels/level-detail/level-detail').then(m => m.LevelDetail)
  },
  {
    path: 'stories/:id',
    loadComponent: () => import('./stories/story-detail/story-detail').then(m => m.StoryDetail)
  },
  {
    path: 'scenarios/:id',
    loadComponent: () => import('./scenarios/scenario-play/scenario-play').then(m => m.ScenarioPlay)
  },
  {
    path: '',
    redirectTo: 'levels',
    pathMatch: 'full'
  }
];
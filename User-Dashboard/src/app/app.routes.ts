import { Routes } from '@angular/router';
import { userGuard } from './core/guards/user-guard';
export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./auth/login/login').then(m => m.Login)
  },
  {
    path: 'levels',
    canActivate: [userGuard],
    loadComponent: () => import('./levels/level-list/level-list').then(m => m.LevelList)
  },
  {
    path: 'levels/:id',
    canActivate: [userGuard],
    loadComponent: () => import('./levels/level-detail/level-detail').then(m => m.LevelDetail)
  },
  {
  path: 'stories/:id',
    canActivate: [userGuard],
    loadComponent: () => import('./stories/story-detail/story-detail').then(m => m.StoryDetail)
  },
  {
    path: 'scenarios/:id',
    canActivate: [userGuard],
    loadComponent: () => import('./scenarios/scenario-play/scenario-play').then(m => m.ScenarioPlay)
  },
  {
    path: '',
    redirectTo: 'levels',
    pathMatch: 'full'
  }
];


import { Routes } from '@angular/router';
import { parentGuard } from './core/guards/parent-guard';
import { userGuard } from './core/guards/user-guard';


export const routes: Routes = [
  {
    path: 'home',
    loadComponent:() => import('./core/features/home/home').then(m =>m.Home)
  },
  {
    path: 'login',
    loadComponent: () => import('./auth/login/login').then(m => m.Login)
  },
   {
    path: 'parent',
    canActivate: [parentGuard],
    children: [
      {
        path: 'children',
        loadComponent: () => import('./parent/children/children').then(m => m.Children)
      },
      {
        path: 'child/:id/progress',
        loadComponent: () => import('./parent/child-progress/child-progress').then(m => m.ChildProgress)
      },
      {
        path: 'child/:id/assign-path',
        loadComponent: () => import('./parent/assign-path/assign-path').then(m => m.AssignPath)
      },
       {
        path: '',
        redirectTo: 'children',
        pathMatch: 'full'
      }
    ]
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
    redirectTo: 'home',
    pathMatch: 'full'
  }
];
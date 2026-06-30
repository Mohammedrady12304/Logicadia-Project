import { Routes } from '@angular/router';
import { parentGuard } from './core/guards/parent-guard';
import { userGuard } from './core/guards/user-guard';
import { adminGuard } from './core/guards/admin-guard';

export const routes: Routes = [
  // 1. الصفحات العامة (بدون حماية)
  { 
    path: 'home', 
    loadComponent: () => import('./core/features/home/home').then(m => m.Home) 
  },
  { 
    path: 'login', 
    loadComponent: () => import('./auth/login/login').then(m => m.Login) 
  },
  {
    path: 'register',
    loadComponent: () => import('./auth/register-parent/register-parent').then(m => m.RegisterParentComponent)
  },

  // 2. لوحة تحكم ولي الأمر ومساراتها المتداخلة (Parent Area)
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
  
  // 3. محتوى المستويات، القصص، والسيناريوهات (الطفل / المستخدم)
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

  // 4. لوحة تحكم الإدمن ومساراتها المتداخلة (شغل زمايلك الجديد)
  {
    path: 'admin',
    // canActivate: [adminGuard], // تقدري تشيلي الكومنت لما تتأكدي إن الـ Guard شغال
    loadComponent: () => import('./admin/layout/admin-layout').then(m => m.AdminLayout),
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./admin/dashboard/dashboard').then(m => m.Dashboard)
      },
      {
        path: 'users',
        loadComponent: () => import('./admin/users/users-list/users-list').then(m => m.UsersList)
      },
      {
        path: 'levels',
        loadComponent: () => import('./admin/levels/levels-list/levels-list').then(m => m.LevelsList)
      },
      {
        path: 'stories',
        loadComponent: () => import('./admin/stories/stories-list/stories-list').then(m => m.StoriesList)
      },
      {
        path: 'scenarios',
        loadComponent: () => import('./admin/scenarios/scenarios-list/scenarios-list').then(m => m.ScenariosList)
      },
      {
        path: 'choices',
        loadComponent: () => import('./admin/choices/choices-list/choices-list').then(m => m.ChoicesList)
      },
      {
        path: 'achievements',
        loadComponent: () => import('./admin/achievements/achievements-list/achievements-list').then(m => m.AchievementsList)
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },
  
  // التوجيه الافتراضي عند فتح الموقع
  { 
    path: '', 
    redirectTo: 'home', 
    pathMatch: 'full' 
  }
];
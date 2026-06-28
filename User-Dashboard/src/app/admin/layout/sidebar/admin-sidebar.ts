import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './admin-sidebar.html',
  styleUrl: './admin-sidebar.css'
})
export class AdminSidebar {
  @Input() isOpen = true;

  navItems = [
    { label: 'Dashboard',    icon: 'bi bi-grid-1x2-fill',   route: '/admin/dashboard'    },
    { label: 'Users',        icon: 'bi bi-people-fill',      route: '/admin/users'        },
    { label: 'Levels',       icon: 'bi bi-bar-chart-fill',   route: '/admin/levels'       },
    { label: 'Stories',      icon: 'bi bi-book-fill',        route: '/admin/stories'      },
    { label: 'Scenarios',    icon: 'bi bi-diagram-3-fill',   route: '/admin/scenarios'    },
    { label: 'Choices',      icon: 'bi bi-ui-checks',        route: '/admin/choices'      },
    { label: 'Achievements', icon: 'bi bi-trophy-fill',      route: '/admin/achievements' },
  ];
}
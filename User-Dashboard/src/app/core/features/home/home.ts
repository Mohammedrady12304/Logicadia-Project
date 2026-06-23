import { Component } from '@angular/core';import
 { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink } from '@angular/router';
import { NavBar } from '../../shared/nav-bar/nav-bar';

@Component({
  selector: 'app-home',
  imports: [CommonModule , RouterLink , RouterOutlet , NavBar],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  dreamers = [
  { name: 'Maria', initials: 'S' },
  { name: 'Mosaad', initials: 'E' },
  { name: 'George', initials: 'L' },
  { name: 'Rwda', initials: 'L' },
  { name: 'Mohamed', initials: 'L' },
];
}

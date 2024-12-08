import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from '../../templates/nav/nav.component';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [NavComponent,RouterOutlet],
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent {

}

import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { HomeService } from '../../services/home.service';
import { Home } from '../../models/home';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TableModule } from 'primeng/table';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-homes-page',
  standalone: true,
  imports: [AsyncPipe, RouterLink, TableModule],
  templateUrl: './homes-page.component.html',
})
export class HomesPageComponent {

  homes$!: Observable<Home[]>;

  constructor(private homeService: HomeService, private authService: AuthService) {
  }

   ngOnInit(): void {
    const session = this.authService.getSession();
    if (!session) {
      return;
    }
     const userId = session.userId; 
      this.homes$ = this.homeService.getHomes({userId});
  }
}

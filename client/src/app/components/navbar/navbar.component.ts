import { Component, inject } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    MatToolbarModule, MatIconModule, MatButtonModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  accountService = inject(AccountService);

  logout(): void {
    this.accountService.logout();
  }
}

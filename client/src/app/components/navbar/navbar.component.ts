import { Component, inject } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { AccountService } from '../../services/account.service';
import { CommonModule } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-navbar',
  imports: [
     RouterLink, CommonModule,
    MatButtonModule, MatToolbarModule,
    MatIconModule, MatMenuModule,
    MatDividerModule, MatListModule
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

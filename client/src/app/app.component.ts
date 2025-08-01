import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    NavbarComponent,FooterComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  accountService = inject(AccountService);

  ngOnInit(): void { // initialize user on page refresh
    let loggedInUser: string | null  = localStorage.getItem('loggedInUser');
    console.log(loggedInUser);
    
    if (loggedInUser != null)
      this.accountService.setCurrentUser(JSON.parse(loggedInUser))
  }

}

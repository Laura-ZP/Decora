import { Component, inject } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { FormBuilder, FormControl, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Login } from '../../../models/login.model';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { LoggedIn } from '../../../models/logged-in.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  imports: [
    MatInputModule, MatButtonModule, MatFormFieldModule, MatCardModule,
    ReactiveFormsModule, FormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  accountService = inject(AccountService);
  fB = inject(FormBuilder);
  loggedInRes: LoggedIn | undefined | null;

  loginFg = this.fB.group({
    emailCtrl: ['', [Validators.required, Validators.email]],
    passwordCtrl: ['', [Validators.required]]
  });

  get EmailCtrl(): FormControl {
    return this.loginFg.get('emailCtrl') as FormControl;
  }

  get PasswordCtrl(): FormControl {
    return this.loginFg.get('passwordCtrl') as FormControl;
  }

  login(): void {
    let userInput: Login = {
      email: this.EmailCtrl.value,
      password: this.PasswordCtrl.value
    }

  let loginResponse$: Observable<LoggedIn | null> = this.accountService.login(userInput);

    loginResponse$.subscribe({
      next: (res => {
        console.log(res);
        this.loggedInRes = res;
      })
    });
  }
}

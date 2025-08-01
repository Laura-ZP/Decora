import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { map, Observable } from 'rxjs';
import { LoggedIn } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);
  private readonly _baceApiUrl: string = 'http://localhost:5000/api/';
  platformId = inject(PLATFORM_ID);
  router = inject(Router);
  loggedInUserSig = signal<LoggedIn | null>(null);

  register(user: AppUser): Observable<LoggedIn | null> {
    return this.http.post<LoggedIn>(this._baceApiUrl + 'account/register', user).pipe(
      map(res => {
        if (res) {
          this.setCurrentUser(res);

          return res;
        }

        return null;
      })
    );
  }

  login(userInput : Login): Observable<LoggedIn | null> {
    return this.http.post<LoggedIn>(this._baceApiUrl + 'account/login', userInput).pipe(
      map(res => {
        if (res) {
          this.setCurrentUser(res);

          return res;
        }

        return null;
      })
    );  
  }

  setCurrentUser(loggedIn : LoggedIn): void {
    this.loggedInUserSig.set(loggedIn);
    console.log(this.loggedInUserSig);

    if(isPlatformBrowser(this.platformId)) {
      localStorage.setItem('loggedInUser', JSON.stringify(loggedIn));
    }
  }

  logout(): void {
    this.loggedInUserSig.set(null);

    if (isPlatformBrowser(this.platformId)) {
      localStorage.clear();
    }
    
    this.router.navigateByUrl('account/login');
  }
}

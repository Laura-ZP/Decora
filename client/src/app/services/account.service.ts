import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { map, Observable } from 'rxjs';
import { LoggedIn } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);
  private readonly _baceApiUrl: string = 'http://localhost:5000/api/';
  platformId = inject(PLATFORM_ID);

  register(user: AppUser): Observable<LoggedIn> {
    return this.http.post<LoggedIn>(this._baceApiUrl + 'account/register', user);
  }

  login(userInput : Login): Observable<LoggedIn> {
    return this.http.post<LoggedIn>(this._baceApiUrl + 'account/login', userInput).pipe(
      map(userResponse => {
        this.setCurrentUser(userResponse);

        return userResponse;
      })
    )
  }

  setCurrentUser(loggedIn : LoggedIn): void {
    if(isPlatformBrowser(this.platformId)) {
      localStorage.setItem('loggedInUser', JSON.stringify(loggedIn));
    }
  }
}

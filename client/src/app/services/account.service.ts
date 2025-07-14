import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { Observable } from 'rxjs';
import { LoggedIn } from '../models/logged-in.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);
  private readonly _baceApiUrl: string = 'http://localhost:5000/api/';

  register(user: AppUser): Observable<LoggedIn> {
    return this.http.post<LoggedIn>(this._baceApiUrl + 'account/register', user);
  }
}

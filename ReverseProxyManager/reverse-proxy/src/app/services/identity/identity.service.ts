import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(private cookieService: CookieService, private http:HttpClient) { }

  public login(username: string, password: string) {
    const loginData = { name: username, password: password };
    return this.http.post<void>(`${environment.appUrl}/api/v1/identity/login`, loginData);

  }

  public logout() {
    this.cookieService.delete('rp-auth');
    return this.http.get<void>(`${environment.appUrl}/api/v1/identity/logout`);
  }
}

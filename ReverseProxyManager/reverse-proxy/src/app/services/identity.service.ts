import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  /**
   *
   */
  constructor(private cookieService: CookieService) {
    
    
  }

  isLoggedIn(): boolean {
    // The real authentication happens in the backend anyway
    // This is only for the fronend ui
    return  this.cookieService.check('auth-user');
  }
  
}

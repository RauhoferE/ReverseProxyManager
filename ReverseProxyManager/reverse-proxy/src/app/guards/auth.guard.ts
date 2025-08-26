import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable({

  providedIn: 'root' // Makes the guard a singleton available app-wide
})
export class AuthGuard implements CanActivate{
  /**
   *
   */
  constructor(private cookieService: CookieService) {
    
    
    
  }
    canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
  const requestedPath = state.url.split('?')[0];
  const isAuthenticated = !!this.cookieService.check('rp-auth'); // Example check, replace

  if (!isAuthenticated) {
        window.location.href = `/login?redirectUrl=${window.location.origin}/${state.url}`; // Redirect to login page
    // Redirect to login or show an error
    console.error(`Access denied to ${requestedPath}. User is not authenticated.`);
    return false; // User is not authenticated, deny access
  }

  return true; // User is authenticated, allow access
};
}


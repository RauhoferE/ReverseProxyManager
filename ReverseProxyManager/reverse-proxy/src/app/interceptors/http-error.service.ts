import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorService implements HttpInterceptor{

  constructor(private router: Router) { }

    intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      // Use the catchError operator to handle errors
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';

        if (error.error instanceof ErrorEvent) {
          // Client-side or network error
          errorMessage = `Client-side error: ${error.error.message}`;
        } else {
          // Server-side error
          errorMessage = `Server-side error: ${error.status} - ${error.statusText}\nMessage: ${error.message}`;
        }

        if (error && error.status && error.status == 401) {
          this.router.navigate(['/login']);
          return throwError(() => undefined);
        }

        console.error(errorMessage); // Log the error to the console

        // You can add more logic here, such as:
        // - Showing a notification or toast message to the user
        // - Redirecting to a login or error page for specific status codes (e.g., 401)
        // - Triggering a shared service to notify other parts of the app

        // Re-throw the error to let the component's catchError handle it
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}

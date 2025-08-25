import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RxjsService {
    // 1. The private source of truth (BehaviorSubject holds the current state)
  private readonly _isLoading = new BehaviorSubject<boolean>(false);

  // 2. The public stream (Observable) for components to subscribe to
  // We expose the state as an Observable using the dollar sign ($) convention.
  public readonly isLoading$ = this._isLoading.asObservable(); 

   public setLoading(status: boolean): void {
     this._isLoading.next(status);
  }
}

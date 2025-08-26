import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { IdentityService } from '../../services/identity/identity.service';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Subject, takeUntil } from 'rxjs';
import { RxjsService } from '../../services/rjxs/rxjs.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NzButtonModule, NzFormModule, NzInputModule, NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit, OnDestroy {

  loginForm: FormGroup = {} as any;

  isLoading: boolean = false;

  destroy$: Subject<void> = new Subject<void>();
  /**
   *
   */
  constructor(private fb: FormBuilder, private identityService: IdentityService, 
    private router: Router, private cookieService: CookieService, private message: NzMessageService,
    private rxjsService: RxjsService
  ) {
    this.loginForm = this.fb.group({
      username: ['', [
        Validators.required,
      ]],
      password: ['', [
        Validators.required,
      ]]
    });
    
  }

  ngOnInit(): void {
        this.rxjsService.isLoading$
              .pipe(
            // Complete the stream when the destroy$ subject emits
            takeUntil(this.destroy$) 
          ).subscribe({
          next: (loading) => {
            this.isLoading = loading;
          }
        });

        this.cookieService.delete('rp-auth');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  submit() {
    if (this.loginForm.invalid) {
      return;
    }

    this.rxjsService.setLoading(true);
    this.identityService.login(this.username?.value, this.password?.value).subscribe({
      next: (res) => {
        this.rxjsService.setLoading(false);
        this.cookieService.set('rp-auth', 'true'); // Example, set a cookie to indicate authentication
          this.router.navigate(['/servers']); 
      },
      error: (err) => {
        this.rxjsService.setLoading(false);
        this.message.create('error', `${err.error?.message}`);
        console.error(err);
      }
    });
  }

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }

}

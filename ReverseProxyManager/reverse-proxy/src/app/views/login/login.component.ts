import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { IdentityService } from '../../services/identity/identity.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NzButtonModule, NzFormModule, NzInputModule, NgIf],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginForm: FormGroup = {} as any;
  /**
   *
   */
  constructor(private fb: FormBuilder, private identityService: IdentityService) {
    this.loginForm = this.fb.group({
      username: ['', [
        Validators.required,
      ]],
      password: ['', [
        Validators.required,
      ]]
    });
    
  }

  submit() {
  throw new Error('Method not implemented.');
  }

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }

}

import { Component, OnDestroy, OnInit } from '@angular/core';
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { Router, RouterOutlet } from "@angular/router";
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapHddRack, bootstrapKey, bootstrapInfoCircle, bootstrapGear  } from '@ng-icons/bootstrap-icons';
import { IdentityService } from '../../services/identity/identity.service';
import { ManagementService } from '../../services/management/management.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NgIf } from '@angular/common';
import { RxjsService } from '../../services/rjxs/rxjs.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-base',
  standalone: true,
  imports: [NzLayoutModule, RouterOutlet, NzMenuModule, NgIcon, NzSpinModule, NgIf],
  providers: [provideIcons({ bootstrapHddRack, bootstrapKey, bootstrapInfoCircle, bootstrapGear })],
  templateUrl: './base.component.html',
  styleUrl: './base.component.scss'
})
export class BaseComponent implements OnInit, OnDestroy {
isLoading: boolean = false;
  destroy$: Subject<void> = new Subject<void>();
  /**
   *
   */
  constructor(private identityService: IdentityService, private router: Router, 
    private managementService: ManagementService, private message: NzMessageService,
    private rxjsService: RxjsService
  ) {
    
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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
  }

logout() {
  this.rxjsService.setLoading(true);
    this.identityService.logout().subscribe({
      next: (res) => {
        this.rxjsService.setLoading(false);
        this.router.navigate(['/login']); 
      },
      error: (err) => {
        this.rxjsService.setLoading(false);
        this.message.create('error', `${err.error?.message}`);
        console.error('Logout failed', err.error?.message || err);
      }
    });
}

recreateConfig() {
  this.rxjsService.setLoading(true);
  this.managementService.applyNewConfig().subscribe({
    next: (res) => {
      this.message.create('success', `Nginx config successfully recreated and server restarted!`);
      console.log('Config recreation triggered successfully');
    },
    error: (err) => {
      this.message.create('error', `${err.error?.message}`);
      console.error('Config recreation failed', err.error?.message || err);
    },
    complete: () => {
      this.rxjsService.setLoading(false);
    }
  });
}

}

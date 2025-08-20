import { Component } from '@angular/core';
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { RouterOutlet } from "@angular/router";
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapHddRack, bootstrapKey, bootstrapInfoCircle, bootstrapGear  } from '@ng-icons/bootstrap-icons';

@Component({
  selector: 'app-base',
  standalone: true,
  imports: [NzLayoutModule, RouterOutlet, NzMenuModule, NgIcon],
  providers: [provideIcons({ bootstrapHddRack, bootstrapKey, bootstrapInfoCircle, bootstrapGear })],
  templateUrl: './base.component.html',
  styleUrl: './base.component.scss'
})
export class BaseComponent {
recreateConfig() {
throw new Error('Method not implemented.');
}

}

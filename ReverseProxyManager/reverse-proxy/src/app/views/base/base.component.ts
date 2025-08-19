import { Component } from '@angular/core';
import { NzLayoutModule } from "ng-zorro-antd/layout";
import { RouterOutlet } from "@angular/router";
import { NzMenuModule } from 'ng-zorro-antd/menu';

@Component({
  selector: 'app-base',
  standalone: true,
  imports: [NzLayoutModule, RouterOutlet, NzMenuModule],
  templateUrl: './base.component.html',
  styleUrl: './base.component.scss'
})
export class BaseComponent {
recreateConfig() {
throw new Error('Method not implemented.');
}

}

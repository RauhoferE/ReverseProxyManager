import { Component } from '@angular/core';
import { NzModalModule } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-cert-edit',
  standalone: true,
  imports: [NzModalModule],
  templateUrl: './cert-edit.component.html',
  styleUrl: './cert-edit.component.scss'
})
export class CertEditComponent {

isVisible: boolean = true;

/**
 *
 */
constructor() {
  
  
}

handleOk() {
throw new Error('Method not implemented.');
}
handleCancel() {
throw new Error('Method not implemented.');
}

}

import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { bootstrapGear, bootstrapHddRack, bootstrapKey, bootstrapWifi } from '@ng-icons/bootstrap-icons';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NZ_MODAL_DATA, NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';
import { IdNameDto } from '../../models/certificateModels';
import { NzSelectModule } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-server-edit',
  standalone: true,
  imports: [NzModalModule, FormsModule, NzFormModule, NzInputModule, ReactiveFormsModule, NzButtonModule, 
      NgIcon, NzSelectModule],
  providers:[provideIcons({bootstrapHddRack, bootstrapWifi,
    bootstrapGear
   })],
  templateUrl: './server-edit.component.html',
  styleUrl: './server-edit.component.scss'
})
export class ServerEditComponent implements OnInit {
serverForm: FormGroup = {} as any;
certificateId: number = -1;
activeCerts: IdNameDto[] = [
  { id: -1, name: 'No Certificate' },
  { id: 1, name: 'Example Certificate' } // Example data, replace with actual certificate data
];
submitButton: string = 'Change';
/**
 *
 */
constructor(private fb: FormBuilder, private modal: NzModalRef, @Inject(NZ_MODAL_DATA) public data: any) {

  this.submitButton = data['submitName'] || 'Change';
      this.serverForm = this.fb.group({
        name: [data['name'] || '', [
          Validators.required,Validators.maxLength(100),
          Validators.minLength(1)
        ]],
        active: [data['active']],
        target: [data['target']|| '', [
          Validators.required,,Validators.maxLength(250),
          Validators.minLength(1)
        ]],
        port: [data['port']|| '', [
          Validators.required,,Validators.max(65536),
          Validators.min(0)
        ]],
        usesHttp: [data['usesHttp']],
        redirectsToHttps: [data['redirectsToHttps']],
        rawSettings: [data['rawSettings']|| ''],
        certificateId: [data['certificateId']],
      });
  
}

  ngOnInit(): void {
    
  }

handleCancel() {
  this.modal.destroy(null);
}

submit() {
  this.modal.destroy(this.serverForm.value);
}

get name() {
    return this.serverForm.get('name');
  }

  get active() {
    return this.serverForm.get('active');
  }

  get target() {
    return this.serverForm.get('target');
  }

  get port() {
    return this.serverForm.get('port');
  }

  get usesHttp() {
    return this.serverForm.get('usesHttp');
  }
  get redirectsToHttps() {
    return this.serverForm.get('redirectsToHttps');
  }
  get rawSettings() {
    return this.serverForm.get('rawSettings');
  }
}

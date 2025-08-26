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
import { httpsCertificateValidator, rawSettingsNumberValidator, rawSettingsStringValidator, redirectsToHttpsValidator, usesHttpOrHttpsValidator } from '../../validators/form-validators';
import { NgIf } from '@angular/common';
import { CertificateService } from '../../services/certificate/certificate.service';
import { RxjsService } from '../../services/rjxs/rxjs.service';
import { Subject, takeUntil } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSpinModule } from 'ng-zorro-antd/spin';

@Component({
  selector: 'app-server-edit',
  standalone: true,
  imports: [NzModalModule, FormsModule, NzFormModule, NzInputModule, ReactiveFormsModule, NzButtonModule,
    NgIcon, NzSelectModule, NgIf, NzSpinModule],
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
  //{ id: 1, name: 'Example Certificate' } // Example data, replace with actual certificate data
];
submitButton: string = 'Change';

isLoadingCertificates: boolean = false;

destroy$: Subject<void> = new Subject<void>();
/**
 *
 */
constructor(private fb: FormBuilder, private modal: NzModalRef, @Inject(NZ_MODAL_DATA) public data: any,
private certificateService: CertificateService,
private message: NzMessageService) {

  this.submitButton = data['submitName'] || 'Change';
      this.serverForm = this.fb.group({
        name: [data['name'] || '', [
          
          // Validators.required,Validators.maxLength(100),
          // Validators.minLength(1)
        ]],
        active: [data['active'] || false],
        target: [data['target']|| '', [
          //rawSettingsStringValidator(1, 250),
          // Validators.required,,Validators.maxLength(250),
          // Validators.minLength(1)
        ]],
        targetPort: [data['port']|| '', [
          //rawSettingsNumberValidator(0, 65536),
          // Validators.required,,Validators.max(65536),
          // Validators.min(0)
        ]],
        usesHttp: [data['usesHttp']|| false],
        redirectsToHttps: [data['redirectsToHttps']|| false],
        rawSettings: [data['rawSettings']|| ''],
        certificateId: [data['certificateId'] || -1]
      },
    {
      validators: [
        redirectsToHttpsValidator(),
        usesHttpOrHttpsValidator(),
        httpsCertificateValidator(),
        rawSettingsStringValidator(1, 100, 'name'),
        rawSettingsStringValidator(1, 250, 'target'),
        rawSettingsNumberValidator(0, 65536, 'targetPort')
      ]
    });
  
}

  async ngOnInit() {
        await this.getActiveCertificates();
  }

handleCancel() {
  this.modal.destroy(null);
}

async getActiveCertificates() {
  this.isLoadingCertificates = true;
  this.certificateService.getActiveCertificates().subscribe({
    next: (res) => {
      this.isLoadingCertificates = false;
      this.activeCerts = [{ id: -1, name: 'No Certificate' }, ...res];
      console.log('Certificates loaded successfully', res);
    },
    error: (err) => {
      this.isLoadingCertificates = false;
      this.message.error('Failed to load certificates');
      console.error('Failed to load certificates', err);
    }});
  }

submit() {
  this.serverForm.updateValueAndValidity();
  if (this.serverForm.invalid) {
    return;
  }

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

  get targetPort() {
    return this.serverForm.get('targetPort');
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

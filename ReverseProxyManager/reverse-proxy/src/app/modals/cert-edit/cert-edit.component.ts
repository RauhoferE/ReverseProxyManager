import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule } from '@angular/forms';
import { bootstrapCheckCircle, bootstrapKey } from '@ng-icons/bootstrap-icons';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NZ_MODAL_DATA, NzModalModule, NzModalRef } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-cert-edit',
  standalone: true,
  imports: [NzModalModule, FormsModule, NzFormModule, NzInputModule, ReactiveFormsModule, NzButtonModule, 
    NgIcon
  ],
  providers:[provideIcons({bootstrapKey })],
  templateUrl: './cert-edit.component.html',
  styleUrl: './cert-edit.component.scss'
})
export class CertEditComponent implements OnInit {

certForm: FormGroup = {} as any;

/**
 *
 */
constructor(private fb: FormBuilder, private modal: NzModalRef, @Inject(NZ_MODAL_DATA) public data: any) {
      this.certForm = this.fb.group({
        name: [data['certName'], [
          Validators.required,
        ]]
      });
  
}

  ngOnInit(): void {
  }

handleCancel() {
  this.modal.destroy(null);
}

submit() {
  this.modal.destroy(this.name?.value);
}

get name() {
    return this.certForm.get('name');
  }

}

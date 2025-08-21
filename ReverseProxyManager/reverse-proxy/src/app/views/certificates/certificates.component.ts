import { Component } from '@angular/core';
import { NzTableModule, NzTableQueryParams, NzTableSortOrder } from 'ng-zorro-antd/table';
import { CertificateDto } from '../../models/certificateModels';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconDirective, NzIconModule, provideNzIcons } from 'ng-zorro-antd/icon';
import { CommonModule } from '@angular/common';
import dayjs from 'dayjs';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapX, bootstrapCheckCircle, bootstrapBoxArrowDown  } from '@ng-icons/bootstrap-icons';
import { CertEditComponent } from "../../modals/cert-edit/cert-edit.component";
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-certificates',
  standalone: true,
  imports: [NzTableModule, NzButtonModule, NzIconModule, CommonModule, NgIcon, CertEditComponent, NzModalModule],
  providers:[provideIcons({bootstrapX, bootstrapCheckCircle, bootstrapBoxArrowDown })],
  templateUrl: './certificates.component.html',
  styleUrl: './certificates.component.scss'
})
export class CertificatesComponent {

  certificates: CertificateDto[] = [];
  loading: boolean = false;

  /**
   *
   */
  constructor(private modalService: NzModalService) {
    
    
  }

  onQueryParamsChange($event: NzTableQueryParams) {
    const { pageSize, pageIndex, sort, filter } = $event;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
}

  rescanCertificates() {
   throw new Error("Method not implemented.");  

  }

  openEditCertModal(id: number, certName: string) {
   var modalRef = this.modalService.create({
    nzTitle: 'Edit Certificate',    
      nzContent: CertEditComponent,
    nzData: { certName: certName },
    nzFooter: null,});

    modalRef.afterClose.subscribe((result: string) => {
      console.log("Modal closed with result: ", result);
    });
  }

  updateCertName($event: string) {
    console.log("Certificate name updated to: ", $event);
  }

  getDate(arg0: Date) {
    dayjs(arg0).format('YYYY-MM-DD HH:mm:ss');
}

}

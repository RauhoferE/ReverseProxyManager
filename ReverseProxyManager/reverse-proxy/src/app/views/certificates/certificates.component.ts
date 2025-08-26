import { Component, OnDestroy, OnInit } from '@angular/core';
import { NzTableModule, NzTableQueryParams, NzTableSortOrder } from 'ng-zorro-antd/table';
import { CertificateDto } from '../../models/certificateModels';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconDirective, NzIconModule, provideNzIcons } from 'ng-zorro-antd/icon';
import { CommonModule } from '@angular/common';
import dayjs from 'dayjs';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapX, bootstrapCheckCircle, bootstrapBoxArrowDown, bootstrapPencilSquare, bootstrapTrash, bootstrapExclamationOctagon  } from '@ng-icons/bootstrap-icons';
import { CertEditComponent } from "../../modals/cert-edit/cert-edit.component";
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { CertificateService } from '../../services/certificate/certificate.service';
import { RxjsService } from '../../services/rjxs/rxjs.service';
import { Observable, Subject, takeUntil } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';

@Component({
  selector: 'app-certificates',
  standalone: true,
  imports: [NzTableModule, NzButtonModule, NzIconModule, CommonModule, NgIcon, NzModalModule, NzPopconfirmModule],
  providers:[provideIcons({bootstrapX, bootstrapCheckCircle, bootstrapBoxArrowDown, bootstrapPencilSquare, bootstrapTrash, bootstrapExclamationOctagon })],
  templateUrl: './certificates.component.html',
  styleUrl: './certificates.component.scss'
})
export class CertificatesComponent implements OnInit, OnDestroy {


  certificates: CertificateDto[] = [];
  isLoading: boolean = false;
  destroy$: Subject<void> = new Subject<void>();
  filterInput: string = '';

  /**
   *
   */
  constructor(private modalService: NzModalService, private certificateService: CertificateService,
    private rxjsService: RxjsService, private message: NzMessageService
  ) {
    
    
    
  }

  async ngOnInit(): Promise<void> {
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

  ngOnDestroy(): void {
        this.destroy$.next();
    this.destroy$.complete();
  }

  async onQueryParamsChange($event: NzTableQueryParams) {
    const { pageSize, pageIndex, sort, filter } = $event;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
    await this.getAllCertificates(this.filterInput, sortField || 'name', sortOrder == null ? true : sortOrder == 'ascend');
}

deleteCertificate(id: number, name: string) {
  this.isLoading = true;
  this.certificateService.deleteCertificate(id).subscribe({
    next: async (res) => {
      this.isLoading = false;
      this.message.success(`Certificate ${name} deleted successfully`);
      this.certificates = this.certificates.filter(x => x.id != id);
    },
    error: (err) => {
      this.isLoading = false;
      this.message.error(err.error?.message || 'Failed to delete certificate');
      console.error('Failed to delete certificate', err.error?.message || err);
    }});
}

async getAllCertificates(filter: string, sortAfter: string, asc: boolean) {
  console.log(`Fetching certificates with filter: ${filter}, sortAfter: ${sortAfter}, asc: ${asc}`);
    this.rxjsService.setLoading(true);
   this.certificateService.getAllCertificates(filter, sortAfter, asc).subscribe({
    next: (res) => {
      this.rxjsService.setLoading(false);
      this.certificates = res;
      console.log('Certificates loaded successfully', res);
    },
    error: (err) => {
      this.rxjsService.setLoading(false);
      this.message.error(err.error?.message || 'Failed to load certificates');
      console.error('Failed to load certificates', err.error?.message || err);
    }});
}

  rescanCertificates() {
    this.rxjsService.setLoading(true);
   this.certificateService.rescanCertificates().subscribe({
    next: async (res) => {
      this.rxjsService.setLoading(false);
      this.message.success('Certificates rescanned successfully');
      await this.getAllCertificates(this.filterInput, 'name', true);
      // this.loading = false;
      // this.certificates = res;
      console.log('Certificates rescanned successfully', res);
    },
    error: (err) => {
      this.rxjsService.setLoading(false);
      this.message.error(err.error?.message || 'Failed to rescan certificates');
      //this.loading = false;
      console.error('Failed to rescan certificates', err.error?.message || err);
    }});

  }

  openEditCertModal(id: number, certName: string) {
   var modalRef = this.modalService.create({
    nzTitle: 'Edit Certificate',    
      nzContent: CertEditComponent,
    nzData: { certName: certName },
    nzFooter: null,});

    modalRef.afterClose.subscribe(async (result: string) => {
      if (result) {
        await this.updateCertName(id, result);
      }
    });
  }

  updateCertName(id: number, $event: string) {
    this.certificateService.updateCertificateName(id, $event).subscribe({
      next: async (res) => {
        this.message.success('Certificate name updated successfully');
        await this.getAllCertificates(this.filterInput, 'name', true);
        console.log('Certificate name updated successfully', res);
      },
      error: (err) => {
        this.message.error(err.error?.message || 'Failed to update certificate name');
        console.error('Failed to update certificate name', err.error?.message || err);
      }});
  }

  getDate(arg0: Date) {
    return dayjs(arg0).format('YYYY-MM-DD HH:mm:ss');
}

}

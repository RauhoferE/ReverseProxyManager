import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import dayjs from 'dayjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule, NzTableQueryParams, NzTableSortOrder } from 'ng-zorro-antd/table';
import { AddServerDto, ServerDto } from '../../models/serverModels';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapCloudCheck, bootstrapKey, bootstrapBodyText, bootstrapGear,
  bootstrapGlobe, bootstrapShuffle,
  bootstrapHdd,
  bootstrapHddRack,
  bootstrapPencilSquare,
  bootstrapTrash,
  bootstrapExclamationOctagon,
  bootstrapSearch,
  bootstrapBoxArrowDown,
  bootstrapX,
  bootstrapCheckCircle
  } from '@ng-icons/bootstrap-icons';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { ServerEditComponent } from '../../modals/server-edit/server-edit.component';
import { ManagementService } from '../../services/management/management.service';
import { Subject, takeUntil } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { RxjsService } from '../../services/rjxs/rxjs.service';
import { CertificateService } from '../../services/certificate/certificate.service';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-servers',
  standalone: true,
  imports: [NzTableModule, NzButtonModule, NzIconModule, CommonModule, NgIcon, NzModalModule, NzPopconfirmModule, FormsModule, NzInputModule],
  providers: [provideIcons({ bootstrapCloudCheck, bootstrapKey, bootstrapBodyText, bootstrapGear,
    bootstrapGlobe, bootstrapShuffle, bootstrapHddRack, bootstrapBoxArrowDown, bootstrapPencilSquare, bootstrapTrash, bootstrapExclamationOctagon, bootstrapSearch, bootstrapX,
    bootstrapCheckCircle
   })],
  templateUrl: './servers.component.html',
  styleUrl: './servers.component.scss'
})
export class ServersComponent implements OnInit, OnDestroy {


servers: ServerDto[] = [];
isLoading: boolean = false;
destroy$: Subject<void> = new Subject<void>();
filterInput: string = '';
private sortField: string = 'name';
private sortOrder: NzTableSortOrder = 'ascend';

/**
 *
 */
constructor(private modalService: NzModalService, private managementService: ManagementService, private messageService: NzMessageService,
  private rxjsService: RxjsService
) {
  
  
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

  ngOnDestroy(): void {
            this.destroy$.next();
    this.destroy$.complete();
  }

getDate(arg0: Date) {
    return dayjs(arg0).format('YYYY-MM-DD HH:mm:ss');
}

deleteServer(id: number,name: string) {
  this.rxjsService.setLoading(true);
  this.managementService.deleteServer(id).subscribe({
    next: async (res) => {
      this.rxjsService.setLoading(false);
      this.messageService.success(`Entry ${name} deleted successfully and configuration written to disk.`);
      await this.getAllServers(this.filterInput, this.sortField, this.sortOrder == null ? true : this.sortOrder == 'ascend');
      console.log('Server deleted successfully', res);
    },
    error: (err) => {
      this.rxjsService.setLoading(false);
      this.messageService.error(err.error?.message || `Failed to delete entry ${name}`);
      console.error('Failed to delete server', err.error?.message || err);
    }});
}

async resetFilter() {
  this.filterInput = '';
  await this.getAllServers(this.filterInput, this.sortField, this.sortOrder == null ? true : this.sortOrder == 'ascend');
}

async applyFilter() {
  await this.getAllServers(this.filterInput, this.sortField, this.sortOrder == null ? true : this.sortOrder == 'ascend');
}

async onQueryParamsChange($event: NzTableQueryParams) {
    const { pageSize, pageIndex, sort, filter } = $event;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
    this.sortField = sortField || 'name';
    this.sortOrder = sortOrder || 'ascend';
    await this.getAllServers(this.filterInput, this.sortField, this.sortOrder == null ? true : this.sortOrder == 'ascend');
}

openChangeServerModal(server: ServerDto){
   var modalRef = this.modalService.create({
    nzTitle: 'Edit Server',    
      nzContent: ServerEditComponent,
    nzData: { 
      submitName: 'Change',
      name: server.name,
      active: server.active,
      target: server.target,
      port: server.targetPort,
      usesHttp: server.usesHttp,
      redirectsToHttps: server.redirectsToHttps,
      rawSettings: server.rawSettings,
      certificateId: server.certificate ? server.certificate.id : -1
    
    },
    nzFooter: null,});

    modalRef.afterClose.subscribe(async (result: AddServerDto) => {
      if (result) {
        await this.updateServer(server.id, result);
      }
    });
}

openAddServerModal(){
   var modalRef = this.modalService.create({
    nzTitle: 'Add Server',    
      nzContent: ServerEditComponent,
    nzData: { 
      submitName: 'Add',
      certificateId: -1
    
    },
    nzFooter: null,});

    modalRef.afterClose.subscribe(async (result: AddServerDto) => {
      console.log("Modal closed with result: ", result);
      if (result) {
        await this.addServer(result);
      }
    });
}

async updateServer(id: number, server: AddServerDto) {
  this.rxjsService.setLoading(true);
  this.managementService.updateServer(id, server).subscribe({
    next: async (res) => {
      this.rxjsService.setLoading(false);
      this.messageService.success('Server updated successfully');
      await this.getAllServers(this.filterInput, this.sortField, this.sortOrder == null ? true : this.sortOrder == 'ascend');
      console.log('Server updated successfully', res);
    },
    error: (err) => {
      this.rxjsService.setLoading(false);
      this.messageService.error(err.error?.message || 'Failed to update server');
      console.error('Failed to update server', err.error?.message || err);
    }});
}

async addServer(server: AddServerDto) {
  this.rxjsService.setLoading(true);
  this.managementService.addServer(server).subscribe({
    next: async (res) => {
      this.rxjsService.setLoading(false);
      this.messageService.success('Server added successfully');
      await this.getAllServers(this.filterInput, this.sortField, this.sortOrder == null ? true : this.sortOrder == 'ascend');
      console.log('Server added successfully', res);
    },
    error: (err) => {
      this.rxjsService.setLoading(false);
      this.messageService.error(err.error?.message || 'Failed to add server');
      console.error('Failed to add server', err.error?.message || err);
    }});
}

async getAllServers(filter: string, sortAfter: string, asc: boolean) {
  this.rxjsService.setLoading(true);
 this.managementService.getServers(filter, sortAfter, asc).subscribe({
  next: (res) => {
    this.rxjsService.setLoading(false);
    this.servers = res;
    console.log('Servers loaded successfully', res);
  },
  error: (err) => {
    this.rxjsService.setLoading(false);
    this.messageService.error(err.error?.message || 'Failed to load servers');
    console.error('Failed to load servers', err.error?.message || err);
  }});
}

recreateConfig() {
  this.rxjsService.setLoading(true);
 this.managementService.applyNewConfig().subscribe({
  next: async (res) => {
    this.rxjsService.setLoading(false);
    this.messageService.success('Configuration recreated successfully');
    console.log('Configuration recreated successfully', res);
  },
  error: (err) => {
    this.rxjsService.setLoading(false);
    this.messageService.error(err.error?.message || 'Failed to recreate config');
    console.error('Failed to recreate config', err.error?.message || err);
  }});
}

}

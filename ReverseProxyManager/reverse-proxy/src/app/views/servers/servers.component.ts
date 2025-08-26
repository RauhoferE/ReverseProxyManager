import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import dayjs from 'dayjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { AddServerDto, ServerDto } from '../../models/serverModels';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapCloudCheck, bootstrapKey, bootstrapBodyText, bootstrapGear,
  bootstrapGlobe, bootstrapShuffle,
  bootstrapHdd,
  bootstrapHddRack
  } from '@ng-icons/bootstrap-icons';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';
import { ServerEditComponent } from '../../modals/server-edit/server-edit.component';
import { ManagementService } from '../../services/management/management.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-servers',
  standalone: true,
  imports: [NzTableModule, NzButtonModule, NzIconModule, CommonModule, NgIcon, ServerEditComponent, NzModalModule],
  providers: [provideIcons({ bootstrapCloudCheck, bootstrapKey, bootstrapBodyText, bootstrapGear,
    bootstrapGlobe, bootstrapShuffle, bootstrapHddRack
   })],
  templateUrl: './servers.component.html',
  styleUrl: './servers.component.scss'
})
export class ServersComponent implements OnInit, OnDestroy {

servers: ServerDto[] = [];
loading: boolean = false;
destroy$: Subject<void> = new Subject<void>();

/**
 *
 */
constructor(private modalService: NzModalService, private managementService: ManagementService) {
  
  
}

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

getDate(arg0: Date) {
    dayjs(arg0).format('YYYY-MM-DD HH:mm:ss');
}

onQueryParamsChange($event: NzTableQueryParams) {
    const { pageSize, pageIndex, sort, filter } = $event;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
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

    modalRef.afterClose.subscribe((result: AddServerDto) => {
      console.log("Modal closed with result: ", result);
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

    modalRef.afterClose.subscribe((result: AddServerDto) => {
      console.log("Modal closed with result: ", result);
    });
}

recreateConfig() {
throw new Error("Method not implemented.");
}

}

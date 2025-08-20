import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import dayjs from 'dayjs';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { ServerDto } from '../../models/serverModels';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {bootstrapCloudCheck, bootstrapKey, bootstrapBodyText, bootstrapGear,
  bootstrapGlobe, bootstrapShuffle
  } from '@ng-icons/bootstrap-icons';

@Component({
  selector: 'app-servers',
  standalone: true,
  imports: [NzTableModule, NzButtonModule, NzIconModule, CommonModule, NgIcon],
  providers: [provideIcons({ bootstrapCloudCheck, bootstrapKey, bootstrapBodyText, bootstrapGear,
    bootstrapGlobe, bootstrapShuffle
   })],
  templateUrl: './servers.component.html',
  styleUrl: './servers.component.scss'
})
export class ServersComponent {

servers: ServerDto[] = [];
loading: boolean = false;

/**
 *
 */
constructor() {
  
  
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

recreateConfig() {
throw new Error('Method not implemented.');
}

}

import { Component } from '@angular/core';
import { NzTableModule, NzTableQueryParams, NzTableSortOrder } from 'ng-zorro-antd/table';
import { CertificateDto } from '../../models/certificateModels';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconDirective, NzIconModule, provideNzIcons } from 'ng-zorro-antd/icon';
import { CheckOutline } from '@ant-design/icons-angular/icons';
import { CommonModule } from '@angular/common';
import dayjs from 'dayjs';
import { NzFlexModule } from 'ng-zorro-antd/flex';
import { NzSpaceModule } from 'ng-zorro-antd/space';

@Component({
  selector: 'app-certificates',
  standalone: true,
  imports: [NzTableModule, NzButtonModule, NzIconModule, CommonModule, NzFlexModule, NzSpaceModule],
  // providers:[provideNzIcons(CheckOutline)],
  templateUrl: './certificates.component.html',
  styleUrl: './certificates.component.scss'
})
export class CertificatesComponent {



  certificates: CertificateDto[] = [];
  loading: boolean = false;

  /**
   *
   */
  constructor() {
    
    
  }

  onQueryParamsChange($event: NzTableQueryParams) {
    const { pageSize, pageIndex, sort, filter } = $event;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = (currentSort && currentSort.key) || null;
    const sortOrder = (currentSort && currentSort.value) || null;
}

  rescanCertificates() {
  throw new Error('Method not implemented.');
  }

  getDate(arg0: Date) {
    dayjs(arg0).format('YYYY-MM-DD HH:mm:ss');
}

}

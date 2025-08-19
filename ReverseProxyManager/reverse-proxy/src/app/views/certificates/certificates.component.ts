import { Component } from '@angular/core';
import { NzTableModule, NzTableSortOrder } from 'ng-zorro-antd/table';
import { CertificateDto } from '../../models/certificateModels';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-certificates',
  standalone: true,
  imports: [NzTableModule, NzButtonModule],
  templateUrl: './certificates.component.html',
  styleUrl: './certificates.component.scss'
})
export class CertificatesComponent {
  certificates: CertificateDto[] = [];
  sortDirections: any[] = ['ascend', 'descend', null];
  column: any;
  name_sortOrder: NzTableSortOrder = null;
  issuer_sortOrder: NzTableSortOrder = null;
  validNotBefore_sortOrder: NzTableSortOrder = null;
  validNotAfter_sortOrder: NzTableSortOrder = null;
  lastUpdated_sortOrder: NzTableSortOrder = null;
  fileAttached_sortOrder: NzTableSortOrder = null;

  /**
   *
   */
  constructor() {
    
    
  }

}

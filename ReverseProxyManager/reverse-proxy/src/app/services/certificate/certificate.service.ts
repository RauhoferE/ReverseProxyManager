import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CertificateDto, IdNameDto } from '../../models/certificateModels';

@Injectable({
  providedIn: 'root'
})
export class CertificateService {

  constructor( private http:HttpClient) { }

  public rescanCertificates(){
    return this.http.get<void>(`${environment.appUrl}/api/v1/certificate/rescan`, {withCredentials: true});
  }

  public deleteCertificate(id:number){
    return this.http.delete<void>(`${environment.appUrl}/api/v1/certificate/${id}`, {withCredentials: true});
  }

  public updateCertificateName(id:number, name:string){
    return this.http.put<void>(`${environment.appUrl}/api/v1/certificate/${id}`, {name:name}, {withCredentials: true});
  }

  public getAllCertificates(filter: string, sortAfter: string, asc: boolean){
    var filterString = `filter=${filter}&`
    if (!filter) {
      filterString = '';
    }

    return this.http.get<CertificateDto[]>(`${environment.appUrl}/api/v1/certificate?${filterString}sortAfter=${sortAfter}&asc=${asc}`, {withCredentials: true});
  }

  public getActiveCertificates(){
    return this.http.get<IdNameDto[]>(`${environment.appUrl}/api/v1/certificate/active`, {withCredentials: true});
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AddServerDto, ServerDto } from '../../models/serverModels';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {

  constructor(private http:HttpClient) { }

  public getServers(filter: string, sortAfter: string, asc: boolean){
    var filterString = `filter=${filter}&`
    if (!filter) {
      filterString = '';
    }

    return this.http.get<ServerDto[]>(`${environment.appUrl}/api/v1/management?${filterString}sortAfter=${sortAfter}&asc=${asc}`, {withCredentials: true});
  }

  public addServer(dto: AddServerDto){
    var newDto: any = {
      active: dto.active,
      certificateId: dto.certificateId,
      name: dto.name,
      rawSettings: dto.rawSettings,
      redirectsToHttps: dto.rawSettings == '' ? dto.redirectsToHttps : false,
      usesHttp: dto.rawSettings == '' ? dto.usesHttp : false,
      target: dto.rawSettings == '' ? dto.target : null,
      targetPort: dto.rawSettings == '' ? dto.targetPort : null,
    }
    return this.http.post<void>(`${environment.appUrl}/api/v1/management`, newDto, {withCredentials: true});
  }

  public updateServer(id:number, dto: AddServerDto){
        var newDto: any = {
      active: dto.active,
      certificateId: dto.certificateId,
      name: dto.name,
      rawSettings: dto.rawSettings,
      redirectsToHttps: dto.rawSettings == '' ? dto.redirectsToHttps : false,
      usesHttp: dto.rawSettings == '' ? dto.usesHttp : false,
      target: dto.rawSettings == '' ? dto.target : null,
      targetPort: dto.rawSettings == '' ? dto.targetPort : null,
    }
    return this.http.put<void>(`${environment.appUrl}/api/v1/management/${id}`, newDto, {withCredentials: true});
  }

  public deleteServer(id:number){
    return this.http.delete<void>(`${environment.appUrl}/api/v1/management/${id}`, {withCredentials: true});
  }

  public applyNewConfig(){
    return this.http.get<void>(`${environment.appUrl}/api/v1/management/apply-config`, {withCredentials: true});
  }
}

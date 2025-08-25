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
    return this.http.get<ServerDto[]>(`${environment.appUrl}/api/v1/management?filter=${filter}&sort=${sortAfter}&asc=${asc}`, {withCredentials: true});
  }

  public addServer(dto: AddServerDto){
    return this.http.post<void>(`${environment.appUrl}/api/v1/management`, dto, {withCredentials: true});
  }

  public updateServer(id:number, dto: AddServerDto){
    return this.http.put<void>(`${environment.appUrl}/api/v1/management/${id}`, dto, {withCredentials: true});
  }

  public deleteServer(id:number){
    return this.http.delete<void>(`${environment.appUrl}/api/v1/management/${id}`, {withCredentials: true});
  }

  public applyNewConfig(){
    return this.http.get<void>(`${environment.appUrl}/api/v1/management/apply-config`, {withCredentials: true});
  }
}

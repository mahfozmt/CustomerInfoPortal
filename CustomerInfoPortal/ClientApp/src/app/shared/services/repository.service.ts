import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  // public getData = (route: string) => {
  //   return this.http.get(this.createCompleteRoute(route, this.envUrl.urlAddress));
  // }
 
  // public create = (route: string, body) => {
  //   return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), body, this.generateHeaders());
  // }
 
  // public update = (route: string, body) => {
  //   return this.http.put(this.createCompleteRoute(route, this.envUrl.urlAddress), body, this.generateHeaders());
  // }
 
  // public delete = (route: string) => {
  //   return this.http.delete(this.createCompleteRoute(route, this.envUrl.urlAddress));
  // }
 

  getData<T>(route: string, parmams: any = null) {
    if (!parmams) {
      return this.http.get<T>(this.createCompleteRoute(route, this.envUrl.urlAddress));
    } else {
      return this.http.get<T>(this.createCompleteRoute(route+parmams, this.envUrl.urlAddress));
    }
  }

  createWithFile<T>(route: string, formBody: FormData) {
    if (formBody) {
      return this.http.post<T>(this.createCompleteRoute(route, this.envUrl.urlAddress), formBody, this.generateHeaders());
    }
  }

  create<T>(route: string, parmams: any = null) {
    if (parmams) {
      return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), parmams, this.generateHeaders());
    }
    else {
      return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), this.generateHeaders());
    }
  }


  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
 
  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
  }


}
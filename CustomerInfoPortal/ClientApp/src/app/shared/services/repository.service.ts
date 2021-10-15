import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  getData<T>(route: string, parmams: any = null) {
    if (!parmams) {
      return this.http.get<T>(this.createCompleteRoute(route, this.envUrl.urlAddress));
    } else {
      return this.http.get<T>(this.createCompleteRoute(route+parmams, this.envUrl.urlAddress));
    }
  }

  create<T>(route: string, parameters: any = null) {
    if (parameters) {
      return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), parameters, this.generateHeaders());
    }
    else {
      return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), this.generateHeaders());
    }
  }

  createWithFile<T>(route: string, formBody: FormData) {
    if (formBody) {
      return this.http.post<T>(this.createCompleteRoute(route, this.envUrl.urlAddress), formBody);
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

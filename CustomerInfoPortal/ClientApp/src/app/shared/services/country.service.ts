import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Country } from 'src/app/Models/Country';
import { ErrorHandlerService } from './error-handler.service';
import { RepositoryService } from './repository.service';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  public result: any;
  errorMessage: string;

  constructor(private repo: RepositoryService, private errorHandler: ErrorHandlerService) { }

  // public getAllCountry() {
  //   this.repo.getData('api/Country/GetCountries')
  //     .subscribe(res => {
  //       this.result = res;
  //     },
  //       (error) => {
  //         this.errorHandler.handleError(error);
  //       this.errorMessage = this.errorHandler.errorMessage;
  //       })
  // }

  getAllCountry(): Observable<Country[]> {
    let route = 'api/Country/GetCountries';
    return this.repo.getData<Country[]>(route);
  }

  saveCountry(countryData: Country) {
    let route = 'api/Country/SaveCountry';
    return this.repo.create<Country[]>(route, countryData);
  }

}

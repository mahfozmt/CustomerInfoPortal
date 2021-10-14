import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from 'src/app/Models/Customer';
import { ErrorHandlerService } from './error-handler.service';
import { RepositoryService } from './repository.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  errorMessage: string;
  result: Object;
  constructor(private repo: RepositoryService, private errorHandler: ErrorHandlerService) { }

 
  getAllCustomers(): Observable<Customer[]> {
    let route = 'api/Customer/GetCustomerList';
    return this.repo.getData<Customer[]>(route);
  }

  CreateCustomer(customer: FormData): Observable<Customer> {
    let route = 'api/Customer/CreateCustomer';
    return this.repo.createWithFile<Customer>(route, customer);
  }

  RemoveCustomer(customer: FormData): Observable<Customer> {
  let route = 'api/Customer/RemoveCustomer';
  return this.repo.createWithFile<Customer>(route, customer);
}

GetCustomerById(id): Observable<Customer> {
  let route = 'api/Customer/GetCustomerById?id='+id;
  return this.repo.getData<Customer>(route);
}   

}







import { Byte } from '@angular/compiler/src/util';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Country } from '../Models/Country';
import { Customer } from '../Models/Customer';
import { CustomerAddress } from '../Models/CustomerAddress';
import { CountryService } from '../shared/services/country.service';
import { CustomerService } from '../shared/services/customer.service';
import { ErrorHandlerService } from '../shared/services/error-handler.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';



declare var $: any;

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {
  public customerForm: FormGroup;
  public customerData: Customer;
  public countries: Country[] = [];
  public customers: Customer[] = [];
  public selectedImage: File;
  public selectedImageName: string;
  public customerPhotoUrl: any = 'https://cdn-icons-png.flaticon.com/512/685/685686.png';
  public selectedImageString: string;
  public customerImageLoaded: boolean = false;
  public pageMode: string = 'new';
  public formSubmited: boolean = false;
  public errorMessage: string;
  ImageExtesion = ["png", "jpeg", "jpg"];
  public avatarImageUrl: string = 'https://cdn-icons-png.flaticon.com/512/685/685686.png';

  @ViewChild('attachedImageInput', {
    static: true
  }) attachedImageInput: ElementRef

  constructor(private countryService: CountryService, private customerService: CustomerService, private frombuilder: FormBuilder, private domSanitizer: DomSanitizer, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.createForm();
    this.GetCustomerList()
    this.GetCountryDropDown();
  }
  GetCountryDropDown() {
    this.countryService.getAllCountry().subscribe(
      (data) => {
        this.countries = data;
      },
      (error) => {
        this.errorHandler.handleError(error);
        this.errorMessage = this.errorHandler.errorMessage;
      });
  }



  createForm() {
    this.customerForm = this.frombuilder.group({
      ID: new FormControl('0'),
      CountryID: new FormControl('0', Validators.required),
      CustomerName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      FatherName: new FormControl('', Validators.maxLength(50)),
      MotherName: new FormControl('', Validators.maxLength(50)),
      MaritalStatus: new FormControl('1'),
      CustomerPhoto: new FormControl(''),
      CustomerAddresses: this.frombuilder.array([
        this.createCustomerAddressFormArrary()
      ])
    });
  }
  createCustomerAddressFormArrary(address: CustomerAddress = new CustomerAddress()) {
    return this.frombuilder.group({
      ID: new FormControl(address.ID),
      CustomerID: new FormControl(address.CustomerID),
      Address: new FormControl(address.Address, [Validators.required, Validators.maxLength(500)])
    });
  }

  GetCustomerList() {
    this.customerService.getAllCustomers().subscribe(
      (data) => {
        this.customers = data;
      },
      (error) => {
        this.errorHandler.handleError(error);
        this.errorMessage = this.errorHandler.errorMessage;
      }
    )
  }

  getCustomerbyId(id) {
    this.pageMode = 'view';
    this.customerService.GetCustomerById(id).subscribe(
      (customerdata) => {
        this.customerForm.patchValue({
          ID: customerdata.ID,
          CountryID: customerdata.CountryID,
          CustomerName: customerdata.CustomerName,
          FatherName: customerdata.FatherName,
          MotherName: customerdata.MotherName,
          MaritalStatus: customerdata.MaritalStatus.toString()
        });
        if (customerdata.CustomerPhoto != null) {
          this.customerPhotoUrl = 'data:image/jpeg;base64,' + customerdata.CustomerPhoto;
          this.customerImageLoaded = true;
        }
        else {
          this.customerImageLoaded = false;
        }
        this.getCustomerAddresses().clear();
        customerdata.CustomerAddresses.forEach((add, i) => {
          this.addCustomerAddresses(add);
        })
        this.customerForm.disable();
        //this.alertWithSuccess("Customer Info loaded Successfully!")
      },
      (error) => {
        this.errorHandler.handleError(error);
        this.errorMessage = this.errorHandler.errorMessage;
        this.alertWithError(this.errorMessage);
      }
    )
  }


  getCustomerAddresses(): FormArray {
    return this.customerForm.get("CustomerAddresses") as FormArray
  }
  addCustomerAddresses(address: CustomerAddress = new CustomerAddress()) {
    const CustomerAddressesForm = this.createCustomerAddressFormArrary(address);
    this.getCustomerAddresses().push(CustomerAddressesForm);
  }

  removeCustomerAddresses(i: number) {
    this.getCustomerAddresses().removeAt(i);
    if (i == 0) {
      this.alertWithError('At Least One Address Need To Provide')
      this.addCustomerAddresses();
    }
  }

  saveCustomer() {
    this.formSubmited = true;
    if (this.customerForm.valid && this.customerForm.get('CountryID').value != 0) {
      this.customerData = this.customerForm.getRawValue();
      if ((this.customerData.ID == null || this.customerData.ID == 0) && !this.customerImageLoaded) { return; }
      let fd = new FormData();
      let id = this.customerData.ID.toString();
      fd.append("ID", this.customerData.ID.toString());
      fd.append("CountryID", this.customerData.CountryID.toString());
      fd.append("CustomerName", this.customerData.CustomerName.toString());
      fd.append("FatherName", this.customerData.FatherName);
      fd.append("MotherName", this.customerData.MotherName);
      fd.append("MaritalStatus", this.customerData.MaritalStatus.toString());
      this.customerData.CustomerAddresses.forEach((item, i) => {
        fd.append(`CustomerAddresses[${i}].ID`, item.ID.toString());
        fd.append(`CustomerAddresses[${i}].CustomerID`, item.CustomerID.toString());
        fd.append(`CustomerAddresses[${i}].Address`, item.Address);
      });
      if (this.selectedImage != null) {
        fd.append("CustomerPhoto", this.selectedImage, this.selectedImage.name);
      }
      this.customerService.CreateCustomer(fd).subscribe(
        (data) => {
          this.alertWithSuccess('Customer Are Saved!!')
          console.log(data);
          this.resetForm();
        },
        (error) => {
          this.errorHandler.handleError(error);
          this.errorMessage = this.errorHandler.errorMessage;
          this.alertWithError(this.errorMessage);
        }
      )
    }
    else {
      this.alertWithError('Please Enter All Required Data')
    }

  }

  removeCustomer() {
    Swal.fire({
      title: 'Are you sure want to remove?',
      text: 'You will not be able to recover this file!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.value) {
        this.customerData = this.customerForm.getRawValue();
        if (this.customerData.ID > 0) {
          let fd = new FormData();
          fd.append("ID", this.customerData.ID.toString());
          this.customerService.RemoveCustomer(fd).subscribe(
            (data) => {
              Swal.fire(
                'Deleted!',
                'The Customer Are Succesfully Deleted.',
                'success'
              )
              this.resetForm();
            },
            (error) => {
              this.errorHandler.handleError(error);
              this.errorMessage = this.errorHandler.errorMessage;
              this.alertWithError(this.errorMessage);
            });
        }
        else {
          console.log('Not found!');
        }

      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire(
          'Cancelled',
          'The Customer Data Is Safe :)',
          'error'
        )
      }
    })

  }


  onUploadCustomerImage(event) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file: File = event.target.files[0];
      this.selectedImage = event.target.files[0];
      let FileIsValid: boolean = (this.ImageExtesion.indexOf(file.name.split('.').pop()) != -1);
      if (FileIsValid) {
        reader.readAsDataURL(file);
        reader.onload = (e: any) => {
          this.selectedImageString = (reader.result as string).split(',')[1];
          this.customerPhotoUrl = e.target.result;
          this.selectedImageName = file.name;
        };
        this.customerImageLoaded = true;
      }
      else {
        this.alertWithError('Only Image Are Allowed')
        this.selectedImage = null;
        this.customerPhotoUrl = null;
      }
    }
  }




  resetForm() {
    this.GetCustomerList();
    this.customerForm.enable();
    this.createForm();
    this.selectedImage = null;
    this.customerPhotoUrl = null;
    this.pageMode = 'new';
    this.customerImageLoaded = false;
    this.formSubmited = false;
    this.attachedImageInput.nativeElement.value = '';
  }

  

  editModeOpen() {
    this.pageMode = 'edit';
    this.customerForm.enable();
  }



  enableEditingAddress(i) {
    this.getCustomerAddresses().controls[i].enable;
  }


  alertWithSuccess(successText) {
    Swal.fire('Good job!', successText, 'success')
  }

  alertWithError(errorText) {
    Swal.fire('Opps!', errorText, 'error')
  }

  confirmBox() {
    Swal.fire({
      title: 'Are you sure want to remove?',
      text: 'You will not be able to recover this file!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          'Deleted!',
          'Your imaginary file has been deleted.',
          'success'
        )
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire(
          'Cancelled',
          'Your imaginary file is safe :)',
          'error'
        )
      }
    })
  }

}

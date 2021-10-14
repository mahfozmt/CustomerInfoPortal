import { Byte } from '@angular/compiler/src/util';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Country } from '../Models/Country';
import { Customer } from '../Models/Customer';
import { CustomerAddress } from '../Models/CustomerAddress';
import { CountryService } from '../shared/services/country.service';
import { CustomerService } from '../shared/services/customer.service';

declare var $: any;

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  public countries: Country[] = [];
  public customers: Customer[] = [];
  public customerPhotoUrl: any = '';
  public selectedFileString: string;
  public selectedFile: File;
  public selectedFileName: string;
  public customerImageSelected: boolean = false;
  public applicationState: string = 'new';
  public mainForm: FormGroup;
  public customerData: Customer;
  public formSubmited: boolean = false;
  @ViewChild('attachedImageInput', {
    static: true
  }) attachedImageInput: ElementRef

  constructor(private countryService: CountryService, private customerService: CustomerService, private fb: FormBuilder, private domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.buildForm();
    this.LoadCustomerList()
    this.LoadCountryDDL();
  }
  LoadCountryDDL() {
    this.countryService.getAllCountry().subscribe(
      (data) => {
        this.countries = data;
      },
      (err) => {
        console.log(err);
      });
  }

  LoadCustomerList() {
    this.customerService.getAllCustomers().subscribe(
      (data) => {
        this.customers = data;
      },
      (err) => {
        console.log(err);
      }
    )
  }

  onImageSelected(event) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file: File = event.target.files[0];
      this.selectedFile = event.target.files[0];
      let FileIsValid: boolean = (this.AllowedExt.indexOf(file.name.split('.').pop()) != -1);
      console.log(file.name, file.name.split('.').pop());
      if (FileIsValid) {
        reader.readAsDataURL(file);
        reader.onload = (e: any) => {
          this.selectedFileString = (reader.result as string).split(',')[1];
          this.customerPhotoUrl = e.target.result;
          this.selectedFileName = file.name;
        };
        this.customerImageSelected = true;
      }
      else {
        alert('Only .jpeg, .jpg, .png Files are Allowed!');
        this.selectedFile = null;
      }
    }
  }

  onImageUploadClick(event) {
    event.preventDefault();
    let element: HTMLElement = document.getElementById('imageInput') as HTMLElement;
    element.click();
  }

  buildForm() {
    this.mainForm = this.fb.group({
      ID: new FormControl('0'),
      CountryID: new FormControl('0', Validators.required),
      CustomerName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      FatherName: new FormControl('', Validators.maxLength(50)),
      MotherName: new FormControl('', Validators.maxLength(50)),
      MaritalStatus: new FormControl('1'),
      CustomerPhoto: new FormControl(''),
      CustomerAddresses: this.fb.array([
        this.buildCustomerAddressFormArrary()
      ])
    });
  }

  addCustomerAddresses(address: CustomerAddress = new CustomerAddress()) {
    const CustomerAddressesForm = this.buildCustomerAddressFormArrary(address);
    this.getCustomerAddresses().push(CustomerAddressesForm);
  }
  buildCustomerAddressFormArrary(address: CustomerAddress = new CustomerAddress()) {
    return this.fb.group({
      ID: new FormControl(address.ID),
      CustomerID: new FormControl(address.CustomerID),
      Address: new FormControl(address.Address, [Validators.required, Validators.maxLength(500)])
    });
  }
  removeCustomerAddresses(i: number) {
    this.getCustomerAddresses().removeAt(i);
    if (i == 0) {
      // this.notify.showWarning('You Have To Provide At Least 1 Address', 'Warnings');
      this.addCustomerAddresses();
    }
  }
  getCustomerAddresses(): FormArray {
    return this.mainForm.get("CustomerAddresses") as FormArray
  }
  onSave() {
    this.formSubmited = true;
    if (this.mainForm.valid && this.mainForm.get('CountryID').value != 0) {
      this.customerData = this.mainForm.getRawValue();
      if ((this.customerData.ID == null || this.customerData.ID == 0) && !this.customerImageSelected) { return; }
      console.log(this.customerData);
      let fd = new FormData();
      let id = this.customerData.ID.toString();
      console.log(id);
      fd.append("ID", this.customerData.ID.toString());
      fd.append("CountryID", this.customerData.CountryID.toString());
      fd.append("CustomerName", this.customerData.CustomerName.toString());
      fd.append("FatherName", this.customerData.FatherName.toString());
      fd.append("MotherName", this.customerData.MotherName.toString());
      fd.append("MaritalStatus", this.customerData.MaritalStatus.toString());
      this.customerData.CustomerAddresses.forEach((item, i) => {
        fd.append(`CustomerAddresses[${i}].ID`, item.ID.toString());
        fd.append(`CustomerAddresses[${i}].CustomerID`, item.CustomerID.toString());
        fd.append(`CustomerAddresses[${i}].Address`, item.Address);
      });
      if (this.selectedFile != null) {
        fd.append("CustomerPhoto", this.selectedFile, this.selectedFile.name);
      }
      console.log(fd);
      fd.forEach((value, key) => {
        console.log(key + " " + value)
      });
      this.customerService.saveCustomer(fd).subscribe(
        (data) => {
          // this.notify.showSuccess('Information Saved Succesfully', 'Information');
          console.log(data);
          this.resetForm();
        },
        (err) => {
          // this.notify.showError('Error Occered!', 'Oops');
          console.log(err);
        }
      )
    }
    else {
      // this.notify.showWarning('Please Provide All Required Data.', 'Warning');
    }

  }


  resetForm() {
    this.LoadCustomerList();
    this.mainForm.enable();
    this.buildForm();
    this.selectedFile = null;
    this.applicationState = 'new';
    this.customerImageSelected = false;
    this.formSubmited = false;
    this.attachedImageInput.nativeElement.value = '';
  }

  loadById(id) {
    this.applicationState = 'view';
    this.customerService.getAllCustomerById(id).subscribe(
      (data) => {
        // this.notify.showInfo('Information Loaded Succesfully', 'Information');
        console.log(data);
        this.mainForm.patchValue({
          ID: data.ID,
          CountryID: data.CountryID,
          CustomerName: data.CustomerName,
          FatherName: data.FatherName,
          MotherName: data.MotherName,
          MaritalStatus: data.MaritalStatus.toString()
        });
        if (data.CustomerPhoto != null) {
          this.customerPhotoUrl = 'data:image/jpeg;base64,' + data.CustomerPhoto;
          this.customerImageSelected = true;
        }
        else {
          this.customerImageSelected = false;
        }
        this.getCustomerAddresses().clear();
        data.CustomerAddresses.forEach((add, i) => {
          this.addCustomerAddresses(add);
        })
        this.mainForm.disable();
      },
      (err) => {
        // this.notify.showError('Error Occered!', 'Oops');
        console.log(err);
      }
    )
  }
  editModeOpen() {
    this.applicationState = 'edit';
    this.mainForm.enable();
  }

  deleteCustomer() {
    this.customerData = this.mainForm.getRawValue();
    if (confirm('Are You Sure About Delete This Record?')) {
      if (this.customerData.ID > 0) {
        let fd = new FormData();
        fd.append("ID", this.customerData.ID.toString());
        this.customerService.deleteCustomer(fd).subscribe(
          (data) => {
            // this.notify.showSuccess('Information Ddeleted Succesfully', 'Information');
            console.log(data);
            this.resetForm();
          },
          (err) => {
            // this.notify.showError('Error Occered!', 'Oops');
            console.log(err);
          });
      }
      else {
        console.log('Not found!');
      }
    }

  }
  enableEditingAddress(i) {
    this.getCustomerAddresses().controls[i].enable;
  }

  AllowedExt = ["png", "jpeg", "jpg"];
}

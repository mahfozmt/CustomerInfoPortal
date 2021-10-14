
using CustomerInfoPortal.Models;
using CustomerInfoPortal.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{
    public class CustomerService : ICustomerService
    {
        public CustomerService( IGenirecRepositoryBase<Customer> CusRepository, IGenirecRepositoryBase<CustomerAddress> AddresRepository)
        {
            
            _CustomerRepository = CusRepository;
            this._AddresRepository = AddresRepository;
        }

        public IGenirecRepositoryBase<Customer> _CustomerRepository { get; }
        public IGenirecRepositoryBase<CustomerAddress> _AddresRepository { get; }



        public IEnumerable<Customer> GetAllCustomer()
        {
            try
            {
                return _CustomerRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Customer GetCustomerById(int id)
        {
            try
            {
                Customer customer = _CustomerRepository.GetByExpresion(x => x.ID == id).FirstOrDefault();
                List<CustomerAddress> customerAddresses = _AddresRepository.GetByExpresion(x => x.CustomerID == customer.ID).ToList();
                customer.CustomerAddresses = customerAddresses;
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Customer CreateCustomer(CustomerViewModel customerViewModel)
        {
            try
            {
                Customer customerData = new Customer
                {
                    ID = customerViewModel.ID == null ? 0 : (int)customerViewModel.ID,
                    CountryID = customerViewModel.CountryID,
                    CustomerName = customerViewModel.CustomerName,
                    FatherName = customerViewModel.FatherName,
                    MotherName = customerViewModel.MotherName,
                    MaritalStatus = customerViewModel.MaritalStatus,
                    //CustomerPhoto= 
                    Country = customerViewModel.Country,
                    CustomerAddresses = customerViewModel.CustomerAddresses
                };
                    
                byte[] PhotoByte = null;
                if (customerViewModel.CustomerPhoto != null)
                {
                    if (customerViewModel.CustomerPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            customerViewModel.CustomerPhoto.CopyTo(ms);
                            PhotoByte = ms.ToArray();
                        }
                    }
                }
                customerData.CustomerPhoto = PhotoByte;


                if (customerData.ID > 0)
                {
                    CustomerAddressesSynchronizer(customerData);

                    if (customerData.CustomerPhoto == null)
                    {
                        Customer customerOldData = _CustomerRepository.GetByExpresion(x => x.ID == customerData.ID).FirstOrDefault();
                        customerData.CustomerPhoto = customerOldData.CustomerPhoto;
                    }
                    customerData.CustomerAddresses = null;
                    _CustomerRepository.Update(customerData);
                }
                else
                {
                    _CustomerRepository.Create(customerData);
                }
                _CustomerRepository.SaveChanges();
                return customerData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveCustomer(int id)
        {
            _CustomerRepository.Remove(id);
            _CustomerRepository.SaveChanges();
        }
        private byte[] ConvertFileToByte(IFormFile file)
        {

            byte[] fileBytes = null;
            if (file != null)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                }
            }
            return fileBytes;
        }

        private void CustomerAddressesSynchronizer(Customer customerData)
        {
            List<CustomerAddress> frontendNewAddress = new List<CustomerAddress>();
            foreach (var item in customerData.CustomerAddresses)
            {
                frontendNewAddress.Add(item);
            }

            List<CustomerAddress> fr = CustomerOldAddresses(customerData.CustomerAddresses, customerData.ID);
            foreach (var item in fr)
            {
                try
                {
                    _AddresRepository.Remove(item.ID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            foreach (CustomerAddress item in frontendNewAddress)
            {
                try
                {
                    if (item.ID > 0)
                    {
                        _AddresRepository.Update(item);
                    }
                    else
                    {
                        item.CustomerID = customerData.ID;
                        _AddresRepository.Create(item);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            _AddresRepository.SaveChanges();
        }

        private List<CustomerAddress> CustomerOldAddresses(List<CustomerAddress> param, int cusId)
        {
            try
            {
                List<CustomerAddress> AddressList = _AddresRepository.GetByExpresion(y => y.CustomerID == cusId).ToList();

                var ids = AddressList.Select(x => x.ID);
                var pIds = param.Select(x => x.ID);
                var dIds = ids.Except(pIds);
                var result = AddressList.Where(x => dIds.Contains(x.ID));
                return result.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}

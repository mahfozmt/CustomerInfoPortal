
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
        public CustomerService( IRepositoryBase<Customer> CusRepository, IRepositoryBase<CustomerAddress> AddresRepository)
        {
            
            _CustomerRepository = CusRepository;
            this._AddresRepository = AddresRepository;
        }

        public IRepositoryBase<Customer> _CustomerRepository { get; }
        public IRepositoryBase<CustomerAddress> _AddresRepository { get; }



        public IEnumerable<Customer> GetAllCustomer()
        {
            try
            {
                return _CustomerRepository.FindAll();
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
                Customer customer = _CustomerRepository.FindByCondition(x => x.ID == id).FirstOrDefault();
                List<CustomerAddress> customerAddresses = _AddresRepository.FindByCondition(x => x.CustomerID == customer.ID).ToList();
                customer.CustomerAddresses = customerAddresses;
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Customer SaveCustomer(CustomerViewModel customerViewModel)
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
                    CustomerPhoto= ConvertFileToByte(customerViewModel.CustomerPhoto),
                    Country = customerViewModel.Country,
                    CustomerAddresses = customerViewModel.CustomerAddresses
                };

                if (customerData.ID > 0)
                {
                    List<CustomerAddress> needUpdated = new List<CustomerAddress>();
                    foreach (var item in customerData.CustomerAddresses)
                    {
                        needUpdated.Add(item);
                    }

                    List<CustomerAddress> fr = CustomerAddressSynch(customerData.CustomerAddresses, customerData.ID);
                    foreach (var item in fr)
                    {
                        try
                        {
                            _AddresRepository.Delete(item.ID);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    foreach (CustomerAddress item in needUpdated)
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
                    _AddresRepository.Save();
                    if (customerData.CustomerPhoto == null)
                    {
                        Customer customerOldData = _CustomerRepository.FindByCondition(x => x.ID == customerData.ID).FirstOrDefault();
                        customerData.CustomerPhoto = customerOldData.CustomerPhoto;
                    }
                    customerData.CustomerAddresses = null;
                    _CustomerRepository.Update(customerData);
                }
                else
                {
                    _CustomerRepository.Create(customerData);
                }
                _CustomerRepository.Save();
                return customerData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomer(int id)
        {
            _CustomerRepository.Delete(id);
            _CustomerRepository.Save();
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

        private List<CustomerAddress> CustomerAddressSynch(List<CustomerAddress> param, int cusId)
        {
            try
            {
                List<CustomerAddress> AddressList = _AddresRepository.FindByCondition(y => y.CustomerID == cusId).ToList();

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

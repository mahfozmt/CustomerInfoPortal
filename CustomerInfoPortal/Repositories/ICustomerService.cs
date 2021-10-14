using CustomerInfoPortal.Models;
using CustomerInfoPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{
  public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomer();
        Customer GetCustomerById(int id);
        Customer CreateCustomer(CustomerViewModel customer);
        void RemoveCustomer(int id);
    }
}

using CustomerInfoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{
  public interface ICustomerService
    {
       
        IEnumerable<Customer> GetAllCustomer();
    }
}

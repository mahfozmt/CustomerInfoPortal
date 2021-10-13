
using CustomerInfoPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(CIPDbContext context)
        {
            _context = context;
        }

        public CIPDbContext _context { get; }

        public IEnumerable<Customer> GetAllCustomer()
        {
            var list = _context.Customer.Include(e => e.CustomerAddresses).AsNoTracking().ToList();
            return list;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int? ID { get; set; }
        public int CountryID { get; set; }
        public string CustomerName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int MaritalStatus { get; set; }
        public IFormFile CustomerPhoto { get; set; }


        public Country Country { get; set; }
        public List<CustomerAddress> CustomerAddresses { get; set; }

    }
}

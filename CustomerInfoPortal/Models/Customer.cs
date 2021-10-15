using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CustomerInfoPortal.Models
{
    public class Customer
    {
        
        public int ID { get; set; }
        [ForeignKey("Country")]
        public int CountryID { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CustomerName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FatherName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100)]
        public string MotherName { get; set; }
        public int MaritalStatus { get; set; }
        [Column(TypeName = "varbinary(MAX)")]
        public byte[] CustomerPhoto { get; set; }

        //Nav
        public  Country Country { get; set; }

        public  List<CustomerAddress> CustomerAddresses { get; set; }
    }
}

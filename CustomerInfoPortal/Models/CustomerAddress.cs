using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CustomerInfoPortal.Models
{
    public class CustomerAddress
    {
        public int ID { get; set; }
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        [Column(name:"CustomerAddress",TypeName = "NVARCHAR(500)")]
        public string Address { get; set; }

        //Nav
        public  Customer Customer { get; set; }
    }
}

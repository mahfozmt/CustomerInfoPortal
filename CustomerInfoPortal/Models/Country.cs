using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerInfoPortal.Models
{
    public class Country
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        public string CountryName { get; set; }

        //Nav
        public  List<Customer> Customers { get; set; }
    }
}

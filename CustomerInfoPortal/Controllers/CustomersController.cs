using CustomerInfoPortal.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerInfoPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _cusService;

        public CustomersController(ICustomerService cusServ)
        {
            this._cusService = cusServ;
        }

        [HttpGet]
        [Route("/api/Customers/GetAllCustomers")]
        public IActionResult GetCustomerList()
        {
            try
            {
                return Ok(_cusService.GetAllCustomer().ToList());
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        //[HttpPost]
        //[Route("/api/Customer/SaveCustomer")]
        //public IActionResult SaveCustomer([FromForm] CustomerHelper customer)
        //{
        //    try
        //    {
        //        if ((customer.CustomerPhoto != null && customer.ID == 0) || customer.ID > 0)
        //        {
        //            Customer customerToSave = customer.GetCustomer();
        //            _cusService.SaveCustomerData(customerToSave);
        //            customerToSave.CustomerAddresses = new List<CustomerAddress>();
        //            return Ok(customerToSave);
        //        }
        //        else
        //        {
        //            return BadRequest("Please Provide Image");

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }

        //}

        //[HttpGet]
        //[Route("/api/Customer/GetCustomerById")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult GetCustomerById(int id)
        //{
        //    try
        //    {
        //        Customer customer = _cusService.GetCustomerById(id);
        //        return Ok(customer);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex);
        //    }
        //}

        //[HttpPost]
        //[Route("/api/Customer/DeleteCustomer")]
        //public IActionResult DeleteCustomer([FromForm] Customer customer)
        //{
        //    try
        //    {
        //        if (customer.ID > 0)
        //        {
        //            _cusService.DeleteCustomer((int)customer.ID);
        //            return Ok(HttpStatusCode.OK);
        //        }
        //        else
        //        {
        //            return BadRequest("Not Valid Request");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }

        //}

    }
}

using CustomerInfoPortal.Models;
using CustomerInfoPortal.Models.ViewModels;
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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _cusService;

        public CustomerController(ICustomerService cusServ)
        {
            this._cusService = cusServ;
        }

        [HttpGet]
        [Route("/api/Customer/GetCustomerList")]
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

        [HttpPost]
        [Route("/api/Customer/CreateCustomer")]
        public IActionResult CreateCustomer([FromForm] CustomerViewModel customerView)
        {
            try
            {

                if (customerView.ID > 0)
                {
                    Customer customer = _cusService.CreateCustomer(customerView);
                    return Ok(customer);
                }
                else
                {
                    if (customerView.CustomerPhoto != null)
                    {
                        Customer customer = _cusService.CreateCustomer(customerView);
                        return Ok(customer);
                    }
                    else
                    {
                        return BadRequest("Insert Image!");
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet]
        [Route("/api/Customer/GetCustomerById")]
        public IActionResult GetCustomerById(int id)
        {
            try
            {
                Customer customer = _cusService.GetCustomerById(id);
                return Ok(customer);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("/api/Customer/RemoveCustomer")]
        public IActionResult RemoveCustomer([FromForm] Customer customer)
        {
            try
            {
                if (customer.ID > 0)
                {
                    _cusService.RemoveCustomer((int)customer.ID);
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid Request!");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}

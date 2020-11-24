using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        
            private readonly MyDbContext _context;

            public CustomerController(MyDbContext context)
            {
                _context = context;
            }

            // GET: api/

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
            {
                return await _context.Customer.ToListAsync();
            }

            // GET: api/Customer/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Customer>> GetCustomer(long id)
            {
                var customer = await _context.Customer.FindAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                return customer;
            }

            // DELETE: api/Customer/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCustomer(long id)
            {
                var customer = await _context.Customer.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                _context.Customer.Remove(customer);
                await _context.SaveChangesAsync();

                return NoContent();
            }


        [Route("/api/sendsms")]
        [Obsolete]
        public async Task<JsonResult> SendSMS(
       [FromQuery(Name = "phone")] string phone,
       [FromQuery(Name = "from")] string from,
       [FromQuery(Name = "content")] string content)
        {
            var customer = new Customer();

            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(content))
            {
                customer.Phone = phone;
                customer.Alphanumeric = from;
                customer.Content = content;

                _context.Customer.Add(customer);
                _context.SaveChanges();
            }
            else
            {
                return new JsonResult(NotFound());
            }

            var sms = new SmsSender();

            var response = await sms.Send(customer.Alphanumeric, customer.Phone, customer.Content);

            return new JsonResult(new { customer, response });
        }

    }
    }

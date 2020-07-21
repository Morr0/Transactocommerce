using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transactocommerce.Models;
using Transactocommerce.Utilities;

namespace Transactocommerce.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private DataContext _context;
        public ProductController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Products.ToArrayAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            await _context.Products.AddAsync(product);
            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException)
            {
                return Conflict();
            }

            return Ok();
        }
    }
}

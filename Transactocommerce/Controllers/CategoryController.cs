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
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private DataContext _context;
        public CategoryController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _context.Categories.AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            await _context.Categories.AddAsync(category);
            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException)
            {
                return Conflict();
            }

            return Ok(category);
        }
    }
}

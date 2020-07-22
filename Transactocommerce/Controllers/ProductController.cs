using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transactocommerce.Controllers.Queiries;
using Transactocommerce.Models;
using Transactocommerce.Utilities;

namespace Transactocommerce.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private DataContext _context;
        private IMapper _mapper;
        public ProductController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany([FromQuery] ProductQuery query)
        {
            IQueryable<Product> products = _context.Product.AsNoTracking();
            products = products.Skip(query.Size * (query.Page))
                .Take(query.Size); // Pagination

            List<Product> list = await products.ToListAsync();
            return Ok(_mapper.Map<List<ProductReadDTO>>(list));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductWriteDTO _product)
        {
            Product product = _mapper.Map<Product>(_product);
            await _context.Product.AddAsync(product);
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

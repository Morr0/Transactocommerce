using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
            // Pagination
            products = products.Skip(query.Size * (query.Page))
                .Take(query.Size);

            // By Category Id
            if (!string.IsNullOrEmpty(query.CategoryId))
            {
                // Check if category exists
                if (await _context.Category.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == query.CategoryId) == null)
                    return NotFound();

                products = products.Where(p => p.CategoryId == query.CategoryId);
            }

            List<Product> list = await products.ToListAsync();
            return Ok(_mapper.Map<List<ProductReadDTO>>(list));
        }

        [HttpPost("many")]
        public async Task<IActionResult> GetManyById([FromBody] JsonElement items)
        { 
            if (items.ValueKind == JsonValueKind.Array)
            {
                Console.WriteLine(items.GetRawText());
                int length = items.GetArrayLength();
                if (length == 0)
                    return BadRequest();

                List<string> ids = new List<string>(length);
                foreach (JsonElement item in items.EnumerateArray())
                    ids.Add(item.GetString());

                List<Product> list = await _context.Product.AsNoTracking()
                    .Where(p => ids.Contains(p.Id))
                    .ToListAsync();
                return Ok(_mapper.Map<List<ProductReadDTO>>(list));
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            Product product = await _context.Product.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductReadDTO>(product));
            }

            return NotFound();
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

            return Ok(_mapper.Map<ProductReadDTO>(product));
        }
    }
}

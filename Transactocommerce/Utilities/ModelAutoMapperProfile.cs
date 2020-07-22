using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactocommerce.Models;

namespace Transactocommerce.Utilities
{
    public class ModelAutoMapperProfile : Profile
    {
        public ModelAutoMapperProfile()
        {
            CreateMap<ProductWriteDTO, Product>();
            CreateMap<Product, ProductReadDTO>();

            
        }
    }
}

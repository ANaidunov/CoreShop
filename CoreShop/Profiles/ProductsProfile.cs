using AutoMapper;
using CoreShop.DTOs;
using CoreShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            // Source Model -> Target
            CreateMap<Product, ProductReadDTO>();

            // Target -> Source Model
            CreateMap<ProductCreateDTO, Product>();

            // Source Model -> Target Create model (for patch update)
            CreateMap<Product, ProductCreateDTO>();
        }
    }
}

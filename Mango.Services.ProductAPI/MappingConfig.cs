using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
                {
                    //ReverseMap to also make available mapping from Product to ProductDto 
                    config.CreateMap<ProductDto, Product>().ReverseMap();
                });
            return mappingConfig;
        }
    }
}

using Mapster;
using back_net.Models;
using back_net.Models.Dtos;

namespace back_net.Mapping;

public class ProductProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.CategoryName, src => src.Category.name);
        config.NewConfig<ProductDto, Product>();
        config.NewConfig<Product, CreateProductDto>();
        config.NewConfig<CreateProductDto, Product>();
        config.NewConfig<Product, UpdateProductDto>();
        config.NewConfig<UpdateProductDto, Product>();
    }
}
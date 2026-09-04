using Mapster;
using back_net.Models.Dtos;

namespace back_net.Mapping;

public class CategoryProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>();
        config.NewConfig<CategoryDto, Category>();
        config.NewConfig<Category, CreateCategoryDto>();
        config.NewConfig<CreateCategoryDto, Category>();
    }
}
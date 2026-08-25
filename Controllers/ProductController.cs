using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using back_net.Repository.IRepository;
using AutoMapper;

namespace back_net.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class ProductController: ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository=productRepository;
        _mapper=mapper;
    }
}
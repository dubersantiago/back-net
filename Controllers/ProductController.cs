using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using back_net.Repository.IRepository;
using AutoMapper;
using back_net.Models.Dtos;
using back_net.Models;

namespace back_net.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class ProductController: ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository=productRepository;
        _categoryRepository=categoryRepository;
        _mapper=mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProducts()
    {
        var products = _productRepository.GetProducts();
        var productDto = _mapper.Map<List<ProductDto>>(products);
        
        return Ok(productDto);
    }

    [HttpGet("{productId:int}",Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProduct(int productId)
    {
        var product = _productRepository.GetProduct(productId);
        if(product == null) return NotFound($"El producto con el id {productId} no existe");
        var productDto = _mapper.Map<ProductDto>(product);
        
        return Ok(productDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateProduct([FromBody] CreateProductDto createproductDto)
    {
        if(createproductDto == null)
        {
            return BadRequest(ModelState);
        }

        if(_productRepository.ProductExist(createproductDto.Name)){
            ModelState.AddModelError("CustomError","El producto ya existe");
            return BadRequest(ModelState);
        }

        if(!_categoryRepository.CategoryExists(createproductDto.CategoryId)){
            ModelState.AddModelError("CustomError","La categoria no existe");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(createproductDto);
        if(!_productRepository.CreateProduct(product))
        {
            ModelState.AddModelError("CustomError",$"Algo salio mal al guardar {product.Name}");
            return StatusCode(500,ModelState);
        }
        return CreatedAtRoute("Getproduct",new { productId=product.ProductId },product);
    }
}
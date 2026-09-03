using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using back_net.Repository.IRepository;
using AutoMapper;
using back_net.Models.Dtos;
using back_net.Models;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace back_net.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[Authorize(Roles = "admin")]
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

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProducts()
    {
        var products = _productRepository.GetProducts();
        var productDto = _mapper.Map<List<ProductDto>>(products);
        
        return Ok(productDto);
    }

    [AllowAnonymous]
    [HttpGet("searchByCategory/{categoryId:int}",Name ="GetProdcutsByCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProdcutsByCategory(int categoryId)
    {
        var products = _productRepository.GetProductsForCategory(categoryId);
        if(products.Count == 0) return NotFound($"No existen productos con la categoria {categoryId}");
        var productDto = _mapper.Map<List<ProductDto>>(products);
        
        return Ok(productDto);
    }

    [HttpGet("search/{query}",Name ="SearchProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult SearchProduct(String query)
    {
        var products = _productRepository.SearchProduct(query);
        if(products.Count == 0) return NotFound($"No existen productos con el nombre '{query}' o descripcion ");
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
        var createdProduct = _productRepository.GetProduct(product.ProductId);
        var productDto = _mapper.Map<ProductDto>(createdProduct);
        return CreatedAtRoute("Getproduct",new { productId=productDto.ProductId },productDto);
    }

    [HttpPatch("BuyProduct/{name}/{cantidad:int}",Name ="BuyProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult BuyProduct(String name, int cantidad)
    {
        if(String.IsNullOrEmpty(name)|| cantidad<=0) return BadRequest("La cantidad o el nombre no sin validos");
        if(!_productRepository.ProductExist(name)) return NotFound($"No se encontro el producto {name}");
        if (!_productRepository.BuyProduct(name, cantidad))
        {
            ModelState.AddModelError("CustomError",$"No se pudo comprar el produdcto o la cantidad es superior al stock disponible");
            return BadRequest(ModelState);
        }
        
        
        return Ok($"Se compro la cantidad {cantidad} de {name}");
    }

    [HttpPut("{productId:int}",Name ="UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpdateProduct(int productId,[FromBody] UpdateProductDto updateProductDto)
    {
        if(updateProductDto == null)
        {
            return BadRequest(ModelState);
        }

        if(!_productRepository.ProductExist(productId)){
            ModelState.AddModelError("CustomError","El producto no existe");
            return BadRequest(ModelState);
        }

        if(!_categoryRepository.CategoryExists(updateProductDto.CategoryId)){
            ModelState.AddModelError("CustomError","La categoria no existe");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(updateProductDto);
        product.ProductId=productId;
        if(!_productRepository.UpdateProduct(product))
        {
            ModelState.AddModelError("CustomError",$"Algo salio mal al actualizar el producto {product.Name}");
            return StatusCode(500,ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{productId:int}",Name ="DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteProduct(int productId)
    {
        if(productId==0) return BadRequest($"No se encontro un producto");
        if(!_productRepository.ProductExist(productId)){
            ModelState.AddModelError("CustomError","El producto no existe");
            return BadRequest(ModelState);
        }

        var product = _productRepository.GetProduct(productId);
        if(!_productRepository.DeleteProduct(product!))
        {
            ModelState.AddModelError("CustomError",$"Algo salio mal al eliminar el producto {product!.Name}");
            return StatusCode(500,ModelState);
        }
        
        return NoContent();
    }
}
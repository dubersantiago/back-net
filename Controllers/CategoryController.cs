using Asp.Versioning;
using AutoMapper;
using back_net.Models.Dtos;
using back_net.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_net.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[Authorize(Roles = "admin")]
public class CategoryController: ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository=categoryRepository;
        _mapper=mapper;
    }

    [AllowAnonymous]
    [MapToApiVersion("1.0")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCategories()
    {
        var categories = _categoryRepository.GetAllCategories();
        var categoriesDto = new List<CategoryDto>();
        foreach (var category in categories)
        {
            categoriesDto.Add(_mapper.Map<CategoryDto>(category));
        }
        return Ok(categoriesDto);
    }

    [AllowAnonymous]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCategoriesorderById()
    {
        var categories = _categoryRepository.GetAllCategories().OrderBy(cat => cat.id);
        var categoriesDto = new List<CategoryDto>();
        foreach (var category in categories)
        {
            categoriesDto.Add(_mapper.Map<CategoryDto>(category));
        }
        return Ok(categoriesDto);
    }

    [AllowAnonymous]    
    [HttpGet("{id:int}",Name = "GetCategory")]
    [ResponseCache(CacheProfileName = "Default10")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCategori(int id)
    {
        Console.WriteLine($"Categoria con el id {id} a las {DateTime.Now}");
        var category = _categoryRepository.GetCategoryById(id);
        Console.WriteLine($"Respuesta con el id {id}  a las {DateTime.Now}");

        if(category == null) return NotFound($"La categoria con el id {id} no existe");

        var categoryDto = _mapper.Map<CategoryDto>(category);

        return Ok(categoryDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateCategories([FromBody] CreateCategoryDto createCategoryDto)
    {
        if(createCategoryDto == null)
        {
            return BadRequest(ModelState);
        }

        if(_categoryRepository.CategoryExists(createCategoryDto.name)){
            ModelState.AddModelError("CustomError","La categoria ya existe");
            return BadRequest(ModelState);
        }

        var category = _mapper.Map<Category>(createCategoryDto);
        if(!_categoryRepository.CreateCategory(category))
        {
            ModelState.AddModelError("CustomError",$"Algo salio mal al guardar {category.name}");
            return StatusCode(500,ModelState);
        }
        return CreatedAtRoute("GetCategory",new { id=category.id },category);
    }

    [HttpPatch("{id:int}",Name ="UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategories(int id,[FromBody] CreateCategoryDto updateCategoryDto)
    {
        if(updateCategoryDto == null)
        {
            return BadRequest(ModelState);
        }

        if(!_categoryRepository.CategoryExists(id)){
            return NotFound($"La categoria con el id {id} no existe");
        }

        var category = _mapper.Map<Category>(updateCategoryDto);
        category.id = id;
        if(!_categoryRepository.UpdateCategory(category))
        {
            ModelState.AddModelError("CustomError",$"Algo salio mal al actualizar {category.name}");
            return StatusCode(500,ModelState);
        }
        return NoContent();
    }

    [HttpDelete("{id:int}",Name ="UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategories(int id)
    {


        if(!_categoryRepository.CategoryExists(id)){
            return NotFound($"La categoria con el id {id} no existe");
        }

        var category = _categoryRepository.GetCategoryById(id);

        if (category == null)
        {
            return NotFound($"La categoria con el id {id} no existe");
        }

        if(!_categoryRepository.DeleteCategory(category.id))
        {
            ModelState.AddModelError("CustomError",$"Algo salio mal al actualizar {category.name}");
            return StatusCode(500,ModelState);
        }

        return NoContent();
    }
}
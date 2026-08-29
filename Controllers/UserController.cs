using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using back_net.Repository.IRepository;
using AutoMapper;
using back_net.Models.Dtos;
using back_net.Models;

namespace back_net.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository=userRepository;
        _mapper=mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetUsers()
    {
        var users = _userRepository.GetUsers();
        var usersDto = _mapper.Map<List<UserDto>>(users);
        
        return Ok(usersDto);
    }    

    [HttpGet("{id:int}",Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetUser(int id)
    {
        var User = _userRepository.GetUser(id);
        if(User == null) return NotFound($"El Usuario con el id {id} no existe");
        var UserDto = _mapper.Map<UserDto>(User);
        
        return Ok(UserDto);
    }

    [HttpPost(Name = "RegisterUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
    {
        if(createUserDto == null) return BadRequest(ModelState);

        if(String.IsNullOrWhiteSpace(createUserDto.Username)) return BadRequest("Username es requerido");

        if (!_userRepository.IsUniqueUser(createUserDto.Username)) return BadRequest("El usuario ya existe");

        var result =await _userRepository.Register(createUserDto);
        if(result == null)
        {
            
            return StatusCode(StatusCodes.Status500InternalServerError,"Error al registrar un usuario");
        }
        
        return CreatedAtRoute("GetUser",new { id=result.Id },result);
    }

    [HttpPost("Login",Name = "LoginUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        if(userLoginDto == null) return BadRequest(ModelState);

        var result = await _userRepository.Login(userLoginDto);
        if(result == null)
        {
            return Unauthorized();
        }
        
        return Ok(result);
    }

    // [HttpDelete("{productId:int}",Name ="DeleteProduct")]
    // [ProducesResponseType(StatusCodes.Status403Forbidden)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // public IActionResult DeleteProduct(int productId)
    // {
    //     if(productId==0) return BadRequest($"No se encontro un producto");
    //     if(!_productRepository.ProductExist(productId)){
    //         ModelState.AddModelError("CustomError","El producto no existe");
    //         return BadRequest(ModelState);
    //     }

    //     var product = _productRepository.GetProduct(productId);
    //     if(!_productRepository.DeleteProduct(product!))
    //     {
    //         ModelState.AddModelError("CustomError",$"Algo salio mal al eliminar el producto {product!.Name}");
    //         return StatusCode(500,ModelState);
    //     }
        
    //     return NoContent();
    // }
}
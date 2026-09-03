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
[Authorize(Roles = "Admin")]
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

    [HttpGet("{id}",Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetUser(string id)
    {
        var User = _userRepository.GetUser(id);
        if(User == null) return NotFound($"El Usuario con el id {id} no existe");
        var UserDto = _mapper.Map<UserDto>(User);
        
        return Ok(UserDto);
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
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

}
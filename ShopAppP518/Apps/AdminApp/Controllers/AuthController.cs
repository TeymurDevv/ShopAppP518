using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShopAppP518.Apps.AdminApp.Dtos.UserDto;
using ShopAppP518.Apps.AdminApp.Services.Interfaces;
using ShopAppP518.Apps.AdminApp.Settings;
using ShopAppP518.Entities;

namespace ShopAppP518.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) return BadRequest();
            AppUser appUser = new AppUser();
            appUser.UserName = registerDto.UserName;
            appUser.Email = registerDto.Email;
            appUser.FullName = registerDto.FullName;
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(appUser, "Member");
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LoginDto loginDto)
        {
            var existUser = await _userManager.FindByNameAsync(loginDto.UserName);
            if (existUser == null) return BadRequest();
            var result = await _userManager.CheckPasswordAsync(existUser, loginDto.Password);
            if (!result)
            {
                return BadRequest();
            }

            IList<string> roles = await _userManager.GetRolesAsync(existUser);
            var Audience = _jwtSettings.Audience;
            var SecretKey = _jwtSettings.secretKey;
            var Issuer = _jwtSettings.Issuer;
            return Ok(new { token = _tokenService.GetToken(SecretKey, Audience, Issuer, existUser, roles) });

        }
        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UserProfile()
        {
            var existedUser = await _userManager.GetUserAsync(User);
            if (existedUser == null) return NotFound();

            return Ok(_mapper.Map<UserGetDto>(existedUser));
        }
    }
}

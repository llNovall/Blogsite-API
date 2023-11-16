using AutoMapper;
using BlogsiteAPI.DataTransferObjects;
using BlogsiteAPI.Utils;
using BlogsiteDomain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogsiteAPI.Controllers.Account
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtSetting _jwtKeySetting;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, IOptions<JwtSetting> options)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtKeySetting = options.Value;
        }

        [HttpPost("Signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] AppUserSignupDTO userInfo)
        {
            if (userInfo == null || !ModelState.IsValid)
                return BadRequest();

            if (userInfo.Password == null)
                return BadRequest();

            AppUser? user = _mapper.Map<AppUser>(userInfo);

            IdentityResult result = await _userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                SignupResponseDTO signupResponseDTO = new()
                {
                    IsSignupSuccess = false,
                    SignupErrors = result.Errors.Select(c => c.Description)
                };

                return BadRequest(signupResponseDTO);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginUser([FromBody] AppUserLoginDTO userInfo)
        {
            if (userInfo == null || !ModelState.IsValid || userInfo?.Username == null || userInfo?.Password == null)
                return BadRequest();

            AppUser? appUser = await _userManager.FindByNameAsync(userInfo.Username);

            if (appUser == null)
                return BadRequest();

            bool isLoginSuccessful = await _userManager.CheckPasswordAsync(appUser, userInfo.Password);

            if (!isLoginSuccessful)
                return BadRequest();

            if (_jwtKeySetting.Key == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(_jwtKeySetting.Key));

            SigningCredentials signingCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);

            IList<Claim> claims = await _userManager.GetClaimsAsync(appUser);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtKeySetting.Issuer,
                audience: _jwtKeySetting.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResponseDTO { Token = tokenString });
        }
    }
}
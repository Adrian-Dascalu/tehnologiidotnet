using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using tehnologiinet.Models;

namespace tehnologiinet.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Login route
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (result.Succeeded)
        {
            // generate jwt token
            var key = Encoding.ASCII.GetBytes("lkgjnsdfkljngfdiyerthiyortegjklfjdnbvxcbxfdkogjrpoiyuteroyigjdf4359859046845");
            var tokenValidityInMinutes = 30;

            var user = await _userManager.FindByEmailAsync(model.Email);
            
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("blablabla", "bl;ablfdgdfgfsdgsd")
            };

            // Add roles to claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                Issuer = "ace.ucv.ro",
                Audience = "ace.ucv.ro",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(tokenHandler.WriteToken(token));
        }
        else
        {
            return BadRequest("Invalid login attempt.");
        }
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok("User Created Succesfully!");
        }
        else
        {
            BadRequest("Could not register user!");
        }

        return Ok();
    }
}
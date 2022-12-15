using InventoryControl.Data.Entities;
using InventoryControl.Models;
using InventoryControl.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryControl.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSignigKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignigKey, SecurityAlgorithms.HmacSha256)
                    );

                return new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };
            }
            else
                return null;

        }

        public async Task<AuthResponse> RegisterAsync(RegisterModel model)
        {
            var userExixts = await _userManager.FindByNameAsync(model.UserName);
            if (userExixts != null)
            {
                return new AuthResponse { Status = "Error", Reasons = new List<string> { "User already exists" }, };
            }

            var user = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Status = "Error",
                    Reasons = result.Errors.Select(x => x.Description).ToList(),
                };
            }
            if (model.Roles != null)
            {
                foreach (var userRole in model.Roles)
                {
                    await _userManager.AddToRoleAsync(user, userRole);
                }
            }
            return new AuthResponse { Status = "Success", Reasons = new List<string> { "User created successfully!" }, };
        }
    }
}

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
                return await GetToken(user);
            }
            else
            {
                throw new Exception("Wrong UserName or password");
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.UserName);

            if (existingUser != null)
            {
                throw new Exception("User already exists");
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
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Data = result.Errors.Select(x => x.Description).ToList()
                };
            }

            foreach (var userRole in model.Roles)
            {
                await _userManager.AddToRoleAsync(user, userRole);
            }

            return new RegisterResponse
            {
                IsSuccess = true,
                Data = new List<string> { "User created successfully!" }
            };
        }

        private async Task<LoginResponse> GetToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }
    }
}

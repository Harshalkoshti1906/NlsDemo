using NlsDemo.service.IService;
using NlsDemo.service.ViewModel;
using NlsDemo.data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.service.Service
{
    public class UserService:IUserService
    {
        #region Properties
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        #endregion

        #region Method
        public async Task<ResponseViewModel> Login(LoginViewModel loginModel)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var user = await userManager.FindByNameAsync(loginModel.Username);
                if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    response.IsSuccess = true;
                    response.Status = Convert.ToInt32(EnumManager.Status.Success);
                    response.Message = Message.Success;
                    response.Data = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo

                    };

                }
                else
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(EnumManager.Status.Error);
                    response.Message = Message.UnAuthorized;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Status = Convert.ToInt32(EnumManager.Status.Error);
                response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return response;
        }

        public async Task<ResponseViewModel> Register(RegisterViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(StatusCodes.Status500InternalServerError);
                    response.Message = Message.AlreadyExists;
                    return response;
                }
               
                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(StatusCodes.Status500InternalServerError);
                    response.Message = Message.CredentialFailed;
                    return response;
                }

                response.IsSuccess = true;
                response.Status = Convert.ToInt32(EnumManager.Status.Success);
                response.Message = Message.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Status = Convert.ToInt32(EnumManager.Status.Error);
                response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return response;
        }

        public async Task<ResponseViewModel> RegisterAdmin(RegisterViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(StatusCodes.Status500InternalServerError);
                    response.Message = Message.AlreadyExists;
                    return response;
                }

                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(StatusCodes.Status500InternalServerError);
                    response.Message = Message.CredentialFailed;
                    return response;
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                //if (!await roleManager.RoleExistsAsync(UserRoles.User))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

                response.IsSuccess = true;
                response.Status = Convert.ToInt32(EnumManager.Status.Success);
                response.Message = Message.Success;
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Status = Convert.ToInt32(EnumManager.Status.Error);
                response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return response;
        }
        #endregion
    }
}

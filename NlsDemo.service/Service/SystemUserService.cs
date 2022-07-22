using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NlsDemo.data.Common;
using NlsDemo.data.dbcontext;
using NlsDemo.data.Entity;
using NlsDemo.service.IService;
using NlsDemo.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.service.Service
{
    public class SystemUserService : ISystemUserService
    {
        #region Properties
        private readonly IMapper _mapper;
        private readonly DatabaseContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region Constructor
        public SystemUserService(IMapper mapper, 
                                 DatabaseContext db,
                                 UserManager<IdentityUser> userManager, 
                                 RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Method
        public async Task<ResponseViewModel> CreateSystemUser(SystemUserViewModel viewModel)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var userExists = await _userManager.FindByNameAsync(viewModel.Email);
                if (userExists != null)
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(StatusCodes.Status500InternalServerError);
                    response.Message = Message.AlreadyExists;
                    return response;
                }


                _db.SystemUsers.Add(_mapper.Map<SystemUser>(viewModel));
                int result = _db.SaveChanges();

                if (result > 0)
                {
                    IdentityUser user = new IdentityUser()
                    {
                        Email = viewModel.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = viewModel.Email
                    };
                    var result1 = await _userManager.CreateAsync(user, viewModel.Password);
                    if (!result1.Succeeded)
                    {
                        response.IsSuccess = false;
                        response.Status = Convert.ToInt32(StatusCodes.Status500InternalServerError);
                        response.Message = Message.CredentialFailed;
                        return response;
                    }

                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    }

                    response.IsSuccess = true;
                    response.Status = Convert.ToInt32(EnumManager.Status.Success);
                    response.Message = Message.Success;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(EnumManager.Status.Error);
                    response.Message = Message.Error;
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

        public async Task<ResponseViewModel> GetById(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var result = _db.SystemUsers.FirstOrDefault(a => a.Id == Id);
                if (result == null)
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(EnumManager.Status.NotFound);
                    response.Message = Message.NotFound;
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Status = Convert.ToInt32(EnumManager.Status.Success);
                    response.Message = Message.Success;
                    response.Data = _mapper.Map<SystemUserViewModel>(result);
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

        public async Task<ResponseViewModel> UpdateUser(SystemUserViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                _db.SystemUsers.Update(_mapper.Map<SystemUser>(model));
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.Status = Convert.ToInt32(EnumManager.Status.Success);
                    response.Message = Message.Success;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(EnumManager.Status.Error);
                    response.Message = Message.Error;
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
        #endregion
    }
}

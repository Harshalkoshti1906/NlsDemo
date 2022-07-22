using NlsDemo.data.Common;
using NlsDemo.service.IService;
using NlsDemo.service.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NlsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Properties
        private readonly IUserService userService;
        #endregion

        #region Constructor
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        #endregion

        #region ApiMethod
        [Route("AdminRegister")]
        [HttpPost]
        [AllowAnonymous]
        public ResponseViewModel AdminRegister(RegisterViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                response = userService.RegisterAdmin(model).Result;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Message.Error;
                response.Status = Convert.ToInt32(EnumManager.Status.Error);
            }
            return response;
        }

        [HttpPost]
        [Route("login")]
        public ResponseViewModel Login(LoginViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                response = userService.Login(model).Result;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Message.Error;
                response.Status = Convert.ToInt32(EnumManager.Status.Error);
            }
            return response;
        }
        #endregion
    }
}

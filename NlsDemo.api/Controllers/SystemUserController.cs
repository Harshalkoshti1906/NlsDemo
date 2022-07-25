using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NlsDemo.data.Common;
using NlsDemo.service.IService;
using NlsDemo.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NlsDemo.api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        #region Properties
        public readonly ISystemUserService _systemUserService;
        #endregion

        #region Constructor
        public SystemUserController(ISystemUserService systemUserService)
        {
            _systemUserService = systemUserService;
        }
        #endregion

        #region Method
        [Route("CreateSystemUser")]
        [HttpPost]
        [AllowAnonymous]
        public ResponseViewModel CreateSystemUser(SystemUserViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                response = _systemUserService.CreateSystemUser(model).Result;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Message.Error;
                response.Status = Convert.ToInt32(EnumManager.Status.Error);
            }
            return response;
        }

        [Route("GetById")]
        [HttpGet]
        public ResponseViewModel GetById(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                response = _systemUserService.GetById(Id).Result;
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
        [Route("UpdateUser")]
        public ResponseViewModel UpdateUser(SystemUserViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                response = _systemUserService.UpdateUser(model).Result;
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class MovieCatalogController : ControllerBase
    {
        #region Properties
        private readonly IMovieCatalogService _movieCatalogService;
        #endregion

        #region Constructor
        public MovieCatalogController(IMovieCatalogService movieCatalogService)
        {
            _movieCatalogService = movieCatalogService;
        }
        #endregion

        #region Method
        [HttpGet]
        [Route("GetMovieCatalog")]
        public ResponseViewModel GetMovieCatalog()
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                response = _movieCatalogService.GetMovieList().Result;
            }
            catch (Exception ex)
            {

                throw;
            }
            return response;
        }
        #endregion


    }
}

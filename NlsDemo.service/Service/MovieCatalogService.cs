using AutoMapper;
using NlsDemo.data.Common;
using NlsDemo.data.dbcontext;
using NlsDemo.service.IService;
using NlsDemo.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.service.Service
{
    public class MovieCatalogService : IMovieCatalogService
    {
        #region Properties
        private readonly IMapper _mapper;
        private readonly DatabaseContext _db;
        #endregion

        #region Constructor
        public MovieCatalogService(IMapper mapper, DatabaseContext db)
        {
            _mapper = mapper;
            _db = db;
        }
        #endregion

        #region Method
        public async Task<ResponseViewModel> GetMovieList()
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var result = _db.MovieCatalogs.ToList();
                if (result != null && result.Count > 0)
                {
                    response.IsSuccess = true;
                    response.Status = Convert.ToInt32(EnumManager.Status.Success);
                    response.Message = Message.Success;
                    response.Data = result;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Status = Convert.ToInt32(EnumManager.Status.NotFound);
                    response.Message = Message.NotFound;
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

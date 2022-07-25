using NlsDemo.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.service.IService
{
    public interface IMovieCatalogService
    {
        Task<ResponseViewModel> GetMovieList();
    }
}

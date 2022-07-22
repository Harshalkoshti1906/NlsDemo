using AutoMapper;
using NlsDemo.data.Entity;
using NlsDemo.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.service.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieCatalog, MovieCatalogViewModel>().ReverseMap();
            CreateMap<SystemUser, SystemUserViewModel>().ReverseMap();
        }
    }
}

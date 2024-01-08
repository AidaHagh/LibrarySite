using AutoMapper;
using DataLayer.ViewModels;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        { 
            CreateMap<ApplicationUsers,RegisterViewModel>().ReverseMap();
            CreateMap<ApplicationUsers,EditUserViewModel>().ReverseMap();
            CreateMap<ApplicationRoles, RoleViewModel>().ReverseMap();
            CreateMap<Book,BookViewModel>().ReverseMap();
            CreateMap<Group,GroupViewModel > ().ReverseMap();
            CreateMap<News,NewsViewModel > ().ReverseMap();
            CreateMap<Author,AuthorViewModel > ().ReverseMap();
        }
    }
}

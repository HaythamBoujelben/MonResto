using AutoMapper;
using MonResto.API.Dto;
using MonRestoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonResto.core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateArticleDto, Article>();
        }
    }
}

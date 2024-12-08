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
            CreateMap<ArticleDto, Article>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<MenuDto, Menu>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<CategoryDto, Category>();
        }
    }
}

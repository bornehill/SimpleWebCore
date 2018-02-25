using AutoMapper;
using SimpleWebCore.Data.Entities;
using SimpleWebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebCore.Data
{
    public class SimpleWebCoreMappingProfile : Profile
    {
        public SimpleWebCoreMappingProfile() {
            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id)).ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
        }
    }
}

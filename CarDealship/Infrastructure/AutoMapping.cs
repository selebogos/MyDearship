using AutoMapper;
using CarDealershipApp.Common.DTOs;
using CarDealershipApp.DAL.Entities;
using CarDealershipApp.Web.UI.Models;

namespace CarDealershipApp.Web.UI.Infrastructure
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<ClientViewModel, ClientDto>();// means you want to map from User to UserDTO
            CreateMap<ClientDto, ClientViewModel>();
            CreateMap<AddressViewModel, AddressDto>();
            CreateMap<AddressDto, AddressViewModel>();
            CreateMap<CountryViewModel, CountryDto>();
            CreateMap<CountryDto,CountryViewModel>();
            CreateMap<DatatableParametersViewModel, DatatableParametersDto>();

            CreateMap<ProductTypeViewModel, ProductTypeDto>();// means you want to map from User to UserDTO
            CreateMap<ProductTypeDto, ProductTypeViewModel>();

            CreateMap<ProductViewModel, ProductDto>();// means you want to map from User to UserDTO
            CreateMap<ProductDto, ProductViewModel>();

            CreateMap<OrderViewModel, OrderDto>();// means you want to map from User to UserDTO
            CreateMap<OrderDto, OrderViewModel>(); 

            CreateMap<ClientProfileViewModel, ClientProfileDto>();// means you want to map from User to UserDTO
            CreateMap<ClientProfileDto, ClientProfileViewModel>();
            CreateMap<OrderItemDto, OrderItemViewModel>();
            CreateMap<OrderItemViewModel, OrderItemDto>();

            CreateMap<Client, ClientDto>().ForMember(c => c.CreatedBy, option => option.Ignore());
           // CreateMap<ClientDto, ClientDto>().ForMember(c => c.Id, option => option.Ignore()).ForMember(c => c.AddedById, option => option.Ignore()).ForMember(c => c.AddressId, option => option.Ignore());
            // means you want to map from User to UserDTO
            CreateMap<ClientDto, Client>().ForMember(c => c.User, option => option.Ignore());
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();

            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<ProductTypeDto, ProductType>();

            CreateMap<NormalRange, NormalRangeDto>();
            CreateMap<NormalRangeDto, NormalRange>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();

            CreateMap<ClientProfile, ClientProfileDto>();
            CreateMap<ClientProfileDto, ClientProfile>();
        }
    }
}

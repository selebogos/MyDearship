using AutoMapper;
namespace CarDealershipApp.Common.Infrastructure
{
    public class CommonAutoMapping:Profile
    {
        /*
        public CommonAutoMapping()
        {
            CreateMap<Patient, ClientDto>().ForMember(c => c.CreatedBy, option => option.Ignore());
            // means you want to map from User to UserDTO
            CreateMap<ClientDto, Patient>().ForMember(c => c.User, option => option.Ignore());
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();

            CreateMap<TestResult, ProductTypeDto>();
            CreateMap<ProductTypeDto, TestResult>();

            CreateMap<NormalRange, NormalRangeDto>();
            CreateMap<NormalRangeDto, NormalRange>();

            CreateMap<Test, ProductDto>();
            CreateMap<ProductDto, Test>();

            CreateMap<Requisition, OrderDto>();
            CreateMap<OrderDto, Requisition>();

            CreateMap<PatientProfile, ClientProfileDto>();
            CreateMap<ClientProfileDto, PatientProfile>();
        }

        */
    }
}

using AutoMapper;
using z5.ms.common.extensions;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.customer
{
    // TODO: cover with tests, make sure the mapping profile is valid
    /// <summary>Mapping profile for Automapper</summary>
    /// <remarks>Map DTO to returned BC entities</remarks>
    public class CustomerProfile : Profile
    {
        /// <inheritdoc />
        public CustomerProfile()
        {
            // UserEntity to Customer
            CreateMap<UserEntity, Customer>()
                .ForMember(dest => dest.Additional, opt => opt.MapFrom(src => src.Json.ToJObject()))
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => src.Email != null ? (bool?)src.IsEmailConfirmed : null))
                .ForMember(dest => dest.MobileVerified, opt => opt.MapFrom(src => src.Mobile != null ? (bool?)src.IsMobileConfirmed : null))
                .ForMember(dest => dest.Activated, opt => opt.MapFrom(src => src.State == UserState.Verified));

            // CustomerChangeDetails to Customer
            CreateMap<CustomerChangeDetails, Customer>()
                .ForMember(dest => dest.EmailVerified, opt => opt.Ignore())
                .ForMember(dest => dest.MobileVerified, opt => opt.Ignore());

            // Customer to UserEntity
            CreateMap<Customer, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Mobile, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmationKey, opt => opt.Ignore())
                .ForMember(dest => dest.MobileConfirmationKey, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmationExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.MobileConfirmationExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.IsMobileConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetKey, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .ForMember(dest => dest.System, opt => opt.Ignore())
                .ForMember(dest => dest.Birthday, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.Ignore())
                .ForMember(dest => dest.ActivationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.FacebookUserId, opt => opt.Ignore())
                .ForMember(dest => dest.GoogleUserId, opt => opt.Ignore())
                .ForMember(dest => dest.TwitterUserId, opt => opt.Ignore())
                .ForMember(dest => dest.B2BUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Json, opt => opt.MapFrom(src => src.Additional.ToString()));

            // CreateCustomer to UserEntity
            CreateMap<CustomerCreate, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmationKey, opt => opt.Ignore())
                .ForMember(dest => dest.MobileConfirmationKey, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmationExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.MobileConfirmationExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.IsMobileConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetKey, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .ForMember(dest => dest.Birthday, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.Ignore())
                .ForMember(dest => dest.ActivationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.FacebookUserId, opt => opt.Ignore())
                .ForMember(dest => dest.GoogleUserId, opt => opt.Ignore())
                .ForMember(dest => dest.TwitterUserId, opt => opt.Ignore())
                .ForMember(dest => dest.B2BUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Json, opt => opt.MapFrom(src => src.Additional.ToString()));

            // UserCreateEmail to CustomerCreate
            CreateMap<RegisterEmailUserCommand, CustomerCreate>();

            // UserCreateMobile to CustomerCreate
            CreateMap<RegisterMobileUserCommand, CustomerCreate>();
        }
    }
}
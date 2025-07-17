using AutoMapper;
using Core.Entities;
using ReverseProxyManager.DTOs;

namespace ReverseProxyManager.MappingProfiles
{
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            this.CreateMap<CertificateEntity, CertificateDto>()
                .ForMember(x => x.ServerEntity, m => m.MapFrom(x => x.ServerEntity));

            this.CreateMap<ServerEntity, ServerDto>()
                .ForMember(x => x.Certificate, m => m.MapFrom(x => x.Certificate));

            this.CreateMap<ServerEntity, IdNameDto>(MemberList.Destination);

            this.CreateMap<CertificateEntity, IdNameDto>(MemberList.Destination);
        }
    }
}

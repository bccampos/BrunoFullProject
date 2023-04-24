using bruno.Application.Authentication;
using bruno.Contracts.Authentication;
using Mapster;

namespace bruno.WebApi.Common.Mapping
{
    public class AuthetincationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest.Token, src => src.Token)
                .Map(dest => dest, src => src.user);
        }
    }
}

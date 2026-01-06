using api.Services;
using Image_Processing_WwwRoot.Interfaces;
using Image_Processing_WwwRoot.Services;

namespace api.Extensions;

public static class RepositoryServiceExtensions
{
    public static IServiceCollection AddRepositoryService(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IPhotoModifySaveService, PhotoModifySaveService>();
        services.AddScoped<IPhotoService, PhotoService>();

        return services;
    }
}

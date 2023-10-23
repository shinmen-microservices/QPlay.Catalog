using Microsoft.Extensions.DependencyInjection;
using QPlay.Catalog.Service.Constants;
using QPlay.Catalog.Service.Models.Entities;
using QPlay.Common.MongoDB;

namespace QPlay.Catalog.Service.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureMongo(this IServiceCollection services)
    {
        services.AddMongo().AddMongoRepository<Item>("items");
        return services;
    }

    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.READ,
                policy =>
                {
                    policy.RequireRole(Roles.ADMIN);
                    policy.RequireClaim(Claims.SCOPE, Claims.READ, Claims.FULL);
                }
            );

            options.AddPolicy(Policies.WRITE,
                policy =>
                {
                    policy.RequireRole(Roles.ADMIN);
                    policy.RequireClaim(Claims.SCOPE, Claims.WRITE, Claims.FULL);
                }
            );
        });

        return services;
    }
}
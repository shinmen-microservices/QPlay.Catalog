using Microsoft.Extensions.DependencyInjection;
using QPlay.Catalog.Service.Models.Entities;
using QPlay.Common.MongoDB;

namespace QPlay.Catalog.Service.Extensions;

public static class ServiceExtensions
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
}
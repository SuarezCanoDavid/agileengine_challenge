using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PicturesCaching.Business;
using PicturesCaching.Business.Configuration;
using PicturesCaching.DataAccess;
using System;

[assembly: FunctionsStartup(typeof(PhotosCaching.Startup))]

namespace PhotosCaching
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.Configure<PhotosFeedOptions>(options =>
            {
                options.ApiKey = configuration.GetValue<string>("apiKey");
            });

            builder.Services.AddHttpClient<IPhotosFeedService, PhotosFeedService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("BaseAddress"));
            });

            builder.Services.AddDbContext<ICacheDbContext, CacheDbContext>(options =>
            {
                options.UseSqlServer("name=Cache.ConnectionString");
            });
        }
    }
}

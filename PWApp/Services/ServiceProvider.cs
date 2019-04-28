using Microsoft.Extensions.DependencyInjection;
using PWApp.Services.Default;

namespace PWApp.Services
{
    public static class ServiceProvider
    {
        public static void Provide(this IServiceCollection services)
        {
            services.AddSingleton<IAccountService, AccountService>();
        }
    }
}
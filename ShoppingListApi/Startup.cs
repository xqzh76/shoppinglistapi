namespace ShoppingListApi
{
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using ShoppingListApi.Data;
    using ShoppingListApi.Infrastructure;
    using ShoppingListApi.Model;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => options.Filters.Add(typeof(ModelValidationFilter)))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSingleton<IRepository<Drink>>(new InMemoryDrinkRepository());
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(this.Configuration)
                .CreateLogger();

            loggerFactory.AddSerilog();

            app.UseMiddleware<CorrelationIdLoggingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false, // only for development purpose, disable https - for prod, https must be used
                AllowedScopes = { "shoppinglist.api" }
            });

            app.UseMvc();
        }
    }
}

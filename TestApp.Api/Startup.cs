using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestApp.Data;
using TestApp.Data.Interfaces;
using TestApp.Services;
using TestApp.Shared;

namespace TestApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            services.AddHttpContextAccessor();
            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connectionSettingsSection = Configuration.GetSection("ConnectionSettings");
            var connectionSettings = connectionSettingsSection.Get<DatabaseConnectionSettings>();

            services.AddScoped(provider =>
            {
                var logger = provider.GetService<ILogger<IDaoFactory>>();
                return DaoFactories.GetFactory(connectionSettings, logger);
            });

            services.AddScoped(provider =>
            {
                var daoFactory = provider.GetService<IDaoFactory>();
                var logger = provider.GetService<ILogger<DeliveryService>>();
                return new DeliveryService(daoFactory.DeliveryDao, logger);
            });

            services.AddScoped(provider =>
            {
                var daoFactory = provider.GetService<IDaoFactory>();
                var logger = provider.GetService<ILogger<ProductService>>();
                return new ProductService(daoFactory.ProductDao, logger);
            });

            services.AddScoped(provider =>
            {
                var daoFactory = provider.GetService<IDaoFactory>();
                var logger = provider.GetService<ILogger<ReportService>>();
                var productService = provider.GetService<ProductService>();
                return new CartService(daoFactory.CartDao, productService, logger);
            });

            services.AddScoped(provider =>
            {
                var daoFactory = provider.GetService<IDaoFactory>();
                var cartService = provider.GetService<CartService>();
                var logger = provider.GetService<ILogger<OrderService>>();
                return new OrderService(daoFactory.OrderDao, cartService, logger);
            });

            services.AddScoped(provider =>
            {
                var daoFactory = provider.GetService<IDaoFactory>();
                var logger = provider.GetService<ILogger<ReportService>>();
                return new ReportService(daoFactory.CartReportDao);
            });

            services.AddScoped(provider =>
            {
                var daoFactory = provider.GetService<IDaoFactory>();
                var logger = provider.GetService<ILogger<UserLocationService>>();
                return new UserLocationService(daoFactory.UserLocationDao, logger);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            
            app.UseCors();

            app.UseMvc();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShoppingCart
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddController();
            // use Scrutor to scan the ShoppingCart assembly
            // if there is a class implementing the IShoppingCartStore interface,
            // this code register it in the service collection adn ASP.NET will be able to
            //inject it into the ShoppingCartController constructor
            services.Scan(selector => 
                selector    
                    .FromAssemblyOf<StartUp>()
                    .AddClasses()
                    .AsImplementedInterFaces());
            // wrap Http calls made in ProductCatalogClient in a Polly policy
            // use polly's fluent API to set up a retry policy with exponential back-off
            // retry at most 3 times and for each time double the amount of waiting time before the next attempt
            services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(
                        3,
                        attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))
                    )); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

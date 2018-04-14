using GarphQl.Core.Query;
using GarphQl.Core.Schema;
using GarphQl.Core.Types;
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.WebSockets;

namespace GraphQl.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<EmployeeType>();
            services.AddSingleton<EmployeeQuery>();
            services.AddSingleton<EmployeeSchema>();
            services.AddSingleton<IDependencyResolver>(
                c => new FuncDependencyResolver(type => c.GetRequiredService(type)));
            services.AddGraphQLHttp();
            services.AddGraphQLWebSocket<EmployeeSchema>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseGraphQLHttp<EmployeeSchema>(new GraphQLHttpOptions());
            app.UseWebSockets();
            app.UseGraphQLWebSocket<EmployeeSchema>(new GraphQLWebSocketsOptions());
        }
    }
}

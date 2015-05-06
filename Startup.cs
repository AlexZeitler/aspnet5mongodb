using System;
using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using AspNet5MongoDb.Mappings;

namespace AspNet5MongoDb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {

            var configuration = new Configuration()
                       .AddJsonFile("config.json")
                       .AddEnvironmentVariables();
                       
            services.AddMvc();
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
            
            
            var mongoHostAddress = configuration.Get("MONGO_PORT_27017_TCP_ADDR");
            var mongoPort = configuration.Get("MONGO_PORT_27017_TCP_PORT");
            
            Console.WriteLine(mongoHostAddress);
            Console.WriteLine(mongoPort);
            
            IMongoClient client = null;
            if(!string.IsNullOrWhiteSpace(mongoHostAddress) && !(string.IsNullOrWhiteSpace(mongoPort))) {
                client = new MongoClient(string.Format("mongodb://{0}:{1}", mongoHostAddress, mongoPort));
            } else {
                   client   = new MongoClient(configuration.Get("mongodbconnectionString"));
            }
       
            var database = client.GetDatabase("aspnet5mongodb");

            services.AddInstance(database);
            services.ConfigureMvc(options =>
            {
                (options.OutputFormatters.First(f => f.Instance is JsonOutputFormatter).Instance as
                    JsonOutputFormatter).SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });


        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");

            var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

            AutoMapperConfiguration.Configure();


        }
    }
}

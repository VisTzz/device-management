using Confluent.Kafka;
using DevicesManagement.Application;
using DevicesManagement.Domain.Interfaces;
using DevicesManagement.Infrastructure;
using DevicesManagement.Infrastructure.Kafka;
using DevicesManagement.Infrastructure.Kafka.Consumers;
using DevicesManagement.Infrastructure.Kafka.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevicesManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<KafkaClientHandle>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<DeviceStatusesProducer<Null, string>>();
            services.AddSingleton<DeviceStatusesProducer<string, long>>();
            services.AddHostedService<DeviceCommandsConsumer>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<DeviceService>();
            services.AddHealthChecks()
                .AddNpgSql(Configuration.GetConnectionString("DefaultConnection"), name: "PostgreSQL");
                //.AddKafka(new Confluent.Kafka.ProducerConfig
                //{
                //    BootstrapServers = Configuration["Kafka:ProducerSettings:BootstrapServers"]
                //}, name: "Kafka");


            //services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<RequestTimerMiddleware>();
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}

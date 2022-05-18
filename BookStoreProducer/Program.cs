using BooksScrapping;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProducer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.Configure<BookClubSettings>(configuration.GetSection("BookClubSettings"));

                    services.AddScoped<BooksScrapper>();

                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        var jobKey = new JobKey("ParseAndSendDataJob");

                        q.AddJob<ParseAndSendDataJob>(opts => opts.WithIdentity(jobKey));

                        q.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity("ParseAndSendDataJob-trigger")
                            .WithSimpleSchedule(x => x
                                //.WithIntervalInHours(24)
                                .WithIntervalInSeconds(10)
                                .RepeatForever()));
                    });

                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);

                    //services.AddHostedService<Worker>();
                });
    }
}

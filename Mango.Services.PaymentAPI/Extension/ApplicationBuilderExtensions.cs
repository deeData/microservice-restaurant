using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//must add for extentions via DI
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mango.Services.PaymentAPI.Messaging;

namespace Mango.Services.PaymentAPI.Extension
{
    //must be static to build extension method
    public static class ApplicationBuilderExtensions
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        //extension method va DI
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app) 
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLfe = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            //when app starts, register new method "OnStart" 
            hostApplicationLfe.ApplicationStarted.Register(OnStart);
            hostApplicationLfe.ApplicationStopped.Register(OnStop);
            return app;
        }

        //Processor to start when app starts
        private static void OnStart() 
        {
            ServiceBusConsumer.Start();
        }
        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }


    }
}

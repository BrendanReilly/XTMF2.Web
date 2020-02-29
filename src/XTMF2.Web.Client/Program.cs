//     Copyright 2017-2020 University of Toronto
// 
//     This file is part of XTMF2.
// 
//     XTMF2 is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     XTMF2 is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Client.Services;
using XTMF2.Web.ApiClient;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Http.Connections.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace XTMF2.Web.Client
{
    /// <summary>
    ///     Main Program entry XTMF2.Web.Client
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            ConfigureLogger();
            var builder = CreateHostBuilder(args);
            AddServices(builder.Services);
            builder.RootComponents.Add<App>("app");
            await builder.Build().RunAsync();
        }

        /// <summary>
        /// Configure client side services.
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<AuthenticationClient>();
            services.AddScoped<XtmfAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, XtmfAuthenticationStateProvider>();
            services.AddScoped<ProjectClient>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<ModelSystemClient>();
            services.AddScoped<ModelSystemEditorClient>();
            services.AddSingleton<NotificationService>();
            services.AddLogging();
            services.AddBlazoredSessionStorage();
            services.AddAuthorizationCore();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>()
            };
        }

        /// <summary>
        ///     Sets up logger for the browser
        /// </summary>
        private static void ConfigureLogger()
        {
           
        }

        /// <summary>
        ///     Creates the blazor host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static WebAssemblyHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            return builder;
        }
    }
}
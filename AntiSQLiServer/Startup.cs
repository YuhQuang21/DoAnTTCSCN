using AntiSqlInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AntiSQLiServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private Sanitize _sanitize;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Sanitize>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            var wsOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };
            app.UseWebSockets(wsOptions);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/send")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await Send(context, webSocket);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
            });
        }

        private async Task Send(HttpContext context, WebSocket webSocket)
        {
            _sanitize = new Sanitize();
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
            if (result != null)
            {
                while (!result.CloseStatus.HasValue)
                {
                    string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                    var messages = await FormatQuery(msg);
                    Console.WriteLine($"Query: {messages[0]}");
                    Console.WriteLine($"Structure template:{messages[1]}");
                    Console.WriteLine("Query parsing............");
                    var resultSanitize = _sanitize.SanitizeQuery(messages[0]) != null ? _sanitize.SanitizeQuery(msg) : "This query is valid!";
                    Console.WriteLine($"Result: {resultSanitize}");
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{resultSanitize}")),
                        result.MessageType,
                        result.EndOfMessage,
                        System.Threading.CancellationToken.None
                        );
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
                }
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, System.Threading.CancellationToken.None);
        }
        private static async Task<List<String>> FormatQuery(String query)
        {
            List<String> result = new List<string>();
            var parameter = Regex.Split(query, "\\$");

            result.Add(parameter[0]);
            string originParams = parameter[0];
            for (int i = 1; i < parameter.Length; i++)
            {
                originParams = originParams.Replace(parameter[i], "@parameter");
            }
            originParams = originParams.Trim();
            result.Add(originParams);

            return result;
        }
    }
}
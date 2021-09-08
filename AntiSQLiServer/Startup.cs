using AntiSqlInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
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
                    Console.WriteLine($"Query input: {messages[0]}");
                    Console.WriteLine("-----------------------------------------------Structure Validate------------------------------------------------------");
                    Console.WriteLine($"Structure :{messages[1]}");
                    Console.WriteLine("----------------------------------------------Parameter separation-----------------------------------------------------");
                    Console.WriteLine("Parmameter:");
                    for (int i = 2; i < messages.Count; i++)
                    {
                        Console.WriteLine($"\t{messages[i]}");
                    }
                    var resultSanitize = _sanitize.SanitizeQuery(messages[0]) != null ? _sanitize.SanitizeQuery(msg) : "This query is valid!";
                    Console.WriteLine($"Result: {resultSanitize}");
                    if (resultSanitize.Contains("This query is valid!"))
                    {
                        Console.WriteLine($"=> Accepted:{messages[0]}");
                    }
                    else
                    {
                        Console.WriteLine($"=> Denied:{messages[0]}");
                        var isLog = await Logger(new string[] { messages[0], resultSanitize });
                        if (isLog) Console.WriteLine("Log succsess!");
                        else Console.WriteLine("Log fail!");
                    }
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------\n");
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
            for (int i = 1; i < parameter.Length; i++)
            {
                result.Add(parameter[i]);
            }
            return result;
        }
        private static async Task<bool> Logger(string[] query)
        {
            using (FileStream fileStream = new FileStream(@"C:\Users\BUI DOAN QUANG HUY\Desktop\New folder (2)\DoAnTTCSCN\AntiSQLiServer\Log\Log.txt", FileMode.Append))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                try
                {
                    foreach (var item in query)
                    {
                        await streamWriter.WriteAsync(item);
                    }
                    await streamWriter.WriteAsync("\n");
                    streamWriter.Close();
                    fileStream.Close();
                    return true;
                }
                catch (Exception)
                {
                }
                finally
                {
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
            return false;
        }
    }
}
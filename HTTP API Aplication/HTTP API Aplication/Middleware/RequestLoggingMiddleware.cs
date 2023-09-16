using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Čitanje zahtjeva
        context.Request.EnableBuffering();
        var requestBodyStream = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var requestBodyText = await requestBodyStream.ReadToEndAsync();
        context.Request.Body.Position = 0;

        // Ispis detalja zahtjeva
        Console.WriteLine("Request Line:");
        Console.WriteLine($"Method: {context.Request.Method}");
        Console.WriteLine($"URL: {context.Request.Host + context.Request.Path}");
        Console.WriteLine($"HTTP Version: {context.Request.Protocol}");
        Console.WriteLine();

        Console.WriteLine($"Request Headers:");
        foreach (var header in context.Request.Headers)
        {
            Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
        }
        Console.WriteLine();

        Console.WriteLine($"Request Body:");
        Console.WriteLine(requestBodyText);
        Console.WriteLine("\n");

        await _next(context);
    }
}

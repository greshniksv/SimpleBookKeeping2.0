using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Text;
using Front.Extensions;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

await app.LoadFilesAsync();

app.UseFileServer(new FileServerOptions {
	FileProvider = new PhysicalFileProvider(
		Path.Combine("C:\\Data\\Projects\\SimpleBookKeeping2.0\\Front\\src")),
	RequestPath = "/main",
	DefaultFilesOptions = { DefaultFileNames = new List<string> { "index.html" } },
	EnableDefaultFiles = true
});

//app.MapGet("/", () => "Hello World!");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
	_ = endpoints.RedirectPermanent("/", "/main");
});

app.Run();



public static class EndpointExtensions
{
	public static IEndpointRouteBuilder Redirect(
		this IEndpointRouteBuilder endpoints,
		string from, string to)
	{
		return Redirect(endpoints,
			new Redirective(from, to));
	}

	public static IEndpointRouteBuilder RedirectPermanent(
		this IEndpointRouteBuilder endpoints,
		string from, string to)
	{
		return Redirect(endpoints,
			new Redirective(from, to, true));
	}

	public static IEndpointRouteBuilder Redirect(
		this IEndpointRouteBuilder endpoints,
		params Redirective[] paths
	)
	{
		foreach (var (from, to, permanent) in paths)
		{
			endpoints.MapGet(from, async http => { http.Response.Redirect(to, permanent); });
		}

		return endpoints;
	}
}

public record Redirective(string From, string To, bool Permanent = false);
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseStaticFiles();

var pwd = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

var output = new StreamWriter("C:\\Data\\Projects\\SimpleBookKeeping2.0\\Front\\src\\libs\\sbk\\sbk.min.js", Encoding.UTF8,
	new FileStreamOptions() { Mode = FileMode.Create, Access = FileAccess.Write});

await output.WriteLineAsync($"\n\n// -= Auto generated {DateTime.Now:dd-MM-yyy HH:mm:ss} =-");
var directory = new DirectoryInfo("C:\\Data\\Projects\\SimpleBookKeeping2.0\\Front\\src\\libs\\sbk");
foreach (FileInfo file in directory.GetFiles("*.js", SearchOption.AllDirectories))
{
	if (file.Name.Contains("sbk.min.js"))
	{
		continue;
	}

	await output.WriteLineAsync($"\n\n// #### {file.Name}");
	await output.FlushAsync();

	var openStream = file.OpenRead();
	await openStream.CopyToAsync(output.BaseStream);
	await output.FlushAsync();
}

await output.FlushAsync();
output.Close();

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
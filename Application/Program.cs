using System.Reflection;
using System.Text.Json.Serialization;
using Application.Extensions;
using BLL.AutoMapperProfiles;
using BLL.Options;
using DAL.DbContexts;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json")
	.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
	.Build();

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(configuration)
	.CreateLogger();

bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
builder.WebHost.UseUrls(configuration["UseUrls"]);

// Add services to the container.
builder.Services.AddControllers(options =>
{
	//options.Filters.Add<ActionFilter>();
	//options.Conventions.Add(new NamespaceRoutingConvention());
}).AddJsonOptions(x =>
	{
		x.JsonSerializerOptions.WriteIndented = !isProduction;
		x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	}
).AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2",
		new OpenApiSecurityScheme {
			Type = SecuritySchemeType.OAuth2,
			Flows = new OpenApiOAuthFlows {
				Password = new OpenApiOAuthFlow {
					TokenUrl = new Uri($"{builder.Configuration["BaseApplicationUrl"]}/connect/token"),
				}
			}
		});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement {
			{
				new OpenApiSecurityScheme {
					Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
					Scheme = "oauth2",
					Name = "Bearer",
					In = ParameterLocation.Header,
				},
				new List<string>()
			}
		});
	
	options.TagActionsBy(api =>
	{
		if (api.GroupName != null)
		{
			return new[] { api.GroupName };
		}

		var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
		if (controllerActionDescriptor != null)
		{
			return new[] { controllerActionDescriptor.ControllerName };
		}

		throw new InvalidOperationException("Unable to determine tag for endpoint.");
	});
	options.DocInclusionPredicate((_, _) => true);

	// Set the comments path for the Swagger JSON and UI.
	//string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	//string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	//options.IncludeXmlComments(xmlPath);
	//options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BLL.xml"));
});

builder.Services.AddMediatR(serviceConfiguration =>
{
	serviceConfiguration.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});
builder.Services.AddDbContext<MainContext>(opt =>
{
	opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
		.UseSnakeCaseNamingConvention();
});

//builder.Services.AddDbContext<MainContext>(opt => opt.UseInMemoryDatabase("TestDb"));
builder.Services.AddAutoMapper(typeof(DbToViewModelProfile), typeof(CommandToDbModelProfile));
builder.Services.AddRepositories();

// Register logger
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// Register DB context
builder.Services.AddScoped<IMainContext, MainContext>();

// Register service provider
IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
builder.Services.AddSingleton<IServiceProvider>(serviceProvider);

// Register settings
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.Name));

WebApplication app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

using IServiceScope scope = app.Services.CreateScope();
MainContext mainContext = scope.ServiceProvider.GetRequiredService<MainContext>();
mainContext.GetDatabase().Migrate();

app.ConfigureExceptionHandler(Log.Logger);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

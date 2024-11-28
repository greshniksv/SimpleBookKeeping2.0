using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application;
using Application.Extensions;
using BLL.AutoMapperProfiles;
using BLL.Interfaces;
using BLL.Options;
using DAL.DbContexts;
using DAL.Interfaces;
using DAL.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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

builder.Services.AddValidatorsFromAssembly(typeof(ICommand<>).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddInternalServices();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
		policy =>
		{
			policy.WithOrigins("https://localhost:6001", "https://localhost:5033", "https://localhost:7061")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//	//.AddCertificate(options => options.AllowedCertificateTypes = CertificateTypes.All)
//	.AddJwtBearer(o =>
//	{
//		o.Authority = builder.Configuration["BaseApplicationUrl"];
//		o.TokenValidationParameters = new TokenValidationParameters {
//			ValidateAudience = false
//		};
//	});

builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddCertificate(options => options.AllowedCertificateTypes = CertificateTypes.All)
	.AddJwtBearer(o =>
	{
		o.Authority = builder.Configuration["BaseApplicationUrl"];
		o.RequireHttpsMetadata = Convert.ToBoolean(configuration["Jwt:RequireHttpsMetadata"]);
		o.TokenValidationParameters = new TokenValidationParameters {
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = false,
			ValidateIssuerSigningKey = false
		};
		o.BackchannelHttpHandler = new HttpClientHandler {
			ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
		};
	});

builder.Services.AddAuthorization();

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

	// Set the comments path for the Swagger JSON and UI.
	string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BLL.xml"));
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DAL.xml"));
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

// Register service provider
builder.Services.AddDbContext<IdentityContext>(opt =>
	{
		opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
			.UseSnakeCaseNamingConvention();
	})
	.AddIdentity<ApplicationUser, ApplicationRole>(config =>
	{
		config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789\\";
		config.Password.RequireDigit = false;
		config.Password.RequiredLength = 3;
		config.Password.RequireLowercase = false;
		config.Password.RequireUppercase = false;
		config.Password.RequireNonAlphanumeric = false;
	})
	.AddEntityFrameworkStores<IdentityContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
	.AddAspNetIdentity<ApplicationUser>()
	.AddJwtBearerClientAuthentication()
	.AddProfileService<ProfileService>()
	.AddDeveloperSigningCredential()        //This is for dev only scenarios when you don’t have a certificate to use.
	.AddInMemoryApiScopes(Config.ApiScopes)
	.AddInMemoryClients(Config.GetClients());

//builder.Services.AddIdentityServer(options => options.Authentication.CookieAuthenticationScheme = "authCookie")
//	.AddAspNetIdentity<ApplicationUser>()
//	.AddOperationalStore(options =>
//	{
//		options.ConfigureDbContext = b =>
//			b.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"),
//					sql => sql.MigrationsAssembly("DAL"))
//				.UseSnakeCaseNamingConvention();

//		// this enables automatic token cleanup. this is optional.
//		options.EnableTokenCleanup = true;
//		options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
//		options.DefaultSchema = DbSchema.IdentityServer;
//	})
//	.AddConfigurationStore(options =>
//	{
//		options.ConfigureDbContext = b =>
//			b.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"),
//					sql => sql.MigrationsAssembly("DAL"))
//				.UseSnakeCaseNamingConvention();
//		options.DefaultSchema = DbSchema.IdentityServer;
//	})
//	.AddJwtBearerClientAuthentication()
//	.AddProfileService<ProfileService>()
//	.AddDeveloperSigningCredential()
//	.AddInMemoryApiScopes(Config.ApiScopes)
//	.AddInMemoryClients(Config.GetClients()); //builder.Configuration.GetSection(CorsOptions.Name)

//builder.Services.AddDbContext<MainContext>(opt => opt.UseInMemoryDatabase("TestDb"));
builder.Services.AddAutoMapper(typeof(DbToViewModelProfile), typeof(CommandToDbModelProfile));
builder.Services.AddRepositories();
builder.Services.AddApiVersioning();

// Register logger
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// Register DB context
builder.Services.AddScoped<IMainContext, MainContext>();
builder.Services.AddScoped<IIdentityContext, IdentityContext>();

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
	app.UseSwaggerUI(options =>
	{
		options.OAuthClientId("client");
	});
}

using IServiceScope scope = app.Services.CreateScope();
MainContext mainContext = scope.ServiceProvider.GetRequiredService<MainContext>();
mainContext.GetDatabase().Migrate();
IIdentityContext identityContext = scope.ServiceProvider.GetRequiredService<IIdentityContext>();
identityContext.GetDatabase().Migrate();

app.UseCors(MyAllowSpecificOrigins);
app.UseRouting();
app.UseIdentityServer();
app.ConfigureExceptionHandler(Log.Logger);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

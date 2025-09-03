using AutoMapper;
using ForParts.Data;
using ForParts.DTOs.Supply;
using ForParts.IRepository.Auth;
using ForParts.IRepository.Supply;
using ForParts.IService.Auth;
using ForParts.IServices.Image;
using ForParts.IServices.Supply;
using ForParts.JWT;
using ForParts.Models.Supply;
using ForParts.Repository.Auth;
using ForParts.Repositorys.Supply;
using ForParts.Service.Auth;
using ForParts.Services.Auth;
using ForParts.Services.Image;
using ForParts.Services.Supply;
using ForParts.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Microsoft.SqlServer.Management.Sdk.Sfc.RequestObjectInfo;
using System.Text.Json.Serialization;
using System.Text.Json;
using ForParts.IRepository.Invoice;
using ForParts.Repository.Invoice;
using ForParts.IRepository.Customer;
using ForParts.Repository.Customer;
using ForParts.IRepository.Product;
using ForParts.Repository.Product;
using ForParts.IService.Client;
using ForParts.IService.Invoice;
using ForParts.Service.Invoice;
using ForParts.IService.Product;
using ForParts.Services.Product;
using ForParts.Service.Client;
using ForParts.IRepository.Budget;
using ForParts.Repository.Budget;
using ForParts.IService.Buget;
using ForParts.Service.Buget;
using ProfileAlias = ForParts.Models.Supply.Profile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Leer la clave JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["JWTSettings:Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//Hacemos esta modificacion del manejo de validacion en ModelState. Se explica todo en la clase ValidationFormatter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFormatter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new ForParts.Converters.DireccionJsonConverter());
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped(typeof(ISupplyRepository<>), typeof(SupplyRepository<>));
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplyExisting, SupplyRepository<ProfileAlias>>();
builder.Services.AddScoped<ISupplyExisting, SupplyRepository<Glass>>();
builder.Services.AddScoped<ISupplyExisting, SupplyRepository<Accessory>>();

builder.Services.AddScoped<IServiceAuth, ServiceAuth>();
builder.Services.AddScoped<IServiceEmailAuth, EmailAuthService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ISupplyService<ProfileDto>, ProfileService>();
builder.Services.AddScoped<ISupplyService<AccessoryDto>, AccessoryService>();
builder.Services.AddScoped<ISupplyService<GlassDto>, GlassService>();
builder.Services.AddScoped<IZureoInvoiceService, ZureoInvoiceService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IBudgetCalculator, CalculadoraPresupuestoRepositorio>();
builder.Services.AddScoped<IFormulaRepository, FormulaRepositorio>();


// Registrar IHttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<Token>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<JwtSettingsConfirmation>(builder.Configuration.GetSection("JwtSettingsConfirmation"));
builder.Services.AddDbContext<ContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseStaticFiles(); //USO DE IMAGENES

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
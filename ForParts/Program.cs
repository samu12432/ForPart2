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
using ForParts.Services.Budget;
using ProfileAlias = ForParts.Models.Supply.Profile;
using ForParts.IService.Buget;
using ForParts.Services.Budget;
using ForParts.IRepository;
using ForParts.Repository;
using ForParts.IRepository.Budget;
using ForParts.Repository.Budget;
using ForParts.Service.Budget;

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
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IFormulaRepository, FormulaRepositorio>();
builder.Services.AddScoped<IBudgetPdfService, BudgetPdfService>();

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
builder.Services.AddScoped<IBudgetCalculator, CalculadoraPresupuestoRepositorio>();



// --- CORS: una sola política coherente ---
const string CorsFront = "CorsFront";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsFront, policy => policy
       .WithOrigins(
            "https://front.edaberturas.lat",
            "http://localhost:5175",
            "https://localhost:7001",
            "http://localhost:7000",
            "http://localhost:41245",
            "https://localhost:44336"
)

        .AllowAnyHeader()
        .AllowAnyMethod()
    // .AllowCredentials() // Habilitalo SOLO si usás cookies/sesión desde el front
    );
});



// Registrar IHttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<Token>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<JwtSettingsConfirmation>(builder.Configuration.GetSection("JwtSettingsConfirmation"));
builder.Services.AddDbContext<ContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContextDb>();
    db.Database.Migrate();
}

app.UseStaticFiles(); //USO DE IMAGENES

app.UseSwagger();
app.UseSwaggerUI();
//app.UseCors("OpenCors");       // <-- aplica CORS abierto
// Manejo genérico de preflight (OPTIONS) si tu app no los mapea
app.UseRouting();
app.UseCors(CorsFront);
//app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();






using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlantShopAPI;
using PlantShopAPI.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add Identity services to the container
builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCredentials", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Add accepted domain name
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Activate Identity APIs
/*builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<CustomDbContext>();*/
builder.Services.AddIdentityApiEndpoints<CustomUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<CustomDbContext>();

// By default, both cookies and proprietary tokens are activated.
// Cookies and tokens are issued at login if the useCookies query string parameter in the login endpoint is true.


// Add services to the container.
builder.Services.AddControllers();
/*builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseInMemoryDatabase("AppDb"));*/
builder.Services.AddDbContext<CustomDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Map Identity routes
/*app.MapIdentityApi<IdentityUser>();*/
app.MapIdentityApi<CustomUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowCredentials");

app.MapControllers();

app.Run();

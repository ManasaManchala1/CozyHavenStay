using Cozy_Haven.Contexts;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Repository;
using Cozy_Haven.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Cozy_Haven
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.WriteIndented = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", opts =>
                {
                    opts.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
                });
            });

            #region Context
            builder.Services.AddDbContext<CozyHavenContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("CozyHavenConnection"));
            });
            #endregion
            #region Repository Injection
            builder.Services.AddScoped<IRepository<string,User>,UserRepository>();
            builder.Services.AddScoped<IRepository<int,Hotel>,HotelRepository>();
            builder.Services.AddScoped<IRepository<int,Room>,RoomRepository>();
            builder.Services.AddScoped<IRepository<int,Booking>,BookingRepository>();
            builder.Services.AddScoped<IRepository<int,Review>,ReviewRepository>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            #endregion
            #region Service Injection
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IHotelService,HotelService>();
            builder.Services.AddScoped<IRoomService,RoomService>();
            builder.Services.AddScoped<IBookingService,BookingService>();
            builder.Services.AddScoped<IReviewService,ReviewService>();
            builder.Services.AddScoped<IPaymentService,PaymentService>();

            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("ReactPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}


using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Start.Blog.Extensions;
using Start.Blog.Helpers;
using Start.Blog.Managers;
using Start.Blog.Services;

namespace Start.Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ISqlHelper<>), typeof(MysqlHelper<>));
            services.AddScoped(typeof(IUserManager<>), typeof(UserManager<>));
            services.AddScoped<IUserService, UserService>();

            services.AddHttpContextAccessor();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = "http://localhost:5000/",
                        ValidateIssuer = true,
                        ValidIssuer = "http://localhost:5000/",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(30),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(BlogConsts.JwtKey)),
                    };
                });

            services.AddControllersWithViews().ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(context.ModelState.GetError());
            }).AddRazorRuntimeCompilation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Start.Blog", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    //���������ͷ��
                    In = ParameterLocation.Header,
                    //ʹ��Authorizeͷ��
                    Type = SecuritySchemeType.Http,
                    //����Ϊ�� bearer��ͷ
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                //�����з�������Ϊ����bearerͷ����Ϣ
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        new string[] {}
                    }
                };

                //ע�ᵽswagger��
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Start.Blog v1"));
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

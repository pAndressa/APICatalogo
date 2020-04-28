using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace APICatalogo
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
            var mappingConfig = new MapperConfiguration(mc=> {
                mc.AddProfile(new MappingProfile());
            });

            services.AddCors(options => 
            {
                options.AddPolicy("PermitirApiRequest",
                    builder => builder.WithOrigins("https://apirequest.io/").WithMethods("GET")
                    );        
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddScoped<ApiLoggingFilter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = Configuration["TokenConfiguration:Audience"],
                     ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                 });

            //Swagger
            services.AddSwaggerGen(c=> 
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "APICatalogo",
                    Description = "Catálogo  de Produtos e Categorias",
                    TermsOfService = new Uri("https://localdotermo/terms"),
                    Contact = new OpenApiContact 
                    { 
                        Name = "Andressa",
                        Email ="andressa@hotmail.com",
                        Url = new Uri("https://contatoapi"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre XXX",
                        Url = new Uri("https://licencaapi/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);//vai ler e interpretar os comentários
            });

            //Versão
            services.AddApiVersioning(options=>
            {
                //usa versão padrão
                options.AssumeDefaultVersionWhenUnspecified = true;

                //define qual a versão padrão
                options.DefaultApiVersion = new ApiVersion(1, 0);                
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddControllers().AddNewtonsoftJson(option => {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //adiciona o middleware do tratamento de erros
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            //adiciona o middleware de autenticação
            app.UseAuthentication();

            //adiciona o middleware que habilita a autorização
            app.UseAuthorization();

            //swagger
            app.UseSwagger();

            app.UseSwaggerUI(c=> 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","APICatalogo");
            });

            // app.UseCors(opt=> opt.WithOrigins("https://apirequest.io/").WithMethods("GET")); se não for necessário para um controlador ou action específico, definir por aqui
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

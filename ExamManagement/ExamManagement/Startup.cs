using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using ExamManagement.Helper;

namespace ExamManagement
{
    public class Startup
    {
        #region Configuration
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #endregion

        #region IServiceCollectionServices
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews().AddSessionStateTempDataProvider();
            services.AddSession();

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<ExamManagementContext>(options => options.UseSqlite(@"Data Source=C:\Users\alifd\Downloads\sqlitestudio-3.3.3\SQLiteStudio\LocalDB.db"));
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, Authorization>("BasicAuthentication", null);
            services.AddScoped<Interfaces.IGetWebPageContent, DataParser>();

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API Docs",
                    Version = "v1"
                ,
                    Description = "<p>You can integrate Product Management APIs on web and mobile devices in a world-class structure.</p>"
                + "<h2>Beginning</h2>"
                + "<p>You can quickly integrate and execute your products by sending data to these services in JSON format and responding to service responses in JSON format. With this guide, you will be able to easily perform integration steps in any language of software.</ p>"
                + "<h2>API Base Endpoint Addresses</h2>"
                + @"<table><thead><tr><th width=""20%"">Service Name</th><th width=""10%""> Service Type </th><th width=""70%""> Address </th></tr></thead><tbody style=""background: lightgray;""><tr><td> Product Management's Services</td><td> REST </td><td> http://localhost:52003/ </td></tr></tbody></table>"

                + "<h2>API Security</h2>"
                + @"<p>
                Product Management APIs is use Simple Basic authentication for API security.
                Authentication information must be sent on the Get/Post header.
                You can find in below the necessary information to create the authentication key;
            </p>
            <p><i style=""color: #92294F"">AuthenticationKey </i>= <i style=""color: #2dbfa7"">ConvertToBase64</i>(username+"":""+password)</p>
            <p>
                Authentication should be sent on the header as follows;
            </p>
            <p>Authorization: Basic AuthenticationKey</p>
            <p>
                <i class=""testt"" style=""color: #92294F"">*</i><i style=""color: grey"">You can find your API keys in API User Info page after signed in.</i>
            </p>"
                });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

            });

        }
        #endregion

        #region IApplicationBuilderApp
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.DefaultModelsExpandDepth(-1);
                c.RoutePrefix = string.Empty;
                c.InjectStylesheet("/Assests/css/customStyle.css");
                c.InjectJavascript("/Assests/js/custom.js");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();


            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        #endregion
    }
}

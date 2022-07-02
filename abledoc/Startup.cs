using abledoc.Resources;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace abledoc
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            services.AddControllersWithViews();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(365);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            #region Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<LocalizationService>();
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ApplicationResource", assemblyName.Name);
                    };
                });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] {
                                    new CultureInfo("en"),
                                    new CultureInfo("fr"),
                                    new CultureInfo("de"),
                                    new CultureInfo("da"),
                                    new CultureInfo("es")
                                };
                options.DefaultRequestCulture = new RequestCulture("en");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.

                // options.SupportedCultures = supportedCultures; // THIS OPTION TEMPORARY COMMENT FOR DECIMAL NUMBER SHOW IN COMMA SEPRATED

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            //app.UseCookiePolicy();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMvc();
            var requestlocalizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(requestlocalizationOptions.Value);

            app.UseAuthorization();

            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "alttxtfile",
                  pattern: "alttxtfile",
                  defaults: new { controller = "phases", action = "AltTxtFile" }
                  );
                endpoints.MapControllerRoute(
                   name: "phase1",
                   pattern: "phases/phase1",
                   defaults: new { controller = "phases", action = "tagging" }
                   );
                endpoints.MapControllerRoute(
                    name: "phase2",
                    pattern: "phases/phase2",
                    defaults: new { controller = "phases", action = "review" }
                    );
                endpoints.MapControllerRoute(
                   name: "alttext",
                   pattern: "phases/alttext",
                   defaults: new { controller = "phases", action = "alttxt" }
                   );
                endpoints.MapControllerRoute(
                    name: "phase4",
                    pattern: "phases/phase4",
                    defaults: new { controller = "phases", action = "final" }
                    );
                endpoints.MapControllerRoute(
                    name: "phase3",
                    pattern: "phases/phase3",
                    defaults: new { controller = "phases", action = "qc" }
                    );
                endpoints.MapControllerRoute(
                    name: "menu",
                    pattern: "settings/menu",
                    defaults: new { controller = "menu", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "quotecontent",
                    pattern: "settings/quotecontent",
                    defaults: new { controller = "quotecontent", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "emailtemplate",
                    pattern: "settings/emailtemplate",
                    defaults: new { controller = "emailtemplate", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "state",
                    pattern: "settings/state",
                    defaults: new { controller = "state", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "unit",
                    pattern: "settings/unit",
                    defaults: new { controller = "unit", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "countries",
                    pattern: "settings/countries",
                    defaults: new { controller = "countries", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "managecompany",
                    pattern: "settings/managecompany",
                    defaults: new { controller = "managecompany", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "description",
                    pattern: "settings/description",
                    defaults: new { controller = "discriptionmaster", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "role",
                    pattern: "settings/role",
                    defaults: new { controller = "role", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "closedjobs",
                    pattern: "more/closedjobs",
                    defaults: new { controller = "closedjobs", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "cancelledjobs",
                    pattern: "more/cancelledjobs",
                    defaults: new { controller = "cancelledjobs", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "PendingInvoicesJobs",
                    pattern: "more/pendinginvoicesjobs",
                    defaults: new { controller = "PendingInvoicesJobs", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "ToBeInvoiced",
                    pattern: "invoices/tobeinvoiced",
                    defaults: new { controller = "ToBeInvoiced", action = "Index" }
                   );
                endpoints.MapControllerRoute(
                    name: "PendingInvoices",
                    pattern: "invoices/pendinginvoices",
                    defaults: new { controller = "PendingInvoices", action = "Index" }
                   );

                endpoints.MapControllerRoute(
                   name: "ApprovedInvoices",
                   pattern: "invoices/approvedinvoices",
                   defaults: new { controller = "ApprovedInvoices", action = "Index" }
                  );

                endpoints.MapControllerRoute(
                   name: "PendingCreditNotes",
                   pattern: "invoices/pendingcreditnotes",
                   defaults: new { controller = "PendingCreditNotes", action = "Index" }
                  );

                endpoints.MapControllerRoute(
                  name: "ApprovedCreditNotes",
                  pattern: "invoices/approvedcreditnotes",
                  defaults: new { controller = "ApprovedCreditNotes", action = "Index" }
                 );
                endpoints.MapControllerRoute(
                  name: "TimesheetView",
                  pattern: "timesheetview",
                  defaults: new { controller = "ApprovedTimesheet", action = "TimesheetView" }
                 );

                endpoints.MapControllerRoute(
                   name: "adgateway",
                   pattern: "adgateway",
                   defaults: new { controller = "gateways", action = "Index" }
                  );

                endpoints.MapControllerRoute(
                   name: "adsales",
                   pattern: "adsales",
                   defaults: new { controller = "ADSales", action = "Index" }
                  );

                endpoints.MapControllerRoute(
                   name: "adsales1",
                   pattern: "adsales1",
                   defaults: new { controller = "ADSales", action = "adsales1" }
                  );

                endpoints.MapControllerRoute(
                 name: "areas",
                  pattern: "{area}/{controller}/{did?}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

               
            });

            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);



        }
    }
}

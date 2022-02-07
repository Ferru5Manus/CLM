using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Features;

namespace SchoolSite100._0
{
    public class Startup
    {
      

     

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddSingleton<AutentificationManager>();
            services.AddSingleton<NewsManager>();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/auth"));
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            List<Claim> claims;

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapGet("/", async context =>
                {
                    string page = File.ReadAllText("Pages/htmlpage.html");
                    await context.Response.WriteAsync(page);
                });
               
                endpoints.MapGet("/auth", async context =>
                {
                    string page = File.ReadAllText("Pages/loginPage.html");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapPost("/registration", async context =>
                {
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                   am.Register(query.loginString,query.passwordString,query.emailString) ;
                    await context.Response.WriteAsJsonAsync(am.IsRegistred(query.loginString, query.passwordString, query.emailString));
                });
                endpoints.MapPost("/login", async context => {
                    var credentials = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    // с заданным логином и паролем мы пойдем в базу
                    // если в базе есть пользователь, то всё ок, если нет, то ничего не делаем
                    var lm = app.ApplicationServices.GetService<AutentificationManager>();
                    if (lm.IsRegistred(credentials.loginString,credentials.passwordString) == true)
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, credentials.loginString)
                        };
                        // создаем объект ClaimsIdentity
                        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                        // добавляем куки нашему пользователю
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                        
                        // перенаправляем на нужную сраницу
                        context.Response.Redirect("/mainPage");
                    }

                    await context.Response.WriteAsync(credentials.loginString);
                });
               
                endpoints.MapGet("/adminUsersPage", async context =>
                {
                    string page = File.ReadAllText("Pages/htmlpage1.html");
                    string page2 = File.ReadAllText("Pages/MainPage.html");
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    if (sm.IsAdmin(login))
                    {
                        await context.Response.WriteAsync(page);
                    }
                    else
                    {
                        await context.Response.WriteAsync(page2);
                    }
                   
                }).RequireAuthorization();
                endpoints.MapGet("/mainPage", async context =>
                {
                    string page = File.ReadAllText("Pages/MainPage.html");

                    await context.Response.WriteAsync(page);
                });
                endpoints.MapPost("/IsAdm", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;

                    await context.Response.WriteAsJsonAsync(sm.IsAdmin(login));
                });
                endpoints.MapGet("/loginPage", async context =>
                {
                    string page = File.ReadAllText("Pages/loginPage.html");

                    await context.Response.WriteAsync(page);
                });
                endpoints.MapGet("/recoveryPage", async context =>
                {
                    string page = File.ReadAllText("Pages/RecoveryPage.html");

                    await context.Response.WriteAsync(page);
                });
                endpoints.MapPost("/regCheck",async context => {
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    await context.Response.WriteAsJsonAsync(am.IsRegistred(query.loginString, query.passwordString));

                });
                endpoints.MapPost("/regRecCheck", async context => {
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    await context.Response.WriteAsJsonAsync(am.IsExists(query.emailString));

                });
                endpoints.MapPost("/prepareRecovery", async context => {
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    am.SendPassword(query.emailString);
                    await context.Response.WriteAsJsonAsync("ok");
                });

                endpoints.MapGet("/getLogins", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var currLogin = sm.GetUserLogins();

                    await context.Response.WriteAsJsonAsync(currLogin);
                });
                endpoints.MapGet("/getIds", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var currId = sm.GetUserIds();

                    await context.Response.WriteAsJsonAsync(currId);
                });
                endpoints.MapGet("/getEmails", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var currEmail = sm.GetUserEmails();

                    await context.Response.WriteAsJsonAsync(currEmail) ;
                });
                endpoints.MapGet("/getRoles", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var currRole = sm.GetUserRoles();

                    await context.Response.WriteAsJsonAsync(currRole);
                });
                endpoints.MapGet("/getForms", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var currForm = sm.GetUserForms();

                    await context.Response.WriteAsJsonAsync(currForm);
                });
                endpoints.MapGet("/newsPage", async context =>
                {
                    string page = File.ReadAllText("site/newsPage.html");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapPut("/changeLogin", async context => 
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    sm.UpdateLogin(query.Id, query.loginString);
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPut("/changeEmail", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    sm.UpdateEmail(query.Id, query.emailString);
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPut("/changeForm", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    sm.UpdateForm(query.Id, query.formString);
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPut("/changeRole", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    sm.UpdateRole(query.Id, query.roleString);
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPost("/getTitles", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var currTitle = sm.GetNewsTit();

                    await context.Response.WriteAsJsonAsync(currTitle);
                });
                endpoints.MapPost("/getNews", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var currNew = sm.GetNewsText();

                    await context.Response.WriteAsJsonAsync(currNew);
                });
                endpoints.MapPost("/getIds", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var currTitle = sm.GetNewsId();

                    await context.Response.WriteAsJsonAsync(currTitle);
                });
                endpoints.MapPost("/removeNew", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var query = await context.Request.ReadFromJsonAsync<NewTemplate>();
                    sm.RemoveNew(query.Id);
                });
                endpoints.MapPost("/addNew", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var query = await context.Request.ReadFromJsonAsync<NewTemplate>();
                    sm.AddNews(query.titleString, query.newString);
                });
                endpoints.MapGet("/adminMainPage", async context =>
                {
                    string page2 = File.ReadAllText("Pages/MainPage.html");
                    string page = File.ReadAllText("Pages/adminMainPage.html");
                    var login = context.User.Identity.Name;
                    var lm = app.ApplicationServices.GetService<AutentificationManager>();

                    
                    if (lm.IsAdmin(login))
                    {
                        await context.Response.WriteAsync(page);
                    }
                    else
                    {
                        await context.Response.WriteAsync(page2);
                    }


                }).RequireAuthorization(); 
               
                endpoints.MapPut("/changeTitle", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var query = await context.Request.ReadFromJsonAsync<NewTemplate>();
                    sm.ChangeTitle( query.newTitle, query.Id);
                });
                endpoints.MapPut("/changeNew", async context =>
                {
                    var sm = app.ApplicationServices.GetService<NewsManager>();
                    var query = await context.Request.ReadFromJsonAsync<NewTemplate>();
                    sm.ChangeText( query.newNewString, query.Id);
                });
            });
        }
    }
}

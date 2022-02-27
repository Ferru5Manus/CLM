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
            services.AddSingleton<FormsManager>();
            services.AddSingleton<TaskManager>();
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
                endpoints.MapGet("/taskAdmPage", async context =>
                {
                    string page = File.ReadAllText("Pages/taskAdminPage.html");
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    string page2 = File.ReadAllText("Pages/MainPage.html");
                    if (sm.GetRole(login)=="Admin" || sm.GetRole(login)=="Teacher")
                    {
                        await context.Response.WriteAsync(page);
                    }
                    else
                    {
                        await context.Response.WriteAsync(page2);
                    }
                }).RequireAuthorization();

                endpoints.MapGet("/taskPage", async context =>
                {
                    string page = File.ReadAllText("Pages/TaskPage.html");
                    await context.Response.WriteAsync(page);
                }).RequireAuthorization();
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
                    // ñ çàäàííûì ëîãèíîì è ïàðîëåì ìû ïîéäåì â áàçó
                    // åñëè â áàçå åñòü ïîëüçîâàòåëü, òî âñ¸ îê, åñëè íåò, òî íè÷åãî íå äåëàåì
                    var lm = app.ApplicationServices.GetService<AutentificationManager>();
                    if (lm.IsRegistred(credentials.loginString,credentials.passwordString) == true)
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, credentials.loginString)
                        };
                        // ñîçäàåì îáúåêò ClaimsIdentity
                        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                        // äîáàâëÿåì êóêè íàøåìó ïîëüçîâàòåëþ
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                        
                        // ïåðåíàïðàâëÿåì íà íóæíóþ ñðàíèöó
                        context.Response.Redirect("/mainPage");
                    }

                    await context.Response.WriteAsync(credentials.loginString);
                });
               
                endpoints.MapGet("/adminUsersPage", async context =>
                {
                    string page = File.ReadAllText("Pages/usersAdminPage.html");
                    string page2 = File.ReadAllText("Pages/MainPage.html");
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    if (sm.GetRole(login)=="Admin")
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
                endpoints.MapPost("/Whois", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;

                    await context.Response.WriteAsJsonAsync(sm.GetRole(login));
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
                endpoints.MapGet("/getUsrIds", async context =>
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
               
                endpoints.MapPost("/changeForm", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    
                    await context.Response.WriteAsJsonAsync(sm.UpdateForm(query.Id, query.formString));
                });
                endpoints.MapPost("/changeRole", async context =>
                {
                    var sm = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    
                    await context.Response.WriteAsJsonAsync(sm.UpdateRole(query.Id, query.roleString));
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

                    
                    if (lm.GetRole(login)=="Admin" || lm.GetRole(login) == "Teacher")
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
                
                endpoints.MapGet("/getFormsIdsfl", async context =>
                {
                    var fm = app.ApplicationServices.GetService<FormsManager>();
                    var currForm = fm.GetFormsId();

                    await context.Response.WriteAsJsonAsync(currForm);
                });
                endpoints.MapGet("/getFormsfl", async context =>
                {
                    var fm = app.ApplicationServices.GetService<FormsManager>();
                    var currForm = fm.GetForms();

                    await context.Response.WriteAsJsonAsync(currForm);
                });                
                endpoints.MapPost("/deleteFormInL", async context =>
                {
                    var fm = app.ApplicationServices.GetService<FormsManager>();
                    var query = await context.Request.ReadFromJsonAsync<FormContent>();

                    await context.Response.WriteAsJsonAsync(fm.RemoveForm(query.Id));
                });
                endpoints.MapPost("/addForm", async context =>
                {
                    //ToDO åñëè óæå åñòü òî âåðíóòü íà ñàéò false
                    var fm = app.ApplicationServices.GetService<FormsManager>();
                    var query = await context.Request.ReadFromJsonAsync<FormContent>();
                    await context.Response.WriteAsJsonAsync(fm.AddForm(query.FormString));
                });
                endpoints.MapPost("/IsReged", async context => {
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var query = await context.Request.ReadFromJsonAsync<RegistrationContent>();
                    await context.Response.WriteAsJsonAsync(am.IsRegistredEarlier(query.loginString, query.emailString));

                });
                endpoints.MapPost("/AddTaskGr", async context =>
                {
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskGroupTemplate>();
                    await context.Response.WriteAsJsonAsync(tm.CreateTaskGr(query.Name,query.formString)); 
                });
                endpoints.MapPost("/getTaskGrTitles", async context => {
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskGroupTemplate>();
                    await context.Response.WriteAsJsonAsync(tm.GetTaskGrIds(query.formString));
                });
                endpoints.MapPost("/getTaskGrNames", async context => {
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskGroupTemplate>();
                    await context.Response.WriteAsJsonAsync(tm.GetTaskGrNames(query.formString));
                });
                endpoints.MapPost("/removeTaskGr", async context => {
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskGroupTemplate>();
                    tm.RemoveTaskGr(query.Id, query.formString);
                });
                endpoints.MapPost("/addTask", async context => {
                    var login = context.User.Identity.Name;
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    await context.Response.WriteAsJsonAsync(tm.CreateTask(query.TaskGrId, query.formString, query.Title, query.Text, query.Answer, login));
                });
                endpoints.MapPost("/getTaskIds", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var currIds= fm.GetTaskIds(query.formString,query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currIds);
                });
                endpoints.MapPost("/getTaskTitles", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var currTitles= fm.GetTaskTitles(query.formString, query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currTitles);
                });
                endpoints.MapPost("/getTaskTextes", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var currTextes = fm.GetTaskTextes(query.formString, query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currTextes);
                });
                endpoints.MapPost("/getTaskAnswers", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var currAnswers = fm.GetTaskAnswers(query.formString, query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currAnswers);
                });
                endpoints.MapPost("/changeTaskTitle", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    
                    await context.Response.WriteAsJsonAsync(fm.ChangeTaskTitle(new TaskTemplate { Answer = query.Answer, formString = query.formString, Id = query.Id, TaskGrId = query.TaskGrId, Text = query.Text, Title = query.Title }));
                });
                endpoints.MapPut("/changeTaskText", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    fm.ChangeTask(new TaskTemplate { Answer = query.Answer, formString = query.formString, Id = query.Id, TaskGrId = query.TaskGrId, Text = query.Text, Title = query.Title });
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPut("/changeTaskAnswer", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    fm.ChangeTask(new TaskTemplate { Answer = query.Answer, formString = query.formString, Id = query.Id, TaskGrId = query.TaskGrId, Text = query.Text, Title = query.Title });
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPut("/removeTask", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    fm.RemoveTask(new TaskTemplate { Answer = query.Answer, formString = query.formString, Id = query.Id, TaskGrId = query.TaskGrId, Text = query.Text, Title = query.Title });
                    await context.Response.WriteAsJsonAsync("ok");
                });
                endpoints.MapPost("/getTaskGrTitles2", async context => {
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];
                    
                    await context.Response.WriteAsJsonAsync(tm.GetTaskGrIds(form));
                });
                endpoints.MapPost("/getTaskGrNames2", async context => {
                    var tm = app.ApplicationServices.GetService<TaskManager>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];
                    await context.Response.WriteAsJsonAsync(tm.GetTaskGrNames(form));
                });
                endpoints.MapPost("/getTaskIds2", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];
                    var currIds = fm.GetTaskIds(form, query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currIds);
                });
                endpoints.MapPost("/getTaskTitles2", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];
                    var currTitles = fm.GetTaskTitles(form, query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currTitles);
                });
                endpoints.MapPost("/getTaskTextes2", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];
                    var currTextes = fm.GetTaskTextes(form, query.TaskGrId);

                    await context.Response.WriteAsJsonAsync(currTextes);
                });
                endpoints.MapPost("/complete", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];

                    await context.Response.WriteAsJsonAsync(fm.Check(new TaskTemplate { Answer=query.Answer,formString=form,Id=query.Id,TaskGrId=query.TaskGrId,Text=query.Text,Title=query.Title},login));
                });
                endpoints.MapPost("/getResult", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];

                    await context.Response.WriteAsJsonAsync(fm.GetResult(new TaskTemplate { formString = form, Id = query.Id, TaskGrId = query.TaskGrId }, login));
                });
                endpoints.MapPost("/getResultsInTaskGr", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                   

                    await context.Response.WriteAsJsonAsync(fm.GetAllResults(new TaskTemplate { formString = query.formString,TaskGrId = query.TaskGrId }));
                });
                endpoints.MapPost("/getLoginsTaskGr", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();


                    await context.Response.WriteAsJsonAsync(fm.GetAllLogins(query.formString));
                });
                endpoints.MapPost("/getResultInTaskGr", async context =>
                {
                    var fm = app.ApplicationServices.GetService<TaskManager>();
                    var query = await context.Request.ReadFromJsonAsync<TaskTemplate>();
                    var am = app.ApplicationServices.GetService<AutentificationManager>();
                    var login = context.User.Identity.Name;
                    var form = am.GetFormByLogin(login)[0];

                    await context.Response.WriteAsJsonAsync(fm.GetResults(new TaskTemplate { TaskGrId=query.TaskGrId,formString=form},login));
                });
            });

        }
    }
}

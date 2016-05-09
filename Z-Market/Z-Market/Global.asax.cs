using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Z_Market.Models;


namespace Z_Market
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.Z_MarketContext, Migrations.Configuration>());
            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            CreateSuperUser(db);
            AddPermisionsToSuperUser(db);
            db.Dispose();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermisionsToSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.FindByName("manager@zmarket.com");
            if (!userManager.IsInRole(user.Id, "View"))
                userManager.AddToRole(user.Id, "View");
            if (!userManager.IsInRole(user.Id, "Edit"))
                userManager.AddToRole(user.Id, "Edit");
            if (!userManager.IsInRole(user.Id, "Create"))
                userManager.AddToRole(user.Id, "Create");
            if (!userManager.IsInRole(user.Id, "Delete"))
                userManager.AddToRole(user.Id, "Delete");
        }

        private void CreateSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.FindByName("manager@zmarket.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "manager@zmarket.com",
                    Email = "manager@zmarket.com"
                };
                userManager.Create(user, "Man123");
            }
        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!roleManager.RoleExists("View"))
                roleManager.Create(new IdentityRole("View"));
            if (!roleManager.RoleExists("Edit"))
                roleManager.Create(new IdentityRole("Edit"));
            if (!roleManager.RoleExists("Create"))
                roleManager.Create(new IdentityRole("Create"));
            if (!roleManager.RoleExists("Delete"))
                roleManager.Create(new IdentityRole("Delete"));
        }
    }
}

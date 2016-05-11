using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Z_Market.Models;
using Z_Market.ModelView;

namespace Z_Market.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();
            foreach (var item in users)
            {
                var userView = new UserView
                {
                    UserId = item.Id,
                    Email = item.Email,
                    Name = item.UserName
                };
                usersView.Add(userView);
            }
            return View(usersView);
        }

        public ActionResult Roles(string userId)
        {
            if(string.IsNullOrEmpty(userId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.Users.ToList().Find(u => u.Id.Equals(userId));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userView = new UserView
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Roles = new List<RoleView>()
            };
            foreach (var rol in user.Roles.Select(item => roleManager.Roles.ToList().Find(r => r.Id.Equals(item.RoleId))))
            {
                userView.Roles.Add(new RoleView
                {
                    RoleId = rol.Id,
                    Name = rol.Name
                });
            }
            return View(userView);
        }

        public ActionResult AddRole(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.Users.ToList().Find(u => u.Id.Equals(userId));
            if(user == null)
                return HttpNotFound();
            var userView = new UserView
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var rolesList = roleManager.Roles.ToList();
            rolesList.Add(new IdentityRole { Id = String.Empty, Name = "[Seleccione un role]" });
            ViewBag.RoleId = new SelectList(rolesList.OrderBy(c => c.Name), "Id", "Name");

            return View(userView);
        }

        [HttpPost]
        public ActionResult AddRole(string userId, FormCollection form)
        {
            var roleId = Request["RoleId"];
            
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.Users.ToList().Find(u => u.Id.Equals(userId));
            var userView = new UserView
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Roles = new List<RoleView>()
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var rolesList = roleManager.Roles.ToList();

            if (string.IsNullOrEmpty(roleId))
            {
                ViewBag.Error = "You must select a role";
                rolesList.Add(new IdentityRole { Id = String.Empty, Name = "[Seleccione un role]" });
                ViewBag.RoleId = new SelectList(rolesList.OrderBy(c => c.Name), "Id", "Name");

                return View(userView);
            }

            var role = rolesList.Find(r => r.Id.Equals(roleId));
            if (!userManager.IsInRole(userId, role.Name))
                userManager.AddToRole(userId, role.Name);

            foreach (var rol in user.Roles.Select(item => roleManager.Roles.ToList().Find(r => r.Id.Equals(item.RoleId))))
            {
                userView.Roles.Add(new RoleView
                {
                    RoleId = rol.Id,
                    Name = rol.Name
                });
            }

            return View("Roles", userView);
        }

        public ActionResult Delete(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.Users.ToList().Find(u => u.Id.Equals(userId));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var role = roleManager.Roles.ToList().Find(r => r.Id.Equals(roleId));
            if (userManager.IsInRole(userId, role.Name))
                userManager.RemoveFromRole(userId, role.Name);
            var userView = new UserView
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Roles = new List<RoleView>()
            };
            foreach (var rol in user.Roles.Select(item => roleManager.Roles.ToList().Find(r => r.Id.Equals(item.RoleId))))
            {
                userView.Roles.Add(new RoleView
                {
                    RoleId = rol.Id,
                    Name = rol.Name
                });
            }
            return View("Roles", userView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
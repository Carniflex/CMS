using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Models;
using System.Web.Security;
using System.Data;
using WebMatrix.WebData;
namespace CMS.Controllers
{
    public class MyAdminController : Controller
    {
        //
        // GET: /MyAdmin/
        
        UsersContext db = new UsersContext();
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllUsers()
        {
             List<MyAdminmodel> users = new List<MyAdminmodel>();
             foreach (var item in db.UserProfiles)
            {
                users.Add(new MyAdminmodel { UserId = item.UserId, UserName = item.UserName, Roles = Roles.GetRolesForUser(item.UserName) });
            }

            int myid = (int)WebSecurity.CurrentUserId;
           
             var allwithoutme = users.Where(p => p.UserId != myid);

            return Json(allwithoutme, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoles(int id)
        {

            MembershipUser mu = Membership.GetUser(id);
            string UserName = mu.UserName;
            var rol = Roles.GetRolesForUser(UserName);
            return Json(rol, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateUser(UserProfile profile, dynamic NewRole, int UserId = 0)
        {
            var mid = profile.UserId;
            mid = UserId;
            profile.UserId = UserId;
            db.Entry(profile).State = EntityState.Modified;
            db.SaveChanges();
            var rol = Roles.GetRolesForUser(profile.UserName);
            rol.ToString();
            if (rol.Length!=0)
            {
                Roles.RemoveUserFromRole(profile.UserName, rol[0]);
            }
          
            Roles.AddUserToRole(profile.UserName, NewRole[0]);
            return Json(profile, JsonRequestBehavior.AllowGet);
        
        
        }
    }
}

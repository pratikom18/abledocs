using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class UsersController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            Users objusers = new Users();
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "ado admin").FirstOrDefault();
            string pagefound = "No";
            string isAdd = "No";
            string isEdit = "No";
            string isDelete = "No";

            if (objAssignedMenu != null)
            {
                if (objAssignedMenu.isview)
                {
                    pagefound = "yes";

                }
                if (objAssignedMenu.isadd)
                {
                    isAdd = "yes";

                }
                if (objAssignedMenu.isupdate)
                {
                    isEdit = "yes";

                }
                if (objAssignedMenu.isdelete)
                {
                    isDelete = "yes";

                }
            }
            ViewBag.isAdd = isAdd;
            ViewBag.isEdit = isEdit;
            ViewBag.isDelete = isDelete;


            if (pagefound == "No")
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                return View();
            }
 
        }

        [HttpGet, LoginAuthorized]
        public JsonResult UsersList()
        {
            Users objUserMaster = new Users();
            var result = objUserMaster.GetUsersList();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult UserDetail(int id)
        {
            
            Users objUsers = new Users();
           
            if (id != 0)
            {
                objUsers = objUsers.GetUserById(id);
                ViewBag.PageMode = "Edit";
            }
            else {
               
                ViewBag.PageMode = "Add";
            }
            UserRoles objUserRoles = new UserRoles();
            List<UserRoles> listUserRoles = objUserRoles.GetUserRolesList();
            ViewBag.RoleList = listUserRoles.Where(x => x.IsActive == true).ToList();
  
            return PartialView("_UserDetail", objUsers);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult UserProfile(int id)
        {
            Users objUsers = new Users();
            
            if (id != 0)
            {
                objUsers = objUsers.GetUserById(id);
               
            }
                      
            return PartialView("~/Views/Shared/_UserProfile.cshtml", objUsers);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult ChangePassword(int id)
        {
            Users objUsers = new Users();

            if (id != 0)
            {
                objUsers = objUsers.GetUserById(id);

            }

            return PartialView("~/Views/Shared/_ChangePassword.cshtml", objUsers);
        }
        [HttpPost, LoginAuthorized]
        public ActionResult Upsert(Users model)
        {

            try
            {
                foreach (string key in Request.Form.Keys)
                {
                    if (key == "UserRoleId")
                    {
                        string a = Request.Form[key].ToString();
                        model.UserRoleId = a;
                    }
                }
                string pwd = Utility.CommonHelper.GetMD5Hash(model.Password);
                model.Password = pwd;
                var id = model.Upsert();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPost, LoginAuthorized]
        public ActionResult UpdateProfile(Users model)
        {

            var databasename = HttpContext.Session.GetString("Database");
            try
            {
                foreach (string key in Request.Form.Keys)
                {
                    if (key == "UserRoleId")
                    {
                        string a = Request.Form[key].ToString();
                        model.UserRoleId = a;
                    }
                }
                model.databasename = databasename;
                var id = model.UpdateProfile();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost, LoginAuthorized]
        public ActionResult ChangePassword()
        {

            var databasename = HttpContext.Session.GetString("Database");
            var post = Request.Form;
            var id = post["ID"];
            var currentPassword = post["CurrentPassword"];
            var newPassword = post["NewPassword"];
            Users objUsers = new Users();
            objUsers = objUsers.GetUserById(Convert.ToInt32(id));
            string currentpwd = Utility.CommonHelper.GetMD5Hash(currentPassword);
            string userpwd = objUsers.Password;
            var msg = "something went wrong";
            var status = false;
            if (currentpwd == userpwd)
            {
                objUsers.ID = Convert.ToInt32(id);
                objUsers.Password = Utility.CommonHelper.GetMD5Hash(newPassword);
                objUsers.UpdatePassword();
                status = true;
                msg = "Password updated successfully.";
            }
            else
            {
                status = false;
                msg = "Current password did not match Please try again.";
            }
            

            return Json(new { status = status, message = msg });
        }


    }
}

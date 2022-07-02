using abledoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace abledoc.Controllers
{
    public class RoleController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {

            UserRoles objUserrole = new UserRoles();
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "role").FirstOrDefault();
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
                // ViewBag.AlphabetList = objUserrole.GetAlphabetList();
                return View();
            }

            return View();

        }

        [HttpGet, LoginAuthorized]
        public JsonResult UserRoleList()
        {
            UserRoles objUserRoles = new UserRoles();
            var result = objUserRoles.GetUserRolesList();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public IActionResult Upsert(UserRoles model)
        {
            try
            {
                var id = model.Upsert();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult RoleDetail(int id)
        {
            UserRoles objUserRoles = new UserRoles();
            if (id != 0)
            {
                objUserRoles = objUserRoles.GetUserRoleById(id);
                ViewBag.PageMode = "Edit";
            }
            else
            {
                ViewBag.PageMode = "Add";
            }

            return PartialView("_RoleDetail", objUserRoles);
        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult RolePermissionList(int roleid)
        {
            ViewBag.RoleId = roleid;
            RolesPermissions objRolesPermissions = new RolesPermissions();
            objRolesPermissions.RolesPermissionsList = objRolesPermissions.GetRolePermissionList(roleid);
            int total = objRolesPermissions.RolesPermissionsList.Count();
            int isview = 0;
            int isadd = 0;
            int isupdate = 0;
            int isdelete = 0;
            bool allChk = false;
            foreach (RolesPermissions item in objRolesPermissions.RolesPermissionsList)
            {
                if (item.isview && item.isadd && item.isupdate && item.isdelete)
                {
                    item.AllChk = true;

                }
                if (item.isview)
                {
                    isview = isview + 1;
                }
                if (item.isadd)
                {
                    isadd = isadd + 1;
                }
                if (item.isupdate)
                {
                    isupdate = isupdate + 1;
                }
                if (item.isdelete)
                {
                    isdelete = isdelete + 1;
                }
            }
            ViewBag.isView = total == isview ? true : false;
            ViewBag.isAdd = total == isadd ? true : false;
            ViewBag.isUpdate = total == isupdate ? true : false;
            ViewBag.isDelete = total == isdelete ? true : false;
            if (total == isview && total == isadd && total == isupdate && total == isdelete)
            {
                allChk = true;
            }

            ViewBag.isAll = allChk;
            return PartialView("_assignRole", objRolesPermissions);
        }


        [HttpPost, LoginAuthorized]
        public JsonResult UpsertRolePermission(int roleid, int menuid, bool view, bool add, bool update, bool delete)
        {
            RolesPermissions objRolesPermissions = new RolesPermissions();

            objRolesPermissions.roleid = roleid;
            objRolesPermissions.menuid = menuid;
            objRolesPermissions.isview = view;
            objRolesPermissions.isadd = add;
            objRolesPermissions.isupdate = update;
            objRolesPermissions.isdelete = delete;

            var result = objRolesPermissions.Upsert();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public ActionResult updateAssign(List<RoleAssign> RoleAssignList)
        {

            foreach (RoleAssign item in RoleAssignList)
            {
                RolesPermissions objRolesPermissions = new RolesPermissions();
                objRolesPermissions.roleid = item.roleid;
                objRolesPermissions.menuid = item.menuid;
                objRolesPermissions.isview = item.isview;
                objRolesPermissions.isadd = item.isadd;
                objRolesPermissions.isupdate = item.isupdate;
                objRolesPermissions.isdelete = item.isdelete;

                var result = objRolesPermissions.Upsert();
            }

            //var jsonResult = Json(new

            //{
            //    data = "successfully"
            //});
            ////jsonResult.m = int.MaxValue;
            //return jsonResult;
            return null;
        }


        [HttpGet, LoginAuthorized]
        public PartialViewResult SettingPermissionList(int roleid)
        {
            ViewBag.RoleId = roleid;
            SettingPermissions settingPermissions = new SettingPermissions();
            settingPermissions.SettingPermissionsList = settingPermissions.GetSettingPermissionList(roleid);
            int total = settingPermissions.SettingPermissionsList.Count();
            int isView = 0;
            foreach (SettingPermissions item in settingPermissions.SettingPermissionsList)
            {
                if (item.IsView)
                {
                    isView = isView + 1;
                }
            }
            bool view = false;
            if (isView == total)
            {
                view = true;
            }
            ViewBag.isView = view;
            return PartialView("_settingPermissions", settingPermissions);
        }


        [HttpPost, LoginAuthorized]
        public JsonResult UpsertSettingPermission(int roleid, int settingid, bool view)
        {
            SettingPermissions settingPermissions = new SettingPermissions();

            settingPermissions.RoleID = roleid;
            settingPermissions.SettingID = settingid;
            settingPermissions.IsView = view;

            var result = settingPermissions.Upsert();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }

    }
}

using abledoc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class MenuController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        
        {
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "menu").FirstOrDefault();
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
        public JsonResult MenuList()
        {
            MenuMaster objMenuMaster = new MenuMaster();
            var result = objMenuMaster.GetMenuMasterList();
            var jsonResult = Json(new

            {
                data = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;
        }
        [HttpPost, LoginAuthorized]
        public IActionResult Upsert(MenuMaster model)
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
        public PartialViewResult MenuDetail(int id)
        {
            MenuMaster objMenuMaster = new MenuMaster();
            if (id != 0)
            {
                objMenuMaster = objMenuMaster.GetMenuMasterById(id);
                ViewBag.PageMode = "Edit";
            }
            else {
                ViewBag.PageMode = "Add";
            }

            return PartialView("_MenuDetail", objMenuMaster);
        }
    }
}

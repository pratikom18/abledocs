using abledoc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class DiscriptionMasterController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IViewLocalizer Localizer;
        public DiscriptionMasterController(IWebHostEnvironment hostEnvironment, IViewLocalizer _Localizer)
        {
            webHostEnvironment = hostEnvironment;
            Localizer = _Localizer;

        }

        [LoginAuthorized]
        public IActionResult Index()
        {
            /*List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "manage company").FirstOrDefault();
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
            }*/
            return View();
        }

        [HttpPost, LoginAuthorized]
        public JsonResult ContentList(JQueryDataTableParamModel param, string LanguageFilter)
        {

            int totalRecords = 0;
            List<DiscriptionMaster> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            DiscriptionMaster objDiscriptionMaster = new DiscriptionMaster();
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                SearchKey = param.sSearch;
            }
            string[] colunms = param.sColumns.Split(',');
            string sortcolunm = Request.Form["iSortCol_0"];
            string sortColunmName = colunms[Convert.ToInt32(sortcolunm)].ToString();
            string sortdir = Request.Form["sSortDir_0"];
            if (!string.IsNullOrEmpty(sortColunmName) && !string.IsNullOrEmpty(sortdir))
            {
                OrderBy = sortColunmName + " " + sortdir;
            }
            allRecord = objDiscriptionMaster.GetContentListForTable(startIndex, endIndex, OrderBy, SearchKey, LanguageFilter, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.id.ToString(),
                             c.Language == null ? "" : c.Language.ToString(),
                             c.ProductName == null ? "" : c.ProductName.ToString(),
                             c.ProductPrice.ToString(),
                             


                         };
            var jsonResult = Json(new

            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            });
            //jsonResult.m = int.MaxValue;
            return jsonResult;




        }

        [HttpGet, LoginAuthorized]
        public PartialViewResult edit(int id = 0)
        {
            DiscriptionMaster model = new DiscriptionMaster();
            if (id > 0)
            {
                model = model.GetContentById(id);
                ViewBag.PageMode = "Update";
            }
            else
            {
                ViewBag.PageMode = "Create";
            }

            return PartialView("Create", model);

        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DiscriptionMaster model)
        {
            foreach (string key in Request.Form.Keys)
            {
                if (key == "country_code")
                {
                    string a = Request.Form[key].ToString();
                    model.country_code = a;
                }
                if (key == "unit")
                {
                    string b = Request.Form[key].ToString();
                    model.unit = b;
                }

            }

            string id = model.InsertUpdate();

                ViewBag.id = id.ToString();

                return Json(new { status = true, message = "Update data successfully."});
            
        }
    

        
        [HttpGet, LoginAuthorized]
        public JsonResult Delete(int id)
        {
            DiscriptionMaster model = new DiscriptionMaster();
            var result = model.DeleteContent(id);
            var jsonResult = Json(new

            {
                data = result
            });

            return jsonResult;
        }

    }
}

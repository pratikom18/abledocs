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
    public class CountriesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IViewLocalizer Localizer;
        public CountriesController(IWebHostEnvironment hostEnvironment, IViewLocalizer _Localizer)
        {
            webHostEnvironment = hostEnvironment;
            Localizer = _Localizer;

        }

        [AuthorizedAction]
        public IActionResult Index()
        {
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "countries").FirstOrDefault();
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

            Countries countries = new Countries();
            countries = countries.GetCountryByDefaultSMTP();

            if (countries != null)
            {
                ViewBag.Code = countries.code;
            }
            else {
                ViewBag.Code = "";
            }
            if (pagefound == "No")
            {
                return RedirectToAction("Index", "Error");
            }
            else
            {
                return View();
            }
        }

        [HttpPost, LoginAuthorized]
        public JsonResult CountriesList(JQueryDataTableParamModel param, string CountrySearch)
        {

            int totalRecords = 0;
            List<Countries> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            Countries objCountriesMaster = new Countries();
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
            allRecord = objCountriesMaster.GetContentListForTable(startIndex, endIndex, OrderBy, SearchKey, CountrySearch, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.id.ToString(),
                             c.code == null ? "" : c.code.ToString(),
                             c.country == null ? "" : c.country.ToString(),
                             c.phone_prefix == null ? "" : c.phone_prefix.ToString(),
                             c.currency == null ? "" : c.currency.ToString(),


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
        public PartialViewResult Edit(int id = 0)
        {
            Countries model = new Countries();
            if (id > 0)
            {
                model = model.GetContentById(id);

                ViewBag.PageMode = "Edit";

            }
            else
            {
                ViewBag.PageMode = "Add";
            }

            return PartialView("Create", model);

        }
        
        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Countries model)
        {
            
                string id = model.InsertUpdate();

                ViewBag.id = id.ToString();

               

            return Json(new { status = true, message = "Countries Update successfully."});
            
        }
    

        
        [HttpGet, LoginAuthorized]
        public JsonResult Delete(int id)
        {
            Countries model = new Countries();
            var result = model.DeleteContent(id);
            var jsonResult = Json(new

            {
                data = result
            });

            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public JsonResult DefaultSMTP(string code)
        {
            Countries model = new Countries();
            model.code = code;
            var result = model.isDefault();
            var jsonResult = Json(new

            {
                data = result
            });

            return jsonResult;
        }

    }
}

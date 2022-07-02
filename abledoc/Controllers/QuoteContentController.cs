using abledoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class QuoteContentController : Controller
    {
        [AuthorizedAction]
        public IActionResult Index()
        {
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "quote content").FirstOrDefault();
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

        [HttpPost, LoginAuthorized]
        public JsonResult ContentList(JQueryDataTableParamModel param, string TypeSearch)
        {

            int totalRecords = 0;
            List<JobQuoteAutopopulate> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            JobQuoteAutopopulate objJobQuoteAutopopulateMaster = new JobQuoteAutopopulate();
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
            allRecord = objJobQuoteAutopopulateMaster.GetContentListForTable(startIndex, endIndex, OrderBy, SearchKey, TypeSearch, out totalRecords);
            int filteredRecords = totalRecords;
            if (SearchKey != "")
            {
                filteredRecords = allRecord.Count();
            }

            var result = from c in allRecord
                         select new[] {
                             c.ID.ToString(),
                             c.Type == null ? "" : c.Type.ToString(),
                             c.Information == null ? "" : c.Information.ToString(),
                             c.typename == null ? "" :c.typename.ToString(),
                             c.display_order.ToString()


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
            var databasename = HttpContext.Session.GetString("Database");

            JobQuoteAutopopulate model = new JobQuoteAutopopulate();
            if (id > 0)
            {
                model = model.GetContentById(id);
                ViewBag.PageMode = "Update";
            }
            else {
                ViewBag.PageMode = "Create";
            }

            return PartialView("Create", model);

        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JobQuoteAutopopulate model)
        {
            foreach (string key in Request.Form.Keys)
            {
                if (key == "country_code")
                {
                    string a = Request.Form[key].ToString();
                    model.country_code = a;
                }
                if (key == "province")
                {
                    string b = Request.Form[key].ToString();
                    model.province = b;
                }
            }

            string id = model.InsertUpdate();

                ViewBag.id = id.ToString();

                return Json(new { status = true, message = "Content Update successfully."});
            
        }
    

        [HttpGet, LoginAuthorized]
        public JsonResult checkcountryrequired(string type)
        {
            CommonMaster model = new CommonMaster();
             var result =  model.GetCommonByTypeCode(type);
            var jsonResult = Json(new

            {
                data = result
            });
            
            return jsonResult;
        }
        [HttpGet, LoginAuthorized]
        public JsonResult GetMaxDisplayOrder(string lang,string type)
        {
            JobQuoteAutopopulate model = new JobQuoteAutopopulate();
            var result = model.GetMaxDisplayOrder(lang, type);
            var jsonResult = Json(new

            {
                data = result
            });

            return jsonResult;
        }

        [HttpGet, LoginAuthorized]
        public JsonResult Delete(int id)
        {
            JobQuoteAutopopulate model = new JobQuoteAutopopulate();
            var result = model.DeleteContent(id);
            var jsonResult = Json(new

            {
                data = result
            });

            return jsonResult;
        }

        [HttpPost, LoginAuthorized]
        public JsonResult GetStateList()
        {
            string country_code = null;

            if (Request.ContentLength > 0)
            {
                foreach (string key in Request.Form.Keys)
                {
                    if (key == "country_code[]")
                    {
                        string a = Request.Form[key].ToString();
                        country_code = a;
                    }
                }
                States model = new States();
                var result = model.GetStateListByCountry(country_code.ToString().Split(",").ToArray());
                var jsonResult = Json(new

                {
                    result = result
                });
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }
            else {

                var jsonResult = Json(new

                {
                    result = new States()
                }) ;
                //jsonResult.m = int.MaxValue;
                return jsonResult;
            }

            
        }

    }
}

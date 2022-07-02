using abledoc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class PipelineController : Controller
    {
        [LoginAuthorized]
        public IActionResult Index()
        {
            List<AssignedMenu> AssignedMenuList = new List<AssignedMenu>();
            AssignedMenuList = Utility.Session.SessionExtensions.GetComplexData<List<AssignedMenu>>(HttpContext.Session, "AssignedMenu");
            AssignedMenu objAssignedMenu = new AssignedMenu();
            objAssignedMenu = AssignedMenuList.Where(x => x.menuname.ToLowerInvariant() == "pipeline").FirstOrDefault();
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
            List<CommonMaster> allRecord = null;

            string SearchKey = "";
            string OrderBy = "";
            int startIndex = param.iDisplayStart;
            int endIndex = param.iDisplayLength;

            CommonMaster objCommonMasterMaster = new CommonMaster();
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
            allRecord = objCommonMasterMaster.GetPipelineList(startIndex, endIndex, OrderBy, SearchKey, out totalRecords);
            int filteredRecords = totalRecords;
            //if (SearchKey != "")
            //{
            //    filteredRecords = allRecord.Count();
            //}

            var result = from c in allRecord
                         select new[] {
                             c.commonid.ToString(),
                             c.type == null ? "" : c.type.ToString(),
                             c.typecode == null ? "" : c.typecode.ToString(),
                             c.typename == null ? "" : c.typename.ToString(),
                             c.display_order == null ? "":c.display_order.ToString()
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
            CommonMaster model = new CommonMaster();
            if (id > 0)
            {
                model = model.GetCommonMasterById(id);
                ViewBag.FormType = "Update";
            }
            else {
                ViewBag.FormType = "Create";
            }

            return PartialView("Create", model);

        }

        [HttpPost, LoginAuthorized]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommonMaster model)
        {
            if (model.type == null)
            {
                model.type = "Pipeline";
            }
            string id = model.InsertUpdateUnit();

            ViewBag.id = id.ToString();

            return Json(new { status = true, message = "Update data successfully." });

        }

        [HttpGet, LoginAuthorized]
        public JsonResult Delete(int id)
        {
            CommonMaster model = new CommonMaster();
            var result = model.DeleteContent(id);
            var jsonResult = Json(new

            {
                data = result
            });

            return jsonResult;
        }
    }
}

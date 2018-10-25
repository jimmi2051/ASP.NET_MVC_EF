using PagedList;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static SupCatServices _service = new SupCatServices(new Servies.Validation.ModelStateWrapper(_modelState));
        IEnumerable _list = _service.listCat();

        // GET: Admin/TypeUsers
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            IEnumerable<Category> list = _service.searchCatByKey(searchString);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/TypeUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TypeUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,Name,Status,GroupName")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (_service.createCat(category))
                    return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/TypeUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _service.getCat(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/TypeUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,Name,Status,GroupName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _service.updateCat(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/TypeUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _service.getCat(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/TypeUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Category category = _service.getCat(id);
            _service.deleteCat(category);
            return RedirectToAction("Index");
        }
    }
}
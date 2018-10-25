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
    public class SupplierController : BaseController
    {
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static SupCatServices _service = new SupCatServices(new Servies.Validation.ModelStateWrapper(_modelState));
        IEnumerable _list = _service.listSup();

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
            IEnumerable<Supplier> list = _service.searchSupByKey(searchString);
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
        public ActionResult Create([Bind(Include = "SupplierID,Name,Address,Phone,Email,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (_service.createSup(supplier))
                    return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Admin/TypeUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _service.getSup(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/TypeUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,Name,Address,Phone,Email,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _service.updateSup(supplier);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Admin/TypeUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _service.getSup(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/TypeUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
           Supplier supplier = _service.getSup(id);
            _service.deleteSup(supplier);
            return RedirectToAction("Index");
        }
    }
}
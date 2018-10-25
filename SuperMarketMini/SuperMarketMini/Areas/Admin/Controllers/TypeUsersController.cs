using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SuperMarketMini.Domain;
using SuperMarketMini.Servies;

namespace SuperMarketMini.Areas.Admin.Controllers
{
    public class TypeUsersController : BaseController
    {
        private static Servies.Validation.ModelStateDictionary _modelState = new Servies.Validation.ModelStateDictionary();
        private static UserServices _service = new UserServices(new Servies.Validation.ModelStateWrapper(_modelState));
        IEnumerable _list = _service.listType();

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
            IEnumerable<TypeUser> list = _service.SearchTypeUsers(searchString);
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
        public ActionResult Create([Bind(Include = "TypeID,DisplayName")] TypeUser typeUser)
        {
            if (ModelState.IsValid)
            {
                if (_service.createTypeUser(typeUser))
                    return RedirectToAction("Index");
                else
                    return View(typeUser);
            }

            return View(typeUser);
        }

        // GET: Admin/TypeUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = _service.getTypeUser(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // POST: Admin/TypeUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,DisplayName")] TypeUser typeUser)
        {
            if (ModelState.IsValid)
            {
                _service.updateTypeUser(typeUser);
                return RedirectToAction("Index");
            }
            return View(typeUser);
        }

        // GET: Admin/TypeUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = _service.getTypeUser(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // POST: Admin/TypeUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TypeUser typeUser = _service.getTypeUser(id);
            _service.deleteTypeUser(typeUser);
            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeFest.NinjaFamily.FamilyTreeApp.Models;

namespace CodeFest.NinjaFamily.FamilyTreeApp.Controllers
{
    public class UserController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /User/
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
               
                var currprev = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                
                if (currprev == null)
                {
                    using (var accDb = new ApplicationDbContext())
                    {
                        var checkRegistrated = accDb.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                        if (checkRegistrated != null)
                        {
                            return Redirect("~/User/Create");
                        }
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return View(currprev);                
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Name,LastName,Image,Location,Birth,Death")] User user)
        {
            if (ModelState.IsValid)
            {
                //if (upload != null && upload.ContentLength > 0)
                //{
                //    var avatar = new CodeFest.NinjaFamily.FamilyTreeApp.Models.File()
                //    {
                //        FileName = System.IO.Path.GetFileName(upload.FileName),
                //        FileType = FileType.Avatar,
                //        ContentType = upload.ContentType
                //    };
                //    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                //    {
                //        avatar.Content = reader.ReadBytes(upload.ContentLength);
                //    }
                //    user.Image = avatar;
                //}

                user.UserName = User.Identity.Name; 
                db.Users.Add(user);
                var currentUserName = User.Identity.Name;
                var accDb = new ApplicationDbContext();
                ApplicationUser currentUser = accDb.Users.FirstOrDefault(x => x.UserName == currentUserName);
                currentUser.Owner = user.ID;
                accDb.SaveChanges();
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,LastName,Image,Location,Birth,Death")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ImageUpload()
        {
            return View();
        }

        //public ActionResult CreateImage()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult ImageUpload(FormCollection formCollection)
        //{
        //    foreach (string item in Request.Files)
        //    {
        //        HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
        //        if (file.ContentLength == 0)
        //            continue;
        //        if (file.ContentLength > 0)
        //        {
        //            // width + height will force size, care for distortion
        //            //Exmaple: ImageUpload imageUpload = new ImageUpload { Width = 800, Height = 700 };

        //            // height will increase the width proportionally
        //            //Example: ImageUpload imageUpload = new ImageUpload { Height= 600 };

        //            // width will increase the height proportionally
        //            ImageUpload imageUpload = new ImageUpload { Width = 600 };

        //            // rename, resize, and upload
        //            //return object that contains {bool Success,string ErrorMessage,string ImageName}
        //            ImageResult imageResult = imageUpload.RenameUploadFile(file);
        //            if (imageResult.Success)
        //            {
        //                //TODO: write the filename to the db
        //                using (var accDB = new ApplicationDbContext())
        //                {
        //                    var curr =  accDB.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
        //                    db.Users.Find(curr.Owner).Img=imageResult.ImageName;
        //                    db.SaveChanges();
        //                }
        //            }
        //            else
        //            {
        //                // use imageResult.ErrorMessage to show the error
        //                ViewBag.Error = imageResult.ErrorMessage;
        //            }
        //        }
        //    }

        //    return RedirectToAction("Details");
        //}
       
    }
}

using BusinessLogic.Models;
using BusinessObjects;
using PagedList;
using Synoptek.SessionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.Security.Application;
using System.Security.Claims;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Admin)]
    public class TestimonialController : BaseController
    {
        #region Common variables  
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion

        #region Gel all Testimonial
        public ActionResult GetAllTestimonial(int? page)
        {
            var pageSize = 3;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var testimonialBA = new Testimonial();
            Session["TestimonialImages"] = null;
            var lstTestimonial = testimonialBA.GetAllTestimonial();
            foreach (var item in lstTestimonial)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "TestimonialImagePath", Convert.ToString(item.ID));
            }
            var lstTestimonialPages = lstTestimonial.ToPagedList(pageIndex, pageSize);
            return View("Index", lstTestimonialPages);
        }
        #endregion

        #region Create Testimonial
        public ActionResult AddTestimonial()
        {
            TestimonialModel testimonialModel = new TestimonialModel();
            return View(testimonialModel);
        }
        #endregion

        #region Testimonial Listing Details Partial
        public ActionResult TestimonialListingDetailsPartial(int? page)
        {
            var testimonialBA = new Testimonial();
            var Testimonialmodel = new TestimonialModelWrapper();
            var lstTestimonial = testimonialBA.GetAllTestimonial();
            var pageSize = 3;
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            foreach (var item in lstTestimonial)
            {
                item.ImagePath = CheckFileExists(item.ImagePath, "TestimonialImagePath", Convert.ToString(item.ID));
            }
            var lstTestimonialPages = lstTestimonial.ToPagedList(pageIndex, pageSize);
            return PartialView("_TestimonialList", lstTestimonialPages);
        }
        #endregion

        #region Save Testimonial details
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SaveTestimonialDetails(TestimonialModel testimonialModel)
        {
            var serialization = new Serialization();
            var testimonialBA = new Testimonial();
            var HashCriteria = new Hashtable();
            var TestimonialDetails = new TestimonialModel();
            var actualCriteria = string.Empty;
            if (ModelState.IsValid)
            {
                HashCriteria.Add("ID", testimonialModel.ID);
                HashCriteria.Add("Author", testimonialModel.Author);
                var description = Sanitizer.GetSafeHtml(testimonialModel.Description);
                HashCriteria.Add("Description", description);
                HashCriteria.Add("UserID", userID);
                actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var result = testimonialBA.SaveTestimonialDetails(actualCriteria);
                var testimonialID = Convert.ToInt32(serialization.DeSerializeBinary(Convert.ToString(result)));
                SaveTestimonialImages(testimonialID);
                Session["TestimonialImages"] = null;
                if (testimonialModel.ID <= 0)
                {
                    TempData["TestimonialSuccess"] = "Testimonial details has been saved successfully..!";
                }
                else
                {
                    TempData["TestimonialSuccess"] = "Testimonial details has been modified successfully..!";
                }
            }
            return RedirectToAction("Admin", "Dashboard");
        }
        #endregion

        #region Upload Testimonial Image
        [HttpPost]
        public ActionResult UploadTestimonialImage()
        {
            var TempImagePath = System.Configuration.ConfigurationManager.AppSettings["TestimonialTempImagePath"];
            var folderExists = Directory.Exists(Server.MapPath(TempImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(TempImagePath));

            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                HttpPostedFileBase images = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var stream = fileContent.InputStream;
                    var fileName = Path.GetFileName(images.FileName);
                    var path = Path.Combine(Server.MapPath(TempImagePath), fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    Session["TestimonialImages"] = fileName;
                }
            }
            return Json("File uploaded successfully");
        }
        #endregion

        #region Save Testimonial Images
        public int SaveTestimonialImages(int TestimonialID)
        {
            var serialization = new Serialization();
            var testimonialBA = new Testimonial();
            var HashCriteria = new Hashtable();
            var result = 0;
            var actualCriteria = string.Empty;
            var destinationPath = string.Empty;
            var imageName = string.Empty;
            var TempImagePath = System.Configuration.ConfigurationManager.AppSettings["TestimonialTempImagePath"];
            var ImagePath = System.Configuration.ConfigurationManager.AppSettings["TestimonialImagePath"];
            ImagePath = ImagePath + '/' + TestimonialID.ToString();
            var folderExists = Directory.Exists(Server.MapPath(ImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(ImagePath));

            var lstImages = Convert.ToString(Session["TestimonialImages"]);
            var ActualImagePath = string.Empty;
            if (lstImages != null && lstImages != "")
            {
                //Delete existing files related to this Testimonial ID
                var existingfiles = Directory.GetFiles(Server.MapPath(ImagePath));
                foreach (var file in existingfiles)
                {
                    if (file.Contains("_" + TestimonialID))
                    {
                        if (file != Path.Combine(Server.MapPath(ImagePath), lstImages))
                            System.IO.File.Delete(file);
                    }
                }
                var extension = lstImages.Substring(lstImages.LastIndexOf('.') + 1);
                var index = Convert.ToString(lstImages).IndexOf(".");
                imageName = lstImages.Substring(0, index);
                var TempserverPath = Server.MapPath(TempImagePath + "/" + lstImages);
                var filepresentonserver = Server.MapPath(ImagePath + "/" + lstImages);
                var imagepresenttrue = System.IO.File.Exists(filepresentonserver);
                if (!imagepresenttrue)
                {
                    var filepresent = Server.MapPath(ImagePath + "/" + imageName + "_" + TestimonialID + "." + extension);
                    var imagepresent = System.IO.File.Exists(filepresent);
                    if (!imagepresent)
                    {
                        ActualImagePath = Server.MapPath(ImagePath + "/" + imageName + "_" + TestimonialID + "." + extension);
                        System.IO.File.Copy(TempserverPath, ActualImagePath);
                    }
                    //Update Testimonial image in database
                    var fileName = imageName + "_" + TestimonialID + "." + extension;
                    HashCriteria.Add("Filename", fileName);
                    HashCriteria.Add("ID", TestimonialID);
                    actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    var resultTestimonial = Convert.ToString(testimonialBA.UpdateTestimonialImagePath(actualCriteria));
                    result = Convert.ToInt32(serialization.DeSerializeBinary(Convert.ToString(resultTestimonial)));
                }
                System.IO.File.Delete(Server.MapPath(TempImagePath + "/" + lstImages));
            }
            return result;
        }
        #endregion

        #region Edit listing details
        public ActionResult Edit(string testimonialID)
        {
            var serialization = new Serialization();
            var testimonialBA = new Testimonial();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var testimonialModel = new TestimonialModel();
            var testimonial_ID = Convert.ToInt32(CipherTool.DecryptString(testimonialID));
            HashCriteria.Add("ID", testimonial_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = testimonialBA.EditTestimonialDetails(actualCriteria);
            testimonialModel = (TestimonialModel)(serialization.DeSerializeBinary(Convert.ToString(result)));
            ViewBag.Logo = CheckFileExists(testimonialModel.ImagePath, "TestimonialImagePath", Convert.ToString(testimonial_ID));
            Session["TestimonialImages"] = testimonialModel.ImagePath;
            return View("AddTestimonial", testimonialModel);
        }
        #endregion

        #region Single listing details
        public ActionResult Details(string testimonialID)
        {
            var serialization = new Serialization();
            var testimonialBA = new Testimonial();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var testimonialModel = new TestimonialModel();
            var testimonial_ID = Convert.ToInt32(CipherTool.DecryptString(testimonialID));
            HashCriteria.Add("ID", testimonial_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = testimonialBA.EditTestimonialDetails(actualCriteria);
            testimonialModel = (TestimonialModel)(serialization.DeSerializeBinary(Convert.ToString(result)));
            ViewBag.Logo = CheckFileExists(testimonialModel.ImagePath, "TestimonialImagePath",Convert.ToString(testimonial_ID));
            return View("SingleTestimonialDetails", testimonialModel);
        }
        #endregion

        #region Delete Testimonial
        public ActionResult Delete(string testimonialID, int? page)
        {
            var serialization = new Serialization();
            var testimonialBA = new Testimonial();
            var HashCriteria = new Hashtable();
            var actualCriteria = string.Empty;
            var TestimonialModel = new TestimonialModel();
            var testimonial_ID = Convert.ToInt32(CipherTool.DecryptString(testimonialID));
            HashCriteria.Add("ID", testimonial_ID);
            HashCriteria.Add("UserID", userID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria); 
            var resultTestimonial = testimonialBA.DeleteCurrentTestimonial(actualCriteria);
            var result = (Convert.ToInt32(serialization.DeSerializeBinary(Convert.ToString(resultTestimonial)))); 
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion 
        
    }
}


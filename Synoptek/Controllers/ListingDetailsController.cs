using BusinessLogic.Models;
using BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Synoptek.SessionManagement;
using System.Security.Claims;
using System.Web.Script.Serialization;

namespace Synoptek.Controllers
{
    [Filters.Authorize(ClaimType = ClaimTypes.Role, ClaimValue = Helpers.Constants.UserRoles.Broker + "," + Helpers.Constants.UserRoles.Admin)]
    public class ListingDetailsController : BaseController
    {
        #region Common variables 
        string userID = Convert.ToString(SessionController.UserSession.UserId);
        #endregion 

        #region Get loan listing fields
        public ActionResult AddListing()
        {
            Session["ListingDocuments"] = null;
            Session["ListingImages"] = null;
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var listingModel = new ListingModel();
            listingModel.loanInformation = new LoanInformation();
            var loanInformation = new LoanInformation();
            var result = listingsBA.GetListingDefaultValuesForLoanInformation();
            loanInformation = (LoanInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));
            loanInformation.ListingType = GetListingLoanType();
            listingModel.loanInformation = loanInformation;
            return View(listingModel);
        }
        #endregion

        #region Get listing type
        public List<ListingType> GetListingLoanType()
        {
            var listingsBA = new Listings();
            var lstLoanType = listingsBA.GetListingLoanTypes();
            return lstLoanType;
        }
        #endregion

        #region Save Loan Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveLoanInformation(LoanInformation model)
        {
            var serialization = new Serialization();
            
            var listingsBA = new Listings();
            long listingID = 0;
           
           if (Request.Form["ginfo-hdn"] != null && Request.Form["ginfo-hdn"] != "")
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model.GuarantorInformation = js.Deserialize<List<GuarantorInformation>>(Request.Form["ginfo-hdn"]);
            }

           if(model.IsCompany)
            {
                ModelState.Remove("PrimaryBorrowerFirstName");
                ModelState.Remove("LastName");
            }
            else
            {
                ModelState.Remove("CompanyName");
            }
            if (ModelState.IsValid)
            {
                var HashCriteria = SetHashCriteriaForLoanInformation(model);
                var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var result = listingsBA.SaveLoanInformation(actualCriteria);
                listingID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            }
            else
            {
                LoanInformation loanInformation = new LoanInformation();
                var result = listingsBA.GetListingDefaultValuesForLoanInformation();
                loanInformation = (LoanInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));
                loanInformation.ListingType = GetListingLoanType();
                loanInformation.IsGuaranteed = model.IsGuaranteed;
                loanInformation.GuarantorInformation = model.GuarantorInformation;
                loanInformation.IsCompany = model.IsCompany;
                //loanInformation.PrimaryBorrowerFirstName = model.PrimaryBorrowerFirstName;
                //loanInformation.LastName = model.LastName;

                return PartialView("_LoanInformation", loanInformation);
            }
            string ListingID = CipherTool.EncryptString(Convert.ToString(listingID), true);
            var jsonResult = new[] {
                      new { ListingID = ListingID, ID = listingID },
                      };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Property Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePropertyInformation(PropertyInformation model)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            long listingID = 0;
            if (ModelState.IsValid)
            {
                var HashCriteria = SetHashCriteriaForPropertyInformation(model);
                var actualCriteria = serialization.SerializeBinary((object)HashCriteria);

                var result = listingsBA.SavePropertyInformation(actualCriteria);
                listingID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            }
            else
            {
                PropertyInformation propertyInformation = new PropertyInformation();
                var result = listingsBA.GetListingDefaultValuesForPropertyInformation();
                propertyInformation = (PropertyInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));
                return PartialView("_PropertyAndBorrowerInfo", propertyInformation);
            }
            string ListingID = CipherTool.EncryptString(Convert.ToString(listingID), true);
            var jsonResult = new[] {
                      new { ListingID = ListingID, ID = listingID },
                      };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Comments Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCommentsInformation(CommentsInformation model)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            long listingID = 0;
            if (ModelState.IsValid)
            {
                Hashtable HashCriteria = new Hashtable();
                HashCriteria.Add("ListingID", model.ID);
                HashCriteria.Add("Comments", model.Comments);
                HashCriteria.Add("IsSellerOffering", model.IsSellerOffering);
                HashCriteria.Add("SellerOfferingPercentage", model.SellerOfferingPercentage);
                HashCriteria.Add("UserID", userID);
                var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var result = listingsBA.SaveCommentsInformation(actualCriteria);
                listingID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
            }
            else
            {
                return PartialView("_Comments", model);
            }
            var ListingID = CipherTool.EncryptString(Convert.ToString(listingID), true);
            var jsonResult = new[] {
                      new { ListingID = ListingID, ID = listingID },
                      };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Document Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDocumentsInformation(DocumentsInformation model)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            long listingID = 0;
            if (ModelState.IsValid)
            {
                var HashCriteria = SetHashCriteriaForDocumentInformation(model);
                var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                var result = listingsBA.SaveDocumentsInformation(actualCriteria);
                listingID = Convert.ToInt64(serialization.DeSerializeBinary(Convert.ToString(result)));
                SaveListingDocuments(Convert.ToInt64(listingID));
                SaveListingImages(Convert.ToInt64(listingID));
                Session["ListingDocuments"] = null;
                Session["ListingImages"] = null;
            }
            else
            {
                return PartialView("_Documents", model);
            }
            string ListingID = Synoptek.SessionManagement.CipherTool.EncryptString(Convert.ToString(listingID), true);
            var jsonResult = new[] {
                      new { ListingID = ListingID, ID = listingID },
                      };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Save Listing Documents
        public long SaveListingDocuments(long listingID)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var result = string.Empty;
            var resultlisting = 0;
            var lstDocuments = Session["ListingDocuments"] as List<ListingLoanDocuments>;
            if (lstDocuments != null && lstDocuments.Count > 0)
            {
                foreach (var tempfile in lstDocuments)
                {
                    var TempDocumentPath = System.Configuration.ConfigurationManager.AppSettings["ListingTempDocumentPath"];
                    TempDocumentPath = TempDocumentPath + "/" + tempfile.DocumentType;
                    var DocumentPath = System.Configuration.ConfigurationManager.AppSettings["ListingDocumentPath"] + "/" + listingID;
                    DocumentPath = DocumentPath + "/" + tempfile.DocumentType;
                    var folderExists = Directory.Exists(Server.MapPath(DocumentPath));
                    if (!folderExists)
                        Directory.CreateDirectory(Server.MapPath(DocumentPath));

                    var TempserverPath = Server.MapPath(TempDocumentPath + "/" + tempfile.FileName);
                    var ActualImagePath = Server.MapPath(DocumentPath + "/" + tempfile.FileName);
                    var docpresent = System.IO.File.Exists(ActualImagePath);
                    var tempPresent = System.IO.File.Exists(TempserverPath);
                    if (!docpresent && tempPresent)
                    {
                        if (!tempfile.IsDeleted)
                            System.IO.File.Copy(TempserverPath, ActualImagePath);
                    }
                    if (docpresent && tempfile.IsDeleted)
                        System.IO.File.Delete(Server.MapPath(DocumentPath + "/" + tempfile.FileName));

                    //Save listing Documents in database 
                    Hashtable HashCriteria = new Hashtable();
                    HashCriteria.Add("FileName", tempfile.FileName);
                    HashCriteria.Add("IsDeleted", tempfile.IsDeleted);
                    HashCriteria.Add("ID", tempfile.id);
                    HashCriteria.Add("DocumentTypeID", tempfile.DocumentTypeID);
                    HashCriteria.Add("ListingID", listingID);
                    HashCriteria.Add("UserID", userID);
                    var actualCriteria = serialization.SerializeBinary((object)HashCriteria);

                    result = Convert.ToString(listingsBA.SaveUploadedListingDocuments(actualCriteria));
                    resultlisting = Convert.ToInt32(serialization.DeSerializeBinary(Convert.ToString(result)));
                    var res = System.IO.File.Exists(TempDocumentPath + "/" + tempfile.FileName);
                    if (res)
                    {
                        System.IO.File.Delete(Server.MapPath(TempDocumentPath + "/" + tempfile.FileName));
                    }
                }
            }
            return resultlisting;
        }
        #endregion

        #region Save Listing Images
        public string SaveListingImages(long listingID)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var uploadFileModel = new List<ListingImage>();
            var destinationPath = string.Empty;
            var imageName = string.Empty;
            var result = string.Empty;
            var ActualImagePath = string.Empty;
            var TempImagePath = System.Configuration.ConfigurationManager.AppSettings["ListingTempImagePath"];
            var ImagePath = System.Configuration.ConfigurationManager.AppSettings["ListingImagePath"];
            ImagePath = ImagePath + '/' + listingID.ToString();
            var lstImages = Session["ListingImages"] as List<ListingImage>;
            var folderExists = Directory.Exists(Server.MapPath(ImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(ImagePath));

            if (lstImages != null && lstImages.Count > 0)
            {
                //Delete existing files related to this listing ID
                //string[] existingfiles = System.IO.Directory.GetFiles(Server.MapPath(ImagePath));
                //foreach (string file in existingfiles)
                //{
                //    if (file.Contains("_" + listingID))
                //    {
                //        if (file != Path.Combine(Server.MapPath(ImagePath), lstImages))
                //            System.IO.File.Delete(file);
                //    }
                //} 
                foreach (var tempfile in lstImages)
                {
                    var extension = tempfile.FileName.Substring(tempfile.FileName.LastIndexOf('.') + 1);
                    var index = Convert.ToString(tempfile.FileName).IndexOf(".");
                    imageName = tempfile.FileName.Substring(0, index);
                    var TempserverPath = Server.MapPath(TempImagePath + "/" + tempfile.FileName);
                    var filepresentonserver = Server.MapPath(ImagePath + "/" + tempfile.FileName);
                    var imagepresenttrue = System.IO.File.Exists(filepresentonserver);
                    if (!imagepresenttrue)
                    {
                        var filepresent = Server.MapPath(ImagePath + "/" + imageName + "_" + listingID + "." + extension);
                        var imagepresent = System.IO.File.Exists(filepresent);
                        if (!imagepresent)
                        {
                            ActualImagePath = Server.MapPath(ImagePath + "/" + imageName + "_" + listingID + "." + extension);
                            if (System.IO.File.Exists(TempserverPath))
                                System.IO.File.Copy(TempserverPath, ActualImagePath);
                        }
                        System.IO.File.Delete(Server.MapPath(TempImagePath + "/" + tempfile.FileName));
                    }

                    //Update listing image in database
                    var HashCriteria = new Hashtable();
                    string FileName = imageName + "_" + listingID + "." + extension;
                    HashCriteria.Add("FileName", FileName);
                    HashCriteria.Add("ID", tempfile.id);
                    HashCriteria.Add("IsDeleted", tempfile.IsDeleted);
                    HashCriteria.Add("ListingID", listingID);
                    HashCriteria.Add("UserID", userID);
                    HashCriteria.Add("IsFeatured", tempfile.IsFeatured);
                    var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
                    result = listingsBA.UpdateListingImagePath(actualCriteria);
                    result = Convert.ToString(serialization.DeSerializeBinary(Convert.ToString(result)));
                    System.IO.File.Delete(Server.MapPath(TempImagePath + "/" + tempfile.FileName));
                }
            }
            return result;
        }
        #endregion

        #region Edit Loan Information
        public ActionResult Edit(string listingID, bool Internal = false)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var HashCriteria = new Hashtable();
            var listingsModel = new ListingModel();
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID));
            listingsModel.loanInformation = new LoanInformation();
            var dropdownValues = new LoanInformation();
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("CurrentTab", "LoanInformation");
            var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = listingsBA.EditCurrentListing(actualCriteria);
            var listingEditModel = (LoanInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var defaultValues = listingsBA.GetListingDefaultValuesForLoanInformation();
            dropdownValues = (LoanInformation)(serialization.DeSerializeBinary(Convert.ToString(defaultValues)));
            listingEditModel.ListingType = GetListingLoanType();
            listingEditModel.RateType = dropdownValues.RateType;
            listingEditModel.AmortizationType = dropdownValues.AmortizationType;
            listingEditModel.PaymentsFrequency = dropdownValues.PaymentsFrequency;
            listingEditModel.LienPositionType = dropdownValues.LienPositionType;
            listingEditModel.JuniorTrustDeedORMortgageList = dropdownValues.JuniorTrustDeedORMortgageList;
            listingsModel.loanInformation = listingEditModel;
            if (Internal)
                return PartialView("_LoanInformation", listingsModel.loanInformation);

            return View("AddListing", listingsModel);
        }
        #endregion

        #region Edit Property Information
        public ActionResult EditPropertyInformation(string listingID)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var HashCriteria = new Hashtable();
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID));
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("CurrentTab", "PropertyInformation");
            var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = listingsBA.EditCurrentListing(actualCriteria);
            var listingEditModel = (PropertyInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));
            var defaultValues = listingsBA.GetListingDefaultValuesForPropertyInformation();
            var resultModel = (PropertyInformation)(serialization.DeSerializeBinary(Convert.ToString(defaultValues)));
            listingEditModel.PropertyType = resultModel.PropertyType;
            listingEditModel.OccupancyStatus = resultModel.OccupancyStatus;
            listingEditModel.ValuationType = resultModel.ValuationType;
            return PartialView("_PropertyAndBorrowerInfo", listingEditModel);
        }
        #endregion

        #region Edit Comments Information
        public ActionResult EditCommentsInformation(string listingID)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var HashCriteria = new Hashtable();
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID));
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("CurrentTab", "CommentsInformation");
            var actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = listingsBA.EditCurrentListing(actualCriteria);
            var listingEditModel = (CommentsInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));
            return PartialView("_Comments", listingEditModel);
        }
        #endregion

        #region Edit Document Information
        public ActionResult EditDocumentInformation(string listingID)
        {
            var serialization = new Serialization();
            var listingsBA = new Listings();
            var actualCriteria = string.Empty;
            var listingEditModel = new DocumentsInformation();
            var listing_ID = Convert.ToInt64(CipherTool.DecryptString(listingID, true));

            var HashCriteria = new Hashtable();
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", listing_ID);
            HashCriteria.Add("CurrentTab", "DocumentInformation");
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var result = listingsBA.EditCurrentListing(actualCriteria);
            listingEditModel = (DocumentsInformation)(serialization.DeSerializeBinary(Convert.ToString(result)));

            HashCriteria = new Hashtable();
            actualCriteria = string.Empty;
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", listing_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultDocuments = listingsBA.GetLoanDocuments(actualCriteria);
            listingEditModel.ListingLoanDocuments = (List<ListingLoanDocuments>)(serialization.DeSerializeBinary(Convert.ToString(resultDocuments)));

            HashCriteria = new Hashtable();
            actualCriteria = string.Empty;
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("ID", listing_ID);
            actualCriteria = serialization.SerializeBinary((object)HashCriteria);
            var resultImages = listingsBA.GetLoanImages(actualCriteria);
            listingEditModel.ListingImages = (List<ListingImage>)(serialization.DeSerializeBinary(Convert.ToString(resultImages)));
            foreach (var item in listingEditModel.ListingImages)
            {
                item.ImagePath = CheckFileExists(item.FileName, "ListingImagePath", Convert.ToString(listing_ID));
            }
            Session["ListingDocuments"] = listingEditModel.ListingLoanDocuments;
            Session["ListingImages"] = listingEditModel.ListingImages;
            return PartialView("_Documents", listingEditModel);
        }
        #endregion

        #region Upload listing loan documents
        public ActionResult UploadloanDocuments(string FileType, string DocID)
        {
            List<ListingLoanDocuments> uploadFileModel = new List<ListingLoanDocuments>();
            string ImagePath = System.Configuration.ConfigurationManager.AppSettings["ListingTempDocumentPath"];
            ImagePath = ImagePath + "/" + FileType;
            bool folderExists = Directory.Exists(Server.MapPath(ImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(ImagePath));
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                HttpPostedFileBase photo = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var stream = fileContent.InputStream;
                    var fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath(ImagePath), fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    { 
                        var result = ((List<ListingLoanDocuments>)Session["ListingDocuments"]).FindAll(x => x.DocumentTypeID == Convert.ToInt32(DocID)); // Deleting additional files with same document type.
                        foreach (var item in result)
                        {
                            FileInfo f = new FileInfo(Server.MapPath(ImagePath) + "/" + item.FileName);
                            if (f.Exists)
                            {
                                f.Delete();
                            }
                        }
                        stream.CopyTo(fileStream);
                    }
                    if (Session["ListingDocuments"] != null)
                    {
                        var isFileNameRepete = ((List<ListingLoanDocuments>)Session["ListingDocuments"]).Find(x => x.FileName == fileName && x.DocumentTypeID == Convert.ToInt32(DocID));
                        if (isFileNameRepete == null)
                        {
                            ((List<ListingLoanDocuments>)Session["ListingDocuments"]).RemoveAll(x => x.DocumentTypeID == Convert.ToInt32(DocID)); // removing documents from session if multiple doc present under same documentyType
                            ((List<ListingLoanDocuments>)Session["ListingDocuments"]).Add(new ListingLoanDocuments { FileName = fileName, IsDeleted = false, DocumentTypeID = Convert.ToInt32(DocID), DocumentType = FileType });
                            ViewBag.Message = "File uploaded successfully";
                        }
                        else
                        {
                            ViewBag.Message = "File is already exists";
                        }
                    }
                    else
                    {
                        uploadFileModel.Add(new ListingLoanDocuments { FileName = fileName, IsDeleted = false, DocumentTypeID = Convert.ToInt32(DocID), DocumentType = FileType });
                        Session["ListingDocuments"] = uploadFileModel;
                        ViewBag.Message = "File uploaded successfully";
                    }
                }
            }
            return Json("File uploaded successfully");
        }
        #endregion

        #region Remove loan Documents
        public JsonResult RemoveUploadFile(string fileName, string DocTypeId, string ListingId, string DocType)
        {
            var _ImagePath = System.Configuration.ConfigurationManager.AppSettings["ListingTempDocumentPath"];
            var serverPath = Server.MapPath(_ImagePath + "/" + fileName.Trim());
            var _ImageActualPath = System.Configuration.ConfigurationManager.AppSettings["ListingDocumentPath"] + "//" + ListingId;
            var actualserverPath = Server.MapPath(_ImageActualPath + "/" + DocType + "/" + fileName.Trim());
            var listingDocuments = (List<ListingLoanDocuments>)Session["ListingDocuments"];
            foreach (var filename in listingDocuments)
            {
                if (fileName != null || fileName != string.Empty)
                {
                    if (filename.DocumentTypeID == Convert.ToInt32(DocTypeId))
                    {
                        FileInfo file = new FileInfo(serverPath);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        FileInfo actualfile = new FileInfo(actualserverPath);
                        if (actualfile.Exists)
                        {
                            actualfile.Delete();
                        }
                        var documents = ((List<ListingLoanDocuments>)Session["ListingDocuments"]).Find(x => x.FileName == fileName && x.DocumentTypeID == Convert.ToInt32(DocTypeId));
                        documents.IsDeleted = true;
                    }
                }
            }
            return Json("File deleted successfully", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Make Featured Image
        public JsonResult MakeFeaturedImage(string fileName, bool isFeatured)
        {
            var listingImages = (List<ListingImage>)Session["ListingImages"];
            var modified = false;
            foreach (var filename in listingImages)
            {
                if (fileName != null || fileName != string.Empty)
                {
                    if (!modified)
                    {
                        var currentImage = ((List<ListingImage>)Session["ListingImages"]).Find(x => x.FileName == fileName.Trim());
                        currentImage.IsFeatured = isFeatured;
                        modified = true;
                    }
                    var remainigImages = ((List<ListingImage>)Session["ListingImages"]).FindAll(x => x.FileName != fileName.Trim());
                    foreach (var item in remainigImages)
                    {
                        item.IsFeatured = false;
                    }
                }
            }
            return Json("Featured image updated successfully", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Upload listing image
        [HttpPost]
        public ActionResult UploadListingImage()
        {
            var tempImagePath = System.Configuration.ConfigurationManager.AppSettings["ListingTempImagePath"];
            var folderExists = Directory.Exists(Server.MapPath(tempImagePath));
            var uploadFileModel = new List<ListingImage>();
            var destinationPath = string.Empty;
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(tempImagePath));

            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];
                HttpPostedFileBase images = Request.Files[file];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var stream = fileContent.InputStream;
                    var fileName = Path.GetFileName(images.FileName);
                    fileName = fileName.Replace(" ", "_");
                    var path = Path.Combine(Server.MapPath(tempImagePath), fileName);
                    destinationPath = Request.Url.GetLeftPart(UriPartial.Authority) + tempImagePath + "/" + fileName;
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    if (Session["ListingImages"] != null)
                    {
                        var isFileNameRepete = ((List<ListingImage>)Session["ListingImages"]).Find(x => x.FileName == fileName);
                        if (isFileNameRepete == null)
                        {
                            ((List<ListingImage>)Session["ListingImages"]).Add(new ListingImage { FileName = fileName, IsDeleted = false, IsFeatured = false });
                            ViewBag.Message = "File uploaded successfully";
                        }
                        else
                        {
                            ViewBag.Message = "File is already exists";
                        }
                    }
                    else
                    {
                        uploadFileModel.Add(new ListingImage { FileName = fileName, IsDeleted = false });
                        Session["ListingImages"] = uploadFileModel;
                    }
                }
            }
            return Json(destinationPath);
        }
        #endregion

        #region Remove Temp File
        [HttpPost]
        public ActionResult RemoveTempFile(string filename)
        {
            var TempImagePath = System.Configuration.ConfigurationManager.AppSettings["ListingTempImagePath"];
            var folderExists = Directory.Exists(Server.MapPath(TempImagePath));
            if (!folderExists)
                Directory.CreateDirectory(Server.MapPath(TempImagePath));

            //Delete existing file  
            var existingfiles = Directory.GetFiles(Server.MapPath(TempImagePath));
            foreach (string file in existingfiles)
            {
                if (file.Contains(filename))
                {
                    if (file == Path.Combine(Server.MapPath(TempImagePath), filename))
                        System.IO.File.Delete(file);
                }
            }
            return Json("File uploaded successfully");
        }
        #endregion

        #region Set Hash Criteria For Loan Information
        [NonAction]
        private Hashtable SetHashCriteriaForLoanInformation(LoanInformation loanInformation)
        {
            var HashCriteria = new Hashtable();
            HashCriteria.Add("ListingID", loanInformation.ID);
            HashCriteria.Add("Name", loanInformation.Name);
            HashCriteria.Add("ListingTypeID", loanInformation.ListingTypeID);
            HashCriteria.Add("RateTypeID", loanInformation.RateTypeID);
            HashCriteria.Add("AmortizationTypeID", loanInformation.AmortizationTypeID);
            HashCriteria.Add("PrePayPenalty", loanInformation.PrePayPenalty);
            HashCriteria.Add("InDefault", loanInformation.InDefault);
            HashCriteria.Add("BalloonPayment", loanInformation.BalloonPayment);
            HashCriteria.Add("LoanTermsModified", loanInformation.LoanTermsModified);
            HashCriteria.Add("OnForbearance", loanInformation.OnForbearance);
            HashCriteria.Add("Registered_wMERS", loanInformation.Registered_wMERS);
            HashCriteria.Add("InForeclosure", loanInformation.InForeclosureID);
            HashCriteria.Add("InBankruptcy", loanInformation.InBankruptcyID);
            HashCriteria.Add("OriginationDate", loanInformation.OriginationDate);
            HashCriteria.Add("PaidToDate", loanInformation.PaidToDate);
            HashCriteria.Add("NextPaymentDate", loanInformation.NextPaymentDate);
            HashCriteria.Add("PayersLastPaymentMadeDate", loanInformation.PayersLastPaymentMadeDate);
            HashCriteria.Add("OriginalLoanAmount", loanInformation.OriginalLoanAmount);
            HashCriteria.Add("UnpaidPrincipalBalance", loanInformation.UnpaidPrincipalBalance);
            HashCriteria.Add("NoteMaturityDate", loanInformation.NoteMaturityDate);
            HashCriteria.Add("AccruedLateCharges", loanInformation.AccruedLateCharges);
            HashCriteria.Add("LoanCharges", loanInformation.LoanCharges);
            HashCriteria.Add("NoOfPaymentsInLast12", loanInformation.NoOfPaymentsInLast12);
            HashCriteria.Add("FirstPaymentDate", loanInformation.FirstPaymentDate);
            HashCriteria.Add("EscrowImpounds", loanInformation.EscrowImpounds);
            HashCriteria.Add("NoteInterestRate", loanInformation.NoteInterestRate);
            HashCriteria.Add("SoldInterestRate", loanInformation.SoldInterestRate);
            HashCriteria.Add("PaymentsFrequency", loanInformation.PaymentsFrequencyID);
            HashCriteria.Add("LateCharges", loanInformation.LateCharges);
            HashCriteria.Add("LateChargesType", loanInformation.LateChargesType);
            HashCriteria.Add("LateChargesAfterXDays", loanInformation.LateChargesAfterXDays);
            HashCriteria.Add("UnpaidInterest", loanInformation.UnpaidInterest);
            HashCriteria.Add("PropertyTaxesDue", loanInformation.PropertyTaxesDue);
            HashCriteria.Add("PaymentTrust", loanInformation.PaymentTrust);
            HashCriteria.Add("pAndL", loanInformation.pAndL);
            HashCriteria.Add("TotalMonthlyLoanPayment", loanInformation.TotalMonthlyLoanPayment);

            HashCriteria.Add("CheckAllThatApply", loanInformation.CheckAllThatApplyID);
            HashCriteria.Add("JuniorTrustDeedORMortgage", loanInformation.JuniorTrustDeedORMortgageID);
            HashCriteria.Add("LienPosition", loanInformation.LienPositionID);
            HashCriteria.Add("PrincipalBalance", loanInformation.PrincipalBalance);
            HashCriteria.Add("MonthlyPayment", loanInformation.MonthlyPayment);
            HashCriteria.Add("Modified", loanInformation.Modified);
            HashCriteria.Add("Current", loanInformation.Current);
            HashCriteria.Add("DrawPeriodStartDate", loanInformation.DrawPeriodStartDate);
            HashCriteria.Add("RepaymentPeriodStartDate", loanInformation.RepaymentPeriodStartDate);
            HashCriteria.Add("InformationAsOf", loanInformation.InformationAsOf);

            HashCriteria.Add("ArmInfo_NextAdjustment", loanInformation.ArmInfo_NextAdjustment);
            HashCriteria.Add("ArmInfo_AdjPaymentChangeDate", loanInformation.ArmInfo_AdjPaymentChangeDate);
            HashCriteria.Add("ArmInfo_AdjustmentFrequency", loanInformation.ArmInfo_AdjustmentFrequency);
            HashCriteria.Add("ArmInfo_Floor", loanInformation.ArmInfo_Floor);
            HashCriteria.Add("ArmInfo_IndexName", loanInformation.ArmInfo_IndexName);
            HashCriteria.Add("ArmInfo_Margin", loanInformation.ArmInfo_Margin);
            HashCriteria.Add("ArmInfo_Ceiling", loanInformation.ArmInfo_Ceiling);
            HashCriteria.Add("ListingStatusID", loanInformation.ListingStatusID);
            HashCriteria.Add("UserID", userID);
            HashCriteria.Add("GuarantorInformation", loanInformation.GuarantorInformation);

            //Brower information
            HashCriteria.Add("IsCompany", loanInformation.IsCompany);
            HashCriteria.Add("PrimaryBorrowerFirstName", loanInformation.PrimaryBorrowerFirstName);
            HashCriteria.Add("LastName", loanInformation.LastName);
            HashCriteria.Add("MailingAddress", loanInformation.MailingAddress);
            HashCriteria.Add("BorrowerCity", loanInformation.BorrowerCity);
            HashCriteria.Add("BorrowerState", loanInformation.BorrowerState);
            HashCriteria.Add("BorrowerZip", loanInformation.BorrowerZip);
            HashCriteria.Add("BorrowerHomePh", loanInformation.BorrowerHomePh);
            HashCriteria.Add("BorrowerWorkPh", loanInformation.BorrowerWorkPh);
            HashCriteria.Add("BorrowerMobilePh", loanInformation.BorrowerMobilePh);
            HashCriteria.Add("BorrowerEmail", loanInformation.BorrowerEmail);
            HashCriteria.Add("BorrowerFax", loanInformation.BorrowerFax);
            HashCriteria.Add("BorrowerSSTaxIDNoPrimary", loanInformation.BorrowerSSTaxIDNoPrimary);
            HashCriteria.Add("CompanyName", loanInformation.CompanyName);

            return HashCriteria;
        }
        #endregion

        #region Set Hash Criteria for Property Information
        [NonAction]
        private Hashtable SetHashCriteriaForPropertyInformation(PropertyInformation listingsModel)
        {
            var HashCriteria = new Hashtable();
            HashCriteria.Add("ListingID", listingsModel.ID);
            HashCriteria.Add("PropertyTypeID", listingsModel.PropertyTypeID);
            HashCriteria.Add("PropertyAddress", listingsModel.PropertyAddress);
            HashCriteria.Add("City", listingsModel.City);
            HashCriteria.Add("Country", listingsModel.Country);
            HashCriteria.Add("Zip", listingsModel.Zip);

            HashCriteria.Add("OccupancyStatusID", listingsModel.OccupancyStatusID);
            HashCriteria.Add("StateName", listingsModel.StateName);
            HashCriteria.Add("PropertyMarketValue", listingsModel.PropertyMarketValue);
            HashCriteria.Add("PropertyValuationDate", listingsModel.PropertyValuationDate);
            HashCriteria.Add("ValuationTypeID", listingsModel.ValuationTypeID);
            HashCriteria.Add("IsCompany", listingsModel.IsCompany);

            HashCriteria.Add("PrimaryBorrowerFirstName", listingsModel.PrimaryBorrowerFirstName);
            HashCriteria.Add("LastName", listingsModel.LastName);
            HashCriteria.Add("MailingAddress", listingsModel.MailingAddress);
            HashCriteria.Add("BorrowerCity", listingsModel.BorrowerCity);
            HashCriteria.Add("BorrowerState", listingsModel.BorrowerState);
            HashCriteria.Add("BorrowerZip", listingsModel.BorrowerZip);
            HashCriteria.Add("BorrowerHomePh", listingsModel.BorrowerHomePh);
            HashCriteria.Add("BorrowerWorkPh", listingsModel.BorrowerWorkPh);
            HashCriteria.Add("BorrowerMobilePh", listingsModel.BorrowerMobilePh);
            HashCriteria.Add("BorrowerEmail", listingsModel.BorrowerEmail);
            HashCriteria.Add("BorrowerFax", listingsModel.BorrowerFax);
            HashCriteria.Add("BorrowerSSTaxIDNoPrimary", listingsModel.BorrowerSSTaxIDNoPrimary);
            HashCriteria.Add("ListingStatusID", listingsModel.ListingStatusID);
            HashCriteria.Add("UserID", userID);
            return HashCriteria;
        }
        #endregion

        #region Set Hash Criteria for Document Information
        [NonAction]
        private Hashtable SetHashCriteriaForDocumentInformation(DocumentsInformation document)
        {
            var HashCriteria = new Hashtable();
            HashCriteria.Add("ListingID", document.ID);
            HashCriteria.Add("ListingStatusID", document.ListingStatusID);
            HashCriteria.Add("IsSponsored", document.IsSponsored);
            HashCriteria.Add("ChainAssignments", document.ChainAssignments);
            HashCriteria.Add("UserID", userID);
            return HashCriteria;
        }
        #endregion
    }
}


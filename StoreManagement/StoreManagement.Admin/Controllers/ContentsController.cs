﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Admin.Controllers
{
    public abstract class ContentsController : BaseController
    {

        private String ContentType { set; get; }

        public ContentsController(String contentType)
        {
            this.ContentType = contentType;
        }


        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            List<Content> resultList = new List<Content>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = ContentRepository.GetContentByTypeAndCategoryId(storeId, ContentType, categoryId, search);
            }


           
            var contentsAdminViewModel = new ContentsAdminViewModel();
            contentsAdminViewModel.Contents = resultList;
            contentsAdminViewModel.Categories = CategoryRepository.GetCategoriesByStoreIdFromCache(storeId, ContentType);
            return View(contentsAdminViewModel);
        }

        //
        // GET: /Content/Details/5

        public ActionResult Details(int id = 0)
        {
            Content content = ContentRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        //
        // GET: /Content/Create

        public ActionResult SaveOrEdit(int id = 0)
        {
            var content = new Content();
            var labels = new List<LabelLine>();
            if (id == 0)
            {
                content.Type = ContentType;
                content.UpdatedDate = DateTime.Now;
                content.CreatedDate = DateTime.Now;
                content.State = true;

            }
            else
            {
                content = ContentRepository.GetSingle(id);
                content.UpdatedDate = DateTime.Now;
                labels = LabelLineRepository.GetLabelLinesByItem(id, ContentType);
                if (!CheckRequest(content)) //security for right store and its item.
                {
                    return HttpNotFound("Not Found");
                }
            }

            ViewBag.SelectedLabels = labels.Select(r => r.LabelId).ToArray();


            return View(content);
        }

        //
        // POST: /Content/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Content content, int[] selectedFileId = null, int[] selectedLabelId = null)
        {
            if (ModelState.IsValid)
            {
                if (content.CategoryId == 0)
                {

                    var labels = new List<LabelLine>();

                    if (content.Id > 0)
                    {
                        labels = LabelLineRepository.GetLabelLinesByItem(content.Id, ContentType);
                    }
                    
                    ViewBag.SelectedLabels = labels.Select(r => r.LabelId).ToArray();

                    ModelState.AddModelError("CategoryId", "You should select category from category tree.");
                    return View(content);
                }

                if (content.Id == 0)
                {
                    ContentRepository.Add(content);
                }
                else
                {
                    ContentRepository.Edit(content);
                }
                ContentRepository.Save();
                int contentId = content.Id;
                if (selectedFileId != null)
                {
                    ContentFileRepository.SaveContentFiles(selectedFileId, contentId);
                }
                LabelLineRepository.SaveLabelLines(selectedLabelId, contentId, ContentType);

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = content.StoreId, categoryId = content.CategoryId });
                }
                else
                {
                    return RedirectToAction("Index", new { categoryId = content.CategoryId });
                }


            }

            return View(content);
        }

    


        //
        // GET: /Content/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id = 0)
        {
            Content content = ContentRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        //
        // POST: /Content/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = ContentRepository.GetSingle(id);
            ContentRepository.Delete(content);
            ContentRepository.Save();


            if (IsSuperAdmin)
            {
                return RedirectToAction("Index", new { storeId = content.StoreId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }



    }
}
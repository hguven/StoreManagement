﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class HomeController : BaseController
    {

        [OutputCache(CacheProfile = "Cache1Hour")]
        public async Task<ActionResult> Index()
        {


            int sliderTake = GetSettingValueInt("HomePageSliderImages_ItemsNumber", StoreConstants.DefaultPageSize);

            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "HomePage");
            var sliderTask = FileManagerService.GetStoreCarouselsAsync(StoreId, sliderTake);
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();


            await Task.WhenAll(pageDesignTask, sliderTask);

            var settings = GetStoreSettings();
            HomePageHelper.StoreSettings = settings;
            HomePageHelper.StoreId = this.StoreId;
            HomePageHelper.StoreSettings = GetStoreSettings();

         
            var pageDesing = pageDesignTask.Result;
            var sliderImages = sliderTask.Result;
        

            StoreLiquidResult liquidResult = HomePageHelper.GetHomePageDesign(pageDesing, sliderImages);
            liquidResult.StoreId = this.StoreId;
            liquidResult.PageTitle = GetSettingValue("HomePage_Title", "");
            liquidResult.StoreSettings = settings;
            // Stop timing.
            stopwatch.Stop();

            Logger.Info("Home:Index:Time elapsed: {0} elapsed milliseconds", stopwatch.ElapsedMilliseconds);
            return View(liquidResult);

        }
        public ActionResult Contact()
        {
        
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult About()
        {
            var item = GetSettingValue(StoreConstants.AboutUs);
            return View(item);
        }
        public ActionResult TermsAndCondition()
        {
            var item = GetSettingValue(StoreConstants.TermsAndCondition);
            return View(item);
        }
        public ActionResult Footer()
        {
            var item = GetSettingValue(StoreConstants.Footer);
            return View(item);
        }

        //[ChildActionOnly]
        //public ActionResult MainLayoutJavaScriptFiles()
        //{
        //    int storeId = StoreId;
        //    var pageDesignTask1 = PageDesignService.GetPageDesignByName(storeId, "MainLayoutJavaScriptFiles");
        //    var pageDesignTask2 = PageDesignService.GetPageDesignByName(storeId, "MainLayoutCssFiles");
        //    Task.WaitAll(pageDesignTask1, pageDesignTask2);

        //    if (pageDesignTask1.Result == null)
        //    {
        //        return Content("MainLayoutJavaScriptFiles pageDesign is null");
        //    }
        //    if (pageDesignTask2.Result == null)
        //    {
        //        return Content("MainLayoutCssFiles pageDesign is null");
        //    }


        //    var contentFiles = pageDesignTask1.Result.PageTemplate.HtmlDecode();
        //    contentFiles += pageDesignTask2.Result.PageTemplate.HtmlDecode();
        //    return Content(contentFiles);
        //}

    }
}
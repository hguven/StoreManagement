﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ProductHelper
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Dictionary<string, string> GetProductsIndexPage(Task<StorePagedList<Product>> productsTask,
            Task<PageDesign> pageDesignTask, Task<List<ProductCategory>> categoriesTask)
        {

            Task.WaitAll(pageDesignTask, productsTask, categoriesTask);
            var products = productsTask.Result;
            var blogsPageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;
            var items = new List<ProductLiquid>();
            foreach (var item in products.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                if (category != null)
                {
                    var blog = new ProductLiquid(item, category, blogsPageDesign);
                    items.Add(blog);
                }
            }

            object anonymousObject = new
                {
                    items = from s in items
                            select new
                                {
                                    s.Product.Name,
                                    s.Product.Description,
                                    s.DetailLink,
                                    HasImage = s.ImageLiquid.HasImage
                                }
                };
            
            var indexPageOutput = LiquidEngineHelper.RenderPage(blogsPageDesign.PageTemplate,anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add("PageOutput", indexPageOutput);
            dic.Add("PageSize", products.pageSize.ToStr());
            dic.Add("PageNumber", (products.page - 1).ToStr());
            dic.Add("TotalItemCount", products.totalItemCount.ToStr());
            dic.Add("IsPagingUp", blogsPageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add("IsPagingDown", blogsPageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);


            return dic;
        }

    }
}
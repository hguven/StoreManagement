﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{
    public class ProductFileApiRepository : BaseApiRepository, IProductFileGeneralRepository
    {

        protected override string ApiControllerName { get { return "ProductFiles"; } }


        public ProductFileApiRepository(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public List<ProductFile> GetProductFilesByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        public List<ProductFile> GetProductFilesByFileManagerId(int fileManagerId)
        {
            throw new NotImplementedException();
        }

        public void DeleteProductFileByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        public void SaveProductFiles(int[] selectedFileId, int productId)
        {
            throw new NotImplementedException();
        }



        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }

    
}

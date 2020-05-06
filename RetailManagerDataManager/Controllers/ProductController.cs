using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RetailManagerDataManager.Models;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailManagerDataManager.Controllers
{
    [Authorize(Roles ="Cashier")]
    public class ProductController : ApiController
    {
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();

            return data.GetProducts();
        }

       
    }
}

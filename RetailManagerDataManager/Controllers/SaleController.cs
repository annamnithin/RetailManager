using Microsoft.AspNet.Identity;
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
    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            SaleData data = new SaleData();
            data.SaveSale(sale, userId);
        }

        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSaleReport()
        {
            SaleData data = new SaleData();

            return data.GetSaleReport();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Net.Http;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        // GET: Orders
        NodeOrders500Entities OrdersEntity = new NodeOrders500Entities();

        public IEnumerable<object> GetSalesPeople()
        {
            var salesPeople = from person in OrdersEntity.SalesPersonTables
                              orderby person.LastName
                              select new {person.salesPersonID, person.FirstName, person.LastName };

            return salesPeople.ToList();
        }

        [System.Web.Http.Route("getcities")]
        [HttpGet]
        public IEnumerable<object> GetCities()
        {
            var cities = from city in OrdersEntity.StoreTables
                         orderby city.City
                         select new { city.storeID, city.City };

            return cities.ToList();
        }

        [System.Web.Http.Route("getmarkupresults")]
        [HttpGet]
        public IEnumerable<object> GetMarkupResults()
        {
            var results = from result in OrdersEntity.Orders
                          where result.pricePaid > 13
                          select new { result.StoreTable.City };

            var grouped = from a in results
                          group a by a.City;

            return grouped.ToList();
        }
    }
}

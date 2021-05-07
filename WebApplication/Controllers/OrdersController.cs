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
using System.Diagnostics;

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

        [System.Web.Http.Route("GetEmployeeSales")]
        [HttpGet]
        public IHttpActionResult GetEmployeeSales(string empID)
        {
            int myInt = Int32.Parse(empID);
            try
            {
                var empSalesSum = (from a in OrdersEntity.Orders
                                   where a.salesPersonID == myInt
                                   select (int?)a.pricePaid).Sum()?? 0;

                return Ok(empSalesSum);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return NotFound();
            }

        }

        [System.Web.Http.Route("GetCitySales")]
        [HttpGet]

        public IHttpActionResult GetCitySales(string cityID)
        {
            try
            {
                int myInt = Int32.Parse(cityID);
                var citySalesSum = (from a in OrdersEntity.Orders
                                    where a.storeID == myInt
                                    select (int?)a.pricePaid).Sum() ?? 0;

                return Ok(citySalesSum);
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return NotFound();
            }

        }

    }
}

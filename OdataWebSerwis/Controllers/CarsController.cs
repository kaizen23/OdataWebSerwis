using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using OdataWebSerwis.Models;

namespace OdataWebSerwis.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using OdataWebSerwis.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Car>("Cars");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CarsController : ODataController
    {
        private OdataWebSerwisContext db = new OdataWebSerwisContext();

        // GET: odata/Cars
        [EnableQuery]
        public IQueryable<Car> GetCars()
        {
            return db.Cars;
        }

        // GET: odata/Cars(5)
        [EnableQuery]
        public SingleResult<Car> GetCar([FromODataUri] int key)
        {
            return SingleResult.Create(db.Cars.Where(car => car.Id == key));
        }

        // PUT: odata/Cars(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Car> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Car car = db.Cars.Find(key);
            if (car == null)
            {
                return NotFound();
            }

            patch.Put(car);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(car);
        }

        // POST: odata/Cars
        public IHttpActionResult Post(Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cars.Add(car);
            db.SaveChanges();

            return Created(car);
        }

        // PATCH: odata/Cars(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Car> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Car car = db.Cars.Find(key);
            if (car == null)
            {
                return NotFound();
            }

            patch.Patch(car);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(car);
        }

        // DELETE: odata/Cars(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Car car = db.Cars.Find(key);
            if (car == null)
            {
                return NotFound();
            }

            db.Cars.Remove(car);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarExists(int key)
        {
            return db.Cars.Count(e => e.Id == key) > 0;
        }
    }
}

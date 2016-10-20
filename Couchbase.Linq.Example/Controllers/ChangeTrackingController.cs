using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Couchbase.Core;
using Couchbase.Linq.Example.Documents;
using Couchbase.Linq.Example.Models;
using Couchbase.Linq.Extensions;
using Couchbase.N1QL;

namespace Couchbase.Linq.Example.Controllers
{
    public class ChangeTrackingController : Controller
    {
        private const string IncrementDocument = "AirlineIdIncrement";

        private readonly IBucket _bucket;
        private readonly BucketContext _db;

        public ChangeTrackingController() : this(ClusterHelper.GetBucket("travel-sample"))
        {
        }

        public ChangeTrackingController(IBucket bucket)
        {
            if (bucket == null)
            {
                throw new ArgumentNullException("bucket");
            }

            _bucket = bucket;
            _db = new BucketContext(bucket);
        }

        public ActionResult Index()
        {
            // Apply mutation state from the previous operation, if any, for read-your-own-write
            // Couchbase Server 4.5 only
            var mutationState = TempData["AirlineMutationState"] as MutationState;

            var query = from p in _db.Query<Airline>().ConsistentWith(mutationState)
                orderby p.Name.ToLower()
                select p;

            return View(query);
        }

        public ActionResult Edit(int id)
        {
            var query = from p in _db.Query<Airline>()
                where p.Id == id
                select new AirlineModel
                {
                    Callsign = p.Callsign,
                    Name = p.Name
                };

            var airline = query.FirstOrDefault();
            if (airline == null)
            {
                return HttpNotFound();
            }

            return View(airline);
        }

        [HttpPost]
        public ActionResult Edit(int id, AirlineModel model)
        {
            // Begin tracking changes
            _db.BeginChangeTracking();

            // Query must execute after call to BeginChangeTracking()
            var query = from p in _db.Query<Airline>()
                where p.Id == id
                select p;

            var airline = query.FirstOrDefault();
            if (airline == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Make changes to document(s) and submit them
            airline.Name = model.Name;
            airline.Callsign = model.Callsign;
            _db.SubmitChanges();

            // Collect mutation state for queries on the next page view
            TempData["AirlineMutationState"] = _db.MutationState;

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View(new AirlineModel());
        }

        [HttpPost]
        public ActionResult Create(AirlineModel model)
        {
            EnsureIncrementInitialized();

            // Begin change tracking
            _db.BeginChangeTracking();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var airline = new Airline()
            {
                Id = (int) _bucket.Increment(IncrementDocument).Value,
                Name = model.Name,
                Callsign = model.Callsign
            };

            // Add the document and save the changes
            _db.Save(airline);
            _db.SubmitChanges();

            // Collect mutation state for queries on the next page view
            TempData["AirlineMutationState"] = _db.MutationState;

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var query = from p in _db.Query<Airline>()
                where p.Id == id
                select new AirlineModel
                {
                    Callsign = p.Callsign,
                    Name = p.Name
                };

            var airline = query.FirstOrDefault();
            if (airline == null)
            {
                return HttpNotFound();
            }

            return View(airline);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection formCollection)
        {
            // Begin change tracking
            _db.BeginChangeTracking();

            var query = from p in _db.Query<Airline>()
                where p.Id == id
                select p;

            var airline = query.FirstOrDefault();
            if (airline == null)
            {
                return HttpNotFound();
            }

            // Remove the document and save the changes
            _db.Remove(airline);
            _db.SubmitChanges();

            // Collect mutation state for queries on the next page view
            TempData["AirlineMutationState"] = _db.MutationState;

            return RedirectToAction("Index");
        }

        #region Helpers

        private void EnsureIncrementInitialized()
        {
            // Create the increment document if it doesn't exist
            // and ensure that it's higher than the numbers in travel-sample

            if (!_bucket.Exists(IncrementDocument))
            {
                var maxId = _db.Query<Airline>().Max(p => p.Id);

                _bucket.Insert(IncrementDocument, maxId);
            }
        }

        #endregion
    }
}
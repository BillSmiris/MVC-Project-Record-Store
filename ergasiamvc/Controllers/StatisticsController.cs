using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Net;
using System.Web.Mvc;
using ergasiamvc.Models;

namespace ergasiamvc.Controllers
{
    public class StatisticsController : Controller
    {
        private ChinookEntities db = new ChinookEntities();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TopXArtists()
        {
            return View();
        }

        [HttpPost, ActionName("TopXArtists")]
        [ValidateAntiForgeryToken]
        public ActionResult GetArtists()
        {
            ViewBag.From = Request["from"];
            ViewBag.To = Request["to"];
            ViewBag.NumberOfResults = Request["nor"];
            int? nor = null;
            if (!string.IsNullOrWhiteSpace(Request["nor"]))
            {
                if (Int32.TryParse(Request["nor"], out int j))
                {
                    nor = j;
                }
            }
            DateTime? fromdate = null;
            if (!string.IsNullOrWhiteSpace(Request["from"]))
            {
                fromdate = DateTime.Parse(Request["from"]);
            }
            DateTime? todate = null;
            if (!string.IsNullOrWhiteSpace(Request["to"]))
            {
                todate = DateTime.Parse(Request["to"]);
            }

            if (fromdate == null && todate == null)
            {
                var result = (from i in db.InvoiceLines group i by i.Track.Album into g select new TopXArtistsModel { artist = g.Key.Artist, count = g.Count() }).OrderByDescending(g => g.count);
                if (nor.HasValue)
                {
                    return View(result.Take(nor.Value).Select(g => g.artist).Distinct().ToList());
                }
                else
                {
                    return View(result.Select(g => g.artist).Distinct().ToList());
                }
            }
            else if (fromdate == null)
            {
                var result = (from i in db.InvoiceLines where i.Invoice.InvoiceDate <= todate group i by i.Track.Album into g select new TopXArtistsModel { artist = g.Key.Artist, count = g.Count() }).OrderByDescending(g => g.count);
                if (nor.HasValue)
                {
                    return View(result.Take(nor.Value).Select(g => g.artist).Distinct().ToList());
                }
                else
                {
                    return View(result.Select(g => g.artist).Distinct().ToList());
                }
            }
            else if (todate == null)
            {
                var result = (from i in db.InvoiceLines where i.Invoice.InvoiceDate >= fromdate group i by i.Track.Album into g select new TopXArtistsModel { artist = g.Key.Artist, count = g.Count() }).OrderByDescending(g => g.count);
                if (nor.HasValue)
                {
                    return View(result.Take(nor.Value).Select(g => g.artist).Distinct().ToList());
                }
                else
                {
                    return View(result.Select(g => g.artist).Distinct().ToList());
                }
            }
            var resultf = (from i in db.InvoiceLines where i.Invoice.InvoiceDate >= fromdate && i.Invoice.InvoiceDate <= todate group i by i.Track.Album into g select new TopXArtistsModel { artist = g.Key.Artist, count = g.Count() }).OrderByDescending(g => g.count);
            if (nor.HasValue)
            {
                return View(resultf.Take(nor.Value).Select(g => g.artist).Distinct().ToList());
            }
            else
            {
                return View(resultf.Select(g => g.artist).Distinct().ToList());
            }
        }

        public ActionResult Top10Tracks()
        {
            return View();
        }

        [HttpPost, ActionName("Top10Tracks")]
        [ValidateAntiForgeryToken]
        public ActionResult GetTracks()
        {
            ViewBag.From = Request["from"];
            ViewBag.To = Request["to"];
            DateTime? fromdate = null;
            if (!string.IsNullOrWhiteSpace(Request["from"]))
            {
                fromdate = DateTime.Parse(Request["from"]);
            }
            DateTime? todate = null;
            if (!string.IsNullOrWhiteSpace(Request["to"]))
            {
                todate = DateTime.Parse(Request["to"]);
            }

            if (fromdate == null && todate == null)
            {
                return View((from i in db.InvoiceLines group i by i.Track into g select new TopTracksModel { track = g.Key, count = g.Count() }).OrderByDescending(g => g.count).ToList().Take(10));
            }
            else if (fromdate == null)
            {
                return View((from i in db.InvoiceLines where i.Invoice.InvoiceDate <= todate group i by i.Track into g select new TopTracksModel { track = g.Key, count = g.Count() }).OrderByDescending(g => g.count).ToList().Take(10));
            }
            else if (todate == null)
            {
                return View((from i in db.InvoiceLines where i.Invoice.InvoiceDate >= fromdate group i by i.Track into g select new TopTracksModel { track = g.Key, count = g.Count() }).OrderByDescending(g => g.count).ToList().Take(10));
            }
            return View((from i in db.InvoiceLines where i.Invoice.InvoiceDate >= fromdate && i.Invoice.InvoiceDate <= todate group i by i.Track into g select new TopTracksModel { track = g.Key, count = g.Count() }).OrderByDescending(g => g.count).ToList().Take(10));
        }

        public ActionResult TopGenres()
        {
            return View((from i in db.InvoiceLines group i by i.Track.Genre into g select new TopGenresModel { genre = g.Key, count = g.Count() }).OrderByDescending(g => g.count).ToList());
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Xml.Linq;
using Svetosavlje.Models;
using Svetosavlje.Services;
using Svetosavlje.Interfaces.Interfaces;
using Svetosavlje.Interfaces.Classes;

namespace Svetosavlje.Controllers
{
    public class HomeController : BaseClasses.BaseController
    {
        private Blogs blogs = new Blogs();
        private ListArhivaData listArhiva = new ListArhivaData();
        private PitanjaPastiru pitanjaPastiru = new PitanjaPastiru();
        private SvetiDana svetiDana = new SvetiDana();
        private IzDanaUDan izDanaUDan = new IzDanaUDan();


        [OutputCache(VaryByParam = "strDate", Duration = 60)]
        public ActionResult Index(string strDate)
        {

            DateTime today = DateTime.Now.AddDays(-13);

            if (strDate != null && strDate != "")
            {
                today = Convert.ToDateTime(strDate);
            }

            Random random = new Random();
            int Month = random.Next(1, 4);
            int Day = random.Next(1, 30);


            Svetosavlje.Models.Main M = new Main(blogs.GetNews(),     //blogs.GetNews(10)
                                                  blogs.GetMissionNews(),
                                                  blogs.GetEditorNews(),
                                                  listArhiva.GetMessageThreads(10),
                                                  pitanjaPastiru.GetQuestionList(0, 10),
                                                  svetiDana.GetSaintNamesList(today.Month, today.Day),
                                                  izDanaUDan.GetQuote(1, today.Month, today.Day),
                                                  izDanaUDan.GetDnevnoCitanje(today.Month, today.Day, today.Year),
                                                  izDanaUDan.GetFastingType(today.Month, today.Day));

            return View(M);
        }


        public ActionResult DnevnoCitanje(string strDate)
        {
            DateTime dtDate = DateTime.Now.AddDays(-13);

            if (strDate != null && strDate != "")
            {
                dtDate = Convert.ToDateTime(strDate);
            }

            DnevnoCitanjeModel model = new DnevnoCitanjeModel();

            model.Citanje = izDanaUDan.GetDnevnoCitanje(dtDate.Month, dtDate.Day, dtDate.Year);

            SvastaraProvider svastara;
            model.DatumCitanja = dtDate.Day.ToString() + ". " + svastara.Mjesec(dtDate.Month) + " " + dtDate.Year.ToString();
            model.DatumKalendar = dtDate.Day + "/" + dtDate.Month + "/" + dtDate.Year;
            return View(model);
        }

        public ActionResult IzDanaUDan(string strDate)
        {
            DateTime dtDate = DateTime.Now.AddDays(-13);

            if (strDate != null && strDate != "")
            {
                dtDate = Convert.ToDateTime(strDate);
            }

            ViewBag.Citanje = izDanaUDan.GetQuote(1, dtDate.Month, dtDate.Day);

            SvastaraProvider svastara;
            ViewBag.DatumCitanja = dtDate.Day.ToString() + ". " + svastara.Mjesec(dtDate.Month) + " " + dtDate.Year.ToString();
            ViewBag.DatumKalendar = dtDate.Day + "/" + dtDate.Month + "/" + dtDate.Year;
            return View();
        }

        public ActionResult Molitve()
        {
            return View();
        }


        public ActionResult Biblioteka()
        {
            return View();
        }

    }


    public class Blogs : IBlogProvider
    {
        private IBlogProvider _provider = Factory.GetBlogProvider();


        public IList<WPBlogModel> GetNews()
        {
            return _provider.GetNews();
        }

        public IList<WPBlogModel> GetEditorNews()
        {
            return _provider.GetEditorNews();
        }

        public IList<WPBlogModel> GetMissionNews()
        {
            return _provider.GetMissionNews();
        }
    }





    public class IzDanaUDan : IQuote, IFastingType, IZachala
    {

        private IDataProvider _provider = Factory.GetDatabaseProvider();

        public string GetQuote(int Autor, int Mjesec, int Dan)
        {
            return _provider.GetQuote(Autor, Mjesec, Dan);
        }

        public string GetFastingType(int Mjesec, int Dan)
        {
            return _provider.GetFastingType(Mjesec, Dan);
        }

        public DnevnoCitanje GetDnevnoCitanje(int Mjesec, int Dan, int Godina)
        {
            return _provider.GetDnevnoCitanje(Mjesec, Dan, Godina);
        }

    }

}

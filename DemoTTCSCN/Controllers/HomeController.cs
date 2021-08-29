using DemoTTCSCN.DAO;
using DemoTTCSCN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoTTCSCN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            return View();
        }
        public ActionResult StudentDetail()
        {
            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            var student = StudentDAO.Instance.GetStudentByID(sessionLogin.IdStudent);
            //var s = session.
            //var data = StudentDAO.Instance.GetStudentByID(session)
            return View(student);
        }
        public ActionResult Transcript()
        {

            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}
using DemoTTCSCN.DAO;
using DemoTTCSCN.Models;
using DemoTTCSCN.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> StudentDetail()
        {
            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            var student = await StudentDAO.Instance.GetStudentByID(sessionLogin.IdStudent);
            if (student != null)
            {
                return View(student);
            }
            var exception = ExceptionService.Instance.getException();
            if (!String.IsNullOrEmpty(exception))
            {
                ViewBag.Error = ExceptionService.Instance.getException();
                return View("ErrorInjection");
            }
            return View("/");
        }
        public ActionResult Transcript()
        {

            UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
            if (sessionLogin == null)
            {
                return View("Error");
            }
            ViewBag.Message = "Your application description page.";
            var exception = ExceptionService.Instance.getException();
            if (!String.IsNullOrEmpty(exception))
            {
                ViewBag.Error = ExceptionService.Instance.getException();
                return View("ErrorInjection");
            }
            return View();
        }

    }
}
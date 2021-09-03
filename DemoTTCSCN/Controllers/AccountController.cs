using DemoTTCSCN.DAO;
using DemoTTCSCN.Models;
using DemoTTCSCN.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DemoTTCSCN.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var userLogin = await AccountDAO.Instance.Login(model.Username, model.Password);
                if (userLogin != null)
                {
                    var sessionLogin = new UserLogin();
                        sessionLogin.Username = userLogin.Username;
                        sessionLogin.Password = userLogin.Password;
                        sessionLogin.IdStudent = userLogin.IdStudent;
                        sessionLogin.Type = userLogin.Type;
                        Session.Add("UserLogin", sessionLogin);
                    if (sessionLogin.Type == 0)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    return RedirectToAction("Index", "Home");
                }
                var exception = ExceptionService.Instance.getException();
                if (!String.IsNullOrEmpty(exception))
                {
                    ViewBag.Error = ExceptionService.Instance.getException();
                    return View("ErrorInjection");
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserLogin"] = null;
            return Redirect("/");
        }
    }
}
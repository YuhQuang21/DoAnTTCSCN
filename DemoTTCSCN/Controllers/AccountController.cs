using DemoTTCSCN.DAO;
using DemoTTCSCN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public ActionResult Login(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var userLogin = AccountDAO.Instance.Login(model.Username, model.Password);
                if (userLogin != null)
                {
                    UserLogin sessionLogin = Session["UserLogin"] as UserLogin;
                    if (sessionLogin == null)
                    {
                        sessionLogin = new UserLogin();
                        sessionLogin.Username = userLogin.Username;
                        sessionLogin.Password = userLogin.Password;
                        sessionLogin.IdStudent = userLogin.IdStudent;
                        sessionLogin.Type = userLogin.Type;
                        Session.Add("UserLogin", sessionLogin);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Index");
        }
    }
}
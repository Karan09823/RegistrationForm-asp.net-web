using REGISTRATION.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace REGISTRATION.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /*==================================================*/

        /*DB connection  */

        DbOperation operation = new DbOperation();


        public ActionResult Register()
        {
            ViewBag.Message = "Register page";
            return View();
        }
        [HttpGet]
        public ActionResult Login(string loginemail)
        {
            var row = operation.GetUser().Where(
                model => model.registered_user_mail == loginemail).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            string email = form["login_user_email"];
            string password = form["login_user_password"];

            var user = operation.GetUser().FirstOrDefault(
                u => u.registered_user_mail == email && u.registered_user_password == password);

            if (user != null)
            {
                Session["uid"] = user.registered_user_id;
                return RedirectToAction("Welcome");
            }
            else
            {
                TempData["LoginErrorMsg"] = "Invalid Email or Password";
                return RedirectToAction("Index"); // or wherever you want to redirect in case of failure
            }

        }

        //  =======================================


        [HttpPost]
        public JsonResult RegisterUser(string user_name, string user_email, string user_password, string user_about)
        {

            bool isRegistered = operation.RegisteringUser(user_name, user_email, user_password, user_about);

            if (isRegistered)
            {
                /*TempData["RegisterUserMsg"] = "User Registered Successfully!";
                return RedirectToAction("Register");*/

                return Json(new
                {
                    success = true,
                    message = "User Registered Successfully ! "
                });


            }
            else
            {
                /* TempData["RegisterUserMsg"] = "Something went wrong!";
                 return RedirectToAction("Register");
                 
 */

                return Json(new
                {
                    success = false,
                    message = "Something Went Wrong ! "
                });

            }


        }


        public ActionResult Welcome()
        {
            int userId = (int)Session["uid"];
            var user = operation.GetUserById(userId.ToString());
            return View(user);
        }




    }
}
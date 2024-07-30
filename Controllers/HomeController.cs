using REGISTRATION.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Threading.Tasks;



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

        // Register
        [HttpPost]
        public JsonResult RegisterUser(string user_name, string user_email, string user_password, string user_about, HttpPostedFileBase user_image)
        {
            try
            {
                string ProfilePictureUrl = null;

                if (user_image != null && (user_image.ContentType == "image/jpeg" || user_image.ContentType == "image/png"))
                {
                    var fileName = Path.GetFileNameWithoutExtension(user_image.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(user_image.FileName);
                    var path = Path.Combine(Server.MapPath("~/UserImageFolder/"), fileName);

                    user_image.SaveAs(path);
                    ProfilePictureUrl = "/UserImageFolder/" + fileName;

                    
                    bool isRegistered = operation.RegisteringUser(user_name, user_email, user_password, user_about, ProfilePictureUrl);

                    return Json(new
                    {
                        success = isRegistered,
                        message = isRegistered ? "User Registered Successfully!" : "Something went wrong during registration!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid file type. Please upload a JPEG or PNG image."
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred: " + ex.Message
                });
            }
        }










        public ActionResult Welcome()
        {
            int userId = (int)Session["uid"];
            var user = operation.GetUserById(userId/*.ToString()*/);
            return View(user);
        }

        /*--userlist fetching--*/
        public ActionResult UserList()
        {
            List<User> users = new List<User>(operation.GetUser());

            return View(users);
        }


        //Edit operation
        [HttpPost]
        public JsonResult Edit(int EditUserId, string EditUserName, string EditUserMail, string EditUserPassword, string EditUserAbout, HttpPostedFileBase EditImagepath, string oldImagePath)
        {
            try
            {
                string EditProfileUrl = oldImagePath; // Default to old image path if no new image is provided

                if (EditImagepath != null && (EditImagepath.ContentType == "image/jpeg" || EditImagepath.ContentType == "image/png"))
                {
                    var EditfileName = Path.GetFileNameWithoutExtension(EditImagepath.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(EditImagepath.FileName);
                    var path = Path.Combine(Server.MapPath("~/UserImageFolder/"), EditfileName);

                    // Save the new image
                    EditImagepath.SaveAs(path);
                    EditProfileUrl = "/UserImageFolder/" + EditfileName;
                }

                // Create or update user info
                User editInfo = new User()
                {
                    registered_user_id = EditUserId,
                    registered_user_name = EditUserName,
                    registered_user_mail = EditUserMail,
                    registered_user_password = EditUserPassword,
                    registered_user_about = EditUserAbout,
                    registered_user_imagepath = EditProfileUrl
                };

                // Perform the edit operation
                bool editConfir = operation.EditOperation(editInfo);

                return Json(new
                {
                    success = editConfir,
                    message = editConfir ? "User details updated successfully!" : "Something went wrong during update!"
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                // e.g., Logger.Error(ex);

                return Json(new
                {
                    success = false,
                    message = "An error occurred: " + ex.Message
                });
            }
        }



        /*--delete fetching--*/
        [HttpPost]
        public JsonResult Delete(int delId)
        {
            bool deleInfo = operation.DeleteOperation(delId);

            if (deleInfo)
            {
                return Json(new
                {
                    success = true,
                    message = "User Data deleted Successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong !"
                });
            }


        }





    }
}
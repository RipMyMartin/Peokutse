using Peokutse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Peokutse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int hour = DateTime.Now.Hour;
            int month = DateTime.Now.Month;
            string holidayMessage = "";
            string imagePath = "~/Images/default.png"; 

            if (hour >= 5 && hour < 12)
            {
                ViewBag.Greeting = "Tere hommikust!";
            }
            else if (hour >= 12 && hour < 18)
            {
                ViewBag.Greeting = "Tere päevast!";
            }
            else if (hour >= 18 && hour < 22)
            {
                ViewBag.Greeting = "Tere õhtust!";
            }
            else
            {
                ViewBag.Greeting = "Tere ööd!";
            }

            switch (month)
            {
                case 12:
                    holidayMessage = "Häid jõule ja head uut aastat!";
                    imagePath = "~/Images/christmas-tree.png";
                    break;
                case 1:
                    holidayMessage = "Head uut aastat!";
                    imagePath = "~/Images/happy-new-year.png";
                    break;
                case 3:
                    holidayMessage = "Tere kevad! Nautige päikest!";
                    imagePath = "~/Images/flowers.png";
                    break;
                case 5:
                    holidayMessage = "Head kevade algust!";
                    imagePath = "~/Images/summer.png";
                    break;
                case 10:
                    holidayMessage = "Tere sügis! Soe ja mugav aeg!";
                    imagePath = "~/Images/autumn.png";
                    break;
                default:
                    holidayMessage = "Tere tulemast meie lehele!";
                    imagePath = "~/Images/summer.png";
                    break;
            }

            ViewBag.Message = holidayMessage;
            ViewBag.ImagePath = imagePath;

            return View();
        }
        [HttpGet]
        public ViewResult Ankeet()
        {
            return View(new Guest());
        }

        [HttpPost]
        public ViewResult Ankeet(Guest guest)
        {
            E_Mail(guest);
            if (ModelState.IsValid)
            {
                return View("Thanks", guest);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public void E_Mail(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "martinsild.mr@gmail.com";
                WebMail.Password = "xyek ctqn hptp frzx";
                WebMail.From = "martinsild.mr@gmail.com";

                string messageBody = $"{guest.Name}, ära unusta! Pidu toimub 12.03.25! Sind ootavad väga!";
                string imagePath = Server.MapPath("~/Images/happy.png");

                WebMail.Send(guest.Email, "Meeldetuletus", messageBody,
                    null, "martinsild.mr@gmail.com", filesToAttach: new string[] { imagePath });

                if (Request.Form["sendReminder"] != null)
                {
                    WebMail.Send("martinsild.mr@gmail.com", "Napimene saadetud",
                        $"Meeldetuletus saadeti kasutajale {guest.Name} ({guest.Email}).");

                    ViewBag.Message = "Meeldetuletus saadeti edukalt, ja meeldetuletus saadeti sulle!";
                }
                else
                {
                    ViewBag.Message = "Meeldetuletus saadeti edukalt, aga sa ei taotlenud meeldetuletust.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Kiri ei õnnestunud! Error: {ex.Message}";
            }
        }
    }
}
using Pumps101.Models;
using Pumps101.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace Pumps101.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public CalculationsRepository repository
        {
            get
            {
                String user = User.Identity.GetUserId();
                Guid userGuid = new Guid(user);
                return new CalculationsRepository(userGuid, User.Identity.IsAuthenticated);
            }
        }

        public ActionResult Index(int level = 0)
        {
            if (User.Identity.IsAuthenticated)
            {
                LevelModel model = repository.getLevel(level, 3);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login","Account",null);
            }
        }

        public ActionResult SubmitLevel(int levelId, string HPGuess)
        {
            double guess = Convert.ToDouble(HPGuess);
            int star;
            bool max;
            String message ="place_holder";
            star = 0;
            max = false;
            message = CheckLevel.checkLevel(levelId, guess, out star, out max);
            if (star == 0)
            {
                if (message.Equals(""))
                {
                    message = "Incorrect guess";
                }
                //else message is already set
            }
            else
            {
                message = "Stars: " + star;
            }
            return Json(new { message = message, star = star, max = max }, JsonRequestBehavior.AllowGet);
        }
    }
}
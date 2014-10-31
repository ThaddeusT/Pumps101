﻿using Pumps101.Models;
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

        [HttpPost]
        public ActionResult SubmitLevel(int levelId, int HPGuess = 0)
        {
            int star;
            bool max;
            String message ="place_holder";
            star = 0;
            max = false;
            message = CheckLevel.checkLevel(levelId, HPGuess, out star, out max);
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
            if (max)
            {
                return RedirectToAction("Index");
            }
            return View(message);
        }

        public ActionResult Login()
        {
            return View("Login");
        }
    }
}
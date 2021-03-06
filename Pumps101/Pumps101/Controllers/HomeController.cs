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
                try
                {
                    LevelModel model = repository.getLevel(level, 3);
                    return View(model);
                }
                catch
                {
                    ErrorHandler.LevelError(new Guid(User.Identity.GetUserId()), level);
                    return RedirectToAction("Index", "Home", new {level= 0});
                }
            }
            else
            {
                return RedirectToAction("Login","Account",null);
            }
        }

        public ActionResult SubmitLevel(int levelId, int level, string HPGuess, string NPSHGuess, string pumpType, string cost)
        {
            double hpguess;
            double npshguess;
            double costguess;
            int star = 0;
            bool max = false;
            String message = "place_holder";
            
            bool hpWasNum = Double.TryParse(HPGuess, out hpguess);
            bool npshWasNum = Double.TryParse(NPSHGuess, out npshguess);
            bool costWasNum = Double.TryParse(cost, out costguess);
            if (level < 7)
            {
                if (hpWasNum)
                {
                    try
                    {
                        message = CheckLevel.checkLevel(levelId, hpguess, out star, out max);
                    }
                    catch
                    {
                        ErrorHandler.LevelError(levelId);
                    }
                }
            }
            else if (level == 7)
            {
                if (hpWasNum && npshWasNum)
                {
                    try
                    {
                        message = CheckLevel.checkLevel(levelId, hpguess, npshguess,out star,out max);
                    }
                    catch
                    {
                        ErrorHandler.LevelError(levelId);
                    }
                }
            }
            else if (level == 8)
            {
                if (hpWasNum && npshWasNum && !String.IsNullOrEmpty(pumpType))
                {
                    try
                    {
                        message = CheckLevel.checkLevel(levelId, hpguess, npshguess, pumpType, out star, out max);
                    }
                    catch
                    {
                        ErrorHandler.LevelError(levelId);
                    }
                }
            }
            else if(level ==9)
            {
                if (hpWasNum && npshWasNum && !String.IsNullOrEmpty(pumpType) && costWasNum)
                {
                    try
                    {
                        message = CheckLevel.checkLevel(levelId, hpguess, npshguess, pumpType, costguess,out star, out max);
                    }
                    catch
                    {
                        ErrorHandler.LevelError(levelId);
                    }
                }
            }
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
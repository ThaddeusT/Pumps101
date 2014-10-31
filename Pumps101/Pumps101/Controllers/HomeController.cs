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

        public ActionResult Index()
        {
            LevelModel model = repository.getLevel(1, 3);
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitLevel(int level = 0, int HPGuess = 0)
        {
            LevelModel model = repository.getLevel(1, 3);
            return View("Index",model);
        }
    }
}
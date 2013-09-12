﻿using System;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Serilog;

namespace WebHarness.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            Log.Information("Hello from Index().");

            try
            {
                try
                {
                    throw new ApplicationException("Bar Happened");
                }
                catch (Exception ex1)
                {
                    throw new ApplicationException("Foo Happened", ex1);
                }
            }
            catch (Exception ex2)
            {
                Log.Error(ex2, "Error occured trying to Bar");
            }

            Log.Warning("{@Complex}",
                new
                {
                    This = "is",
                    A = "complex",
                    Type = "that",
                    Is = "getting",
                    Logged = "."
                });

            return View();
        }

        public ActionResult About()
        {
            throw new NotImplementedException("Maybe another day...");
        }

        public void SendLogData() {
      IHubContext context = GlobalHost.ConnectionManager.GetHubContext<LogHub>();
      context.Clients.All.sendLogEvent(new {
                                         message = "hello world"
                                       });
          
        }
    }
}

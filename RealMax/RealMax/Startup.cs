using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealMax.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(RealMax.Startup))]
namespace RealMax
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}

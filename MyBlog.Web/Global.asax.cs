using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MyBlog.Web.AtomPub;
using System.IO;
using System.Text;
using System.ServiceModel.Syndication;

namespace MyBlog.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly Blog Blog = new Blog();

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            AtomAdapter.itemFactory = new SyndicationItemFactory();
            AtomAdapter.service = Blog;
            var factory = new WebServiceHostFactory();
            routes.Add(new ServiceRoute("Atom", factory, typeof(AtomAdapter)));

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}
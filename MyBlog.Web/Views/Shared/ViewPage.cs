using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Web.Views.Shared
{
    public class ViewPage<T> : ViewPage
    {
        public new T Model { get { return (T)base.Model; } }
    }
}
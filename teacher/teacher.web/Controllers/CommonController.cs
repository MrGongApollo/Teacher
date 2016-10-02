using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.web.Filter;

namespace teacher.web.Controllers
{
    [LoginChecked]
    public class CommonController : BaseController
    {

        public ActionResult Share_Search()
        {
            return View();
        }

        public ActionResult Share_MsgCenter()
        {
            return View();
        }

        public ActionResult User_Setting()
        {
            return View();
        }
	}
}
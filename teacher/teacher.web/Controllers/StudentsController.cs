using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teacher.web.Controllers
{
    public class StudentsController : BaseController
    {
        #region 视图
        public ActionResult StudentsInfo()
        {
            return View();
        }
        public ActionResult StudentsScore()
        {
            return View();
        }
        public ActionResult StudentsAbsent()
        {
            return View();
        }
        #endregion
        
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.Data.Models;
using teacher.web.Filter;

namespace teacher.web.Controllers
{
    [LoginChecked]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }

        #region 获取当前用户
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCurentUser()
        {
            base.fin_r = base.error_r;
            T_User user = Session["User"] as T_User;
            user.UserPSW = "********";
            base.fin_r = base.success_r;
            return JsonR(user, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
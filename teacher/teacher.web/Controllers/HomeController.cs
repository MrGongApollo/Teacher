using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.Data.Models;
using teacher.web.Filter;
using teacher.Data;

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
            try
            {
                using (TeacherEntities et=new TeacherEntities())
                {
                   T_User _u=et.T_User.FirstOrDefault(u=>u.UserID==user.UserID);
                   Session["User"] = _u;
                    user=_u;
                }
                base.fin_r = base.success_r;
                return JsonR(new {
                    UserID = user.UserID,
                    UserNickName=user.UserNickName,
                    LastLoginTime=user.LastLoginTime
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR(JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
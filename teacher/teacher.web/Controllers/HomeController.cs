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
            #region 打招呼
            //int h = DateTime.Now.Hour;
            //string word = string.Empty;
            //if (h >= 6 && h < 8)
            //{
            //    word = "早上好,记得按时吃早餐哦！";
            //}
            //else if (h >= 8 && h < 11)
            //{
            //    word = "上午好！";
            //}
            //else if (h >= 11 && h < 13)
            //{
            //    word = "中午好,记得午饭要吃饱哟！";
            //}
            //else if (h >= 13 && h < 17)
            //{
            //    word = "下午好,工作的同时注意保护眼睛哦！";
            //}
            //else if (h >= 17 && h < 19)
            //{
            //    word = "傍晚好,晚饭记得按时吃哦！";
            //}
            //else if (h >= 19 && h < 22)
            //{
            //    word = "晚上好,晚饭记得按时吃哦！";
            //}
            //else if (h >= 22 || h < 3)
            //{
            //    word = "深夜了,记得早点休息哦！";
            //}
            //else
            //{
            //    word = "凌晨咯,千万不要熬夜哦！";
            //}
            #endregion
            using (teacher.Data.TeacherEntities et = new teacher.Data.TeacherEntities())
            {
                var list = et.T_Exam.GroupBy(k => k.Course).Select(p => new
                {
                    name = p.Key,
                    value = p.Count()
                }).ToList();

                var examList = et.T_Exam.OrderByDescending(o => o.ExamTime).Take(5)
                    .Select(p => new {
                    info=p.ExamName+"("+p.Course+")",
                    date=p.ExamTime
                    })
                    .ToList();

                ViewBag.ExamList = examList;
                ViewBag.DataList = list;
            }
            return View(ViewBag);
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
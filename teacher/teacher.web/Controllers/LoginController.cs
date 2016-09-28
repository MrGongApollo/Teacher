using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.web.Models;
using System.Security.Cryptography;
using System.Text;
using teacher.web.Help;
using System.Web.Security;
using System.Security.Principal;
using teacher.Data;
using teacher.Data.Models;

namespace teacher.web.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        #region 用户登录
        [HttpPost]
        public ActionResult LoginChecked(string username, string password)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities db = new TeacherEntities())
                {
                    T_User user = db.T_User.Where(u => u.UserID == username).FirstOrDefault();
                    #region 判断用户是否存在
                    if (user != null)
                    {
                        #region 密码正确
                        if (user.UserPSW == new CommonHelper().MD5(password))
                        {
                            user.LastLoginTime = DateTime.Now;
                            db.SaveChanges();
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(90),
               true, string.Format("{0}:{1}", username, password), FormsAuthentication.FormsCookiePath);

                            string ticString = FormsAuthentication.Encrypt(ticket);

                            //把票据信息写入Cookie和Session  
                            //SetAuthCookie方法用于标识用户的Identity状态为true  
                            HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticString));
                            FormsAuthentication.SetAuthCookie(username, false);
                            HttpContext.Session["USER_LOGON_TICKET"] = ticString;

                            //重写HttpContext中的用户身份，可以封装自定义角色数据；  
                            //判断是否合法用户，可以检查：HttpContext.User.Identity.IsAuthenticated的属性值  
                            string[] roles = ticket.UserData.Split(',');
                            IIdentity identity = new FormsIdentity(ticket);
                            IPrincipal principal = new GenericPrincipal(identity, roles);
                            HttpContext.User = principal;

                            HttpContext.Session["User"] = user;
                            base.fin_r = new KeyValueModel
                            {
                                Key = "success",
                                Value = "1秒钟后自动跳转"
                            };
                        }
                        #endregion
                        #region 不正确
                        else
                        {
                            base.fin_r.Value = "用户名或者密码不正确";
                        }
                        #endregion
                    }
                    #endregion
                    #region 用户不存在
                    else
                    {
                        base.fin_r.Value = "不存在该用户";
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR();
        }
        #endregion

        #region 用户注销
        [HttpGet]
        public ActionResult SignOut()
        {

            Session["User"] = null;
            base.fin_r = new KeyValueModel
            {
                Key = "success",
                Value = "注销成功"
            };
            return JsonR(JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
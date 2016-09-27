using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace teacher.web.Filter
{
    #region 登录验证
    /// <summary>
    /// 登录验证
    /// </summary>
    public class LoginCheckedAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                return true;
            }
            else
            {
                if (httpContext.Request.IsAjaxRequest())
                {
                    throw new HttpException(908,"身份过期");
                }
                else
                {
                    httpContext.Response.Redirect("~/Login/Index");
                }
                return false;
            }
        }


    }
    #endregion
}
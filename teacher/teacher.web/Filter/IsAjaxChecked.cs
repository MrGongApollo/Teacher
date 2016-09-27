using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace teacher.web.Filter
{
    #region 判断是否是ajax请求
    /// <summary>
    /// 判断是否是ajax请求
    /// </summary>
    public class IsAjaxCheckedAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAjaxRequest())
            {
                return false;
            }
            return true;
        }


    }
    #endregion
}
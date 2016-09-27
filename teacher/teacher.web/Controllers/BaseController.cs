using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.web.Models;

namespace teacher.web.Controllers
{
    public class BaseController : Controller
    {
        #region 操作类型枚举
        /// <summary>
        /// 操作类型枚举
        /// </summary>
        public enum OperateTypeEnum
        {
            /// <summary>
            /// 查看
            /// </summary>
            view,
            /// <summary>
            /// 登录
            /// </summary>
            login,
            /// <summary>
            /// 新增
            /// </summary>
            add,
            /// <summary>
            /// 修改
            /// </summary>
            modify,
            /// <summary>
            /// 删除
            /// </summary>
            delete
        }
        #endregion

        #region 返回Json
        protected KeyValueModel success_r = new KeyValueModel { Key = "success", Value = "操作成功！" },
                error_r = new KeyValueModel { Key = "error", Value = "操作失败！" },
                fin_r = new KeyValueModel();

        public JsonResult JsonR(object obj)
        {
            JsonRetMsg _jsonRet = new JsonRetMsg()
            {
                Data = obj,
                Ret = fin_r
            };
            return Json(_jsonRet);
        }
        public JsonResult JsonR()
        {
            JsonRetMsg _jsonRet = new JsonRetMsg()
            {
                Data = null,
                Ret = fin_r
            };
            return Json(_jsonRet);
        }
        public JsonResult JsonR(JsonRequestBehavior behavior)
        {
            JsonRetMsg _jsonRet = new JsonRetMsg()
            {
                Data =null,
                Ret = fin_r
            };
            return Json(_jsonRet, behavior);
        }
        public JsonResult JsonR(object obj, JsonRequestBehavior behavior)
        {
            JsonRetMsg _jsonRet = new JsonRetMsg() {
            Data = obj,
            Ret = fin_r
            };
            return Json(_jsonRet, behavior);
        }
        private class JsonRetMsg
        {
            public KeyValueModel Ret { get; set; }
            public object Data { get; set; }
        }
        #endregion

        #region 
        protected override void OnException(ExceptionContext filterContext)
        {
            #region 记录日志
            
            #endregion
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
        #endregion

    }
}
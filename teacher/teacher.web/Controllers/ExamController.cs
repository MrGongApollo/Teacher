using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace teacher.web.Controllers
{
    public class ExamController : BaseController
    {
        #region 视图
        /// <summary>
        /// 考试记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamList()
        {
            return View();
        }
        #endregion
        
	}
}
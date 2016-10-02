using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.web.Filter;

namespace teacher.web.Controllers
{
    [LoginChecked]
    public class LotteryController : BaseController
    {
        #region 视图
        /// <summary>
        /// 抽奖分组
        /// </summary>
        /// <returns></returns>
        public ActionResult LotteryGroup()
        {
            return View();
        }
        /// <summary>
        /// 抽奖记录
        /// </summary>
        /// <returns></returns>
        public ActionResult LotteryResult()
        {
            return View();
        }
        /// <summary>
        /// 开始抽奖
        /// </summary>
        /// <returns></returns>
        public ActionResult Lottery()
        {
            return View();
        }
        #endregion
       
	}
}
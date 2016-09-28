using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using teacher.web.Filter;
using teacher.Data;

namespace teacher.web.Controllers
{
    [LoginChecked]
    public class XTController : BaseController
    {
        #region 视图
        public ActionResult XT_SysLogs()
        {
            return View();
        }

        public ActionResult XT_Menus()
        {
            return View();
        }

        public ActionResult XT_UserManage()
        {
            return View();
        }

        public ActionResult XT_FontIcons()
        {
            return View();
        }

        public ActionResult XT_SysMenus()
        {
            return View();
        }

        #endregion

        #region 系统菜单
        #region 获取系统菜单
        [HttpGet]
        public JsonResult GetIndexMenus()
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities db = new TeacherEntities())
                {
                    var menus = db.T_SysMenus.OrderBy(o => o.SortNum);
                    var topMenus = menus.Where(k => k.MenuLevel == 1).Select(p => new TopSysMenu
                    {
                        title = p.MenuName,
                        key = p.MenuId,
                        islink = p.Islink,
                        icon = p.MenuIcon,
                        href = p.MenuUrl
                    }).ToList();

                    foreach (var mn in topMenus)
                    {
                        mn.children = menus.Where(k => k.MenuLevel == 2 && k.ParentId == mn.key).Select(
                            p => new BaseSysMenu
                            {
                                title = p.MenuName,
                                islink = p.Islink,
                                icon = p.MenuIcon,
                                href = p.MenuUrl
                            }).ToList();
                    }

                    base.fin_r = base.success_r;
                    return JsonR(topMenus, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR(new { }, JsonRequestBehavior.AllowGet);
        }

        #region IndexMenu实体
        private class BaseSysMenu
        {
            public string title { get; set; }
            public bool islink { get; set; }
            public string href { get; set; }
            public string icon { get; set; }
        }

        private class TopSysMenu : BaseSysMenu
        {
            public List<BaseSysMenu> children { get; set; }

            public string key { get; set; }
        }
        #endregion

        #endregion
        #endregion
    }
}
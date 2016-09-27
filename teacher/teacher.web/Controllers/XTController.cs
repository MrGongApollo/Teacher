using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using teacher.web.Filter;

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

        //#region 系统菜单
        //#region 获取一级菜单
        //[HttpGet]
        //public JsonResult GetSysTopMenus()
        //{
        //    try
        //    {
        //        using (WechatEntities context = new WechatEntities())
        //        {
        //            var list = context.T_SysMenus.Where(m => m.MenuLevel == 1 && m.IsDeleted == false).OrderBy(s => s.SortNum)
        //                  .Select(p => new
        //                  {
        //                      name = p.MenuName,
        //                      key = p.MenuId
        //                  }).ToList();
        //            return Json(list, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        //#region 保存菜单
        ///// <summary>
        ///// 保存菜单
        ///// </summary>
        ///// <param name="menu">菜单数据</param>
        ///// <param name="operatype">操作类型</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult SysMenusSave(T_SysMenus menu, string operatype)
        //{
        //    KeyValueModel ret = base.error_r;
        //    try
        //    {
        //        using (WechatEntities context = new WechatEntities())
        //        {
        //            switch (operatype)
        //            {
        //                #region 新增
        //                case "add":
        //                    if (context.T_SysMenus.Any(m => m.MenuId == menu.MenuId))
        //                    {
        //                        ret.Value = "已经存在菜单代码为" + menu.MenuId + "的菜单";
        //                    }
        //                    else
        //                    {
        //                        switch (menu.MenuLevel)
        //                        {
        //                            case 1:
        //                            case 2:
        //                                var cnt = context.T_SysMenus.Where(m => m.MenuLevel == menu.MenuLevel && m.IsDeleted == false);
        //                                menu.CreateTime = DateTime.Now;
        //                                context.T_SysMenus.Add(menu);
        //                                context.SaveChanges();
        //                                ret = base.success_r;
        //                                break;
        //                        }

        //                    }
        //                    break;
        //                #endregion
        //                #region 修改
        //                case "modify":
        //                    T_SysMenus _menu = context.T_SysMenus.Where(m => m.MenuId == menu.MenuId && m.IsDeleted == false).FirstOrDefault();
        //                    if (_menu != null)
        //                    {
        //                        _menu.ModifyTime = DateTime.Now;
        //                        _menu.SortNum = menu.SortNum;
        //                        _menu.MenuName = menu.MenuName;
        //                        _menu.ParentId = menu.ParentId;
        //                        _menu.ParentMenuName = menu.ParentMenuName;
        //                        _menu.Islink = menu.Islink;
        //                        _menu.MenuIcon = menu.MenuIcon;
        //                        _menu.MenuUrl = menu.MenuUrl;
        //                        context.SaveChanges();
        //                        ret = base.success_r;
        //                    }
        //                    else
        //                    {
        //                        ret.Value = "未能找到此菜单，可能已被删除！";
        //                    }

        //                    break;
        //                #endregion
        //                #region 删除
        //                case "delete":
        //                    T_SysMenus MN = context.T_SysMenus.Where(m => m.MenuId == menu.MenuId && m.IsDeleted == false).FirstOrDefault();
        //                    if (MN != null)
        //                    {
        //                        MN.IsDeleted = true;
        //                        MN.ModifyTime = DateTime.Now;
        //                        context.SaveChanges();
        //                        ret = base.success_r;
        //                    }
        //                    break;
        //                #endregion
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Value = ex.Message;
        //    }

        //    return Json(ret, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        //#region 获取菜单信息
        //[HttpGet]
        //public ActionResult GetSysMenus(int offSet = 0, int pageSize = 10, string sortType = "asc", string orderBy = "CreateTime")
        //{
        //    try
        //    {
        //        using (WechatEntities context = new WechatEntities())
        //        {
        //            var list = context.T_SysMenus.Where(m => m.IsDeleted == false);
        //            int cnt = list.Count();
        //            string orderExpression = string.Format("{0} {1}", orderBy, sortType);
        //            var _list = list.OrderBy(orderExpression).Skip(offSet).Take(pageSize).ToList();

        //            return Json(new
        //            {
        //                total = cnt,
        //                rows = _list
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //#endregion

        //#region 获取系统菜单
        //[HttpGet]
        //public JsonResult GetIndexMenus()
        //{
        //    base.fin_r = base.error_r;
        //    try
        //    {
        //        using (WechatEntities db = new WechatEntities())
        //        {
        //            var menus = db.T_SysMenus.Where(m => m.IsDeleted == false).OrderBy(o => o.SortNum);
        //            var topMenus = menus.Where(k => k.MenuLevel == 1).Select(p => new TopSysMenu
        //            {
        //                title = p.MenuName,
        //                key = p.MenuId,
        //                islink = p.Islink,
        //                icon = p.MenuIcon,
        //                href = p.MenuUrl
        //            }).ToList();

        //            foreach (var mn in topMenus)
        //            {
        //                mn.children = menus.Where(k => k.MenuLevel == 2 && k.ParentId == mn.key).Select(
        //                    p => new BaseSysMenu
        //                    {
        //                        title = p.MenuName,
        //                        islink = p.Islink,
        //                        icon = p.MenuIcon,
        //                        href = p.MenuUrl
        //                    }).ToList();
        //            }

        //            base.fin_r = base.success_r;
        //            return JsonR(topMenus, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        base.fin_r.Value = ex.Message;
        //    }
        //    return JsonR(new { }, JsonRequestBehavior.AllowGet);
        //}

        //#region IndexMenu实体
        //private class BaseSysMenu
        //{
        //    public string title { get; set; }
        //    public bool islink { get; set; }
        //    public string href { get; set; }
        //    public string icon { get; set; }
        //}

        //private class TopSysMenu : BaseSysMenu
        //{
        //    public List<BaseSysMenu> children { get; set; }

        //    public string key { get; set; }
        //}
        //#endregion

        //#endregion
        //#endregion

        //#region 字体图标
        //[HttpGet]
        //public ActionResult GetIcons()
        //{
        //    KeyValueModel ret = base.error_r;
        //    try
        //    {
        //        using (WechatEntities et = new WechatEntities())
        //        {
        //            var Icons = et.T_FontIcons.Where(i => i.IsDeleted == false).OrderBy(s => s.SortNum).Select(p => new
        //             {
        //                 FontClass = p.FontClass
        //             }).ToList();
        //            ret = base.success_r;
        //            ret.Value = JsonConvert.SerializeObject(Icons);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Value = ex.Message;
        //    }
        //    return Json(ret, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
    }
}
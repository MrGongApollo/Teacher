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
using teacher.Data.Models;
using teacher.web.Models;
using System.Linq.Dynamic;
using teacher.web.Help;

namespace teacher.web.Controllers
{
    [LoginChecked]
    public class XTController : BaseController
    {
        #region 视图
        public ActionResult XT_Setting()
        {
            return View();
        }

        #endregion

        #region 系统菜单
        //#region 获取一级菜单
        //[HttpGet]
        //public JsonResult GetSysTopMenus()
        //{
        //    try
        //    {
        //        using (TeacherEntities context = new TeacherEntities())
        //        {
        //            var list = context.T_SysMenus.Where(m => m.MenuLevel == 1).OrderBy(s => s.SortNum)
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

        #region 获取菜单信息
        [HttpGet]
        public ActionResult GetMenus(int MenuLevel, string MenuName, int offSet = 0, int pageSize = 10, string sortType = "asc", string orderBy = "MenuLevel")
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    var list = (System.Linq.IQueryable<T_SysMenus>)context.T_SysMenus;
                    if (!string.IsNullOrEmpty(MenuName))
                    {
                        list = list.Where(m => m.MenuName.Contains(MenuName));
                    }
                    switch (MenuLevel)
                    {
                        case -1:
                            break;
                        default:
                            list = list.Where(m => m.MenuLevel ==MenuLevel);
                            break;
                    }
                    int cnt = list.Count();
                    string orderExpression = string.Format("{0} {1}", orderBy, sortType);
                    var _list = list.OrderBy(orderExpression).Skip(offSet).Take(pageSize).ToList();                   
                    return Json(new
                    {
                        total = cnt,
                        rows = _list
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

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
        //        using (TeacherEntities context = new TeacherEntities())
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
        //                                var cnt = context.T_SysMenus.Where(m => m.MenuLevel == menu.MenuLevel);
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
        //                    T_SysMenus _menu = context.T_SysMenus.Where(m => m.MenuId == menu.MenuId).FirstOrDefault();
        //                    if (_menu != null)
        //                    {
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

        #region 系统设置
        #region 科目
        #region 获取科目名称
        [HttpGet]
        public JsonResult GetCourses()
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    var list = context.T_Course.OrderBy(c => c.CourseID).ToList();
                    base.fin_r = base.success_r;
                    return JsonR(list, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR(JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 保存科目
        /// <summary>
        /// 保存学生信息
        /// </summary>
        /// <param name="stu">科目</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CourseSave(T_Course course, string operatype)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    switch (operatype)
                    {
                        #region 新增
                        case "add":
                            if (context.T_Course.Any(c => c.CourseName == course.CourseName))
                            {
                                base.fin_r.Value = "已经存在\"" + course.CourseName + "\"科目";
                            }
                            else
                            {
                                context.T_Course.Add(course);
                                context.SaveChanges();
                                course.CourseID = context.T_Course.FirstOrDefault(c => c.CourseName == course.CourseName).CourseID;
                                base.fin_r = base.success_r;
                                return JsonR(course);
                            }
                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_Course Course = context.T_Course.Where(m => m.CourseID == course.CourseID).FirstOrDefault();
                            if (Course != null)
                            {
                                context.T_Course.Remove(Course);
                                context.SaveChanges();
                                base.fin_r = base.success_r;
                            }
                            else
                            {
                                base.fin_r.Value = "未能找到此科目信息，可能已被删除！";
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }

            return JsonR();
        }
        #endregion
        #endregion

        #region 班级
        #region 获取班级名称
        [HttpGet]
        public JsonResult GetClass()
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    var list = context.T_Class.OrderBy(c => c.ClassID).ToList();
                    base.fin_r = base.success_r;
                    return JsonR(list, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR(JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 保存班级
        /// <summary>
        /// 保存班级
        /// </summary>
        /// <param name="Sclass">班级</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClassSave(T_Class Sclass, string operatype)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    switch (operatype)
                    {
                        #region 新增
                        case "add":
                            if (context.T_Class.Any(c => c.ClassName == Sclass.ClassName))
                            {
                                base.fin_r.Value = "已经存在\"" + Sclass.ClassName + "\"";
                            }
                            else
                            {
                                context.T_Class.Add(Sclass);
                                context.SaveChanges();
                                Sclass.ClassID = context.T_Class.FirstOrDefault(c => c.ClassName == Sclass.ClassName).ClassID;
                                base.fin_r = base.success_r;
                                return JsonR(Sclass);
                            }
                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_Class _Class = context.T_Class.Where(m => m.ClassID == Sclass.ClassID).FirstOrDefault();
                            if (_Class != null)
                            {
                                context.T_Class.Remove(_Class);
                                context.SaveChanges();
                                base.fin_r = base.success_r;
                            }
                            else
                            {
                                base.fin_r.Value = "未能找到此科目信息，可能已被删除！";
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }

            return JsonR();
        }
        #endregion
        #endregion

        #region 格式化数据库
        [HttpGet]
        public ActionResult DbDestroy()
        {
            try
            {
                using (TeacherEntities et=new TeacherEntities())
                {
                    #region 请假记录表
                    var AbsentResultList = et.T_AbsentResult;
                    foreach (var item in AbsentResultList)
                    {
                        et.T_AbsentResult.Remove(item);
                    }
                    #endregion
                    #region 班级表
                    var ClassList = et.T_Class;
                    foreach (var item in ClassList)
                    {
                        et.T_Class.Remove(item);
                    }
                    #endregion
                    #region 科目表
                    var CourseList = et.T_Course;
                    foreach (var item in CourseList)
                    {
                        et.T_Course.Remove(item);
                    }
                    #endregion
                    #region 考试表
                    var ExamList = et.T_Exam;
                    foreach (var item in ExamList)
                    {
                        et.T_Exam.Remove(item);
                    }
                    #endregion
                    #region 抽奖分组表
                    var LotteryGroupList = et.T_LotteryGroup;
                    foreach (var item in LotteryGroupList)
                    {
                        et.T_LotteryGroup.Remove(item);
                    }
                    #endregion
                    #region 抽奖组员表
                    var LotteryGroupMemberList = et.T_LotteryGroupMember;
                    foreach (var item in LotteryGroupMemberList)
                    {
                        et.T_LotteryGroupMember.Remove(item);
                    }
                    #endregion
                    #region 抽奖记录表
                    var T_LotteryResultList = et.T_LotteryResult;
                    foreach (var item in T_LotteryResultList)
                    {
                        et.T_LotteryResult.Remove(item);
                    }
                    #endregion
                    #region 试题表
                    var QuestionList = et.T_Question;
                    foreach (var item in QuestionList)
                    {
                        et.T_Question.Remove(item);
                    }
                    #endregion
                    #region 成绩表
                    var ScoreList = et.T_Score;
                    foreach (var item in ScoreList)
                    {
                        et.T_Score.Remove(item);
                    }
                    #endregion
                    #region 学生表
                    var StudentList = et.T_Students;
                    foreach (var item in StudentList)
                    {
                        et.T_Students.Remove(item);
                    }
                    #endregion        
                    et.SaveChanges();
                    base.fin_r = base.success_r;
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR(JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion


        #region 个人设置
        /// <summary>
        /// 个人设置
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserSet(T_User user)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                   T_User u= Session["User"] as T_User;
                   T_User _u=context.T_User.FirstOrDefault(c => c.UserID == u.UserID);
                    if(!string.IsNullOrEmpty(user.UserPSW)){
                        _u.UserPSW = new CommonHelper().MD5(user.UserPSW);
                    }
                    _u.UserNickName = user.UserNickName;
                    context.SaveChanges();
                    base.fin_r = base.success_r;
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR();
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.Data;
using teacher.Data.Models;
using teacher.web.Filter;
using System.Linq.Dynamic;
using teacher.web.Models;

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
            List<KeyValueModel> list = new List<KeyValueModel>();
            using (TeacherEntities et=new TeacherEntities())
            {
               T_LotteryGroup LG= et.T_LotteryGroup.FirstOrDefault(l=>l.IsDefault);
               if (LG != null)
               {
                  list = et.T_LotteryGroupMember.Where(m => m.LotteryGroupID == LG.LotteryGroupID).OrderBy(o => o.MemberID)
                       .Select(p => new KeyValueModel
                       {
                           Key = p.StudentCode,
                           Value = p.StudentName
                       }).ToList();
                  ViewBag.Members = list;
               }
            }

            return View(ViewBag);
        }
        #endregion


        #region 抽奖分组
        #region 获取抽奖分组
        [HttpGet]
        public JsonResult GetLotteryGroup()
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    var list = context.T_LotteryGroup.OrderByDescending(l=>l.IsDefault).ThenBy(c => c.LotteryGroupID).ToList();
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

        #region 获取分组成员
        [HttpPost]
        public ActionResult GetLotteryGroupMembers(string LotteryGroupID, string sortType, string orderBy, int offSet = 0, int pageSize = 10)
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    if (string.IsNullOrEmpty(LotteryGroupID))
                    {
                        return Json(new
                    {
                        total = 0,
                        rows = new List<T_LotteryGroup>()
                    });
                    }
                   int _id= int.Parse(LotteryGroupID);
                   var list = context.T_LotteryGroupMember.Where(l => l.LotteryGroupID.Equals(_id));
                    int cnt = list.Count();
                    string orderExpression = string.Format("{0} {1}", orderBy, sortType);
                    var _list = list.OrderBy(orderExpression).Skip(offSet).Take(pageSize).ToList();
                    return Json(new
                    {
                        total = cnt,
                        rows = _list
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 保存抽奖分组
        /// <summary>
        /// 保存抽奖分组
        /// </summary>
        /// <param name="lotterygroup">抽奖分组</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LotteryGroupSave(T_LotteryGroup lotterygroup, string operatype)
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
                            if (context.T_LotteryGroup.Any(l => l.GroupName.Equals(lotterygroup.GroupName)))
                            {
                                base.fin_r.Value = "已经存在\"" + lotterygroup.GroupName + "\"";
                            }
                            else
                            {
                                context.T_LotteryGroup.Add(lotterygroup);
                                context.SaveChanges();
                                lotterygroup.LotteryGroupID = context.T_LotteryGroup.FirstOrDefault(c => c.GroupName == lotterygroup.GroupName).LotteryGroupID;
                                base.fin_r = base.success_r;
                                return JsonR(lotterygroup);
                            }
                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_LotteryGroup LG = context.T_LotteryGroup.Where(l => l.LotteryGroupID == lotterygroup.LotteryGroupID).FirstOrDefault();
                            if (LG != null)
                            {
                                context.T_LotteryGroup.Remove(LG);
                                context.SaveChanges();
                                base.fin_r = base.success_r;
                            }
                            else
                            {
                                base.fin_r.Value = "未能找到此抽奖组别信息，可能已被删除！";
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

        #region 保存抽奖分组成员
        /// <summary>
        /// 抽奖分组成员
        /// </summary>
        /// <param name="lotterygroup">抽奖分组成员</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LotteryGroupMemberSave(KeyValueModel member, string operatype)
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
                            string[] array = member.Value.Split(',');
                            int LotteryGroupID = int.Parse(array[0]);
                            string _Key=array[1];
                            switch (member.Key)
                            {
                                #region 学生
                                case "stu":
                                    T_LotteryGroupMember stu = context.T_LotteryGroupMember.FirstOrDefault(l => l.StudentCode.Equals(_Key) && l.LotteryGroupID == LotteryGroupID);
                                    if (stu != null)
                                    {
                                        base.fin_r = base.success_r;
                                        base.fin_r.Value = "已经智能忽略保存此成员信息";
                                    }
                                    else
                                    {
                                        stu = context.T_LotteryGroupMember.Create();
                                        stu.MemberID = Guid.NewGuid().ToString("N");
                                        T_Students stuinfo = context.T_Students.FirstOrDefault(u => u.StudentCode == _Key);
                                        stu.StudentCode = stuinfo.StudentCode;
                                        stu.StudentName = stuinfo.StudentName;
                                        stu.LotteryGroupID = LotteryGroupID;
                                        stu.CreateTime = DateTime.Now;
                                        context.T_LotteryGroupMember.Add(stu);
                                        context.SaveChanges();
                                        base.fin_r = base.success_r;
                                    }
                                    break;
                                #endregion
                                #region 班级
                                case "class":
                                    var stuList = context.T_Students.Where(u => u.StudentClass == _Key);
                                    foreach (T_Students _stu in stuList)
                                    {
                                        if (!context.T_LotteryGroupMember.Any(l => l.StudentCode.Equals(_stu.StudentCode) && l.LotteryGroupID == LotteryGroupID))
                                        {
                                            T_LotteryGroupMember _m = context.T_LotteryGroupMember.Create();
                                            _m.MemberID = Guid.NewGuid().ToString("N");
                                            _m.StudentCode = _stu.StudentCode;
                                            _m.StudentName = _stu.StudentName;
                                            _m.LotteryGroupID = LotteryGroupID;
                                            _m.CreateTime = DateTime.Now;
                                            context.T_LotteryGroupMember.Add(_m);
                                        }
                                    }
                                    context.SaveChanges();
                                    base.fin_r = base.success_r;
                                    break;
                                #endregion
                                #region 异常类型
                                default:
                                    base.fin_r.Value = "未知类型成员！";
                                    break;
                                #endregion
                            }
                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_LotteryGroupMember LM = context.T_LotteryGroupMember.Where(l => l.MemberID == member.Value).FirstOrDefault();
                            if (LM != null)
                            {
                                context.T_LotteryGroupMember.Remove(LM);
                                context.SaveChanges();
                                base.fin_r = base.success_r;
                            }
                            else
                            {
                                base.fin_r.Value = "未能找到此成员信息，可能已被删除！";
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

        #region 设置默认组别
        /// <summary>
        /// 设置默认组别
        /// </summary>
        /// <param name="LotteryGroupID">组别编号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetGroupDefault(int LotteryGroupID)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                   var list= context.T_LotteryGroup.Where(l => l.IsDefault);
                   foreach (var groupitem in list)
                   {
                       groupitem.IsDefault = false;
                   }
                   context.T_LotteryGroup.FirstOrDefault(l => l.LotteryGroupID == LotteryGroupID).IsDefault = true;
                   context.SaveChanges();
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

        #region 抽奖记录
        #region 获取抽奖记录
        [HttpPost]
        public ActionResult GetLotteryResults(LotteryResultFilter filter, string sortType, string orderBy, int offSet = 0, int pageSize = 10)
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    string sql = @"SELECT * FROM T_LotteryResult WHERE 1=1";
                    sql += string.IsNullOrEmpty(filter.StudentCode) ? "" : " AND StudentCode LIKE '%" + filter.StudentCode + "%'";
                    sql += string.IsNullOrEmpty(filter.StudentName) ? "" : " AND StudentName LIKE '%" + filter.StudentName + "%'";
                    sql += string.IsNullOrEmpty(filter.CreateTimeStart) ? "" : " And (CreateTime>='" + filter.CreateTimeStart + "' And CreateTime<='" + DateTime.Parse(filter.CreateTimeEnd).AddDays(1).ToString("yyyy-MM-dd") + "')";
                    var list = context.T_LotteryResult.SqlQuery(sql);
                    int cnt = list.Count();
                    string orderExpression = string.Format("{0} {1}", orderBy, sortType);
                    var _list = list.OrderBy(orderExpression).Skip(offSet).Take(pageSize).ToList();
                    return Json(new
                    {
                        total = cnt,
                        rows = _list
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public class LotteryResultFilter
        {
            public string StudentCode { get; set; }
            public string StudentName { get; set; }

            public string CreateTimeStart { get; set; }
            public string CreateTimeEnd { get; set; }
        }
        #endregion
        #region 保存抽奖记录
        [HttpGet]
        public ActionResult SaveLotteryResult(string StudentCode, string StudentName)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities et = new TeacherEntities())
                {
                    T_LotteryResult Rs = et.T_LotteryResult.Create();
                    Rs.ResultID = Guid.NewGuid().ToString("N");
                    Rs.StudentCode = StudentCode;
                    Rs.StudentName = StudentName;
                    Rs.CreateTime = DateTime.Now;
                    et.T_LotteryResult.Add(Rs);
                    et.SaveChanges();
                    base.fin_r = base.success_r;
                    return JsonR(Rs, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                base.fin_r.Value = ex.Message;
            }
            return JsonR(JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 删除抽奖记录
        [HttpGet]
        public ActionResult DeleteLotteryResult(string ResultID)
        {
            base.fin_r = base.error_r;
            try
            {
                using (TeacherEntities et = new TeacherEntities())
                {
                    T_LotteryResult Rs=et.T_LotteryResult.FirstOrDefault(c => c.ResultID == ResultID);
                    et.T_LotteryResult.Remove(Rs);
                    et.SaveChanges();
                    base.fin_r = base.success_r;
                    return JsonR(Rs, JsonRequestBehavior.AllowGet);
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
        
    }
}
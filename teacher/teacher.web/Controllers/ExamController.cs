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

        #region 考试记录

        #region 获取考试记录

        [HttpPost]
        public ActionResult GetExam(T_ExamFilter filter, string sortType, string orderBy, int offSet = 0, int pageSize = 10)
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    string sql = @"SELECT * FROM T_Exam WHERE 1=1";
                    sql += string.IsNullOrEmpty(filter.Course) ? "" : " AND Course='" + filter.Course + "'";
                    sql += string.IsNullOrEmpty(filter.ExamName) ? "" : " AND ExamName LIKE '%" + filter.ExamName + "%'";
                    sql += string.IsNullOrEmpty(filter.ExamTimeStart) ? "" : " And (ExamTime>='" + filter.ExamTimeStart + "' And ExamTime<='" + DateTime.Parse(filter.ExamTimeEnd).AddDays(1).ToString("yyyy-MM-dd") + "')";

                    var list = context.T_Exam.SqlQuery(sql);
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

        public class T_ExamFilter
        {
            /// <summary>
            /// 考试时间开始
            /// </summary>
            public string ExamTimeStart { get; set; }
            /// <summary>
            /// 考试时间结束
            /// </summary>
            public string ExamTimeEnd { get; set; }
            /// <summary>
            /// 科目
            /// </summary>
            public string Course { get; set; }
            /// <summary>
            /// 考试名称
            /// </summary>
            public string ExamName { get; set; }

        }
        #endregion

        #region 保存考试记录
        /// <summary>
        /// 保存考试记录
        /// </summary>
        /// <param name="exam">考试记录</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExamSave(T_Exam exam, string operatype)
        {
            KeyValueModel ret = base.error_r;
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    switch (operatype)
                    {
                        #region 新增
                        case "add":
                            exam.ExamID = Guid.NewGuid().ToString("N");
                            context.T_Exam.Add(exam);
                            context.SaveChanges();
                            ret = base.success_r;
                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_Exam Exam = context.T_Exam.Where(m => m.ExamID == exam.ExamID).FirstOrDefault();
                            if (Exam != null)
                            {
                                context.T_Exam.Remove(Exam);
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            else
                            {
                                ret.Value = "未能找到此试卷信息，可能已被删除！";
                            }
                            break;
                        #endregion
                        default:
                            ret.Value = "未知操作类型！";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ret.Value = ex.Message;
            }

            return Json(ret);
        }
        #endregion
        #endregion
    }
}
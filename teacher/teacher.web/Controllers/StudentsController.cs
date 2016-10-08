using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.Data;
using teacher.Data.Models;
using System.Linq.Dynamic;
using teacher.web.Models;
using teacher.web.Filter;

namespace teacher.web.Controllers
{
    [LoginChecked]
    public class StudentsController : BaseController
    {
        #region 视图
        public ActionResult StudentsInfo()
        {
            return View();
        }
        public ActionResult StudentsScore()
        {
            return View();
        }
        public ActionResult StudentsAbsent()
        {
            return View();
        }
        #endregion

        #region 学生信息
        #region 获取学生信息
        [HttpGet]
        public ActionResult GetStudents(string Sno, string Snm, string Status, int offSet = 0, int pageSize = 10, string sortType = "asc", string orderBy = "StudentCode")
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    string sql = @"SELECT * FROM T_Students
                                WHERE 1=1
                                ";
                    if (!string.IsNullOrEmpty(Snm))
                    {
                        sql += " AND StudentName LIKE '%" + Snm + "%'";
                    }
                    if (!string.IsNullOrEmpty(Sno))
                    {
                        sql += " AND StudentCode LIKE '%" + Sno + "%'";
                    }

                    switch (Status)
                    {
                        case "0":
                        case "1":
                            sql += " AND StudyStatus=" + int.Parse(Status);
                            break;
                    }
                    var list = context.T_Students.SqlQuery(sql);
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

        #region 保存学生信息
        /// <summary>
        /// 保存学生信息
        /// </summary>
        /// <param name="stu">学生信息</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StudentsSave(T_Students stu, string operatype)
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
                            if (context.T_Students.Any(m => m.StudentCode == stu.StudentCode))
                            {
                                ret.Value = "已经存在学号为" + stu.StudentCode + "的学生";
                            }
                            else
                            {
                                context.T_Students.Add(stu);
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            break;
                        #endregion
                        #region 修改
                        case "modify":
                            T_Students _stu = context.T_Students.Where(m => m.StudentCode == stu.StudentCode).FirstOrDefault();
                            if (_stu != null)
                            {
                                _stu.StudyStatus = stu.StudyStatus;
                                _stu.StudentClass = stu.StudentClass;
                                _stu.Sex = stu.Sex;
                                _stu.EnrollmentYear = stu.EnrollmentYear;
                                _stu.BirthDay = stu.BirthDay;
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            else
                            {
                                ret.Value = "未能找到此学生信息，可能已被删除！";
                            }

                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_Students STU = context.T_Students.Where(m => m.StudentCode == stu.StudentCode).FirstOrDefault();
                            if (STU != null)
                            {
                                context.T_Students.Remove(STU);
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            else
                            {
                                ret.Value = "未能找到此学生信息，可能已被删除！";
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ret.Value = ex.Message;
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 更新毕业生
        [HttpGet]
        public ActionResult UpdateGraduate()
        {
            KeyValueModel ret = base.error_r;
            try
            {
                using (TeacherEntities et=new TeacherEntities())
                {
                    int nowYear=DateTime.Today.Year;
                   var list=et.T_Students.Where(s => s.EnrollmentYear != null && nowYear - s.EnrollmentYear >= 3);
                   foreach (T_Students stu in list)
                   {
                       stu.StudyStatus = false;
                   }
                   et.SaveChanges();
                   ret = base.success_r;
                }
            }
            catch (Exception ex)
            {
                ret.Value = ex.Message;
            }
            return Json(ret,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 学生成绩

        #region 获取学生成绩
        [HttpPost]
        public ActionResult GetStudentsScore(T_ScoreFilter filter, string sortType, string orderBy, int offSet = 0, int pageSize = 10)
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    string sql = @"SELECT * FROM T_Score WHERE 1=1";
                    sql += string.IsNullOrEmpty(filter.StudentClass) ? "" : " AND StudentClass='" + filter.StudentClass + "'";
                    sql += string.IsNullOrEmpty(filter.StudentCode) ? "" : " AND StudentCode LIKE '%" + filter.StudentCode + "%'";
                    sql += string.IsNullOrEmpty(filter.StudentName) ? "" : " AND StudentName LIKE '%" + filter.StudentName + "%'";
                    sql += string.IsNullOrEmpty(filter.ExamName) ? "" : " AND ExamName LIKE '%" + filter.ExamName + "%'";
                    sql += string.IsNullOrEmpty(filter.Course) ? "" : " AND Course='" + filter.Course + "'";

                    sql += string.IsNullOrEmpty(filter.ExamTimeStart) ? "" : " And (ExamTime>='" + filter.ExamTimeStart + "' And ExamTime<='" + DateTime.Parse(filter.ExamTimeEnd).AddDays(1).ToString("yyyy-MM-dd") + "')";
                    var list = context.T_Score.SqlQuery(sql);
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

        public class T_ScoreFilter : teacher.web.Controllers.ExamController.T_ExamFilter
        {
            public string StudentCode { get; set; }
            public string StudentName { get; set; }
            public string StudentClass { get; set; }
        }
        #endregion

        #region 保存学生成绩
        /// <summary>
        /// 保存学生信息
        /// </summary>
        /// <param name="score">学生成绩</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StudentsScoreSave(T_Score score, string operatype)
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
                            score.ScoreID = Guid.NewGuid().ToString("N");
                            context.T_Score.Add(score);
                                context.SaveChanges();
                                ret = base.success_r;
                            break;
                        #endregion
                        #region 修改
                        case "modify":
                            T_Score _score = context.T_Score.Where(m => m.ScoreID == score.ScoreID).FirstOrDefault();
                            if (_score != null)
                            {
                                _score.Score = score.Score;
                                _score.StudentClass = score.StudentClass;
                                _score.StudentCode = score.StudentCode;
                                _score.StudentName = score.StudentName;
                                _score.ExamTime = score.ExamTime;
                                _score.ExamName = score.ExamName;
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            else
                            {
                                ret.Value = "未能找到此考试成绩，可能已被删除！";
                            }

                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_Score SC = context.T_Score.Where(m => m.ScoreID == score.ScoreID).FirstOrDefault();
                            if (SC != null)
                            {
                                context.T_Score.Remove(SC);
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            else
                            {
                                ret.Value = "未能找到此考试成绩，可能已被删除！";
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ret.Value = ex.Message;
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 请假记录

        #region 获取请假记录
        [HttpPost]
        public ActionResult GetStudentsAbsentResult(T_AbsentResult filter, string sortType, string orderBy, int offSet = 0, int pageSize = 10)
        {
            try
            {
                using (TeacherEntities context = new TeacherEntities())
                {
                    string sql = @"SELECT * FROM T_AbsentResult WHERE 1=1";
                    sql += string.IsNullOrEmpty(filter.StudentCode) ? "" : " AND StudentCode LIKE '%" + filter.StudentCode + "%'";
                    sql += string.IsNullOrEmpty(filter.StudentName) ? "" : " AND StudentName LIKE '%" + filter.StudentName + "%'";
                    var list = context.T_AbsentResult.SqlQuery(sql);
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

        #region 保存请假记录
        /// <summary>
        /// 保存学生信息
        /// </summary>
        /// <param name="stu">请假记录</param>
        /// <param name="operatype">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AbsentResultSave(T_AbsentResult absent, string operatype)
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
                            absent.AbsentID = Guid.NewGuid().ToString("N");
                            absent.CreateTime = DateTime.Now;
                            context.T_AbsentResult.Add(absent);
                            context.SaveChanges();
                            ret = base.success_r;
                            break;
                        #endregion
                        #region 删除
                        case "delete":
                            T_AbsentResult AbR = context.T_AbsentResult.Where(m => m.AbsentID == absent.AbsentID).FirstOrDefault();
                            if (AbR != null)
                            {
                                context.T_AbsentResult.Remove(AbR);
                                context.SaveChanges();
                                ret = base.success_r;
                            }
                            else
                            {
                                ret.Value = "未能找到此学生信息，可能已被删除！";
                            }
                            break;
                        #endregion
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
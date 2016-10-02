using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacher.Data;
using teacher.Data.Models;
using System.Linq.Dynamic;
using teacher.web.Models;

namespace teacher.web.Controllers
{
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

        #region 获取学生信息
        [HttpGet]
        public ActionResult GetStudents(string Sno, string Snm,string Status,int offSet = 0, int pageSize = 10, string sortType = "asc", string orderBy = "StudentCode")
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
	}
}
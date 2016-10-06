//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace teacher.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("T_Exam")]
    public partial class T_Exam
    {
        /// <summary>
        /// 考试编号
        /// </summary>
        [Key]
        [MaxLength(32)]
        public string ExamID { get; set; }
        /// <summary>
        /// 所属科目
        /// </summary>
        [MaxLength(100)]
        public string Course { get; set; }
        /// <summary>
        /// 考试名称
        /// </summary>
        [MaxLength(200)]
        public string ExamName { get; set; }
        /// <summary>
        /// 考试时间
        /// </summary>
        public Nullable<System.DateTime> ExamTime { get; set; }
    }
}
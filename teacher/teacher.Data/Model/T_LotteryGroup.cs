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

    [Table("T_LotteryGroup")]
    public partial class T_LotteryGroup
    {
        [Key]
        public int LotteryGroupID { get; set; }
        [MaxLength(200)]
        public string GroupName { get; set; }
        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}

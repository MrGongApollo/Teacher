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

    [Table("T_SysMenus")]
    public partial class T_SysMenus
    {
        [Key]
        [MaxLength(32)]
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string ParentId { get; set; }
        public int SortNum { get; set; }
        public Nullable<int> MenuLevel { get; set; }
        public string MenuUrl { get; set; }
        public bool Islink { get; set; }
        public string ParentMenuName { get; set; }
    }
}

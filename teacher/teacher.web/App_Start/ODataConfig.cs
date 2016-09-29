using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Builder;
using teacher.Data.Models;

namespace teacher.web
{
    public class ODataConfig
    {
        public static IEdmModel GetEdmModel()
        {

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "Odata";
            builder.ContainerName = "DefaultContainer";

            builder.EntitySet<T_SysMenus>("T_SysMenus");
            builder.EntitySet<T_SysMenus>("T_SysMenus");
            builder.EntitySet<T_LotteryResult>("T_LotteryResult");
            builder.EntitySet<T_Students>("T_Students");
            return builder.GetEdmModel();

        }
    }
}
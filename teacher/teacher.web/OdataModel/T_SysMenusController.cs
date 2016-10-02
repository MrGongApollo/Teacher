using System.Linq;
using teacher.Data.Models;
using System.Web.OData;
using System.Web.OData.Query;

namespace teacher.web.OdataModel
{
    public class T_SysMenusController : OdataBaseController
    {
        [EnableQuery]
        public IQueryable<T_SysMenus> Get()//ODataQueryOptions queryOptions
        {
            //如果odata query中有过滤
            //if (queryOptions.Filter != null)
            //{
            //    queryOptions.Filter.Validator = new RestrictiveFilterByQueryValidator();
            //}

            //过滤可以自定义，如果其它自定义呢？使用ODataValidationSettings
            //设置max top
            //ODataValidationSettings settings = new ODataValidationSettings() { MaxTop = 9 };

            ////设置orderby的属性
            //settings.AllowedOrderByProperties.Add("Id");

            //queryOptions.Validate(settings);

            //return queryOptions.ApplyTo(OrderList.AsQueryable()) as IQueryable<Order>;


            return base._db.T_SysMenus;
        }
    }
}
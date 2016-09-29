using System.Linq;
using teacher.Data.Models;
using System.Web.OData;
using System.Web.OData.Query;

namespace teacher.web.OdataModel
{
    public class T_SysMenusController : OdataBaseController
    {
        [EnableQuery]
        public IQueryable<T_SysMenus> Get()
        {
            return base._db.T_SysMenus;
        }
    }
}
using System.Linq;
using teacher.Data.Models;
using System.Web.OData;

namespace teacher.web.OdataModel
{
    public class T_LotteryResultController : OdataBaseController
    {
        [EnableQuery]
        public IQueryable<T_LotteryResult> Get()
        {
            return base._db.T_LotteryResult;
        }
    }
}
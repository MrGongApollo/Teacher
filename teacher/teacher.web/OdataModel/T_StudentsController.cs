using System.Linq;
using teacher.Data.Models;
using System.Web.OData;

namespace teacher.web.OdataModel
{
    public class T_StudentsController : OdataBaseController
    {
        [EnableQuery]
        public IQueryable<T_Students> Get()
        {
            return base._db.T_Students;
        }
    }
}
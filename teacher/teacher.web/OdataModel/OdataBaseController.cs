using System.Web.OData;
using teacher.Data;

namespace teacher.web.OdataModel
{
   // [HttpBasicAuthorize]
    public class OdataBaseController : ODataController
    {
        protected TeacherEntities _db = new TeacherEntities();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using teacher.Data.Models;
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
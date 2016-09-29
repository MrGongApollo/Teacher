using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using teacher.Data.Models;
using teacher.web.OdataModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;

namespace Web.OdataModel
{
    public class T_StudentsController : OdataBaseController
    {
        [Queryable]
        public IQueryable<T_Students> Get()
        {
            return base._db.T_Students;
        }
    }
}
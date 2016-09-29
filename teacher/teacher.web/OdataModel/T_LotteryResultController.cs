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
using System.Web.Http.OData.Query;

namespace Web.OdataModel
{
    public class T_LotteryResultController : OdataBaseController
    {
        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<T_LotteryResult> Get()
        {
            return base._db.T_LotteryResult;
        }
    }
}
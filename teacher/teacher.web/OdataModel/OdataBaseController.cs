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

        /// <summary>
        ///// 自定义过滤查询的Validator
        ///// </summary>
        //private class RestrictiveFilterByQueryValidator : FilterQueryValidator
        //{
        //    public override void ValidateSingleValuePropertyAccessNode(SingleValuePropertyAccessNode propertyAccessNode, ODataValidationSettings settings)
        //    {
        //        if (propertyAccessNode.Property.Name == "Quantity")
        //        {
        //            throw new ODataException("不允许针对Quantity属性过滤");
        //        }
        //        base.ValidateSingleValuePropertyAccessNode(propertyAccessNode, settings);
        //    }
        //}
	}
}
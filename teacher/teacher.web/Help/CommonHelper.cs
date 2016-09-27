using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace teacher.web.Help
{
    public class CommonHelper
    {
        #region Md5加密，32位,大写
        /// <summary>
        /// Md5加密，32位,大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] encs = md5.ComputeHash(data);
            return BitConverter.ToString(encs).Replace("-", "").ToUpper();
        }
        #endregion
    }
}
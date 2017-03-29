using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace HKTDC.WebAPI.Common.Services
{
    public class BaseService
    {
        private EntityContext Db = new EntityContext();

        public bool checkAdminPermission(string UserId, string ProcessName)
        {
            bool havePermission = false;
            List<UserRole> roleList = new List<UserRole>();
            SqlParameter[] sqlp = {
                     new SqlParameter ("UserId",UserId),
                     new SqlParameter("ProcessName",ProcessName.ToUpper())};

            roleList = Db.Database.SqlQuery<UserRole>("exec [K2_GetUserRole] @UserId,@ProcessName", sqlp).ToList();
            foreach (var i in roleList)
            {
                if (isAdminRole(i.RoleName))
                {
                    havePermission = true;
                    break;
                }
            }

            return havePermission;
        }

        public bool checkHavePermission(string UserId, string ProcessName, string MenuItem)
        {
            bool havePermission = false;
            string menuItem = Db.SPAMenuItem.Where(p => p.ItemName.ToUpper() == MenuItem.ToUpper()).Select(p => p.SPAMenuItemGUID).FirstOrDefault();
            if (!string.IsNullOrEmpty(menuItem))
            {
                SqlParameter[] sqlp = {
                     new SqlParameter("EmployeeId",UserId),
                     new SqlParameter("ProcessName",ProcessName.ToUpper()),
                     new SqlParameter("Page",menuItem)};
                MenuList MList = Db.Database.SqlQuery<MenuList>("exec [K2_GetMenu] @ProcessName,@EmployeeId,@Page", sqlp).FirstOrDefault();
                if(MList != null && !string.IsNullOrEmpty(MList.EMPLOYEENO))
                {
                    havePermission = true;
                }
            }

            return havePermission;
        }

        public bool isAdminRole(string role)
        {
            if (role == "Administrator" || role == "Administrators" || role == "Admins" || role == "Admin")
            {
                return true;
            }
            return false;
        }

        public int? TryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }

        /// <summary>
        /// Exception handling & Create the Error Logs to the Errorlog Table
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="UserId">UserId</param>
        /// using the multiple catch and show the custom Message related to the Exception
        /// <returns></returns>
        public ExceptionProcessing ErrorLog(Exception ex, string UserId)
        {
            ExceptionProcessing exceptionp = new ExceptionProcessing();
            Models.ErrorLog Errthrow = new Models.ErrorLog();
            if (ex is MissingMemberException || ex is NotSupportedException)
            {
                Errthrow.LogPriority = "Heigh";
                exceptionp.Code = HttpStatusCode.InternalServerError;
                exceptionp.Message = "{\"Error_Code\": 101,\"Message\": \" System Not Respond.\"}";
            }
            else if (ex is FormatException || ex is OverflowException || ex is ArgumentNullException)
            {
                Errthrow.LogPriority = "Medium";
                exceptionp.Code = HttpStatusCode.BadRequest;
                exceptionp.Message = "{\"Error_Code\": 102,\"Message\": \" Invalid Input/output Format.\"}";
            }
            else if (ex is BadImageFormatException || ex is XmlException || ex is JsonException || ex is InvalidCastException)
            {
                Errthrow.LogPriority = "Medium";
                exceptionp.Code = HttpStatusCode.NotAcceptable;
                exceptionp.Message = "{\"Error_Code\": 103,\"Message\": \" Unable to Parse. \"}";
            }
            else if (ex is IOException)
            {
                Errthrow.LogPriority = "Heigh";
                exceptionp.Code = HttpStatusCode.Forbidden;
                exceptionp.Message = "{\"Error_Code\": 104,\"Message\": \" Unable to Perform IO Operation.\"}";
            }
            else if (ex is SqlException)
            {
                Errthrow.LogPriority = "Heigh";
                exceptionp.Code = HttpStatusCode.InternalServerError;
                exceptionp.Message = "{\"Error_Code\": 105,\"Message\": \"Unable to Perform DatatBase Operation.\"}";
            }
            else if (ex is NullReferenceException)
            {
                Errthrow.LogPriority = "Heigh";
                exceptionp.Code = HttpStatusCode.InternalServerError;
                exceptionp.Message = "{\"Error_Code\": 107,\"Message\": \"Nullable Value Exception.\"}";
            }
            else if (ex is UnauthorizedAccessException)
            {
                Errthrow.LogPriority = "Heigh";
                exceptionp.Code = HttpStatusCode.Unauthorized;
                exceptionp.Message = "The User is unauthorized.";
            }
            else
            {
                Errthrow.LogPriority = "Medium";
                exceptionp.Code = HttpStatusCode.InternalServerError;
                exceptionp.Message = "{\"Error_Code\": 108,\"Message\": \"Unhandled Exception.\"}";
            }
            Errthrow.ErrorCode = ((int)exceptionp.Code).ToString();
            Errthrow.LogUserId = UserId;
            Errthrow.LogType = "Error";
            Errthrow.StackTrace = ex.StackTrace;
            Errthrow.InnerMessage = exceptionp.Message;
            Errthrow.ErrorMessage = ex.Message;
            Errthrow.ErrorSource = ex.Source;
            Errthrow.LogCreatedDateTime = DateTime.Now;
            Db.ErrorLogs.Add(Errthrow);
            Db.SaveChanges();
            return exceptionp;
        }
    }

    /// <summary>
    /// Most Linque Mathod used for distincted by mention Column.
    /// </summary>
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }

        }
    }
}
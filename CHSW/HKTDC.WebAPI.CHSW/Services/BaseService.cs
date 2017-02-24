using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class BaseService
    {
        private EntityContext Db = new EntityContext();

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

        public void GenerateLog(string msg)
        {
            Models.ErrorLog Errthrow = new Models.ErrorLog();
            Errthrow.LogType = "Debug";
            Errthrow.ErrorMessage = msg;
            Errthrow.LogCreatedDateTime = DateTime.Now;
            Db.ErrorLogs.Add(Errthrow);
            Db.SaveChanges();
        }
    }
}
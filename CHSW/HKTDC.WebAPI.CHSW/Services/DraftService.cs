using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class DraftService : CommonService
    {
        private EntityContext Db = new EntityContext();

        /// <summary>
        /// Delete the Draft & related data from the DB  & related Attached Files From the UNC Path
        /// </summary>
        /// <param name="refid">User Sent reference ID</param>
        /// <returns> it's successfull sent SUCCESS else failed</returns>
        /// SP: K2_DeleteDraft
        public string DeleteDraft(string UserId, string refid)
        {
            string result = null;
            try
            {
                RequestFormMaster form = Db.RequestFormMasters.Where(p => p.ReferenceID == refid).FirstOrDefault();
                if (form != null && form.PreparerUserID == UserId)
                {
                    SqlParameter[] sqlp = {
                     new SqlParameter ("ReferId",refid ) };
                    var attachlist = Db.Database.SqlQuery<string>("exec [K2_DeleteDraft] @ReferId", sqlp).ToList();
                    result = attachlist[0].ToString();
                    if (!string.IsNullOrEmpty(result.ToString()))
                    {
                        string Dircur = ConfigurationManager.AppSettings.Get("UNCPath") + result.ToString(); //UNC Path From Web Config
                        if (Directory.Exists(Dircur))
                            System.IO.Directory.Delete(Dircur, true);
                        result = "SUCCESS";
                    }
                    else
                    {
                        result = "Failed";
                    }
                }
                else
                {
                    result = "Unauthorized";
                }
            }
            catch (Exception ex)
            {
                result = "Failed";
                throw ex;
            }
            return result;
        }
    }
}
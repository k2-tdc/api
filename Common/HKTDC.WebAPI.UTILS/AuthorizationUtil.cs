using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Utils
{
    public static class AuthorizationUtil
    {
        public static bool CheckApiAuthorized(string ApiRoute, string HttpVerb, string identity, string Resource)
        {
            bool blReturn = false;

            try
            {
                using (EntityContext Db = new EntityContext())
                {
                    SqlParameter[] sqlp = {
                        new SqlParameter("ApiRoute", ApiRoute),
                        new SqlParameter("HttpVerb", HttpVerb),
                        new SqlParameter("Identity", identity),
                        new SqlParameter("Resource", DBNull.Value)
                    };

                    if(!string.IsNullOrEmpty(Resource))
                    {
                        sqlp[3].Value = Resource;
                    }
                    int result = Db.Database.SqlQuery<int>("exec [pApiAuthorizationCheck] @ApiRoute,@HttpVerb,@Identity,@Resource", sqlp).FirstOrDefault();
                    
                    if (result == 1)
                    {
                        blReturn = true;
                    }
                }
            } catch(Exception e)
            {
                
            }

            return blReturn;

        }
    }
}

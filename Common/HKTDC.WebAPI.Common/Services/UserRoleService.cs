using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class UserRoleService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<UserRoleDTO> GetUserRoleList(string UserId, string process)
        {
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "User Role"))
                {
                    if (!string.IsNullOrEmpty(process))
                    {
                        var list = (from a in Db.SPAUserRole
                                    join b in Db.ProcessList on a.ProcessID equals b.ProcessID into ps
                                    from b in ps.DefaultIfEmpty()
                                    where b.ProcessName == process
                                    select new UserRoleDTO
                                    {
                                        UserRoleGUID = a.SPAUserRoleGUID,
                                        Role = a.RoleName,
                                        Desc = a.Desc,
                                        Process = b.ProcessName
                                    });
                        //if (!string.IsNullOrEmpty(process))
                        //{
                        //    list = list.Where(b => b.Process == process);
                        //}
                        return list.ToList();
                    }

                    return new List<UserRoleDTO>();
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> SaveUserRole(dynamic item)
        {
            bool success = false;
            string msg = "";
            string Role, Desc, UserRoleGUID;
            int ProcessId;
            try
            {
                Role = item.Role;
                Desc = item.Desc;
                UserRoleGUID = item.UserRoleGUID;
                ProcessId = item.ProcessId;
                if(!string.IsNullOrEmpty(UserRoleGUID))
                {
                    var userRole = Db.SPAUserRole.Where(p => p.SPAUserRoleGUID == UserRoleGUID).FirstOrDefault();
                    if(userRole != null)
                    {
                        userRole.RoleName = Role;
                        userRole.Desc = Desc;
                        userRole.ProcessID = ProcessId;
                        Db.SaveChanges();
                        success = true;
                        msg = UserRoleGUID;
                    } else
                    {
                        success = false;
                        msg = "User Role not found.";
                    }
                } else
                {
                    Guid newUserRole = Guid.NewGuid();
                    SPAUserRole userRole = new SPAUserRole();
                    userRole.SPAUserRoleGUID = newUserRole.ToString();
                    userRole.RoleName = Role;
                    userRole.Desc = Desc;
                    userRole.ProcessID = ProcessId;
                    Db.SPAUserRole.Add(userRole);
                    Db.SaveChanges();
                    success = true;
                    msg = newUserRole.ToString();
                }
            } catch(Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public Tuple<bool, string> DeleteUserRole(string UserId, string RoleId)
        {
            bool success = false;
            string msg = "";
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "User Role"))
                {
                    //var userRole = Db.SPAUserRole.Where(p => p.SPAUserRoleGUID == RoleId).FirstOrDefault();
                    //if(userRole != null)
                    //{
                    //    Db.SPAUserRole.Remove(userRole);
                    //    Db.SaveChanges();
                    //    success = true;
                    //} else
                    //{
                    //    success = false;
                    //    msg = "User Role not found.";
                    //}
                    SqlParameter[] sqlp = { new SqlParameter("RoleId", RoleId) };
                    Db.Database.ExecuteSqlCommand("exec [K2_DeleteUserRole] @RoleId", sqlp);
                    success = true;
                }
                else
                {
                    success = false;
                    msg = "Permission denied.";
                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public UserRoleDetailDTO GetUserRoleDetail(string UserId, string RoleId)
        {
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "User Role"))
                {
                    UserRoleDetailDTO userRoleDetail = (from a in Db.SPAUserRole
                                                        join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                                                        from b in pa.DefaultIfEmpty()
                                                        where a.SPAUserRoleGUID == RoleId
                                                        select new UserRoleDetailDTO
                                                        {
                                                            UserRoleGUID = a.SPAUserRoleGUID,
                                                            Role = a.RoleName,
                                                            Desc = a.Desc,
                                                            ProcessId = a.ProcessID,
                                                            ProcessName = b.ProcessName
                                                        }).FirstOrDefault();
                    Db.SPAUserRole.Where(p => p.SPAUserRoleGUID == RoleId).Select(p => new UserRoleDetailDTO
                    {
                        UserRoleGUID = p.SPAUserRoleGUID,
                        Role = p.RoleName,
                        Desc = p.Desc,
                        ProcessId = p.ProcessID
                    }).FirstOrDefault();
                    if (userRoleDetail != null)
                    {
                        SqlParameter[] sqlp = { new SqlParameter("RoleId", RoleId) };
                        userRoleDetail.Member = Db.Database.SqlQuery<UserRoleMemberDTO>("exec [K2_GetUserRoleMemberGroupList] @RoleId", sqlp).ToList();
                    }
                    return userRoleDetail;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> SaveUserRoleMember(dynamic item, string UserId)
        {
            bool success = false;
            string msg = "";
            string Dept, User, Query, UserRoleGUID, ExpiryDate;
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "User Role"))
                {
                    Dept = item.Dept;
                    //User = item.User;
                    Query = item.Query;
                    UserRoleGUID = item.UserRoleGUID;
                    ExpiryDate = item.ExpiryDate;
                    if (!string.IsNullOrEmpty(UserRoleGUID))
                    {
                        if (!string.IsNullOrEmpty(Dept))
                        {
                            SqlParameter[] sqlp = {
                                new SqlParameter("UserRoleId", UserRoleGUID),
                                new SqlParameter("Code", DBNull.Value),
                                new SqlParameter("Type", DBNull.Value),
                                new SqlParameter("ExpiryDate", DBNull.Value),
                                new SqlParameter("UserId", UserId)
                            };
                            if (!string.IsNullOrEmpty(ExpiryDate))
                            {
                                sqlp[3].Value = ExpiryDate;
                            }
                            sqlp[1].Value = Dept;
                            sqlp[2].Value = "Department";
                            Db.Database.ExecuteSqlCommand("exec [K2_SaveUserRoleMember] @UserRoleId,@Code,@Type,@ExpiryDate,@UserId", sqlp);
                        }
                        if (!string.IsNullOrEmpty(Query))
                        {
                            SqlParameter[] sqlp = {
                                new SqlParameter("UserRoleId", UserRoleGUID),
                                new SqlParameter("Code", DBNull.Value),
                                new SqlParameter("Type", DBNull.Value),
                                new SqlParameter("ExpiryDate", DBNull.Value),
                                new SqlParameter("UserId", UserId)
                            };
                            if (!string.IsNullOrEmpty(ExpiryDate))
                            {
                                sqlp[3].Value = ExpiryDate;
                            }
                            sqlp[1].Value = Query;
                            sqlp[2].Value = "Query";
                            Db.Database.ExecuteSqlCommand("exec [K2_SaveUserRoleMember] @UserRoleId,@Code,@Type,@ExpiryDate,@UserId", sqlp);
                        }
                        if (!string.IsNullOrEmpty(item.User.ToString()))
                        {
                            foreach (dynamic user in item.User)
                            {
                                SqlParameter[] sqlp = {
                                    new SqlParameter("UserRoleId", UserRoleGUID),
                                    new SqlParameter("Code", DBNull.Value),
                                    new SqlParameter("Type", DBNull.Value),
                                    new SqlParameter("ExpiryDate", DBNull.Value),
                                    new SqlParameter("UserId", UserId)
                                };
                                if (!string.IsNullOrEmpty(ExpiryDate))
                                {
                                    sqlp[3].Value = ExpiryDate;
                                }
                                sqlp[1].Value = user.ToString();
                                sqlp[2].Value = "User";
                                Db.Database.ExecuteSqlCommand("exec [K2_SaveUserRoleMember] @UserRoleId,@Code,@Type,@ExpiryDate,@UserId", sqlp);
                            }
                        }
                        success = true;
                    }
                    else
                    {
                        success = false;
                        msg = "Invalid User Role.";
                    }
                } else
                {
                    success = false;
                    msg = "Permission Denied.";
                }
            } catch(Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public Tuple<bool, string> UpdateUserRoleMember(dynamic item, string UserId)
        {
            bool success = false;
            string msg = "";
            string ExpiryDate, UserRoleMemberGUID;
            try
            {
                if(checkHavePermission(UserId, "ADMIN", "User Role"))
                {
                    ExpiryDate = item.ExpiryDate;
                    UserRoleMemberGUID = item.UserRoleMemberGUID;
                    if (!string.IsNullOrEmpty(UserRoleMemberGUID)) {
                        var userRoleMemberGp = Db.SPAUserRoleMemberGroup.Where(p => p.SPAUserRoleMemberGroupGUID == UserRoleMemberGUID).FirstOrDefault();
                        if(userRoleMemberGp != null)
                        {
                            userRoleMemberGp.ExpiryDate = !string.IsNullOrEmpty(ExpiryDate)?DateTime.ParseExact(ExpiryDate, "yyyyMMdd", CultureInfo.InvariantCulture): (DateTime?)null;
                            Db.SaveChanges();

                            SqlParameter[] sqlp = {
                                    new SqlParameter("RoleGUID", userRoleMemberGp.SPAUserRoleGUID)
                                };
                            Db.Database.ExecuteSqlCommand("exec [pUserRoleMemberResolve] @RoleGUID", sqlp);
                            success = true;
                        } else
                        {
                            success = false;
                            msg = "User Role Member not found.";
                        }
                    } else
                    {
                        success = false;
                        msg = "Invalid user role member.";
                    }
                } else
                {
                    success = false;
                    msg = "Permission Denied.";
                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public Tuple<bool, string> DeleteUserRoleMember(string UserId, string UserRoleMemberGUID)
        {
            bool success = false;
            string msg = "";
            try
            {
                if(checkHavePermission(UserId, "ADMIN", "User Role"))
                {
                    SqlParameter[] sqlp = {
                                    new SqlParameter("SPAUserRoleMemberGroupGUID", UserRoleMemberGUID)
                                };
                    Db.Database.ExecuteSqlCommand("exec [K2_DeleteUserRoleMember] @SPAUserRoleMemberGroupGUID", sqlp);
                    success = true;
                } else
                {
                    success = false;
                    msg = "Permission Denied.";
                }
            } catch(Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public UserRoleMemberDetailDTO GetUserRoleMemberDetail(string UserRoleMemberGUID)
        {
            SqlParameter[] sqlp = { new SqlParameter("UserRoleMemberId", UserRoleMemberGUID) };
            return Db.Database.SqlQuery<UserRoleMemberDetailDTO>("exec [K2_GetUserRoleMemberGroup] @UserRoleMemberId", sqlp).FirstOrDefault();
        }
    }
}
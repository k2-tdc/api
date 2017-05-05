using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class UserPermissionService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<UserPermissionDTO> GetUserPermissionList(string UserId, string process)
        {
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "Role Permission"))
                {
                    List<UserPermissionDTO> list = new List<UserPermissionDTO>();
                    SqlParameter[] sqlp = {
                        new SqlParameter("Process", DBNull.Value)
                    };
                    if (!string.IsNullOrEmpty(process))
                    {
                        sqlp[0].Value = process;
                    }
                    var perList = Db.Database.SqlQuery<UserPermissionSP>("exec [K2_GetRolePermissionList] @Process", sqlp).ToList();
                    foreach (var p in perList)
                    {
                        UserPermissionDTO tmp = new UserPermissionDTO();
                        tmp.RolePermissionGUID = p.RolePermissionGUID.Split(',');
                        tmp.Permission = p.Permission;
                        tmp.Role = p.Role;
                        tmp.Process = p.Process;
                        list.Add(tmp);
                    }
                    return list;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> DeleteUserPermission(string UserId, string UserPermissionGUID)
        {
            bool success = false;
            string msg = "";
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "Role Permission"))
                {
                    var userPermission = Db.SPAUserRoleEntity.Where(p => p.SPAUserRoleEntityGUID == UserPermissionGUID).FirstOrDefault();
                    if (userPermission != null)
                    {
                        Db.SPAUserRoleEntity.Remove(userPermission);
                        Db.SaveChanges();
                        success = true;
                    }
                    else
                    {
                        success = false;
                        msg = "User Permission not found.";
                    }
                }
                else
                {
                    success = false;
                    msg = "Permission Denied.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public List<UserPermissionMenuItemDTO> GetMenuItem(string process, string UserId)
        {
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "Role Permission"))
                {
                    var list = (from a in Db.SPAMenuMaster
                                join b in Db.SPAMenuGroup on a.SPAMenuMasterGUID equals b.SPAMenuMasterGUID
                                join c in Db.SPAMenuItem on b.SPAMenuGroupGUID equals c.SPAMenuGroupGUID
                                where a.Name == process
                                select new UserPermissionMenuItemDTO
                                {
                                    MenuItemGUID = c.SPAMenuItemGUID,
                                    MenuItemName = c.ItemName
                                }).ToList();
                    return list;
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserRoleDTO> GetUserRole(int ProcessId, string UserId)
        {
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "Role Permission"))
                {
                    var list = Db.SPAUserRole.Where(p => p.ProcessID == ProcessId).Select(p => new UserRoleDTO
                    {
                        UserRoleGUID = p.SPAUserRoleGUID,
                        Role = p.RoleName
                    }).ToList();
                    return list;
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> SaveUserPermission(dynamic item, string UserId)
        {
            bool success = false;
            string msg = "";
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "Role Permission"))
                {
                    foreach(var a in item.data)
                    {
                        SqlParameter[] sqlp = {
                            new SqlParameter("SPAUserRoleGUID", a.UserRoleGUID.ToString()),
                            new SqlParameter("SPAMenuItemGUID", a.MenuItemGUID.ToString()),
                            new SqlParameter("SPAUserRoleEntityGUID", DBNull.Value),
                            new SqlParameter("Remark", DBNull.Value)
                        };
                        if(!string.IsNullOrEmpty(a.RolePermissionGUID.ToString()))
                        {
                            sqlp[2].Value = a.RolePermissionGUID.ToString();
                        }
                        Db.Database.ExecuteSqlCommand("exec [K2_SaveUserPermission] @SPAUserRoleGUID,@SPAMenuItemGUID,@SPAUserRoleEntityGUID,@Remark", sqlp);
                    }
                    success = true;
                }
                else
                {
                    success = false;
                    msg = "Permission Denied.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public UserPermissionDetailDTO GetRolePermissionDetail(string RolePermissionGUID, string UserId)
        {
            try
            {
                if (checkHavePermission(UserId, "ADMIN", "Role Permission"))
                {
                    string[] perGUID = RolePermissionGUID.Split(',');
                    List<UserPermissionDetailUserRoleDTO> userRoleList = new List<UserPermissionDetailUserRoleDTO>();

                    if (perGUID.Length > 0)
                    {
                        var userPermissionDetail = (from a in Db.SPAUserRoleEntity
                                                    join b in Db.SPAUserRole on a.SPAUserRoleGUID equals b.SPAUserRoleGUID
                                                    join c in Db.ProcessList on b.ProcessID equals c.ProcessID
                                                    where perGUID.Contains(a.SPAUserRoleEntityGUID)
                                                    select new UserPermissionDetailDTO
                                                    {
                                                        ProcessId = c.ProcessID,
                                                        ProcessName = c.ProcessName,
                                                        MenuItemGUID = a.SPAMenuItemGUID
                                                    }).FirstOrDefault();
                        userPermissionDetail.Role = (from a in Db.SPAUserRoleEntity
                                                     join b in Db.SPAUserRole on a.SPAUserRoleGUID equals b.SPAUserRoleGUID
                                                     where perGUID.Contains(a.SPAUserRoleEntityGUID)
                                                     select new UserPermissionDetailUserRoleDTO
                                                     {
                                                         RolePermissionGUID = a.SPAUserRoleEntityGUID,
                                                         UserRoleGUID = a.SPAUserRoleGUID,
                                                         Role = b.RoleName
                                                     }).ToList();
                        return userPermissionDetail;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            } catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
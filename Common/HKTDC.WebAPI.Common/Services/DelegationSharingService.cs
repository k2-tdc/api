using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class DelegationSharingService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<DelegationDTO> GetDelegationList(string cuUserId, string UserId)
        {
            try
            {
                List<DelegationDTO> list = new List<DelegationDTO>();
                SqlParameter[] sqlp = {
                    new SqlParameter("UserId", DBNull.Value),
                    new SqlParameter("cuUserId", cuUserId)
                };
                if(!string.IsNullOrEmpty(UserId))
                {
                    sqlp[0].Value = UserId;
                }
                list = Db.Database.SqlQuery<DelegationDTO>("exec [K2_DelegationGetList] @UserId,@cuUserId", sqlp).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DelegationDTO> GetSharingList(string cuUserId, string UserId)
        {
            try
            {
                List<DelegationDTO> list = new List<DelegationDTO>();
                SqlParameter[] sqlp = {
                    new SqlParameter("UserId", DBNull.Value),
                    new SqlParameter("cuUserId", cuUserId)
                };
                if (!string.IsNullOrEmpty(UserId))
                {
                    sqlp[0].Value = UserId;
                }
                list = Db.Database.SqlQuery<DelegationDTO>("exec [K2_SharingGetList] @UserId,@cuUserId", sqlp).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserDTO> GetDelegationUser(string Dept)
        {

            var list = from a in Db.vUser select a;
            if (!string.IsNullOrEmpty(Dept))
            {
                list = list.Where(a => a.DeptCode == Dept);
            }
            return list.Select(a => new UserDTO
            {
                UserID = a.UserID,
                EmployeeID = a.EmployeeID,
                FullName = a.FullName
            }).OrderBy(a => a.FullName).ToList();
        }

        public List<DelegationActionDTO> GetDelegationAction(string type)
        {
            return Db.AppSetting.Where(p => p.Category == type).Select(p => new DelegationActionDTO
            {
                Value = p.AppValue,
                Key = p.AppKey
            }).ToList();
        }

        public Tuple<bool, string> SaveDelegation(string cuUserId, dynamic item, bool isSharing = false)
        {
            bool success = false;
            string msg = "";
            try
            {
                int? DelegationID, TaskID;
                string UserID, Dept, DelegateUserID, StartDate, EndDate, Action, Remark, Permission;
                int ProcessID;
                DelegationID = item.DelegationID;
                UserID = item.UserID;
                ProcessID = item.ProcessID;
                TaskID = item.TaskID;
                Dept = item.Dept;
                DelegateUserID = item.DelegateUserID;
                StartDate = item.StartDate;
                EndDate = item.EndDate;
                if (isSharing)
                {
                    Action = "Sharing";
                }
                else
                {
                    Action = item.Action;
                }
                Remark = item.Remark;
                Permission = item.Permission;
                SqlParameter[] sqlp = {
                    new SqlParameter("DelegationID", DBNull.Value),
                    new SqlParameter("cuUserID", cuUserId),
                    new SqlParameter("UserID", UserID),
                    new SqlParameter("ProcessID", ProcessID),
                    new SqlParameter("TaskID", DBNull.Value),
                    new SqlParameter("Dept", Dept),
                    new SqlParameter("DelegateUserID", DelegateUserID),
                    new SqlParameter("StartDate", DBNull.Value),
                    new SqlParameter("EndDate", DBNull.Value),
                    new SqlParameter("Action", Action),
                    new SqlParameter("Remark", DBNull.Value),
                    new SqlParameter("Permission", DBNull.Value),
                };

                if (DelegationID.GetValueOrDefault(0) != 0)
                {
                    sqlp[0].Value = DelegationID;
                }
                if (TaskID.GetValueOrDefault(0) != 0)
                {
                    sqlp[4].Value = TaskID;
                }
                if (!string.IsNullOrEmpty(StartDate))
                {
                    sqlp[7].Value = StartDate;
                }
                if (!string.IsNullOrEmpty(EndDate))
                {
                    sqlp[8].Value = EndDate;
                }
                if (!string.IsNullOrEmpty(Remark))
                {
                    sqlp[10].Value = Remark;
                }
                if(!string.IsNullOrEmpty(Permission))
                {
                    sqlp[11].Value = Permission;
                }

                string returnMsg = Db.Database.SqlQuery<string>("exec [K2_DelegationSaveDelegation] @DelegationID,@cuUserID,@UserID,@ProcessID,@TaskID,@Dept,@DelegateUserID,@StartDate,@EndDate,@Action,@Remark,@Permission", sqlp).FirstOrDefault();

                if (returnMsg == "1")
                {
                    success = true;
                } else
                {
                    success = false;
                    msg = returnMsg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public DelegationDetailDTO GetDelegationDetails(string UserId, int DelegationID)
        {
            try
            {
                bool haveAdminPermission = checkAdminPermission(UserId, "admin");
                DelegationDetailDTO detail = new DelegationDetailDTO();
                detail = (from a in Db.DelegationList
                          join c in Db.ProcessActivityGroup on a.ActivityGroupID equals c.GroupID into pc
                          from c in pc.DefaultIfEmpty()
                          join b in Db.ProcessList on c.ProcessID equals b.ProcessID into ps
                          from b in ps.DefaultIfEmpty()
                          where a.DelegationID == DelegationID && a.FromWorkerID == (haveAdminPermission ? a.FromWorkerID : UserId)
                          select new DelegationDetailDTO
                          {
                              DelegationID = a.DelegationID,
                              UserID = a.FromWorkerID,
                              ProcessID = (a.ActivityGroupID == (int?)null ? 0 : c.ProcessID),
                              TaskID = a.ActivityGroupID,
                              Dept = a.UserDeptCode,
                              DelegateUserID = a.ToWorkerID,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              Action = a.DelegationType,
                              Remark = a.Remark,
                              ProcessName = (a.ActivityGroupID == (int?)null ? "All" : b.ProcessName)
                          }).FirstOrDefault();
                return detail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DelegationDetailDTO GetSharingDetails(string UserId, int DelegationID)
        {
            try
            {
                bool haveAdminPermission = checkAdminPermission(UserId, "admin");
                DelegationDetailDTO detail = new DelegationDetailDTO();
                detail = (from a in Db.SharingList
                          join c in Db.ProcessActivityGroup on a.ActivityGroupID equals c.GroupID into pc
                          from c in pc.DefaultIfEmpty()
                          join b in Db.ProcessList on c.ProcessID equals b.ProcessID into ps
                          from b in ps.DefaultIfEmpty()
                          where a.DelegationID == DelegationID && a.FromWorkerID == (haveAdminPermission ? a.FromWorkerID : UserId)
                          select new DelegationDetailDTO
                          {
                              DelegationID = a.DelegationID,
                              UserID = a.FromWorkerID,
                              ProcessID = (a.ActivityGroupID == (int?)null ? 0 : c.ProcessID),
                              TaskID = a.ActivityGroupID,
                              Dept = a.UserDeptCode,
                              DelegateUserID = a.ToWorkerID,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              Action = a.DelegationType,
                              Remark = a.Remark,
                              ProcessName = (a.ActivityGroupID == (int?)null ? "All" : b.ProcessName),
                              Permission = a.Permission
                          }).FirstOrDefault();
                return detail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> DeleteDelegation(string UserId, int DelegationID)
        {
            bool success = false;
            string msg = "";
            try
            {
                bool haveAdminPermission = checkAdminPermission(UserId, "admin");
                var detail = Db.DelegationList.Where(p => p.DelegationID == DelegationID).FirstOrDefault();
                if (detail != null)
                {
                    if (haveAdminPermission || detail.FromWorkerID == UserId)
                    {
                        Db.DelegationList.Remove(detail);
                        Db.SaveChanges();
                        success = true;
                    }
                    else
                    {
                        msg = "Unauthorized operation.";
                    }
                }
                else
                {
                    msg = "Delegation not found.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public Tuple<bool, string> DeleteSharing(string UserId, int DelegationID)
        {
            bool success = false;
            string msg = "";
            try
            {
                bool haveAdminPermission = checkAdminPermission(UserId, "admin");
                var detail = Db.SharingList.Where(p => p.DelegationID == DelegationID).FirstOrDefault();
                if (detail != null)
                {
                    if (haveAdminPermission || detail.FromWorkerID == UserId)
                    {
                        Db.SharingList.Remove(detail);
                        Db.SaveChanges();
                        success = true;
                    }
                    else
                    {
                        msg = "Unauthorized operation.";
                    }
                }
                else
                {
                    msg = "Delegation not found.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public List<UserDTO> GetShareUserList(string UserId, string process)
        {
            try
            {
                List<UserDTO> list = new List<UserDTO>();
                int ProcessId = Db.ProcessList.Where(p => p.ProcessName == process).Select(p => p.ProcessID).FirstOrDefault();
                if(ProcessId != 0)
                {
                    DateTime currentTime = DateTime.Now;
                    //list = Db.DelegationList.Where(p => p.ProcessID == ProcessId && p.ToWorkerID == UserId && p.DelegationType == "Sharing" && p.Enabled == "1" && p.StartDate <= currentTime && p.EndDate >= currentTime).DistinctBy(p => p.FromUser_UserID).Select(p => new UserDTO
                    //{
                    //    UserID = p.FromUser_UserID,
                    //    EmployeeID = p.FromUser_EmployeeID,
                    //    FullName = p.FromUser_FullName
                    //}).ToList();
                    list = (from a in Db.SharingList
                            join b in Db.ProcessActivityGroup on a.ActivityGroupID equals b.GroupID
                            where b.ProcessID == ProcessId && a.ToWorkerID == UserId && a.DelegationType == "Sharing" && a.Enabled == "1" && a.StartDate <= currentTime && a.EndDate >= currentTime
                            select new UserDTO
                            {
                                UserID = a.FromWorkerID,
                                EmployeeID = a.FromWorkerEmployeeID,
                                FullName = a.FromWorkerFullName
                            }).DistinctBy(a => a.UserID).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
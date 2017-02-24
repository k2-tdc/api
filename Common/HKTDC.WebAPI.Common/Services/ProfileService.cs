using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class ProfileService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<NotificationProfileDTO> GetProfileList(string UserId, string profile)
        {
            try
            {
                if(!String.IsNullOrEmpty(profile))
                {
                    bool havePermission = checkAdminPermission(UserId, "admin");
                    if(!havePermission)
                    {
                        throw new UnauthorizedAccessException();
                    }
                }

                var list = (from a in Db.NotificationProfile
                            join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                            from b in pa.DefaultIfEmpty()
                            join c in Db.ProcessActivityGroup on a.ActivityGroupID equals c.GroupID into pc
                            from c in pc.DefaultIfEmpty()
                            join d in Db.vUser on new { a.UserID, a.EmployeeID } equals new { d.UserID, d.EmployeeID } into pd
                            from d in pd.DefaultIfEmpty()
                            where a.UserID == (profile==null?UserId:profile)
                            select new NotificationProfileDTO
                            {
                                EmailNotificationProfileID = a.EmailNotificationProfileID,
                                ProcessName = b.ProcessDisplayName,
                                StepName = c.GroupDisplayName,
                                UserID = a.UserID,
                                UserFullName = d.FullName,
                                EmployeeID = a.EmployeeID,
                                WeekDay1 = a.WeekDay1,
                                WeekDay2 = a.WeekDay2,
                                WeekDay3 = a.WeekDay3,
                                WeekDay4 = a.WeekDay4,
                                WeekDay5 = a.WeekDay5,
                                WeekDay6 = a.WeekDay6,
                                WeekDay7 = a.WeekDay7,
                                TimeSlot = a.TimeSlot1?1:(
                                           a.TimeSlot2?2:(
                                           a.TimeSlot3?3:(
                                           a.TimeSlot4?4:(
                                           a.TimeSlot5?5:(
                                           a.TimeSlot6?6:(
                                           a.TimeSlot7?7:(
                                           a.TimeSlot8?8:(
                                           a.TimeSlot9?9:(
                                           a.TimeSlot10?10:(
                                           a.TimeSlot11?11:(
                                           a.TimeSlot12?12:(
                                           a.TimeSlot13?13:(
                                           a.TimeSlot14?14:(
                                           a.TimeSlot15?15:(
                                           a.TimeSlot16?16:(
                                           a.TimeSlot17?17:(
                                           a.TimeSlot18?18:(
                                           a.TimeSlot19?19:(
                                           a.TimeSlot20?20:(
                                           a.TimeSlot21?21:(
                                           a.TimeSlot22?22:(
                                           a.TimeSlot23?23:(
                                           a.TimeSlot24?24:0
                                           ))))))))))))))))))))))),
                                cc = a.cc,
                                bcc = a.bcc,
                                Remark = a.Remark
                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool,string> SaveProfile(dynamic item)
        {
            bool success = false;
            string msg = "";
            int ProcessId, StepId;
            int? ProfileId;
            string UserId, CC, BCC;
            bool WeekDay1, WeekDay2, WeekDay3, WeekDay4, WeekDay5, WeekDay6, WeekDay7;
            bool TimeSlot1 = false, TimeSlot2 = false, TimeSlot3 = false, TimeSlot4 = false, TimeSlot5 = false, TimeSlot6 = false, TimeSlot7 = false,
                TimeSlot8 = false, TimeSlot9 = false, TimeSlot10 = false, TimeSlot11 = false, TimeSlot12 = false, TimeSlot13 = false, TimeSlot14 = false,
                TimeSlot15 = false, TimeSlot16 = false, TimeSlot17 = false, TimeSlot18 = false, TimeSlot19 = false, TimeSlot20 = false, TimeSlot21 = false,
                TimeSlot22 = false, TimeSlot23 = false, TimeSlot24 = false;
            try
            {
                ProfileId = item.ProfileId;
                ProcessId = item.ProcessId;
                StepId = item.StepId;
                UserId = item.UserId;
                CC = item.CC;
                BCC = item.BCC;
                WeekDay1 = item.WeekDay1;
                WeekDay2 = item.WeekDay2;
                WeekDay3 = item.WeekDay3;
                WeekDay4 = item.WeekDay4;
                WeekDay5 = item.WeekDay5;
                WeekDay6 = item.WeekDay6;
                WeekDay7 = item.WeekDay7;

                int timeSlot = item.TimeSlot;
                switch(timeSlot)
                {
                    case 1: TimeSlot1 = true; break;
                    case 2: TimeSlot2 = true; break;
                    case 3: TimeSlot3 = true; break;
                    case 4: TimeSlot4 = true; break;
                    case 5: TimeSlot5 = true; break;
                    case 6: TimeSlot6 = true; break;
                    case 7: TimeSlot7 = true; break;
                    case 8: TimeSlot8 = true; break;
                    case 9: TimeSlot9 = true; break;
                    case 10: TimeSlot10 = true; break;
                    case 11: TimeSlot11 = true; break;
                    case 12: TimeSlot12 = true; break;
                    case 13: TimeSlot13 = true; break;
                    case 14: TimeSlot14 = true; break;
                    case 15: TimeSlot15 = true; break;
                    case 16: TimeSlot16 = true; break;
                    case 17: TimeSlot17 = true; break;
                    case 18: TimeSlot18 = true; break;
                    case 19: TimeSlot19 = true; break;
                    case 20: TimeSlot20 = true; break;
                    case 21: TimeSlot21 = true; break;
                    case 22: TimeSlot22 = true; break;
                    case 23: TimeSlot23 = true; break;
                    case 24: TimeSlot24 = true; break;
                    default: break;
                }

                if (ProfileId.GetValueOrDefault(0) != 0)
                {
                    var profile = Db.NotificationProfile.Where(p => p.EmailNotificationProfileID == ProfileId).FirstOrDefault();
                    if (profile != null)
                    {
                        string employeeID = "";
                        if (UserId == "DEFAULT")
                        {
                            employeeID = UserId;
                        }
                        else
                        {
                            var UserRecord = Db.vUser.Where(p => p.UserID == UserId).FirstOrDefault();
                            employeeID = UserRecord.EmployeeID;
                        }
                        profile.ProcessID = ProcessId;
                        profile.ActivityGroupID = StepId;
                        profile.UserID = UserId;
                        profile.EmployeeID = employeeID;
                        profile.cc = CC;
                        profile.bcc = BCC;
                        profile.WeekDay1 = WeekDay1;
                        profile.WeekDay2 = WeekDay2;
                        profile.WeekDay3 = WeekDay3;
                        profile.WeekDay4 = WeekDay4;
                        profile.WeekDay5 = WeekDay5;
                        profile.WeekDay6 = WeekDay6;
                        profile.WeekDay7 = WeekDay7;
                        profile.TimeSlot1 = TimeSlot1;
                        profile.TimeSlot2 = TimeSlot2;
                        profile.TimeSlot3 = TimeSlot3;
                        profile.TimeSlot4 = TimeSlot4;
                        profile.TimeSlot5 = TimeSlot5;
                        profile.TimeSlot6 = TimeSlot6;
                        profile.TimeSlot7 = TimeSlot7;
                        profile.TimeSlot8 = TimeSlot8;
                        profile.TimeSlot9 = TimeSlot9;
                        profile.TimeSlot10 = TimeSlot10;
                        profile.TimeSlot11 = TimeSlot11;
                        profile.TimeSlot12 = TimeSlot12;
                        profile.TimeSlot13 = TimeSlot13;
                        profile.TimeSlot14 = TimeSlot14;
                        profile.TimeSlot15 = TimeSlot15;
                        profile.TimeSlot16 = TimeSlot16;
                        profile.TimeSlot17 = TimeSlot17;
                        profile.TimeSlot18 = TimeSlot18;
                        profile.TimeSlot19 = TimeSlot19;
                        profile.TimeSlot20 = TimeSlot20;
                        profile.TimeSlot21 = TimeSlot21;
                        profile.TimeSlot22 = TimeSlot22;
                        profile.TimeSlot23 = TimeSlot23;
                        profile.TimeSlot24 = TimeSlot24;
                        Db.SaveChanges();
                        success = true;
                    }
                }
                else
                {
                    //var haveRecord = Db.NotificationProfile.Where(p => p.UserID == UserId && p.ActivityGroupID == StepId && p.ProcessID == ProcessId && (bool)p.GetType().GetProperty("TimeSlot" + timeSlot).GetValue(p) == true).FirstOrDefault();
                    string employeeID = "";
                    if (UserId == "DEFAULT")
                    {
                        employeeID = UserId;
                    }
                    else
                    {
                        var UserRecord = Db.vUser.Where(p => p.UserID == UserId).FirstOrDefault();
                        employeeID = UserRecord.EmployeeID;
                    }
                    NotificationProfile profile = new NotificationProfile();
                    profile.ProcessID = ProcessId;
                    profile.ActivityGroupID = StepId;
                    profile.UserID = UserId;
                    profile.EmployeeID = employeeID;
                    profile.cc = CC;
                    profile.bcc = BCC;
                    profile.WeekDay1 = WeekDay1;
                    profile.WeekDay2 = WeekDay2;
                    profile.WeekDay3 = WeekDay3;
                    profile.WeekDay4 = WeekDay4;
                    profile.WeekDay5 = WeekDay5;
                    profile.WeekDay6 = WeekDay6;
                    profile.WeekDay7 = WeekDay7;
                    profile.TimeSlot1 = TimeSlot1;
                    profile.TimeSlot2 = TimeSlot2;
                    profile.TimeSlot3 = TimeSlot3;
                    profile.TimeSlot4 = TimeSlot4;
                    profile.TimeSlot5 = TimeSlot5;
                    profile.TimeSlot6 = TimeSlot6;
                    profile.TimeSlot7 = TimeSlot7;
                    profile.TimeSlot8 = TimeSlot8;
                    profile.TimeSlot9 = TimeSlot9;
                    profile.TimeSlot10 = TimeSlot10;
                    profile.TimeSlot11 = TimeSlot11;
                    profile.TimeSlot12 = TimeSlot12;
                    profile.TimeSlot13 = TimeSlot13;
                    profile.TimeSlot14 = TimeSlot14;
                    profile.TimeSlot15 = TimeSlot15;
                    profile.TimeSlot16 = TimeSlot16;
                    profile.TimeSlot17 = TimeSlot17;
                    profile.TimeSlot18 = TimeSlot18;
                    profile.TimeSlot19 = TimeSlot19;
                    profile.TimeSlot20 = TimeSlot20;
                    profile.TimeSlot21 = TimeSlot21;
                    profile.TimeSlot22 = TimeSlot22;
                    profile.TimeSlot23 = TimeSlot23;
                    profile.TimeSlot24 = TimeSlot24;
                    profile.CreatedOn = DateTime.Now;
                    Db.NotificationProfile.Add(profile);
                    Db.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success,msg);
        }

        public bool DeleteProfile(int ProfileId)
        {
            bool success = false;
            try
            {
                var profile = Db.NotificationProfile.Where(p => p.EmailNotificationProfileID == ProfileId).FirstOrDefault();
                if (profile != null)
                {
                    Db.NotificationProfile.Remove(profile);
                    Db.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }

        public NotificationProfileDetailDTO GetProfileDetail(int ProfileId, string UserId)
        {
            bool haveAdminPermission = checkAdminPermission(UserId, "admin");

            var v = (from a in Db.NotificationProfile
                     join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                     from b in pa.DefaultIfEmpty()
                     where a.EmailNotificationProfileID == ProfileId && a.UserID == (haveAdminPermission? a.UserID: UserId)
                     select new NotificationProfileDetailDTO
                     {
                         EmailNotificationProfileID = a.EmailNotificationProfileID,
                         ProcessID = a.ProcessID,
                         ProcessName = b.ProcessName,
                         StepID = a.ActivityGroupID,
                         UserID = a.UserID,
                         EmployeeID = a.EmployeeID,
                         WeekDay1 = a.WeekDay1,
                         WeekDay2 = a.WeekDay2,
                         WeekDay3 = a.WeekDay3,
                         WeekDay4 = a.WeekDay4,
                         WeekDay5 = a.WeekDay5,
                         WeekDay6 = a.WeekDay6,
                         WeekDay7 = a.WeekDay7,
                         TimeSlot = a.TimeSlot1 ? 1 : (
                                    a.TimeSlot2 ? 2 : (
                                    a.TimeSlot3 ? 3 : (
                                    a.TimeSlot4 ? 4 : (
                                    a.TimeSlot5 ? 5 : (
                                    a.TimeSlot6 ? 6 : (
                                    a.TimeSlot7 ? 7 : (
                                    a.TimeSlot8 ? 8 : (
                                    a.TimeSlot9 ? 9 : (
                                    a.TimeSlot10 ? 10 : (
                                    a.TimeSlot11 ? 11 : (
                                    a.TimeSlot12 ? 12 : (
                                    a.TimeSlot13 ? 13 : (
                                    a.TimeSlot14 ? 14 : (
                                    a.TimeSlot15 ? 15 : (
                                    a.TimeSlot16 ? 16 : (
                                    a.TimeSlot17 ? 17 : (
                                    a.TimeSlot18 ? 18 : (
                                    a.TimeSlot19 ? 19 : (
                                a.TimeSlot20 ? 20 : (
                                a.TimeSlot21 ? 21 : (
                                a.TimeSlot22 ? 22 : (
                                a.TimeSlot23 ? 23 : (
                                a.TimeSlot24 ? 24 : 0
                                ))))))))))))))))))))))),
                         cc = a.cc,
                         bcc = a.bcc,
                         Remark = a.Remark
                     }).FirstOrDefault();
            return v;
        }

        public List<UserDTO> GetProfileFieldList(string currentUser)
        {
            var list = (from a in Db.vUser
                        select new UserDTO
                        {
                            UserID = a.UserID,
                            EmployeeID = a.EmployeeID,
                            FullName = a.FullName
                        }).OrderBy(p => p.FullName).ToList();
            list.Insert(0, new UserDTO {
                UserID = "DEFAULT",
                EmployeeID = "DEFAULT",
                FullName = "-- Default --"
            });
            return list;
        }
    }
}
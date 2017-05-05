using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class WorkerRuleService : BaseService
    {
        private EntityContext Db = new EntityContext();
        
        public List<ProcessListDTO> GetProcessList(string UserId, string MenuId)
        {
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                List<ProcessListDTO> list = new List<ProcessListDTO>();
                try
                {
                    SqlParameter[] sqlp = { new SqlParameter("UserId", UserId), new SqlParameter("MenuId", MenuId) };
                    list = Db.Database.SqlQuery<ProcessListDTO>("exec [K2_WorkerGetProcessList] @UserId, @MenuId", sqlp).ToList();
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public List<WorkerRuleDTO> GetWorkerRuleList(string UserId, string process)
        {
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    if (!string.IsNullOrEmpty(process))
                    {
                        var list = (from a in Db.ProcessWorkerRule
                                    join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                                    from b in pa.DefaultIfEmpty()
                                    where b.ProcessName == process
                                    select new WorkerRuleDTO
                                    {
                                        WorkerRuleId = a.WorkerRuleID,
                                        Code = a.RuleCode,
                                        Worker = a.RuleName,
                                        Summary = a.Summary,
                                        Score = a.Score,
                                        ProcessName = b.ProcessName
                                    });
                        //if (!string.IsNullOrEmpty(process))
                        //{
                        //    list = list.Where(p => p.ProcessName == process);
                        //}
                        return list.ToList();
                    } else
                    {
                        return new List<WorkerRuleDTO>();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public Tuple<bool, string> SaveWorkerRule(string UserId, dynamic item)
        {
            bool success = false;
            string msg = "";
            string Code, Worker, WorkerType, Summary, Remark;
            int Score;
            int? WorkerRuleId, ProcessId;
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    WorkerRuleId = item.WorkerRuleId;
                    ProcessId = item.ProcessId;
                    Code = item.Code;
                    Worker = item.Worker;
                    WorkerType = item.WorkerType;
                    Summary = item.Summary;
                    Remark = item.Remark;
                    Score = item.Score;

                    ProcessWorkerRule rule = null;
                    if (WorkerRuleId.GetValueOrDefault(0) == 0)
                    {
                        rule = Db.ProcessWorkerRule.Where(p => p.RuleCode == Code).FirstOrDefault();
                    }
                    if(rule == null)
                    {
                        SqlParameter[] sqlp = {
                            new SqlParameter("UserId", UserId),
                            new SqlParameter("WorkerRuleId", DBNull.Value),
                            new SqlParameter("ProcessId", DBNull.Value),
                            new SqlParameter("Code", DBNull.Value),
                            new SqlParameter("Worker", Worker),
                            new SqlParameter("WorkerType", WorkerType),
                            new SqlParameter("Summary", DBNull.Value),
                            new SqlParameter("Remark", DBNull.Value),
                            new SqlParameter("Score", Score),
                        };

                        if(WorkerRuleId.GetValueOrDefault(0) != 0)
                        {
                            sqlp[1].Value = WorkerRuleId;
                        }

                        if(ProcessId.GetValueOrDefault(0) != 0)
                        {
                            sqlp[2].Value = ProcessId;
                        }

                        if(!string.IsNullOrEmpty(Code))
                        {
                            sqlp[3].Value = Code;
                        }

                        if(!string.IsNullOrEmpty(Summary))
                        {
                            sqlp[6].Value = Summary;
                        }

                        if(!string.IsNullOrEmpty(Remark))
                        {
                            sqlp[7].Value = Remark;
                        }

                        string ruleId = Db.Database.SqlQuery<string>("exec [K2_WorkerSaveWorkerRule] @UserId,@WorkerRuleId,@ProcessId,@Code,@Worker,@WorkerType,@Summary,@Remark,@Score", sqlp).FirstOrDefault();
                        success = true;
                        msg = ruleId;
                    } else
                    {
                        success = false;
                        msg = "The Code already exists.";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Tuple.Create(success, msg);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public WorkerRuleDetailDTO GetWorkerRuleDetail(string UserId, int WorkerRuleId)
        {
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    WorkerRuleDetailDTO detail = new WorkerRuleDetailDTO();
                    detail = (from a in Db.ProcessWorkerRule
                              join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                              from b in pa.DefaultIfEmpty()
                              where a.WorkerRuleID == WorkerRuleId
                              select new WorkerRuleDetailDTO
                              {
                                  WorkerRuleId = a.WorkerRuleID,
                                  ProcessId = a.ProcessID,
                                  ProcessName = b.ProcessName,
                                  ProcessDisplayName = b.ProcessDisplayName,
                                  Code = a.RuleCode,
                                  Worker = a.RuleName,
                                  WorkerType = a.WorkerType,
                                  Summary = a.Summary,
                                  Remark = a.Remark,
                                  Score = a.Score
                              }).FirstOrDefault();
                    //detail.Rules = Db.ProcessWorkerRuleSetting.Where(p => p.WorkerRuleID == WorkerRuleId).ToList().Select(p => new WorkerRuleRuleListDTO
                    //{
                    //    WorkerRuleSettingId = p.WorkerRuleSettingID,
                    //    ModifiedOn = p.ModifiedOn.ToShortDateString(),
                    //    ModifiedBy = p.ModifiedByFullName,
                    //    Summary = p.Summary,
                    //    Score = p.Score,
                    //    StartDate = DbFunctions.TruncateTime(p.StartDate).ToString(),
                    //    EndDate = DbFunctions.TruncateTime(p.EndDate).ToString()
                    //}).OrderByDescending(p => p.ModifiedOn).ToList();

                    return detail;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public List<WorkerRuleRuleDTO> GetWorkerRuleRule(int WorkerRuleId)
        {
            //if (checkHavePermission(UserId, "ADMIN", "Worker Rule"))
            //{
                try
                {
                    List<WorkerRuleRuleDTO> list = new List<WorkerRuleRuleDTO>();
                    string workerType = Db.ProcessWorkerRule.Where(p => p.WorkerRuleID == WorkerRuleId).Select(p => p.WorkerType).FirstOrDefault();
                    SqlParameter[] sqlp = { new SqlParameter("WorkerType", workerType) };
                    list = Db.Database.SqlQuery<WorkerRuleRuleDTO>("exec [pProcessWorkerRuleTemplateGet] @WorkerType", sqlp).ToList();
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            //}
            //else
            //{
            //    throw new UnauthorizedAccessException();
            //}
        }

        public List<WorkerRuleNatureDTO> GetWorkerRuleNature()
        {
            try
            {
                List<WorkerRuleNatureDTO> list = new List<WorkerRuleNatureDTO>();
                list = Db.Database.SqlQuery<WorkerRuleNatureDTO>("exec [pProcessWorkerRuleNatureGet]").ToList();
                return list;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleGradingDTO> GetWorkerRuleGrade()
        {
            try
            {
                List<WorkerRuleGradingDTO> list = new List<WorkerRuleGradingDTO>();
                list = Db.Database.SqlQuery<WorkerRuleGradingDTO>("exec [pProcessWorkerRuleGradingGet]").ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRulePriorityDTO> GetWorkerRulePriority(int WorkerRuleId)
        { 
            try
            {
                List<WorkerRulePriorityDTO> list = new List<WorkerRulePriorityDTO>();
                string workerType = Db.ProcessWorkerRule.Where(p => p.WorkerRuleID == WorkerRuleId).Select(p => p.WorkerType).FirstOrDefault();
                SqlParameter[] sqlp = { new SqlParameter("WorkerType", workerType) };
                list = Db.Database.SqlQuery<WorkerRulePriorityDTO>("exec [pProcessWorkerRulePriorityGet] @WorkerType", sqlp).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleDepartmentDTO> GetWorkerRuleDepartment()
        {
            try
            {
                List<WorkerRuleDepartmentDTO> list = new List<WorkerRuleDepartmentDTO>();
                list = Db.Database.SqlQuery<WorkerRuleDepartmentDTO>("exec [pProcessWorkerRuleDepartmentGet]").ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleCriteriaDTO> GetWorkerRuleCriteria(int WorkerRuleId)
        {
            try
            {
                List<WorkerRuleCriteriaDTO> list = new List<WorkerRuleCriteriaDTO>();
                string workerType = Db.ProcessWorkerRule.Where(p => p.WorkerRuleID == WorkerRuleId).Select(p => p.WorkerType).FirstOrDefault();
                SqlParameter[] sqlp = { new SqlParameter("WorkerType", workerType) };
                list = Db.Database.SqlQuery<WorkerRuleCriteriaDTO>("exec [pProcessWorkerRuleOtherCriteriaGet] @WorkerType", sqlp).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleOrgChartDTO> GetWorkerRuleOrgChart()
        {
            try
            {
                List<WorkerRuleOrgChartDTO> list = new List<WorkerRuleOrgChartDTO>();
                list = Db.Database.SqlQuery<WorkerRuleOrgChartDTO>("exec [pProcessWorkerRuleOrgChartGet]").ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleUserGroupDTO> GetWorkerRuleUserGroup()
        {
            try
            {
                List<WorkerRuleUserGroupDTO> list = new List<WorkerRuleUserGroupDTO>();
                list = Db.Database.SqlQuery<WorkerRuleUserGroupDTO>("exec [pProcessWorkerRuleUserGroupGet]").ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleTeamDTO> GetWorkerRuleTeam()
        {
            try
            {
                List<WorkerRuleTeamDTO> list = new List<WorkerRuleTeamDTO>();
                list = Db.Database.SqlQuery<WorkerRuleTeamDTO>("exec [pProcessWorkerRuleTeamGet]").ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkerRuleTeamFilterDTO> GetWorkerRuleTeamFilter()
        {
            try
            {
                List<WorkerRuleTeamFilterDTO> list = new List<WorkerRuleTeamFilterDTO>();
                list = Db.Database.SqlQuery<WorkerRuleTeamFilterDTO>("exec [pProcessWorkerRuleTeamFilterGet]").ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> SaveWorkerRuleRule(string cuUserId, dynamic item)
        {
            bool success = false;
            string msg = "";
            string UserId,Department,DateFrom,DateTo,Remark, UserId1, UserId2, Team;
            int Score, Rule, Nature, Priority, WorkerRuleId;
            int? WorkerSettingId, Grade1, Grade2, Grade3, Grade4, LevelNo, GroupID, GroupID1, TeamFilter;
            if (checkHavePermission(cuUserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    UserId = item.UserId;
                    Department = item.Department;
                    DateFrom = item.DateFrom;
                    DateTo = item.DateTo;
                    Remark = item.Remark;
                    UserId1 = item.UserId1;
                    UserId2 = item.UserId2;
                    Team = item.Team;
                    Score = item.Score;
                    Rule = item.Rule;
                    Nature = item.Nature;
                    Priority = item.Priority;
                    WorkerRuleId = item.WorkerRuleId;
                    WorkerSettingId = this.TryParseNullable(item.WorkerSettingId.ToString());
                    Grade1 = this.TryParseNullable(item.Grade1.ToString());
                    Grade2 = this.TryParseNullable(item.Grade2.ToString());
                    Grade3 = this.TryParseNullable(item.Grade3.ToString());
                    Grade4 = this.TryParseNullable(item.Grade4.ToString());
                    LevelNo = this.TryParseNullable(item.LevelNo.ToString());
                    GroupID = this.TryParseNullable(item.GroupID.ToString());
                    GroupID1 = this.TryParseNullable(item.GroupID1.ToString());
                    TeamFilter = this.TryParseNullable(item.TeamFilter.ToString());
                    SqlParameter[] sqlp = {
                        new SqlParameter("CurUserId", cuUserId),
                        new SqlParameter("WorkerRuleId", WorkerRuleId),
                        new SqlParameter("WorkerRuleSettingID", DBNull.Value),
                        new SqlParameter("Rule", Rule),
                        new SqlParameter("Nature", Nature),
                        new SqlParameter("Score", Score),
                        new SqlParameter("UserId", DBNull.Value),
                        new SqlParameter("LevelNo", DBNull.Value),
                        new SqlParameter("GroupID", DBNull.Value),
                        new SqlParameter("GroupID1", DBNull.Value),
                        new SqlParameter("Team", DBNull.Value),
                        new SqlParameter("TeamFilter", DBNull.Value),
                        new SqlParameter("Grade1", DBNull.Value),
                        new SqlParameter("Grade2", DBNull.Value),
                        new SqlParameter("Priority", Priority),
                        new SqlParameter("Grade3", DBNull.Value),
                        new SqlParameter("Grade4", DBNull.Value),
                        new SqlParameter("Department", DBNull.Value),
                        new SqlParameter("DateFrom", DBNull.Value),
                        new SqlParameter("DateTo", DBNull.Value),
                        new SqlParameter("Criteria", DBNull.Value),
                        new SqlParameter("Remark", Remark),
                        new SqlParameter("UserId1", DBNull.Value),
                        new SqlParameter("UserId2", DBNull.Value)
                    };

                    if (WorkerSettingId.GetValueOrDefault(0) != 0)
                    {
                        sqlp[2].Value = WorkerSettingId;
                    }
                    if(!string.IsNullOrEmpty(UserId))
                    {
                        sqlp[6].Value = UserId;
                    }
                    if (LevelNo.GetValueOrDefault(0) != 0)
                    {
                        sqlp[7].Value = LevelNo;
                    }
                    if (GroupID.GetValueOrDefault(0) != 0)
                    {
                        sqlp[8].Value = GroupID;
                    }
                    if (GroupID1.GetValueOrDefault(0) != 0)
                    {
                        sqlp[9].Value = GroupID1;
                    }
                    if (!string.IsNullOrEmpty(Team))
                    {
                        sqlp[10].Value = Team;
                    }
                    if (TeamFilter.GetValueOrDefault(0) != 0)
                    {
                        sqlp[11].Value = TeamFilter;
                    }
                    if (Grade1.GetValueOrDefault(0) != 0)
                    {
                        sqlp[12].Value = Grade1;
                    }
                    if (Grade2.GetValueOrDefault(0) != 0)
                    {
                        sqlp[13].Value = Grade2;
                    }
                    if (Grade3.GetValueOrDefault(0) != 0)
                    {
                        sqlp[15].Value = Grade3;
                    }
                    if (Grade4.GetValueOrDefault(0) != 0)
                    {
                        sqlp[16].Value = Grade4;
                    }
                    if (!string.IsNullOrEmpty(Department))
                    {
                        sqlp[17].Value = Department;
                    }
                    if(!string.IsNullOrEmpty(DateFrom))
                    {
                        sqlp[18].Value = DateFrom;
                    }
                    if(!string.IsNullOrEmpty(DateTo))
                    {
                        sqlp[19].Value = DateTo;
                    }
                    if(!string.IsNullOrEmpty(item.Criteria.ToString()))
                    {
                        string criteria = "";
                        foreach(var i in item.Criteria)
                        {
                            if (!string.IsNullOrEmpty(criteria)) criteria += ';';
                            criteria += i;
                        }
                        sqlp[20].Value = criteria;
                    }
                    if(!string.IsNullOrEmpty(UserId1))
                    {
                        sqlp[22].Value = UserId1;
                    }
                    if(!string.IsNullOrEmpty(UserId2))
                    {
                        sqlp[23].Value = UserId2;
                    }

                    var WorkerRuleSettingID = Db.Database.SqlQuery<int>("exec [K2_WorkerSaveWorkerRule_Rule] @CurUserId,@WorkerRuleId,@WorkerRuleSettingID,@Rule,@Nature,@Score,@UserId,@LevelNo,@GroupID,@GroupID1,@Team,@TeamFilter,@Grade1,@Grade2,@Priority,@Grade3,@Grade4,@Department,@DateFrom,@DateTo,@Criteria,@Remark,@UserId1,@UserId2", sqlp).FirstOrDefault();
                    success = true;
                    msg = WorkerRuleSettingID.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Tuple.Create(success, msg);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public Tuple<bool, string> DeleteWorkerRule(string UserId, int WorkerRuleId)
        {
            bool success = false;
            string msg = "";
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    SqlParameter[] sqlp = { new SqlParameter("WorkerRuleId", WorkerRuleId) };
                    Db.Database.ExecuteSqlCommand("exec [K2_WorkerDeleteWorkerRule] @WorkerRuleId", sqlp);
                    success = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Tuple.Create(success, msg);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public Tuple<bool, string> DeleteWorkerRuleRule(string UserId, int WorkerRuleSettingId)
        {
            bool success = false;
            string msg = "";
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    //var setting = Db.ProcessWorkerRuleSetting.Where(p => p.WorkerRuleSettingID == WorkerRuleSettingId).FirstOrDefault();
                    //if (setting != null)
                    //{
                    //    Db.ProcessWorkerRuleSetting.Remove(setting);
                    //    Db.SaveChanges();
                    //    success = true;
                    //} else
                    //{
                    //    msg = "Invalid Rule Record.";
                    //}
                    SqlParameter[] sqlp = { new SqlParameter("SettingID", WorkerRuleSettingId) };
                    Db.Database.ExecuteSqlCommand("exec [K2_WorkerDeleteWorkerRuleRule] @SettingID", sqlp);
                    success = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Tuple.Create(success, msg);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public WorkerRuleRuleDetailDTO GetWorkerRuleRuleDetail(string UserId, int WorkerRuleSettingID)
        {
            if (checkHavePermission(UserId, "ADMIN", "Process Worker"))
            {
                try
                {
                    WorkerRuleRuleDetailDTO detail = new WorkerRuleRuleDetailDTO();
                    SqlParameter[] sqlp = { new SqlParameter("WorkerRuleSettingId", WorkerRuleSettingID) };
                    detail = Db.Database.SqlQuery<WorkerRuleRuleDetailDTO>("exec [K2_WorkerGetWorkerRule_RuleDetail] @WorkerRuleSettingId", sqlp).FirstOrDefault();
                    if(detail != null)
                    {
                        if (!string.IsNullOrEmpty(detail.OtherCriteria))
                        {
                            detail.Criteria = detail.OtherCriteria.Split(',').Select(p => int.Parse(p)).ToArray();
                        }
                        detail.Reference = Db.ProcessRequestFormAttachment.Where(p => p.FormID == WorkerRuleSettingID).Select(p => new FormAttachment
                        {
                            AttachmentGUID = p.AttachmentGUID,
                            FileName = p.FileName,
                            FormID = p.FormID,
                            UploadedByDeptName = p.UploadedByDeptName,
                            UploadedByEmployeeID = p.UploadedByEmployeeID,
                            UploadedByFullName = p.UploadedByFullName,
                            UploadedByUserID = p.UploadedByUserID,
                            UploadedDate = p.UploadedDate
                        }).ToList();
                    }

                    return detail;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public List<WorkerRuleRuleListDTO> GetWorkerRuleRuleList(string curUser, int WorkerRuleId, string UserId, string WorkerId)
        {
            if (checkHavePermission(curUser, "ADMIN", "Process Worker"))
            {
                try
                {
                    List<WorkerRuleRuleListDTO> list = new List<WorkerRuleRuleListDTO>();
                    SqlParameter[] sqlp = {
                        new SqlParameter("WorkerRuleId", WorkerRuleId),
                        new SqlParameter("UserId", DBNull.Value),
                        new SqlParameter("WorkerId", DBNull.Value)
                    };
                    if(!string.IsNullOrEmpty(UserId))
                    {
                        sqlp[1].Value = UserId;
                    }
                    if(!string.IsNullOrEmpty(WorkerId))
                    {
                        sqlp[2].Value = WorkerId;
                    }
                    list = Db.Database.SqlQuery<WorkerRuleRuleListDTO>("exec [K2_WorkerGetWorkerRule_RuleList] @WorkerRuleId,@UserId,@WorkerId", sqlp).ToList();
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
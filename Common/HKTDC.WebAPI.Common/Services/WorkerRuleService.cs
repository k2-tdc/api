using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class WorkerRuleService : BaseService
    {
        private EntityContext Db = new EntityContext();
        
        public List<ProcessListDTO> GetProcessList(string UserId)
        {
            if (checkHavePermission(UserId, "ADMIN", "Worker Rule"))
            {
                List<ProcessListDTO> list = new List<ProcessListDTO>();
                try
                {
                    SqlParameter[] sqlp = { new SqlParameter("UserId", UserId) };
                    list = Db.Database.SqlQuery<ProcessListDTO>("exec [K2_WorkerGetProcessList] @UserId", sqlp).ToList();
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
            if (checkHavePermission(UserId, "ADMIN", "Worker Rule"))
            {
                try
                {
                    var list = (from a in Db.ProcessWorkerRule
                                join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                                from b in pa.DefaultIfEmpty()
                                select new WorkerRuleDTO
                                {
                                    WorkerRuleId = a.WorkerRuleID,
                                    Code = a.RuleCode,
                                    Worker = a.RuleName,
                                    Summary = a.Summary,
                                    Score = a.Score,
                                    ProcessName = b.ProcessName
                                });
                    if(!string.IsNullOrEmpty(process))
                    {
                        list = list.Where(p => p.ProcessName == process);
                    }
                    return list.ToList();
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
            if (checkHavePermission(UserId, "ADMIN", "Worker Rule"))
            {
                try
                {
                    WorkerRuleId = item.WorkRuleId;
                    ProcessId = item.ProcessId;
                    Code = item.Code;
                    Worker = item.Worker;
                    WorkerType = item.WorkerType;
                    Summary = item.Summary;
                    Remark = item.Remark;
                    Score = item.Score;

                    var rule = Db.ProcessWorkerRule.Where(p => p.RuleCode == Code).FirstOrDefault();
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

                        Db.Database.ExecuteSqlCommand("exec [K2_WorkerSaveWorkerRule] @UserId,@WorkerRuleId,@ProcessId,@Code,@Worker,@WorkerType,@Summary,@Remark,@Score", sqlp);
                        success = true;
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
            if (checkHavePermission(UserId, "ADMIN", "Worker Rule"))
            {
                try
                {
                    WorkerRuleDetailDTO detail = new WorkerRuleDetailDTO();
                    detail = (from a in Db.ProcessWorkerRule
                              join b in Db.ProcessList on a.ProcessID equals b.ProcessID into pa
                              from b in pa.DefaultIfEmpty()
                              select new WorkerRuleDetailDTO
                              {
                                  WorkerRuleId = a.WorkerRuleID,
                                  ProcessName = b.ProcessName,
                                  ProcessDisplayName = b.ProcessDisplayName,
                                  Code = a.RuleCode,
                                  Worker = a.RuleName,
                                  WorkerType = a.WorkerType,
                                  Summary = a.Summary,
                                  Remark = a.Remark,
                                  Score = a.Score
                              }).FirstOrDefault();
                    detail.Rules = Db.ProcessWorkerRuleSetting.Where(p => p.WorkerRuleID == WorkerRuleId).Select(p => new WorkerRuleRuleListDTO
                    {
                        WorkerRuleSettingId = p.WorkerRuleSettingID,
                        ModifiedOn = p.ModifiedOn,
                        ModifiedBy = p.ModifiedByFullName,
                        Summary = p.Summary,
                        Score = p.Score,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate
                    }).ToList();

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

        public List<WorkerRuleRuleDTO> GetWorkerRuleRule(string Code)
        {
            //if (checkHavePermission(UserId, "ADMIN", "Worker Rule"))
            //{
                try
                {
                    List<WorkerRuleRuleDTO> list = new List<WorkerRuleRuleDTO>();
                    string workerType = Db.ProcessWorkerRule.Where(p => p.RuleCode == Code).Select(p => p.WorkerType).FirstOrDefault();
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

        public List<WorkerRulePriorityDTO> GetWorkerRulePriority(string Code)
        { 
            try
            {
                List<WorkerRulePriorityDTO> list = new List<WorkerRulePriorityDTO>();
                string workerType = Db.ProcessWorkerRule.Where(p => p.RuleCode == Code).Select(p => p.WorkerType).FirstOrDefault();
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

        public List<WorkerRuleCriteriaDTO> GetWorkerRuleCriteria(string Code)
        {
            try
            {
                List<WorkerRuleCriteriaDTO> list = new List<WorkerRuleCriteriaDTO>();
                string workerType = Db.ProcessWorkerRule.Where(p => p.RuleCode == Code).Select(p => p.WorkerType).FirstOrDefault();
                SqlParameter[] sqlp = { new SqlParameter("WorkerType", workerType) };
                list = Db.Database.SqlQuery<WorkerRuleCriteriaDTO>("exec [pProcessWorkerRuleOtherCriteriaGet] @WorkerType", sqlp).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
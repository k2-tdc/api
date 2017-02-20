using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Services
{
    public class EmailService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<EmailTemplateDTO> GetEmailTemplateList(string process, int StepId)
        {
            try
            {
                var list = (from a in Db.EmailTemplate
                            join b in Db.ProcessList on a.ProcessID equals b.ProcessID into ps
                            from b in ps.DefaultIfEmpty()
                            join c in Db.ProcessActivityGroup on a.ActivityGroupID equals c.GroupID into pc
                            from c in pc.DefaultIfEmpty()
                            select new EmailTemplateDTO
                            {
                                EmailTemplateID = a.EmailTemplateID,
                                ProcessId = a.ProcessID,
                                ActivityGroupId = a.ActivityGroupID,
                                ProcessName = b.ProcessDisplayName,
                                pName = b.ProcessName,
                                StepName = c.GroupDisplayName,
                                Subject = a.Subject,
                                Body = a.Body,
                                Enabled = a.Enabled
                            });
                if(!String.IsNullOrEmpty(process))
                {
                    list = list.Where(b => b.pName == process); 
                }
                if(StepId != 0)
                {
                    list = list.Where(a => a.ActivityGroupId == StepId);
                }
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<bool, string> SaveEmailTemplate(dynamic item)
        {
            bool success = false;
            string msg = "";
            int ProcessId, StepId;
            int? TemplateId;
            string Subject, Body, UserId;
            bool Enabled;
            try
            {
                TemplateId = item.TemplateId;
                ProcessId = item.ProcessId;
                StepId = item.StepId;
                Subject = item.Subject;
                Body = item.Body;
                UserId = item.UserId;
                Enabled = item.Enabled;

                if (TemplateId.GetValueOrDefault(0) != 0)
                {
                    var template = Db.EmailTemplate.Where(p => p.EmailTemplateID == TemplateId).FirstOrDefault();
                    if (template != null)
                    {
                        var haveRecord = Db.EmailTemplate.Where(p => p.ProcessID == ProcessId && p.ActivityGroupID == StepId && p.EmailTemplateID != TemplateId).FirstOrDefault();
                        if (haveRecord == null)
                        {
                            template.ProcessID = ProcessId;
                            template.ActivityGroupID = StepId;
                            template.Subject = Subject;
                            template.Body = Body;
                            template.CreatedOn = DateTime.Now;
                            template.CreatedBy = UserId;
                            template.Enabled = Enabled;
                            Db.SaveChanges();
                            success = true;
                        } else
                        {
                            success = false;
                            msg = "This process and activity group already have email template.";
                        }
                    } else
                    {
                        success = false;
                        msg = "Email template not found.";
                    }
                }
                else
                {
                    var haveRecord = Db.EmailTemplate.Where(p => p.ProcessID == ProcessId && p.ActivityGroupID == StepId).FirstOrDefault();
                    if (haveRecord == null)
                    {
                        EmailTemplate template = new EmailTemplate();
                        template.ProcessID = ProcessId;
                        template.ActivityGroupID = StepId;
                        template.Subject = Subject;
                        template.Body = Body;
                        template.CreatedOn = DateTime.Now;
                        template.CreatedBy = UserId;
                        template.Enabled = Enabled;
                        Db.EmailTemplate.Add(template);
                        Db.SaveChanges();
                        success = true;
                    } else
                    {
                        success = false;
                        msg = "This process and activity group already have email template.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success, msg);
        }

        public Tuple<bool,string> DeleteEmailTemplate(String UserId, int TemplateId)
        {
            bool success = false;
            string msg = "";
            try
            {
                var template = Db.EmailTemplate.Where(p => p.EmailTemplateID == TemplateId).FirstOrDefault();
                if (template != null)
                {
                    if (template.CreatedBy == UserId)
                    {
                        Db.EmailTemplate.Remove(template);
                        Db.SaveChanges();
                        success = true;
                    } else
                    {
                        success = false;
                        string userName = Db.vUser.Where(p => p.UserID == template.CreatedBy).Select(p => p.FullName).FirstOrDefault();
                        msg = "Fail! This Email Template was created by " + userName + ".";
                    }
                } else
                {
                    success = false;
                    msg = "Fail! Email Template not found.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tuple.Create(success,msg);
        }

        public EmailTemplateDetailDTO GetEmailTemplateDetail(int TemplateId)
        {
            try
            {
                //return Db.EmailTemplate.Where(p => p.EmailTemplateID == TemplateId).FirstOrDefault();
                return (from a in Db.EmailTemplate
                        join b in Db.ProcessList on a.ProcessID equals b.ProcessID into ps
                        from b in ps.DefaultIfEmpty()
                        where a.EmailTemplateID == TemplateId
                        select new EmailTemplateDetailDTO
                        {
                            EmailTemplateID = a.EmailTemplateID,
                            ProcessID = a.ProcessID,
                            ActivityGroupID = a.ActivityGroupID,
                            ProcessName = b.ProcessName,
                            Subject = a.Subject,
                            Body = a.Body,
                            Enabled = a.Enabled,
                            CreatedBy = a.CreatedBy,
                            CreatedOn = a.CreatedOn
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
using HKTDC.Service.EmailNotification.DBContent;
using HKTDC.Service.EmailNotification.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification
{
    class Notification : BaseClass
    {
        private SmtpClient client;
        private EntityContext Db;
        private EventLog eventLog1;
        private bool TestMode;
        private string userAddress;
        private string sender;
        private string bcc;
        private string administrator;
        private const string taskQueued = "Queued";
        private const string taskProcessing = "Processing";
        private const string taskCompleted = "Completed";

        public Notification()
        {
            client = new SmtpClient();
            Db = new EntityContext();

            this.initialize();
        }

        public void setLogger(EventLog eventLog)
        {
            this.eventLog1 = eventLog;
        }

        public void setTestMode(bool testMode, string userAddress)
        {
            this.TestMode = testMode;
            this.userAddress = userAddress;
        }

        public void initialize()
        {
            List<AppSetting> setting = new List<AppSetting>();
            setting = Db.AppSetting.Where(p => p.Category == "Email Notification").ToList();
            int tmpPort;
            bool tmpPortBool = Int32.TryParse(setting.Where(p => p.AppKey == "SMTPPort").Select(p => p.AppValue).FirstOrDefault(), out tmpPort);
            client.Port = tmpPortBool ? tmpPort : 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            bool tmpSSL;
            bool tmpSSLBool = Boolean.TryParse(setting.Where(p => p.AppKey == "SMTPSSL").Select(p => p.AppValue).FirstOrDefault(), out tmpSSL);
            client.EnableSsl = tmpSSLBool ? tmpSSL : false;
            client.Host = setting.Where(p => p.AppKey == "SMTPHost").Select(p => p.AppValue).FirstOrDefault();

            sender = setting.Where(p => p.AppKey == "Sender").Select(p => p.AppValue).FirstOrDefault();
            bcc = setting.Where(p => p.AppKey == "BCC").Select(p => p.AppValue).FirstOrDefault();
            administrator = setting.Where(p => p.AppKey == "Administrator").Select(p => p.AppValue).FirstOrDefault();
        }

        public void sendEmail(List<NotificationList> list)
        {
            foreach (var t in list.DistinctBy(p => p.TemplateID))
            {
                var result = this.generateEmailContent(list, t);
                MailInfo mailInfo = result.Item1;
                List<int> taskArr = result.Item2;
                if (!TestMode)
                {
                    userAddress = Db.vUser.Where(p => p.UserID == t.ActionOwnerUserID && p.EmployeeID == t.ActionOwnerEmployeeID).Select(p => p.Email).FirstOrDefault();
                }
                if (!String.IsNullOrEmpty(mailInfo.Content))
                {
                    try
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress(sender);
                            mail.To.Add(userAddress);
                            mail.Subject = (generateEmailSubject(t, mailInfo.RefID)).Replace('\r', ' ').Replace('\n', ' ');
                            mail.Body = mailInfo.Content;
                            mail.IsBodyHtml = true;

                            //if (!String.IsNullOrEmpty(t.cc))
                            //{
                            //    string[] tmp = t.cc.Trim().Split(';');
                            //    foreach (var i in tmp)
                            //    {
                            //        mail.CC.Add(i);
                            //    }
                            //}
                            if (!String.IsNullOrEmpty(bcc))
                            {
                                string[] tmp = bcc.Trim().Split(';');
                                foreach (var i in tmp)
                                {
                                    mail.Bcc.Add(i);
                                }
                            }

                            client.Send(mail);

                            Thread.Sleep(5000);

                            if (TestMode) // Test Mode
                            {
                                UpdateTaskRecord(taskArr, taskQueued);
                            }
                            else
                            {
                                // Update notifcation log
                                WriteLog(taskArr, t, userAddress);
                                UpdateTaskRecord(taskArr, taskCompleted);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        eventLog1.WriteEntry("Send Email Error " + ex.Message + " " + ex.InnerException, EventLogEntryType.Error);
                        sendEmailToAdministrator("Email Notification Service Error", "Send Email Error " + ex.Message + " " + ex.InnerException);
                        WriteLog(taskArr, t, userAddress, ex.Message);
                        UpdateTaskRecord(taskArr, taskQueued);
                    }
                }
                else
                {
                    eventLog1.WriteEntry("No email body. Task ID: " + t.EmailNotificationTasksID + " Template ID: " + t.TemplateID, EventLogEntryType.Warning);
                    sendEmailToAdministrator("Email Notification Service Stored Procedure Warning", "No email body. Task ID: " + t.EmailNotificationTasksID + " Template ID: " + t.TemplateID);
                    WriteLog(taskArr, t, userAddress, "No email body. Task ID: " + t.EmailNotificationTasksID + " Template ID: " + t.TemplateID);
                    UpdateTaskRecord(taskArr, taskQueued);
                }
            }
        }
 
        public string generateEmailSubject(NotificationList t, string refId)
        {
            string subject = t.EmailSubject;
            if(subject.IndexOf("<RefID>") > 0)
            {
                subject = subject.Replace("<RefID>", refId);
            }
            return subject;
        }

        public Tuple<MailInfo, List<int>> generateEmailContent(List<NotificationList> list, NotificationList t)
        {
            List<int> taskArr = new List<int>();
            bool generated = false;
            MailInfo info = new MailInfo();
            foreach (var item in list.Where(p => p.TemplateID == t.TemplateID))
            {
                string[] tmpBody = item.EmailBody.Trim().Split(' '); // 0 - SP name, 1 - parameter
                if (tmpBody.Length == 2)
                {
                    if (!generated)
                    {
                        List<SqlParameter> tmpParameter = new List<SqlParameter>();
                        string[] parm = tmpBody[1].Trim().Split(',');

                        foreach (var i in parm)
                        {
                            if (i.Equals("<ProcessID>"))
                            {
                                tmpParameter.Add(new SqlParameter("@ProcessID", t.ProcessID));
                            }
                            else if (i.Equals("<ActivityGroupID>"))
                            {
                                tmpParameter.Add(new SqlParameter("@ActivityGroupID", t.StepOrGroupID));
                            }
                            else if (i.Equals("<OwnerID>"))
                            {
                                tmpParameter.Add(new SqlParameter("@OwnerID", t.ActionOwnerUserID));
                            }
                        }
                        tmpBody[1] = tmpBody[1].Replace("<", "@");
                        tmpBody[1] = tmpBody[1].Replace(">", "");
                        info = Db.Database.SqlQuery<MailInfo>("exec [" + tmpBody[0] + "] " + tmpBody[1], tmpParameter.ToArray()).FirstOrDefault();
                        if (!String.IsNullOrEmpty(info.TaskID.Trim()))
                        {
                            taskArr = new List<int>();
                            string[] taskInfo = info.TaskID.Split(',');
                            foreach (var taskId in taskInfo)
                            {
                                if (!String.IsNullOrEmpty(taskId.Trim())) {
                                    taskArr.Add(Int32.Parse(taskId));
                                }
                            }
                            generated = true;
                        } else
                        {
                            info.Content = null;
                            taskArr.Add(item.EmailNotificationTasksID);
                        }
                    }
                }
                else
                {
                    eventLog1.WriteEntry("Stored Procedure format of email template is wrong " + item.EmailBody, EventLogEntryType.Warning);

                    var taskRecords = Db.EmailNotificationTasks.Where(p => p.EmailNotificationTasksID == item.EmailNotificationTasksID).FirstOrDefault();
                    taskRecords.IsEmailed = taskQueued;
                    Db.SaveChanges();

                    break;
                }
            }
            return Tuple.Create(info, taskArr);
        }

        public void WriteLog(List<int> taskArr, NotificationList t, string userAddress, string errorMessage = null)
        {
            foreach (var i in taskArr)
            {
                EmailNotificaitonLog log = new EmailNotificaitonLog();
                log.EmailNotificationTasksID = i;
                log.EmailNotificationProfileID = t.ProfileID;
                log.EmailTemplateID = t.TemplateID;
                log.Recipient = userAddress;
                log.LoggedOn = DateTime.Now;
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    log.ErrorMessage = errorMessage;
                }
                Db.EmailNotificationLog.Add(log);
                Db.SaveChanges();
            }
        }

        public void UpdateTaskRecord(List<int> taskArr, string status)
        {
            var taskRecords = Db.EmailNotificationTasks.Where(p => taskArr.Contains(p.EmailNotificationTasksID)).ToList();
            taskRecords.ForEach(p => p.IsEmailed = status);
            Db.SaveChanges();
        }

        public void sendEmailToAdministrator(string subject, string content)
        {
            try
            {
                if (!String.IsNullOrEmpty(administrator))
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(sender);
                        string[] tmp = administrator.Trim().Split(';');
                        foreach (var i in tmp)
                        {
                            mail.To.Add(i);
                        }
                        mail.Subject = subject;
                        mail.Body = content;
                        mail.IsBodyHtml = true;

                        client.Send(mail);

                        Thread.Sleep(5000);
                    }
                }
            } catch(Exception e)
            {
                eventLog1.WriteEntry("Unable to send email to administrators " + e.Message, EventLogEntryType.Error);
            }
        }

        ~Notification()
        {
            Dispose(true);
        }
    }
}

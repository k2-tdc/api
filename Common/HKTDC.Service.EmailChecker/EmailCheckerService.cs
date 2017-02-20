using HKTDC.WebAPI.EmailChecker.DBContent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HKTDC.WebAPI.EmailChecker
{
    public partial class EmailCheckerService : ServiceBase
    {
        private System.Timers.Timer timer;
        private ServiceController sc;

        public EmailCheckerService()
        {
            InitializeComponent();
            this.AutoLog = false;

            if (!System.Diagnostics.EventLog.SourceExists("EmailCheckerServices"))
            {
                System.Diagnostics.EventLog.CreateEventSource("EmailCheckerServices", "EmailCheckerServices");
            }

            eventLog.Source = "EmailCheckerServices";
            eventLog.Log = "EmailCheckerServices";
            
            sc = new ServiceController("EmailNotification");
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Email Notification Checker have been started.", EventLogEntryType.Information);
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Interval = 60000;
            timer.Start();
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sc.Refresh();
            switch(sc.Status)
            {
                case ServiceControllerStatus.Stopped:
                    sendEmail("Stopped");
                    break;
                case ServiceControllerStatus.Paused:
                    //sendEmail("Paused");
                    break;
                case ServiceControllerStatus.StopPending:
                    eventLog.WriteEntry("Email Notification Services Stopping.", EventLogEntryType.Warning);
                    break;
                default: break;
            }
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
            eventLog.WriteEntry("Email Notification Checker have been stopped.", EventLogEntryType.Error);
        }

        protected void sendEmail(string status)
        {
            try
            {
                using(EntityContext Db = new EntityContext())
                {
                    using (SmtpClient client = new SmtpClient())
                    {
                        var setting = Db.AppSetting.Where(p => p.Category == "Email Notification Checker").ToList();
                        var isSend = setting.Where(p => p.AppKey == "IsSend").FirstOrDefault();
                        if (isSend.AppValue == "0")
                        {
                            eventLog.WriteEntry("Email Notification Services have been " + status, EventLogEntryType.Warning);
                            int tmpPort;
                            bool tmpPortBool = Int32.TryParse(setting.Where(p => p.AppKey == "SMTPPort").Select(p => p.AppValue).FirstOrDefault(), out tmpPort);
                            client.Port = tmpPortBool ? tmpPort : 25;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = true;
                            bool tmpSSL;
                            bool tmpSSLBool = Boolean.TryParse(setting.Where(p => p.AppKey == "SMTPSSL").Select(p => p.AppValue).FirstOrDefault(), out tmpSSL);
                            client.EnableSsl = tmpSSLBool ? tmpSSL : false;
                            client.Host = setting.Where(p => p.AppKey == "SMTPHost").Select(p => p.AppValue).FirstOrDefault();
                            using (MailMessage mail = new MailMessage())
                            {
                                var sender = setting.Where(p => p.AppKey == "Sender").Select(p => p.AppValue).FirstOrDefault();
                                mail.From = new MailAddress(sender);
                                var toAddress = setting.Where(p => p.AppKey == "To").Select(p => p.AppValue).FirstOrDefault();
                                string[] toAddressStr = toAddress.Trim().Split(';');
                                foreach (var to in toAddressStr)
                                {
                                    mail.To.Add(to);
                                }
                                mail.Subject = "Email Notification Service have been " + status;
                                mail.Body = "Email Notification Service have been " + status;
                                mail.IsBodyHtml = true;
                                client.Send(mail);
                                Thread.Sleep(5000);

                                isSend.AppValue = "1";
                                Db.SaveChanges();
                            }
                        }
                    }
                }
            } 
            catch(Exception ex)
            {
                eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }
    }
}

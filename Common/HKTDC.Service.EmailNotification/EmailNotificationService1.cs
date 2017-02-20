using HKTDC.Service.EmailNotification.DBContent;
using HKTDC.Service.EmailNotification.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HKTDC.Service.EmailNotification
{
    public partial class EmailNotificationService : ServiceBase
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private Task _task;

        //private System.Timers.Timer timer;
        //private ManualResetEvent _shutDownEvent = new ManualResetEvent(false);
        //private List<ManualResetEvent> handles;

        public EmailNotificationService()
        {
            InitializeComponent();
            this.AutoLog = false;

            if (!System.Diagnostics.EventLog.SourceExists("EmailNotificationService"))
            {
                System.Diagnostics.EventLog.CreateEventSource("EmailNotificationService", "EmailNotificationService");
            }

            eventLog1.Source = "EmailNotificationService";
            eventLog1.Log = "EmailNotificationService";

            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Email Notification Service has been Started", EventLogEntryType.Information);
            //timer = new System.Timers.Timer();
            //timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            //timer.Interval = 60000;
            //timer.AutoReset = false;
            //timer.Start();

            _task = new Task(ProcessTask, _cancellationToken);
            _task.Start();
        }
        private void ProcessTask()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                //Thread.Sleep(60000);
                try
                {
                    using (EntityContext Db = new EntityContext())
                    {
                        List<NotificationList> sendList = new List<NotificationList>();

                        SqlParameter[] sqlp = { new SqlParameter("@CurrentTime", DateTime.Now) };
                        sendList = Db.Database.SqlQuery<NotificationList>("exec [K2_GetNotificationList] @CurrentTime", sqlp).ToList();
                        if (sendList.Count > 0)
                        {
                            var receiverEmail = "";
                            var settings = Db.AppSetting.Where(p => p.Category == "Email Notification" && p.AppKey.Contains("Test Mode")).ToList();
                            if (settings != null)
                            {
                                string testMode = settings.Where(p => p.AppKey == "Test Mode").Select(p => p.AppValue).FirstOrDefault();
                                if (!String.IsNullOrEmpty(testMode) && testMode == "0")
                                {
                                    receiverEmail = settings.Where(p => p.AppKey == "Test Mode Receiver").Select(p => p.AppValue).FirstOrDefault();
                                }
                            }

                            foreach (var request in sendList.DistinctBy(p => p.ActionOwnerUserID))
                            {
                               
                                List<NotificationList> list = new List<NotificationList>();
                                list = sendList.Where(p => p.ActionOwnerUserID == request.ActionOwnerUserID && p.ActionOwnerEmployeeID == request.ActionOwnerEmployeeID).ToList();
                                //ThreadPool.QueueUserWorkItem(SendEmailProcess, Tuple.Create(list, handle, receiverEmail));
                                SendEmailProcess(Tuple.Create(list, receiverEmail));
                            }

                            if (!String.IsNullOrEmpty(receiverEmail))
                            {
                                var setting = Db.AppSetting.Where(p => p.Category == "Email Notification" && p.AppKey == "Test Mode").FirstOrDefault();
                                if (setting != null)
                                {
                                    setting.AppValue = "1";
                                    Db.SaveChanges();
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    eventLog1.WriteEntry(ex.Message + " " + ex.InnerException, EventLogEntryType.Error);
                }
                finally
                {

                }


            }
        }
        //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        using (EntityContext Db = new EntityContext())
        //        {
        //            List<NotificationList> sendList = new List<NotificationList>();
        //            handles = new List<ManualResetEvent>();

        //            SqlParameter[] sqlp = { new SqlParameter("@CurrentTime", DateTime.Now) };
        //            sendList = Db.Database.SqlQuery<NotificationList>("exec [K2_GetNotificationList] @CurrentTime", sqlp).ToList();
        //            if (sendList.Count > 0)
        //            {
        //                var receiverEmail = "";
        //                var settings = Db.AppSetting.Where(p => p.Category == "Email Notification" && p.AppKey.Contains("Test Mode")).ToList();
        //                if (settings != null)
        //                {
        //                    string testMode = settings.Where(p => p.AppKey == "Test Mode").Select(p => p.AppValue).FirstOrDefault();
        //                    if (!String.IsNullOrEmpty(testMode) && testMode == "0")
        //                    {
        //                        receiverEmail = settings.Where(p => p.AppKey == "Test Mode Receiver").Select(p => p.AppValue).FirstOrDefault();
        //                    }
        //                }

        //                foreach (var request in sendList.DistinctBy(p => p.ActionOwnerUserID))
        //                {
        //                    var handle = new ManualResetEvent(false);
        //                    handles.Add(handle);
        //                    List<NotificationList> list = new List<NotificationList>();
        //                    list = sendList.Where(p => p.ActionOwnerUserID == request.ActionOwnerUserID && p.ActionOwnerEmployeeID == request.ActionOwnerEmployeeID).ToList();
        //                    ThreadPool.QueueUserWorkItem(SendEmailProcess, Tuple.Create(list, handle, receiverEmail));
        //                }

        //                if (!String.IsNullOrEmpty(receiverEmail))
        //                {
        //                    var setting = Db.AppSetting.Where(p => p.Category == "Email Notification" && p.AppKey == "Test Mode").FirstOrDefault();
        //                    if (setting != null)
        //                    {
        //                        setting.AppValue = "1";
        //                        Db.SaveChanges();
        //                    }
        //                }

        //                WaitHandle.WaitAll(handles.ToArray());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        eventLog1.WriteEntry(ex.Message + " " + ex.InnerException, EventLogEntryType.Error);
        //    }
        //    finally
        //    {
        //        timer.Start();
        //    }
        //}

        protected override void OnStop()
        {
            //timer.Stop(); // disable timer
            //timer.Enabled = false;
            //timer.Dispose();
            ////WaitHandle.WaitAll(handles.ToArray()); // Wait for all thread finish
            //timer = null;
            //_shutDownEvent.Set();
            eventLog1.WriteEntry("Event Notficiation services is stopping.", EventLogEntryType.Information);
            _cancellationTokenSource.Cancel();
            _task.Wait();
            eventLog1.WriteEntry("Event Notficiation services have been stopped.", EventLogEntryType.Information);
        }

        //protected void SendEmailProcess(Object stateInfo)
        //{
        //    var tuple = (Tuple<List<NotificationList>, ManualResetEvent, string>)stateInfo;

        //    try
        //    {
        //        while (!_shutDownEvent.WaitOne(0))
        //        {
        //            Notification mail = new Notification();
        //            mail.setLogger(eventLog1);
        //            if (!String.IsNullOrEmpty(tuple.Item3))
        //            {
        //                mail.setTestMode(true, tuple.Item3);
        //            } else
        //            {
        //                mail.setTestMode(false, null);
        //            }
        //            mail.sendEmail(tuple.Item1);

        //            break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        eventLog1.WriteEntry(ex.Message + " " + ex.InnerException, EventLogEntryType.Error);
        //    }
        //    finally
        //    {
        //        ManualResetEvent mreEvent = tuple.Item2;
        //        mreEvent.Set();
        //    }
        //}

        protected void SendEmailProcess(Object stateInfo)
        {
            var tuple = (Tuple<List<NotificationList>, string>)stateInfo;

            try
            {
                Notification mail = new Notification();
                mail.setLogger(eventLog1);
                if (!String.IsNullOrEmpty(tuple.Item2))
                {
                    mail.setTestMode(true, tuple.Item2);
                }
                else
                {
                    mail.setTestMode(false, null);
                }
                mail.sendEmail(tuple.Item1);
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(ex.Message + " " + ex.InnerException, EventLogEntryType.Error);
            }
            finally
            {
                
            }
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }

        }
    }
}

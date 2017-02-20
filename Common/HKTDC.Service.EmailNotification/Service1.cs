using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification
{
    public partial class Service1 : ServiceBase
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private Task _task;

        public Service1()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        protected override void OnStart(string[] args)
        {
            _task = new Task(DoSomeWork, _cancellationToken);
            _task.Start();
        }

        protected override void OnStop()
        {
            _cancellationTokenSource.Cancel();
            _task.Wait();
        }

        private void DoSomeWork()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {

                // Do you heavy weight job here
                Thread.Sleep(60000);
                Task[] ta = new Task[10];
                
                var task1 = Task.Factory.StartNew(Timer_Elapsed);
                

            }
        }

        private void Timer_Elapsed()
        {
            
        }
    }
}

using FluentScheduler;
using System.Web.Hosting;
using Utils;

namespace Repositories
{
    class AutoPurge : IJob, IRegisteredObject
    {
        private readonly object _lock = new object();
        private DataAccess dataAccess = new DataAccess(DataAccess.GetWebConfigConnectionString("Infoskærm"));
        private bool _shuttingDown;


        public AutoPurge()
        {
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;
                dataAccess.Open();
                InformationsRepository.PurgeInformation(dataAccess);
                dataAccess.Close();
                // Do work, son!
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}
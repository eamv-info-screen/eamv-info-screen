using FluentScheduler;

namespace Repositories
{
   public class AutoRegistry : Registry
    {
        public AutoRegistry()
        {
            // Schedule an IJob to run at an interval
            Schedule<AutoPurge>().ToRunNow().AndEvery(10).Minutes();

        }
    }
}
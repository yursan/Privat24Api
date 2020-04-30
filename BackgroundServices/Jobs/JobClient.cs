using Hangfire;
using System;
using System.Linq.Expressions;

namespace BackgroundServices.Jobs
{
    public class JobClient : IJobClient
    {
        public void AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cron)
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cron, TimeZoneInfo.Utc);
        }

        public string EnqueueBackgroundJob<T>(Expression<Action<T>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
    }
}
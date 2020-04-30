using System;
using System.Linq.Expressions;

namespace BackgroundServices.Jobs
{
	public interface IJobClient
	{
		string EnqueueBackgroundJob<T>(Expression<Action<T>> methodCall);

		void AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cron);
	}
}
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundServices.Jobs
{
    public interface IJob
    {
        Task Execute(CancellationToken cancellationToken);
    }
}
namespace Privat24
{
    public interface IPrivat24Factory
    {
        IPrivat24ApiClient CreatePublicClient();
    }
}
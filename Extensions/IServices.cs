
namespace MauiAuth0App.Extensions;
public interface IServices
{
    public void Start();
    public void Stop();

    public void StartTokenHandler(HttpClient client);
}
namespace Disc.Fm.Service;

public class UserNotificationService
{
    public async Task<bool> DisplayNotification(string title, string message, string accept, string cancel)
    {
        return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
    }
}

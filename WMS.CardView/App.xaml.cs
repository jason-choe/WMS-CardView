using System.Windows;

namespace WMS.CardView
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (Application.Current.MainWindow == null) // ✅ 중복 생성 방지
            {
                MainWindow mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
        }
    }
}
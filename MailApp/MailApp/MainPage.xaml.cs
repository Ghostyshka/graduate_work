using Autofac;
using MailApp.Core.Interfaces;
using MailApp.Core.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MailApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel _vm;
        public MainPage()
        {
            this.InitializeComponent();
            _vm = App.Container.Resolve<MainPageViewModel>();
            DataContext = _vm;
        }

        private void DOWNLOAD_Click(object sender, RoutedEventArgs e)
        {
            var emailService = App.Container.Resolve<IEmailService>();
            emailService.LoadEmails();
        }
    }
}

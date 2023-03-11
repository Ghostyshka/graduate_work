using Autofac;
using MailApp.Core.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Microsoft.Identity.Client;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MailApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Container.Resolve<MainPageViewModel>();
        }

        private void DOWNLOAD_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

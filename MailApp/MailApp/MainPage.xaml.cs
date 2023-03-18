using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MailApp.Core.Interfaces;
using MailApp.Core.Models;
using MailApp.Core.ViewModels;
using MimeKit;
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
            LoadEmails();
        }

        private async Task LoadEmails()
        {
            var vm = App.Container.Resolve<MainPageViewModel>();
            vm.IsLoading = true;
            var emailService = App.Container.Resolve<IEmailService>();
            var emails = await emailService.LoadEmails(vm.From.Date, vm.To.Date);
            vm.MimeMessages = new ObservableCollection<MimeMessage>(emails);

            vm.EmailDatas = new ObservableCollection<EmailData>(vm.MimeMessages.Select(x => new EmailData()
            {
                Subject = x.Subject
            }));
            vm.IsLoading = false;
        }
    }
}

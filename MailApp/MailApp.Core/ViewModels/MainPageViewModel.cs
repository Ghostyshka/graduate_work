using MailApp.Core.Models;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MailApp.Core.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public DateTimeOffset MaxYear { get; set; } = DateTimeOffset.Now;
        private DateTimeOffset _from = DateTimeOffset.Now.AddDays(-7);
        private DateTimeOffset _to = DateTimeOffset.Now.AddDays(1);
        private bool _isLoading;
        public ObservableCollection<EmailData> _emailDatas = new ObservableCollection<EmailData>();
        public ObservableCollection<MimeMessage> MimeMessages { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value) return;
                _isLoading = value;
                OnPropertyChanged("IsLoading");
                OnPropertyChanged("IsNotLoading");
            }
        }
        public bool IsNotLoading => !_isLoading;


        public DateTimeOffset From
        {
            get => _from;
            set
            {
                if (_from == value) return;
                _from = value;
                OnPropertyChanged("From");
            }
        }
        public DateTimeOffset To
        {
            get => _to;
            set
            {
                if (_to == value) return;
                _to = value;
                OnPropertyChanged("To");
            }
        }
        public ObservableCollection<EmailData> EmailDatas
        {
            get => _emailDatas;
            set
            {
                _emailDatas = value;
                OnPropertyChanged("EmailDatas");
            }
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
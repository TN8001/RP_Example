using NoRP_Example;
using NoRP_Example.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using ViewModelKit;

namespace NoRP_Example.ViewModels
{
    //Update-Package Fody -Version 1.29.4.0
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<CommentViewModel> Comments { get; }

        public bool IsRunning { get; set; }

        public DelegateCommand NameChangeCommand { get; }
        public DelegateCommand ColorChangeCommand { get; }
        public DelegateCommand ConnectCommand { get; }
        public DelegateCommand DisconnectCommand { get; }

        private NicoLiveModel model = new NicoLiveModel();
        private Dictionary<string, UserModel> userDict = new Dictionary<string, UserModel>();

        private void OnNameChange(object state)
        {
            var menuItem = state as MenuItem;
            var comment = menuItem.DataContext as CommentViewModel;
            var ib = new InputBox
            {
                DataContext = comment,
                Text = comment.Name,
            };

            if(ib.ShowDialog() == true)
                comment.Name = ib.Text;
        }
        private void OnColorChange(object state)
        {
            var menuItem = state as MenuItem;
            var comment = menuItem.DataContext as CommentViewModel;
            comment.Color = (Color)menuItem.Tag;
        }

        private async void OnConnect()
        {
            await model.ConnectAsync("lv");
            IsRunning = true;
        }

        private void OnDisconnect()
        {
            model.Disconnect();
            IsRunning = false;
        }


        public MainViewModel()
        {
            // ファイルから読む体で
            userDict.Add("111", new UserModel("111", "社会人P") { Color = Colors.LightGreen });
            userDict.Add("222", new UserModel("222", "八百屋"));

            //Comments = model.Comments.ToReadOnlyReactiveCollection(x =>
            //{
            //    if(!userDict.TryGetValue(x.ID, out var user))
            //    {
            //        user = new UserModel(x.ID);
            //        userDict.Add(user.ID, user);
            //    }

            //    return new CommentViewModel(x, user);
            //}).AddTo(disposable);

            ConnectCommand.Execute();
        }

    }
}

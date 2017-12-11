using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using RP_Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace RP_Example.ViewModels
{
    public class MainViewModel : IDisposable
    {
        public ReadOnlyReactiveCollection<CommentViewModel> Comments { get; }

        public ReactiveProperty<bool> IsRunning { get; } = new ReactiveProperty<bool>(false);

        public ReactiveCommand NameChangeCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ColorChangeCommand { get; } = new ReactiveCommand();
        public AsyncReactiveCommand ConnectCommand { get; }
        public ReactiveCommand DisconnectCommand { get; }

        private NicoLiveModel model = new NicoLiveModel();
        private Dictionary<string, UserModel> userDict = new Dictionary<string, UserModel>();
        private CompositeDisposable disposable { get; } = new CompositeDisposable();


        public MainViewModel()
        {
            // ファイルから読む体で
            userDict.Add("111", new UserModel("111", "社会人P") { Color = Colors.LightGreen });
            userDict.Add("222", new UserModel("222", "八百屋"));

            Comments = model.Comments.ToReadOnlyReactiveCollection(x =>
            {
                if(!userDict.TryGetValue(x.ID, out var user))
                {
                    user = new UserModel(x.ID);
                    userDict.Add(user.ID, user);
                }

                return new CommentViewModel(x, user);
            }).AddTo(disposable);

            #region Command
            NameChangeCommand.Subscribe(menuItem =>
            {
                var comment = ((MenuItem)menuItem).DataContext as CommentViewModel;
                var ib = new InputBox
                {
                    DataContext = comment,
                    Text = comment.Name.Value,
                };

                if(ib.ShowDialog() == true)
                    comment.Name.Value = ib.Text;
            });

            ColorChangeCommand.Subscribe(menuItem =>
            {
                var comment = ((MenuItem)menuItem).DataContext as CommentViewModel;
                comment.Color.Value = (Color)((MenuItem)menuItem).Tag;
            });

            ConnectCommand = IsRunning.Select(x => !x).ToAsyncReactiveCommand();
            ConnectCommand.Subscribe(async _ =>
            {
                await model.ConnectAsync("lv");
                IsRunning.Value = true;
            }).AddTo(disposable); 

            DisconnectCommand = IsRunning.ToReactiveCommand();
            DisconnectCommand.Subscribe(_ =>
            {
                model.Disconnect();
                IsRunning.Value = false;
            }).AddTo(disposable);
            #endregion

            ConnectCommand.Execute();
        }

        public void Dispose() => disposable.Dispose();
    }
}

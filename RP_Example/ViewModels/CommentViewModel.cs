using NicoLiveLibrary;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using RP_Example.Models;
using System;
using System.Reactive.Disposables;
using System.Windows.Media;

namespace RP_Example.ViewModels
{
    public class CommentViewModel : IDisposable
    {
        public ReactiveProperty<int> Number { get; }
        public ReactiveProperty<string> ID { get; }
        public ReactiveProperty<string> Text { get; }

        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<Color?> Color { get; }

        private CompositeDisposable disposable { get; } = new CompositeDisposable();


        public CommentViewModel(CommentEntity model, UserModel user)
        {
            Number = new ReactiveProperty<int>(model.Number);
            ID = new ReactiveProperty<string>(model.ID);
            Text = new ReactiveProperty<string>(model.Text);

            Name = user.ToReactivePropertyAsSynchronized(x => x.Name).AddTo(disposable);
            Color = user.ToReactivePropertyAsSynchronized(x => x.Color).AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}

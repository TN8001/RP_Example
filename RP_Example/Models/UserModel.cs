using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace RP_Example.Models
{
    // ファイルに保存する体で ここがRPだとかえって面倒そう？？
    // 保存処理は入れていないがID==Name||Color==nullは保存しない
    // AddDateが1週間以上離れていれば保存しない といった処理のつもり
    public class UserModel : INotifyPropertyChanged
    {
        public string ID { get; }
        public string Name
        {
            get => _Name;
            set
            {
                if(!Set(ref _Name, value)) return;
                if(ID == value) return;

                if(IsAnonymous())
                    AddDate = DateTime.Now;
            }
        }
        private string _Name;
        public Color? Color { get => _Color; set => Set(ref _Color, value); }
        private Color? _Color;

        private DateTime? AddDate;

        public UserModel(string id, string name = null)
        {
            ID = id;
            Name = name ?? id;
        }

        private bool IsAnonymous() => !int.TryParse(ID, out var _);

        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}

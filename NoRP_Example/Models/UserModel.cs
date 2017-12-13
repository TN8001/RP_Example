using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using ViewModelKit;

namespace NoRP_Example.Models
{
    public class UserModel : ViewModelBase
    {
        public string ID { get; }
        public string Name { get; set; }
        public Color? Color { get; set; }

        private DateTime? AddDate;


        public UserModel(string id, string name = null)
        {
            ID = id;
            Name = name ?? id;
        }

        private void OnNameChanged()
        {
            if(IsAnonymous())
                AddDate = DateTime.Now;
        }
        private bool IsAnonymous() => !int.TryParse(ID, out var _);
    }
}

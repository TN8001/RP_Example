using NicoLiveLibrary;
using NoRP_Example.Models;
using System;
using System.Windows.Media;

namespace NoRP_Example.ViewModels
{
    public class CommentViewModel 
    {
        public int Number => model.Number;
        public string ID => model.ID;
        public string Text => model.Text;

        public string Name { get => user.Name; set => user.Name = value; }
        public Color? Color { get => user.Color; set => user.Color = value; }

        private CommentEntity model;
        private UserModel user;


        public CommentViewModel(CommentEntity model, UserModel user)
        {
            this.model = model;
            this.user = user;
        }
    }
}

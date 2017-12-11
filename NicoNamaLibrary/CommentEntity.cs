
namespace NicoLiveLibrary
{
    public class CommentEntity
    {
        public int Number { get; }
        public string ID { get; }
        public string Text { get; }

        public CommentEntity(int number, string id, string text)
        {
            Number = number;
            ID = id;
            Text = text;
        }
    }
}

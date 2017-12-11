using NicoLiveLibrary;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RP_Example.Models
{
    public class NicoLiveModel
    {
        public ObservableCollection<CommentEntity> Comments { get; } = new ObservableCollection<CommentEntity>();

        private NicoLiveClient client = new NicoLiveClient();


        public NicoLiveModel() => client.OnComment += (comment) => Comments.Add(comment);


        public async Task ConnectAsync(string liveID)
        {
            Comments.Clear();

            var allComments = await client.GetAllCommentAsync();
            foreach(var comment in allComments)
                Comments.Add(comment);

            client.Connect(liveID);
        }
        public void Disconnect() => client.Disconnect();
    }
}

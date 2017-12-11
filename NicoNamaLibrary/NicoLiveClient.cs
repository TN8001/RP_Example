using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NicoLiveLibrary
{
    public delegate void NicoLiveEventHandler(CommentEntity comment);

    // コメント取得ライブラリの超雑なシミュレートｗ
    public class NicoLiveClient
    {
        private static string[] ids = { "111", "222", "333", "qwertyuiop" };
        private static string[] comments = { "わこ", "ｗｗ", "８８８", "おつおつ" };

        public event NicoLiveEventHandler OnComment;

        private Timer timer;
        private int number;
        private Random random = new Random();


        public NicoLiveClient() => timer = new Timer(Tick);

        public async Task<IEnumerable<CommentEntity>> GetAllCommentAsync()
        {
            await Task.Delay(1000);

            var list = new List<CommentEntity>();
            for(var i = 0; i < 5; i++)
            {
                var id = ids[random.Next(ids.Length)];
                var comment = comments[random.Next(comments.Length)];
                list.Add(new CommentEntity(++number, id, comment));
            }

            return list;
        }

        public void Connect(string liveID)
        {
            if(!liveID.ToLower().StartsWith("lv")) throw new ArgumentException("not liveID.");

            timer.Change(1000, 1000);
        }
        public void Disconnect()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite); // 停止
            number = 0;
        }

        private void Tick(object state)
        {
            var id = ids[random.Next(ids.Length)];
            var comment = comments[random.Next(comments.Length)];
            OnComment(new CommentEntity(++number, id, comment));
        }
    }
}

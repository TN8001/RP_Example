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

        private int number;
        private Random random = new Random();
        private CancellationTokenSource tokenSource;

        public async Task<IEnumerable<CommentEntity>> ConnectAsync(string liveID)
        {
            if(!liveID.ToLower().StartsWith("lv")) throw new ArgumentException("not liveID.");

            number = 0;
            var list = new List<CommentEntity>();

            await Task.Run(() => // 過去コメ取得に時間がかかってる体
            {
                for(var i = 0; i < 5; i++)
                {
                    Task.Delay(500).Wait();
                    var id = ids[random.Next(ids.Length)];
                    var comment = comments[random.Next(comments.Length)];
                    list.Add(new CommentEntity(++number, id, comment));
                }
            });

            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            // 「_」ワーニング消し 特に意味はない 
            var _ = Task.Run(() => GetCommentLoop(token), token);

            return list;
        }
        public void Disconnect() => tokenSource?.Cancel();

        private void GetCommentLoop(CancellationToken token)
        {
            while(true)
            {
                if(token.IsCancellationRequested) break;

                Task.Delay(random.Next(1000, 2000)).Wait(); // 適当にコメント間隔

                var id = ids[random.Next(ids.Length)];
                var comment = comments[random.Next(comments.Length)];

                if(token.IsCancellationRequested) break;
                OnComment(new CommentEntity(++number, id, comment));
            }
        }
    }
}

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
            for(var i = 0; i < 5; i++)
            {
                var id = ids[random.Next(ids.Length)];
                var comment = comments[random.Next(comments.Length)];
                list.Add(new CommentEntity(++number, id, comment));
            }
            await Task.Delay(2000); // 過去コメ取得に時間がかかってる体

            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var _ = Task.Run(() => GetComment(token), token);


            return list;
        }

        public void Disconnect()
        {
            tokenSource?.Cancel();
        }

        private void GetComment(CancellationToken token)
        {
            while(true)
            {
                Task.Delay(random.Next(1000, 2000)).Wait();

                var id = ids[random.Next(ids.Length)];
                var comment = comments[random.Next(comments.Length)];
                OnComment(new CommentEntity(++number, id, comment));

                if(token.IsCancellationRequested)
                    break;
            }
        }
    }
}

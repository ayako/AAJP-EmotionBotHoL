using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;

// [en] Added Cognitive Services Face API related libraries
// [ja] Cognitive Services Face API 関連ライブラリー追加
using Microsoft.ProjectOxford.Face;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        //[en] Get Face API Endpoint, Face API Key from Web.config
        //[ja] Web.config で設定した Face API Endpoint, Face API Key を取得
        readonly string faceApiEndpoint = ConfigurationManager.AppSettings["FaceApiEndpoint"];
        readonly string faceApiKey = ConfigurationManager.AppSettings["FaceApiKey"];

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;

            // [en] Set bot message | default when no attachment
            // [ja] Botからの返答を設定 | attachment がない場合は初期メッセージ
            var result = $"表情判定 BOTです。\n 写真を送ると、写っている人の表情を判定します。";

            // [en] When attachment(photo) attached
            // [ja] attachment(写真) が送られてきた場合
            if (message.Attachments?.Count != 0)
            {
                // [en] Get photo as stream from attachment url
                // [ja] attachment の URL から画像を Stream として取得
                var photoUrl = message.Attachments[0].ContentUrl;
                var client = new HttpClient();
                var photoStream = await client.GetStreamAsync(photoUrl);

                // [en] Detect emotion by Face API
                // [ja] FaceAPI で表情を分析
                var faceClient = new FaceServiceClient(faceApiKey, faceApiEndpoint);
                var faceResult = await faceClient.DetectAsync(
                            photoStream,
                            returnFaceId: true,
                            returnFaceLandmarks: true,
                            returnFaceAttributes: Enum.GetValues(typeof(FaceAttributeType)).OfType<FaceAttributeType>().ToArray()
                            );

                // [en] Success to get emotion scores
                // [ja] 分析結果が取得できた場合
                try
                {
                    // [en] Get all emotion scores
                    // [ja] 各 Emotion のスコアを取得
                    var emotion = faceResult[0].FaceAttributes.Emotion;

                    //// [en] CASE_1: Happiness score
                    //// [ja] CASE_1: 笑顔判定
                    //var score = emotion.Happiness;
                    //result = $"この写真は 笑顔 " + (int)(score * 100) + "% です。";

                    // [en] CASE_2: 8 kinds of emotion scores
                    // [ja] CASE_2: 8 種類の表情判定
                    var emotionResult = new Dictionary<string, float>()
                    {
                        { "怒っている", emotion.Anger},
                        { "軽蔑している", emotion.Contempt },
                        { "うんざりしている", emotion.Disgust },
                        { "怖がっている", emotion.Fear },
                        { "楽しい", emotion.Happiness},
                        { "中立の", emotion.Neutral},
                        { "悲しんでいる", emotion.Sadness },
                        { "驚いている", emotion.Surprise}
                    }
                            .OrderByDescending(kv => kv.Value)
                            .ThenBy(kv => kv.Key)
                            .ToList();

                            result = $"この写真は " + emotionResult.First().Key + " 表情に見えます。(score: " + (int)(emotionResult.First().Value * 100) + "%)";

                }

                // [ja] 分析結果が取得できない場合
                catch
                {
                    result = $"表情を判定できませんでした。";
                }
            }

            // [ja] Botから返答送信
            await context.PostAsync(result);
            context.Wait(MessageReceivedAsync);
        }
    }
}

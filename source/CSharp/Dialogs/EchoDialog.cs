using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;

// [en] Added Cognitive Services Face API related libraries
// [ja] Cognitive Services Face API �֘A���C�u�����[�ǉ�
using Microsoft.ProjectOxford.Face;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        //[en] Get Face API Key from Web.config
        //[ja] Web.config �Őݒ肵�� Face API Key ���擾
        readonly string faceApiKey = ConfigurationManager.AppSettings["FaceApiKey"];

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;

            // [en] Set bot message | default when no attachment
            // [ja] Bot����̕ԓ���ݒ� | attachment ���Ȃ��ꍇ�͏������b�Z�[�W
            var result = $"�\��� BOT�ł��B\n �ʐ^�𑗂�ƁA�ʂ��Ă���l�̕\��𔻒肵�܂��B";

            // [en] When attachment(photo) attached
            // [ja] attachment(�ʐ^) �������Ă����ꍇ
            if (message.Attachments?.Count != 0)
            {
                // [en] Get photo as stream from attachment url
                // [ja] attachment �� URL ����摜�� Stream �Ƃ��Ď擾
                var photoUrl = message.Attachments[0].ContentUrl;
                var client = new HttpClient();
                var photoStream = await client.GetStreamAsync(photoUrl);

                // [en] Detect emotion by Face API
                // [ja] FaceAPI �ŕ\��𕪐�
                var faceClient = new FaceServiceClient(faceApiKey);
                var faceResult = await faceClient.DetectAsync(
                            photoStream,
                            returnFaceId: true,
                            returnFaceLandmarks: true,
                            returnFaceAttributes: Enum.GetValues(typeof(FaceAttributeType)).OfType<FaceAttributeType>().ToArray()
                            );

                // [en] Success to get emotion scores
                // [ja] ���͌��ʂ��擾�ł����ꍇ
                try
                {
                    // [en] Get all emotion scores
                    // [ja] �e Emotion �̃X�R�A���擾
                    var emotion = faceResult[0].FaceAttributes.Emotion;

                    //// [en] CASE_1: Happiness score
                    //// [ja] CASE_1: �Ί画��
                    //var score = emotion.Happiness;
                    //result = $"���̎ʐ^�� �Ί� " + (int)(score * 100) + "% �ł��B";

                    // [en] CASE_2: 8 kinds of emotion scores
                    // [ja] CASE_2: 8 ��ނ̕\���
                    var emotionResult = new Dictionary<string, float>()
                    {
                        { "�{���Ă���", emotion.Anger},
                        { "�y�̂��Ă���", emotion.Contempt },
                        { "���񂴂肵�Ă���", emotion.Disgust },
                        { "�|�����Ă���", emotion.Fear },
                        { "�y����", emotion.Happiness},
                        { "������", emotion.Neutral},
                        { "�߂���ł���", emotion.Sadness },
                        { "�����Ă���", emotion.Surprise}
                    }
                            .OrderByDescending(kv => kv.Value)
                            .ThenBy(kv => kv.Key)
                            .ToList();

                            result = $"���̎ʐ^�� " + emotionResult.First().Key + " �\��Ɍ����܂��B(score: " + (int)(emotionResult.First().Value * 100) + "%)";

                }

                // [ja] ���͌��ʂ��擾�ł��Ȃ��ꍇ
                catch
                {
                    result = $"�\��𔻒�ł��܂���ł����B";
                }
            }

            // [ja] Bot����ԓ����M
            await context.PostAsync(result);
            context.Wait(MessageReceivedAsync);
        }
    }
}
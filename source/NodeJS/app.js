/*-----------------------------------------------------------------------------
A simple echo bot for the Microsoft Bot Framework. 
-----------------------------------------------------------------------------*/

var restify = require('restify');
var builder = require('botbuilder');
var botbuilder_azure = require("botbuilder-azure");
// [en] Added modules
// [ja] モジュール追加
require('dotenv').config();
var request = require('request');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
   console.log('%s listening to %s', server.name, server.url);
});

// Create chat connector for communicating with the Bot Framework Service
var connector = new builder.ChatConnector({
    appId: process.env.MicrosoftAppId,
    appPassword: process.env.MicrosoftAppPassword,
    openIdMetadata: process.env.BotOpenIdMetadata
});

// Listen for messages from users
server.post('/api/messages', connector.listen());

/*----------------------------------------------------------------------------------------
* Bot Storage: This is a great spot to register the private state storage for your bot. 
* We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
* For samples and documentation, see: https://github.com/Microsoft/BotBuilder-Azure
* ---------------------------------------------------------------------------------------- */

var tableName = 'botdata';
var azureTableClient = new botbuilder_azure.AzureTableClient(tableName, process.env['AzureWebJobsStorage']);
var tableStorage = new botbuilder_azure.AzureBotStorage({ gzipData: false }, azureTableClient);

// Create your bot with a function to receive messages from the user
var bot = new builder.UniversalBot(connector, function (session) {

    // [en] Set bot message | default when no attachment
    // [ja] Botからの返答を設定 | attachment がない場合は初期メッセージ
    var msg = "表情判定 BOTです。\n\n写真を送ると、写っている人の表情を判定します。";

    // [en] When attachment(photo) attached
    // [ja] attachment(写真) が送られてきた場合
    if (session.message.attachments.length > 0) {
        // [en] Detect emotion by Face API
        // [ja] FaceAPI で表情を分析
        var FaceApiRequestOptions = {
            uri: process.env['FACE_API_URL'] + "/detect?returnFaceId=false&returnFaceLandmarks=false&returnFaceAttributes=emotion",
            headers: {
                "Content-Type": "application/json",
                "Ocp-Apim-Subscription-Key": process.env['FACE_API_KEY']
            },
            json: {
                "url": session.message.attachments[0].contentUrl
            }

        };

        request.post(FaceApiRequestOptions, function (error, response, body) {

            // [en] Success to get emotion scores
            // [ja] 分析結果が取得できた場合
            if (!error && response.statusCode == 200 && response.body.length != 0) {

                // [en] CASE_1: Happiness score
                // [ja] CASE_1: 笑顔判定
                //var score = response.body[0].faceAttributes.emotion.happiness;
                //msg = "この写真は 笑顔 " + (score.toFixed(2) * 100) + "% です。";

                // [en] CASE_2: 8 kinds of emotion scores
                // [ja] CASE_2: 8 種類の表情判定
                var emotion = response.body[0].faceAttributes.emotion;
                msg = "この写真は\n\n"
                        + "- 怒り　　: " + (emotion.anger.toFixed(2) * 100) + "%\n\n"
                        + "- 軽蔑　　: " + (emotion.contempt.toFixed(2) * 100) + "%\n\n"
                        + "- むかつき: " + (emotion.disgust.toFixed(2) * 100) + "%\n\n"
                        + "- 恐れ　　: " + (emotion.fear.toFixed(2) * 100) + "%\n\n"
                        + "- 楽しい　: " + (emotion.happiness.toFixed(2) * 100) + "%\n\n"
                        + "- 中立　　: " + (emotion.neutral.toFixed(2) * 100) + "%\n\n"
                        + "- 悲しい　: " + (emotion.sadness.toFixed(2) * 100) + "%\n\n"
                        + "- 驚き　　: " + (emotion.surprise.toFixed(2) * 100) + "%\n\n"
                        + "という表情に見えます。";

            } else {
                msg = "表情を判定できませんでした。";
            }

            session.send(msg);
        });

    // 画像がない場合
    } else {
        session.send(msg);
    }
});
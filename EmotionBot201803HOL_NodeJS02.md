# Azure Antenna : Azure Bot Service ではじめるチャットボット開発ハンズオン
## Azure Bot Service & Cognitive Services Face API による表情分析チャットボット

# 2. Azure Bot Service テンプレート を使った Bot Framework アプリの開発 (Node.JS 編)

Microsoft Cognitive Services は 画像、文章、言語、情報を処理する機能を API 経由で利用できるサービスです。
Cognitive Services の一つである Face API では、画像を分析して人間の顔やその表情を数値化し、結果を JSON 形式で取得することができます。

以下の手順では画像を送信すると、表情分析スコアを表示する BOT を作成します。ベースは Microsoft Azure Bot Service の Node.JS Basic テンプレート (Bot Framework v3.13) を利用し、Cognitive Services Face API の呼び出しを行います。
> Azure Bot Service の作成方法は [1. Azure Portal から Azure Bot Service の作成 (Node.JS 編)](EmotionBot201803HOL_NodeJS01.md) をご覧ください。

- 手順
    - [Bot アプリケーションテンプレートのダウンロードと準備](#bot-%E3%82%A2%E3%83%97%E3%83%AA%E3%82%B1%E3%83%BC%E3%82%B7%E3%83%A7%E3%83%B3%E3%83%86%E3%83%B3%E3%83%97%E3%83%AC%E3%83%BC%E3%83%88%E3%81%AE%E3%83%80%E3%82%A6%E3%83%B3%E3%83%AD%E3%83%BC%E3%83%89%E3%81%A8%E6%BA%96%E5%82%99)
    - [Bot 開発に必要な ライブラリーのインストール](#bot-%E9%96%8B%E7%99%BA%E3%81%AB%E5%BF%85%E8%A6%81%E3%81%AA-%E3%83%A9%E3%82%A4%E3%83%96%E3%83%A9%E3%83%AA%E3%83%BC%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)
    - [Bot の情報保持用ストレージ および Face API キーの設定](#bot-%E3%81%AE%E6%83%85%E5%A0%B1%E4%BF%9D%E6%8C%81%E7%94%A8%E3%82%B9%E3%83%88%E3%83%AC%E3%83%BC%E3%82%B8-%E3%81%8A%E3%82%88%E3%81%B3-face-api-%E3%82%AD%E3%83%BC%E3%81%AE%E8%A8%AD%E5%AE%9A)
    - [会話のハンドリングの記述](#%E4%BC%9A%E8%A9%B1%E3%81%AE%E3%83%8F%E3%83%B3%E3%83%89%E3%83%AA%E3%83%B3%E3%82%B0%E3%81%AE%E8%A8%98%E8%BF%B0)
    - [Face API を呼び出すロジックの記述](#face-api-%E3%82%92%E5%91%BC%E3%81%B3%E5%87%BA%E3%81%99%E3%83%AD%E3%82%B8%E3%83%83%E3%82%AF%E3%81%AE%E8%A8%98%E8%BF%B0)
    - [8 種類の表情スコアを回答にセットするロジックの記述](#8-%E7%A8%AE%E9%A1%9E%E3%81%AE%E8%A1%A8%E6%83%85%E3%82%B9%E3%82%B3%E3%82%A2%E3%82%92%E5%9B%9E%E7%AD%94%E3%81%AB%E3%82%BB%E3%83%83%E3%83%88%E3%81%99%E3%82%8B%E3%83%AD%E3%82%B8%E3%83%83%E3%82%AF%E3%81%AE%E8%A8%98%E8%BF%B0)
    - [BOT アプリケーションの最終動作確認](#bot-%E3%82%A2%E3%83%97%E3%83%AA%E3%82%B1%E3%83%BC%E3%82%B7%E3%83%A7%E3%83%B3%E3%81%AE%E6%9C%80%E7%B5%82%E5%8B%95%E4%BD%9C%E7%A2%BA%E8%AA%8D)
- [Appendix](#appendix)

この BOT アプリは Bot Framework Channel Emulator を使ってローカル環境で稼働確認することが可能です。
<img src="/media/20180313_34n.PNG" width="300">


# Bot アプリケーションテンプレートのダウンロードと準備
Azure Portal で [1. Azure Portal から Azure Bot Service の作成 (Node.JS 編)](EmotionBot201803HOL_NodeJS01.md) で作成した Azure Bot Service を参照し、Web App Bot の設定ペインのメニューから **ビルド** をクリックします。
**zipファイルをダウンロード** をクリックすると、Zip ファイルの形でダウンロードできます。

<img src="/media/20180527_01.png" width="500">

**zipファイルをダウンロード** が表示されたらクリックして、ソースコードをダウンロードします。

<img src="/media/20180527_02.png" width="500"><img src="/media/20180527_03.png" width="500">

**保存** をクリックして、ローカル環境にソースコードを保存します。

<img src="/media/20180527_04.png" width="500">

ダウンロードしたソースコード (Zip ファイル) を右クリックして、[すべて展開] を選択して展開します。

<img src="/media/20180313_19.PNG" width="250"><img src="/media/20180313_20.PNG" width="400">

展開して作成されたフォルダーを右クリックして、[プロパティ] を選択します。

<img src="/media/20180313_21.PNG" width="300">

[読み取り専用属性] をクリックして、選択なしに変更、[OK] をクリックして解除します。

<img src="/media/20180313_22.PNG" width="250">

[変更をこのフォルダー、サブフォルダーおよびファイルに適用する] を選択して [OK] をクリックします。

<img src="/media/20180313_23.PNG" width="250">

展開および読み取り専用属性を解除したソースコードフォルダーを Node.JS 実行環境に配置します。

<img src="/media/20180313_24n.PNG" width="500">



# Bot 開発に必要な ライブラリーのインストール
ローカル環境にないライブラリーをインストールします。

```bash
# ソースコードのディレクトリに移動する
cd <ボットの名前>-src

# package.json の dependencies のパッケージのみインストールする
npm install --only=prod

# 追加のパッケージを devDependencies としてインストールする
npm install --save-dev request dotenv
```

> - botbuilder: Microsoft Bot Framework ベースのBOT SDK
> - botbuilder-azure: Azure への接続を行う SDK
> - restify: REST形式のwebサービス構築に特化したフレームワーク
> - request: HTTPリクエストを扱う際に便利なモジュール
> - dotenv: 環境変数を扱う際に便利なモジュール


# Bot の情報保持用ストレージ および Face API キーの設定
ソースコードフォルダー直下に *.env* という新規ファイルを作成します。
その中に Bot の情報保持用の Azure Storage の設定、および Face API の URL, キー を設定を追加します。

```.env
FACE_API_URL="YOUR_FACE_API_ENDPOINT"
FACE_API_KEY="YOUR_FACE_API_KEY"
AzureWebJobsStorage="YOUR_STORAGE_CONNECTION_STRING"
```

| 値 | 説明 |
|---|--|
| YOUR_FACE_API_URL | [Cognitive Services の無料サブスクリプションの申し込み方法](CognitiveSubscriptionTrial.md) の手順で取得した Face API のエンドポイント(URL) |
| YOUR_FACE_API_KEY | [同上](CognitiveSubscriptionTrial.md) の手順で取得した Face API の Face API キー |
| YOUR_STORAGE_CONNECTION_STRING | 1. Azure Portal から Azure Bot Service の作成 (Node.JS 編) の [Azure Bot Service アプリの Bot Channel 関連設定](EmotionBot201803HOL_NodeJS01.md#azure-bot-service-アプリの-bot-channel-関連設定) で保存した AzureWebsStorage の設定文字列 |

*.env* を保存します。


# 会話のハンドリングの記述
*app.js* ファイルをクリックして表示し、こちらを編集していきます。

冒頭に、先ほど追加した ライブラリー を追加します。

```js:app.js
require('dotenv').config();
var request = require('request');
```

次に、ファイル末尾の下記の処理を削除します。

```js:app.js
// bot.set('storage', tableStorage);

// bot.dialog('/', function (session) {
//     session.send('You said ' + session.message.text);
// });
```

そして、以降 `var bot = new builder.UniversalBot(connector);` を書き換えていきます。この処理は、ユーザーから Bot に対してメッセージ(画像などの添付ファイルを含む)が送られた時の処理です。

まずは、下記のように編集してください。

```js:app.js
// // Create your bot with a function to receive messages from the user
// var bot = new builder.UniversalBot(connector);

var bot = new builder.UniversalBot(connector, function (session) {

    // Botからの返答を設定 | attachment がない場合は初期メッセージ
    var msg = "表情判定 BOTです。\n\n写真を送ると、写っている人の表情を判定します。";

    // attachment(写真) が送られてきた場合
    if (session.message.attachments.length > 0) {
        // FaceAPI で表情を分析

            // 分析結果が取得できた場合
            // CASE_1: 笑顔判定
            // CASE_2: 8 種類の表情判定

        session.send(msg);

    // 画像がない場合
    } else {
        session.send(msg);
    }
});
```

忘れずに *app.js* を保存します。

## BOT の動作確認
ここで一旦 BOT の動作確認を行いましょう。

まず、作成した Node.JS アプリケーションを実行します。

ここでは、コマンドラインからの実行で進めます。ターミナル（シェル、コマンドプロンプトなど）を開き、下記を実行してください。

```bash
node app.js
```

すると、下記のようにアプリケーションの URL とポート番号が表示されます。この場合、このアプリケーションにアクセスするには、 `http://localhost:3978` というURLを用います。

```bash
restify listening to http://[::]:3978
```

その後、Bot Framework Channel Emulator を起動してアクセスを行います。

Bot Framework Channel Emulator の上部中央にある *Enter your endpont URL* の欄に `http://上記のアプリケーションのURL:ポート/api/messages` を入力し、 [CONNECT] をクリックしましょう。

- 例: `http://localhost:3978/api/messages`

<img src="/media/20180313_32n.PNG" width="300">

何か文字を入力して送信すると、指定した初期メッセージが返信されることを確認してください。

<img src="/media/20180313_33n.PNG" width="500">


# Face API を呼び出すロジックの記述

ふたたび、 `var bot = new builder.UniversalBot(connector);` の内容を編集します。

まず、 `// FaceAPI で表情を分析` の部分に処理を加えましょう。

先ほど設定した Face API Key を使って `FaceApiRequestOption` を作成し、Web API で呼び出します。得られる結果を `response` に格納します。

```js:app.js
var bot = new builder.UniversalBot(connector, function (session) {
    :(中略)
        // FaceAPI で表情を分析
        var FaceApiRequestOptions = {
            uri: process.env['FACE_API_URL'] + "/detect?"
                + "returnFaceId=false&returnFaceLandmarks=false&returnFaceAttributes=emotion",
            headers: {
                "Content-Type": "application/json",
                "Ocp-Apim-Subscription-Key": process.env['FACE_API_KEY']
            },
            json: {
                "url": session.message.attachments[0].contentUrl
            }

        };

        request.post(FaceApiRequestOptions, function (error, response, body) {

            // 分析結果が取得できた場合
                // CASE_1: 笑顔判定
                // CASE_2: 8 種類の表情判定

            session.send(msg);

        });

    // 画像がない場合
    } else {
        session.send(msg);
    }
});
```


つぎに、 `// 分析結果が取得できた場合` を編集します。 `response` から `happiness` のスコアを取得し、メッセージを設定します。

```js:app.js
var bot = new builder.UniversalBot(connector, function (session) {
    :(中略)
        request.post(FaceApiRequestOptions, function (error, response, body) {

            // 分析結果が取得できた場合
            if (!error && response.statusCode == 200 && response.body.length != 0) {

                // CASE_1: 笑顔判定
                var score = response.body[0].faceAttributes.emotion.happiness;
                msg = "この写真は 笑顔 " + (score.toFixed(2) * 100) + "% です。";

                // CASE_2: 8 種類の表情判定

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
```


## Bot アプリケーションの動作確認 (Happiness スコアの表示)
ここでもう一度 BOT の動作確認を行います。

ふたたび、アプリケーションを実行します。*app.js* が実行中の場合は、まず停止してください。

> コマンドラインで実行中のアプリケーションを停止するには、 *ctrl + c* を入力します。


```bash
# 実行中の場合は、ctrl + c で停止する

# ふたたび app.js を実行する
node app.js
```

アプリケーションを再実行できたら、Bot Framework Channel Emulator で動作を確認します。

画像アイコンをクリックしてローカルの画像を選択し、Bot に送信すると、Happiness のスコアが返答されるのを確認してください。

<img src="/media/20180313_34n.PNG" width="300">


# 8 種類の表情スコアを回答にセットするロジックの記述

今度は、`// 分析結果が取得できた場合` の `// CASE_2: 8 種類の表情判定` を編集します。

まず、 `CASE_1: 笑顔判定` をコメントアウトし、その下の `case_2` に処理を追加してください。

この処理は、得られた 8 種類の表情スコアを取得して回答にセットします。

```js:app.js
var bot = new builder.UniversalBot(connector, function (session) {
    :(中略)
        request.post(FaceApiRequestOptions, function (error, response, body) {

            // 分析結果が取得できた場合
            if (!error && response.statusCode == 200 && response.body.length != 0) {

                // CASE_1: 笑顔判定
                //var score = response.body[0].faceAttributes.emotion.happiness;
                //msg = "この写真は 笑顔 " + (score.toFixed(2) * 100) + "% です。";

                // CASE_2: 8 種類の表情判定
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
```


忘れずに *app.js* を保存しておきます。

# BOT アプリケーションの最終動作確認
再度アプリケーションを再実行し、Bot Framework Channel Emulator で動作を確認します。

まず何かメッセージを入力すると、デフォルトの回答が返信されます。

その後、画像を送信して、8つの感情が回答として返信されれば、BOT アプリケーションは完成です。

<img src="/media/20180313_35n.PNG" width="300">

# Appendix
こちらを再度 Azure Bot Service にデプロイ (アップロード) すると、作成した BOT を公開できます。

完成形のソースコードは GitHub にて公開しています。

https://github.com/ayako/AAJP-EmotionBotHoL/tree/master/source

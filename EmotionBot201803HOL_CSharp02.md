# Azure Antenna : Azure Bot Service ではじめるチャットボット開発ハンズオン
## Azure Bot Service & Cognitive Services Face API による表情分析チャットボット

# 2. Azure Bot Service テンプレート を使った Bot Framework アプリの開発 (C# 編)

Microsoft Cognitive Services は 画像、文章、言語、情報を処理する機能を API 経由で利用できるサービスです。
Cognitive Services の一つである Face API では、画像を分析して人間の顔やその表情を数値化し、結果を JSON 形式で取得することができます。

以下の手順では画像を送信すると、表情分析スコアを表示する BOT を作成します。ベースは Microsoft Azure Bot Service の C# Basic テンプレート (Bot Framework v3.12) を利用し、Cognitive Services Face API C# ライブラリーを用いて呼び出しを行います。
> Azure Bot Service の作成方法は [1. Azure Portal から Azure Bot Service の作成 (C# 編)](EmotionBot201803HOL_CSharp01.md) をご覧ください。

- 手順
    - [Bot アプリケーションテンプレートのダウンロードと準備](#bot-%E3%82%A2%E3%83%97%E3%83%AA%E3%82%B1%E3%83%BC%E3%82%B7%E3%83%A7%E3%83%B3%E3%83%86%E3%83%B3%E3%83%97%E3%83%AC%E3%83%BC%E3%83%88%E3%81%AE%E3%83%80%E3%82%A6%E3%83%B3%E3%83%AD%E3%83%BC%E3%83%89%E3%81%A8%E6%BA%96%E5%82%99)
    - [Cognitive Services Face API の C# ライブラリーのインストール](#cognitive-services-face-api-%E3%81%AE-c-%E3%83%A9%E3%82%A4%E3%83%96%E3%83%A9%E3%83%AA%E3%83%BC%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)
    - [Bot の情報保持用ストレージ および Face API キーの設定](#bot-%E3%81%AE%E6%83%85%E5%A0%B1%E4%BF%9D%E6%8C%81%E7%94%A8%E3%82%B9%E3%83%88%E3%83%AC%E3%83%BC%E3%82%B8-%E3%81%8A%E3%82%88%E3%81%B3-face-api-%E3%82%AD%E3%83%BC%E3%81%AE%E8%A8%AD%E5%AE%9A)
    - [会話のハンドリングの記述](#%E4%BC%9A%E8%A9%B1%E3%81%AE%E3%83%8F%E3%83%B3%E3%83%89%E3%83%AA%E3%83%B3%E3%82%B0%E3%81%AE%E8%A8%98%E8%BF%B0)
    - [Face API を呼び出すロジックの記述](#face-api-%E3%82%92%E5%91%BC%E3%81%B3%E5%87%BA%E3%81%99%E3%83%AD%E3%82%B8%E3%83%83%E3%82%AF%E3%81%AE%E8%A8%98%E8%BF%B0)
    - [8 種類の表情スコアを回答にセットするロジックの記述](#8-%E7%A8%AE%E9%A1%9E%E3%81%AE%E8%A1%A8%E6%83%85%E3%82%B9%E3%82%B3%E3%82%A2%E3%82%92%E5%9B%9E%E7%AD%94%E3%81%AB%E3%82%BB%E3%83%83%E3%83%88%E3%81%99%E3%82%8B%E3%83%AD%E3%82%B8%E3%83%83%E3%82%AF%E3%81%AE%E8%A8%98%E8%BF%B0)
    - [BOT アプリケーションの最終動作確認](#bot-%E3%82%A2%E3%83%97%E3%83%AA%E3%82%B1%E3%83%BC%E3%82%B7%E3%83%A7%E3%83%B3%E3%81%AE%E6%9C%80%E7%B5%82%E5%8B%95%E4%BD%9C%E7%A2%BA%E8%AA%8D)
- [Appendix](#appendix)

この BOT アプリは Bot Framework Channel Emulator を使ってローカル環境で稼働確認することが可能です。
<img src="/media/20180313_34.PNG" width="500">


# Bot アプリケーションテンプレートのダウンロードと準備
Azure Portal で [1. Azure Portal から Azure Bot Service の作成 (C# 編)](EmotionBot201803HOL_CSharp01.md) で作成した Azure Bot Service を参照し、Web App Bot の設定ペインのメニューから **ビルド** をクリックします。
**zipファイルをダウンロード** をクリックすると、Visual Studio プロジェクトの形でダウンロードできます。

<img src="/media/20180313_10.PNG" width="500">

**zipファイルをダウンロード** が表示されたらクリックして、ソースコードをダウンロードします。

<img src="/media/20180313_16.PNG" width="500"><img src="/media/20180313_17.PNG" width="500">

**保存** をクリックして、ローカル環境にソースコードを保存します。

<img src="/media/20180313_18.PNG" width="500">

ダウンロードしたソースコード (Zip ファイル) を右クリックして、[すべて展開] を選択して展開します。

<img src="/media/20180313_19.PNG" width="250"><img src="/media/20180313_20.PNG" width="400">

展開して作成されたフォルダーを右クリックして、[プロパティ] を選択します。

<img src="/media/20180313_21.PNG" width="300">

[読み取り専用属性] をクリックして、選択なしに変更、[OK] をクリックして解除します。

<img src="/media/20180313_22.PNG" width="250">

[変更をこのフォルダー、サブフォルダーおよびファイルに適用する] を選択して [OK] をクリックします。

<img src="/media/20180313_23.PNG" width="250">

展開および読み取り専用属性を解除したソースコードフォルダーを開きます。Microsoft.Bot.Sample.SimpleEchoBot.sln をクリック、Visual Studio 2017 で Bot アプリケーションのソリューションを開きます。

<img src="/media/20180313_24.PNG" width="500">

警告が表示されたら [OK] をクリックして続けます。

<img src="/media/20180313_25.PNG" width="500">


# Cognitive Services Face API の C# ライブラリーのインストール
Visual Studio 2017 で Bot アプリケーションのソリューションが表示されたら、ローカル環境にないライブラリー および 
Cognitive Services Face API のライブラリーをインストールします。

<img src="/media/20180313_26.PNG" width="500">

ソリューションエクスプローラーでプロジェクト名 (ソリューションの下) を右クリックして、*NuGet パッケージの管理* を選択します。

<img src="/media/20180313_27.PNG" width="500">

[このソリューションに一部の NuGet パッケージが見つかりません．．．]　というメッセージが表示されたら、その横に表示される [復元] をクリックして復元します。

<img src="/media/20180313_28.PNG" width="500">

*参照* をクリックし、*project oxford face* と入力して検索します。*Microsoft.ProjectOxford.Face* を選択し、*インストール* をクリックしてインストールします。

<img src="/media/20180313_29.PNG" width="500">

Face API のライブラリーと、依存関係のあるライブラリーが合わせて表示されますので、*OK* をクリックしてインストールします。

<img src="/media/20180313_30.PNG" width="300"><img src="/media/20180313_31.PNG" width="300">

インストールが終了したら、NuGet のウインドウを閉じます。



# Bot の情報保持用ストレージ および Face API キーの設定
*Web.config* をクリックして表示し、Bot の情報保持用の Azure Storage の設定、および Face API のキー を設定を追加します。

<configration> のセクションから、以下のような <appSettings> セクションを探します。
```Web.config
<configuration>
   :(中略)
  <appSettings>
    <!-- update these with your Microsoft App Id and your Microsoft App Password-->
    <add key="MicrosoftAppId" value="" />
    <add key="MicrosoftAppPassword" value="" />
  </appSettings>
    :(後略)
```

<appSetting> セクションに、AzureWebStorage と FaceApiKey の設定を以下のように追加します。

```Web.config
<configuration>
   :(中略)
  <appSettings>
    <!-- update these with your Microsoft App Id and your Microsoft App Password-->
    <add key="MicrosoftAppId" value="" />
    <add key="MicrosoftAppPassword" value="" />
    <!-- ストレージの接続文字列 および Face API Key を入力 -->
    <add key="AzureWebJobsStorage" value="YOUR_STORAGE_CONNECTION_STRING" />
    <add key="FaceApiKey" value="YOUR_FACE_API_KEY" />
  </appSettings>
    :(後略)
```
- YOUR_STORAGE_CONNECTION_STRING
1. Azure Portal から Azure Bot Service の作成 (C# 編) の [Azure Bot Service アプリの Bot Channel 関連設定](https://github.com/ayako/AAJP-EmotionBotHoL/blob/master/EmotionBot201803HOL_CSharp01.md#azure-bot-service-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%AE-bot-channel-%E9%96%A2%E9%80%A3%E8%A8%AD%E5%AE%9A) で保存した AzureWebsStorage の設定文字列
- YOUR_FACE_API_KEY
[Cognitive Services の無料サブスクリプションの申し込み方法](CognitiveSubscriptionTrial.md) の手順で取得した Face API キー


Crtl+S または ツールバーから *ファイル* ＞ *(ソリューション名)の保存* をクリックして *Web.config* を保存します。


# 会話のハンドリングの記述
*Dialogs* フォルダー をクリックして開きます。*EchoDialog.cs* をクリックして表示し、こちらを編集していきます。

冒頭に、先ほど追加した Microsoft.ProjectOxford.Face を追加します。合わせて JSON を利用するためのライブラリーも追加しておきます。

```EchoDialog.cs
using Microsoft.ProjectOxford.Face;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
```

デフォルトで記述されている MessageReceivedAsync 部分を削除し、下記の通り置き換えます。
こちらには、ユーザーから Bot に対してメッセージ(画像などの添付ファイルを含む)が送られた時の処理を記載します。

```EchoDialog.cs
public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
{
    var message = await argument as Activity;

    // [ja] Botからの返答を設定 | attachment(画像) がない場合は初期メッセージ
    var result = $"表情判定 BOTです。\n 写真を送ると、写っている人の表情を判定します。";

    // [ja] attachment(画像) が送られてきた場合
    if (message.Attachments?.Count != 0)
    {
        // [ja] attachment の URL から画像を Stream として取得
        var photoUrl = message.Attachments[0].ContentUrl;
        var client = new HttpClient();
        var photoStream = await client.GetStreamAsync(photoUrl);

        // [ja] FaceAPI で表情を分析

        // [ja] 分析結果が取得できた場合
        try
        {
            // [ja] 各 Emotion のスコアを取得
            //// [ja] CASE_1: 笑顔判定
            //// [ja] CASE_2: 8 種類の表情判定
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
```

また、StartAsyncの直前にある会話カウントは使用しないため、削除します。
代わりに Web.comfig で設定した Face API キーを取得するコードを追加します。

```EchoDialog.cs
namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        //protected int count = 1;

        //[ja] Web.config で設定した Face API Key を取得
        readonly string faceApiKey = ConfigurationManager.AppSettings["FaceApiKey"];

        public async Task StartAsync(IDialogContext context)
        {
        :(後略)
```


EchoDialog.cs の末尾にある AfterResetAsync も使用しないため、削除します。

```EchoDialog.cs
//public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
//{
//    :(中略)
//}
```

Crtl+S または ツールバーから *ファイル* ＞ *(ソリューション名)の保存* をクリックして *EchoDialog.cs* を保存します。

## BOT の動作確認
ここで一旦 BOT の動作確認を行います。F5 または デバック＞デバックの開始 をクリックして、プロジェクトのビルドおよび起動を行います。ブラウザが起動して Bot Framework のデフォルト画面が表示されたら、Bot Framework Channel Emulator を起動してアクセスを行います。
Bot Framework Channel Emulator の上部中央にある *Bot Url* に、起動しているブラウザと同じ URL (デフォルトでは http://localhost:3984) に **/api/messages** を追加したアドレス (http://localhost:3984/api/messages) を指定します。

<img src="/media/20180313_32.PNG" width="300">

何か文字を入力して送信すると、指定した初期メッセージが返信されることを確認してください。

<img src="/media/20180313_33.PNG" width="500">


# Face API を呼び出すロジックの記述
先ほど設定した Face API Key を使って FaceServiceClient を作成、FaceServiceClient.DetectAsync で呼び出して、得られる結果を faceResult に取得します。
まず、faceResult に取得した Happoness のスコアを取得してメッセージに設定します。

```EchoDialog.cs
public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
{
    :(中略)
        // [ja] FaceAPI で表情を分析
        var faceClient = new FaceServiceClient(faceApiKey);
        var faceResult = await faceClient.DetectAsync(
                    photoStream,
                    returnFaceId: true,
                    returnFaceLandmarks: true,
                    returnFaceAttributes: Enum.GetValues(typeof(FaceAttributeType)).OfType<FaceAttributeType>().ToArray()
                    );

        // [ja] 分析結果が取得できた場合
        try
        {
            // [ja] 各 Emotion のスコアを取得
            var emotion = faceResult[0].FaceAttributes.Emotion;

            // [ja] 各 Emotion のスコアを取得
            // [ja] CASE_1: 笑顔判定
            var score = emotion.Happiness;
            result = $"この写真は 笑顔 " + (int)(score * 100) + "% です。";

            // [ja] CASE_2: 8 種類の表情判定

    :(後略)
}
```

## Bot アプリケーションの動作確認 (Happiness スコアの表示)
ここでもう一度 BOT の動作確認を行います。、F5 または デバック＞デバックの開始 をクリックして、プロジェクトのビルドおよび起動を行います。ブラウザが起動して Bot Framework のデフォルト画面が表示されたら、Bot Framework Channel Emulator からアクセスを行います。
画像アイコンをクリックしてローカルの画像を選択し、Bot に送信すると、Happiness のスコアが返答されるのを確認してください。

<img src="/media/20180313_34.PNG" width="500">


# 8 種類の表情スコアを回答にセットするロジックの記述
今度は得られた8種類の表情スコアを KeyValuePair に代入して、スコア数値が一番大きいものを取得して回答にセットします。先ほどの Happiness スコアの取得と表示はコメントアウトします。

```EchoDialog.cs
public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
{
    :(中略)

        // [ja] 分析結果が取得できた場合
        try
        {
            // [ja] 各 Emotion のスコアを取得
            var emotion = faceResult[0].FaceAttributes.Emotion;

            //// [ja] CASE_1: 笑顔判定
            //var score = emotion.Happiness;
            //result = $"この写真は 笑顔 " + (int)(score * 100) + "% です。";

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

    :(後略)
}
```


忘れずに *EchoDialog.cs* を保存しておきます。

# BOT アプリケーションの最終動作確認
F5 または デバック＞デバックの開始 をクリックして、プロジェクトのビルドおよび起動を行います。ブラウザが起動して Bot Framework のデフォルト画面が表示されたら、Bot Framework Channel Emulator を起動してアクセスを行います。
まず何かメッセージを入力すると、デフォルトの回答が返信されます。

その後、画像を送信して、一番スコアが大きい感情が回答として返信されれば、BOT アプリケーションは完成です。

<img src="/media/20180313_35.PNG" width="500">

# Appendix
こちらを Visual Studio から Azure Bot Service にデプロイ (アップロード) すると、作成した BOT を公開できます。

完成形のソースコードは GitHub にて公開しています。

https://github.com/ayako/AAJP-EmotionBotHoL/tree/master/source

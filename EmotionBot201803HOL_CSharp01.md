# Azure Antenna : Azure Bot Service ではじめるチャットボット開発ハンズオン
## Azure Bot Service & Cognitive Services Face API による表情分析チャットボット

# Azure Portal から Azure Bot Service の作成 (C# 編)

Azure Bot Service を利用すると、開発環境がなくてもブラウザーだけで＆1ステップでチャットボット (Chatbot, 以下 Bot) が作成できます。
以下の手順では、Azure Bot Service のテンプレートを用いて、ブラウザだけで簡単な "おうむ返し" を行う Bot を作成します。

- [Azure Bot Service について](#azure-bot-service-%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6)
    - Azure Bot Service とは
    - Azure Bot Service の仕組み
- [準備](#%E6%BA%96%E5%82%99)
- 手順
    - [Azure Portal から新規 Azure Bot Service アプリの作成](#azure-portal-%E3%81%8B%E3%82%89%E6%96%B0%E8%A6%8F-azure-bot-service-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%AE%E4%BD%9C%E6%88%90)
    - [Azure Bot Service アプリの設定確認&テスト](#azure-bot-service-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%AE%E8%A8%AD%E5%AE%9A%E7%A2%BA%E8%AA%8D%E3%83%86%E3%82%B9%E3%83%88)
    - [Azure Bot Service アプリのソースコード確認](#azure-bot-service-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%AE%E3%82%BD%E3%83%BC%E3%82%B9%E3%82%B3%E3%83%BC%E3%83%89%E7%A2%BA%E8%AA%8D)
    - [Azure Bot Service アプリで使用されているライブラリー群](#azure-bot-service-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%A7%E4%BD%BF%E7%94%A8%E3%81%95%E3%82%8C%E3%81%A6%E3%81%84%E3%82%8B%E3%83%A9%E3%82%A4%E3%83%96%E3%83%A9%E3%83%AA%E3%83%BC%E7%BE%A4)
    - [Azure Bot Service アプリの Bot Channel 関連設定](#azure-bot-service-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%AE-bot-channel-%E9%96%A2%E9%80%A3%E8%A8%AD%E5%AE%9A)

![](/media/20180313_09.PNG)


# Azure Bot Service について
## Azure Bot Service とは
Chatbot というのは、Web API であり、Web サービスのアプリケーション(Bot アプリ)の構築と、Bot アプリをホストする Web ホスティング環境 (Web サーバー) が必要です。Azure Bot Service では、予め用意されている Bot Framework による Bot アプリ のテンプレートを元に Bot アプリを作成し、Web ホストとして Azure App Service Web App または Azure Function を利用して Bot アプリをデプロイ(配置、公開)するサービスです。
また、Bot Channel と呼ばれる、Bot アプリに対して メッセージングツール (Microsoft Skype, Microsoft Teams, Slack, Facebook Messenger, ...) からメッセージを送受信できるインターフェースを備えています。

## Azure Bot Service の仕組み
Azure Bot service には下記2つの機能 (サービス) があり、一度の作成操作を行うだけで Bot アプリを作成＆公開できます。また、開発環境がなくても (Azure に用意されている) オンラインエディターを使ってコードを直接編集することも可能です。
これまで Bot Framework による Bot アプリを開発した際に、個別に必要であった作業や各種設定、登録を全て Azure Portal から自動で行うため、開発が簡単になりました。

### Bot アプリの作成＋ホストする機能 : Azure App Service (Web App) または Azure Function
Web サービスホストの機能を提供する Azure App Service Web App または Azure Function をホスト先として、Bot Framework ベースの Bot アプリの作成および配置を行います。

### Bot アプリを各種メッセージングサービスに接続する機能 : Bot Channel
Bot アプリの作成＆配置と同時に、メッセージングツールへのインターフェースとなる Bot Channel への登録をも行います。



# 準備
## Microsoft アカウント の取得
Azure サブスクリプション申し込みに必要です。(今回は Azure Bot Service 以外にも Cognitive Services Face API などを利用するうえで同じ１つのアカウントを利用することにします)
>[Microsoft アカウント登録手続き](https://www.microsoft.com/ja-jp/msaccount/signup/default.aspx)

## Azure サブスクリプション申し込み
上記↑で取得した Microsoft アカウントで申し込みを行います。(無料サブスクリプションをご利用いただけます)
>[Azure の無料サブスクリプションの申し込み方法](/AzureSubscriptionTrial.md)



# 手順
## Azure Portal から新規 Azure Bot Service アプリの作成
[Azure Portal](https://portal.azure.com) をブラウザーで開きます。
左端ナビゲーションバーの [＋リソースの新規作成] をクリックします。
![](/media/20180313_01.PNG)


検索欄に [bot] と入力します。
![](/media/20180313_02.PNG)

Web App Bot、Function Bot, Bot Channels Registration という同じ Bot マークのサービスが表示されます。
![](/media/20180313_03.PNG)

- Botの新規作成を行うには、Web App Bot (Azure Web App ベース)または Function Bot (Azure Function ベース)を選択します。
- 作成済みの Bot アプリ (Bot Framework のテンプレートなどを用いて作成したもの) を登録するには、Bot Channels Registration を選択します。

今回は **Web App Bot** を選択、Web App Bot ペインで [作成] をクリックします。

![](/media/20180313_04.PNG)


Bot Service ペインで設定内容を入力します。

- ボット名
    - Bot Channel への登録名 (BotID) になります。Bot Channel の中でユニークになる名前を設定します。
- リソースグループ
    - ここでは新規で作成していますが、作成済みのリソースグループを選択してもOKです。
- 場所
    - Botアプリをホストする Web App のロケーションを選択します。
- 価格レベル
    - ここではF0(無料のプラン)を選択します。
- アプリ名
    - ボット名と同じ名前が自動入力されます。BotアプリをホストするWebサーバーとしての名前になります。azurewebsites.net の中でユニークになる名前を設定します。
- ボットテンプレート
- クリックしてテンプレートを選択します。
    - C# と Node.js から選択できます。今回は C#、全ての Bot アプリのベースになる Basic を選択します。
    - LUIS や QnA Maker を選択すると、同じマイクロソフトアカウントに紐づいている LUiS App や QnA Maker App を簡単に選択＆設定できます。(他のアカウントに紐づけているものも手動で設定可能です)
- App Service Plan
    - 新規 (または作成済み) の Web App のプランを選択します。無料の Free Plan を利用可能です。

「Azure Storage」「Application Insights」「Microsoft アプリ ID とパスワード」は、デフォルトのままで構いません。

入力したら、[作成] をクリックして Bot アプリの作成を行います。

![](/media/20180313_05.PNG)


「展開が成功しました」というメッセージが表示されたら作成完了です。[リソースに移動] をクリックして、Bot アプリの設定確認とテストを行います。

![](/media/20180313_06.PNG)


## Azure Bot Service アプリの設定確認＆テスト
作成した Web App Bot の設定ペインが表示されます。
概要ペインにメッセージングエンドポイントが表示されています。Bot Framework Emulrator を使ってテスト行う場合は、こちらの URL にアクセスします。

![](/media/20180313_07.PNG)

設定ペインのメニューから Web チャットでテスト をクリックすると、作成した Bot アプリ の動作確認を行うことができます。

![](/media/20180520_02.PNG)

Basic テンプレートでは、入力した文字をそのまま返す (おうむ返し) が実装されています。また、会話数カウントも合わせて実装されており、**reset** と入力すると、カウントをリセットします。

![](/media/20180313_09.PNG)


## Azure Bot Service アプリのソースコード確認
作成された Bot アプリのソースコードを確認します。
Web App Bot の設定ペインのメニューから **ビルド** をクリックします。

![](/media/20180313_10.PNG)

**オンラインコードエディターを開く** をクリックすると、ブラウザーの別タブでオンラインエディターが表示されます。
![](/media/20180313_11.PNG)

>ローカル環境で確認したい場合は、**zipファイルをダウンロード** をクリックすると、Visual Studio プロジェクトの形でダウンロードできます。また、Visual Studio Team Service や Git を紐づけて、ソースコード更新→ Azure のデプロイ を自動化することもできます。

オンラインエディターでソースコードを確認します。WWWROOT > Dialogs > EchoDialog.cs の順にクリックして表示します。
おうむ返しの機能 および 会話カウント＆リセットの機能が EchoDialig.cs に実装されています。

![](/media/20180313_12.PNG)

```EchoDialig.cs
public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
{
    var message = await argument;

    // reset と入力された場合
    if (message.Text == "reset")
    {
        PromptDialog.Confirm(
            context,
            AfterResetAsync, //リセット処理
            "Are you sure you want to reset the count?",
            "Didn't get that!",
            promptStyle: PromptStyle.Auto);
    }
    // 通常入力の場合
    else
    {
        // count に会話カウントを追加、message.text で入力された文字を返答
        await context.PostAsync($"{this.count++}: You said {message.Text}");
        context.Wait(MessageReceivedAsync);
    }
}

public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
{
    var confirm = await argument;
    if (confirm)
    {
        this.count = 1;
        await context.PostAsync("Reset count.");
    }
    else
    {
        await context.PostAsync("Did not reset count.");
    }
    context.Wait(MessageReceivedAsync);
}
```

## Azure Bot Service アプリで使用されているライブラリー群
Bot アプリで使用されているライブラリーを確認しておきます。
オンラインエディターでソースコードを確認します。WWWROOT > package.config の順にクリックして表示します。

2018 年 3 月現在のバージョンは以下になっています。
- .NET Framework 4.6
- ASP.NET.WebAPI 5.2.3
- Bot Builder & Bot Connector v3.12
ほか、Bot アプリの構築に便利な Azure SDK が合わせて設定済みです。

![](/media/20180313_13.PNG)

## Azure Bot Service アプリの Bot Channel 関連設定
プレビュー時には Bot Channel に登録を行って発行される Bot ID, および Microsoft AppID, Password を手動でアプリ側に設定する必要がありました。Azure Bot Service GA 後、これらの作業はアプリ設定時に自動で行われるようになりましたので、操作は不要です。
オンラインエディターで WWWROOT > web.config を確認すると、"MicrosoftAppID" および "MicrosoftAppPassword" はブランクになっています。

![](/media/20180313_14.PNG)

これらは Bot アプリをホストしている Azure Web App 側に自動で設定されています。
Azure Bot Service のアプリケーション設定ペインで **アプリケーション設定** をクリックして表示します。*アプリ設定* の項目に BotID, MicrosoftAppId, MicrosoftAppPassword が自動で追加＆設定されており、値も確認できます。

次の [2. Azure Bot Service テンプレート を使った Bot Framework アプリの開発](EmotionBot201803HOL_CSharp02.md) で使用するために、**AzureWebJobsStorage** の横に表示されている文字列 (DefaultEndpointsProtrol=https://~ で始まる文字列全部) をコピーして保存しておきます。

![](/media/20180313_15.PNG)

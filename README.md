# Azure Antenna : Azure Bot Service ではじめるチャットボット開発ハンズオン

## Azure Bot Service & Cognitive Services Face API による表情分析チャットボット

Azure Bot Service および Cognitive Services Face API を利用して、写真から人間の表情を分析するチャットボットの開発をお試しいただくハンズオン ラーニングです。

<img src="/media/20180313_34.PNG" width="500">

## 準備事項
このハンズオン ラボを行うには、以下をご準備ください。

- Azure および Cognitive Servces Face API のサブスクリプション
  - [Azure の無料サブスクリプションの申し込み方法](/AzureSubscriptionTrial.md) の手順で無料サブスクリプションをお申込みいただけます。
  - [Cognitive Services 無料サブスクリプションの申し込み方法](/CognitiveSubscriptionTrial.md) の手順で Face API をお申し込みいただき、エンドポイント(URL) と キー を取得してください。

- 開発環境
  - C# をご利用の場合 : [Visual Studio 2017 Community](https://www.visualstudio.com/vs/) 以上
  - Node.js をご利用の場合 : コードエディター ([Visual Studio Code](https://code.visualstudio.com/) など) 、および [Node.js](https://nodejs.org/ja/) ランタイムがインストールされていること

- [Bot Framework Emulator](https://emulator.botframework.com/)
  - チャットボットのテストに使用するクライアント。ローカル環境でチャットボットのテストを実施できます。(Windows/Mac/Linux)


## ハンズオン

### Azure Bot Service の作成 (C# 編)
- [1. Azure Portal から Azure Bot Service の作成 (C# 編)](EmotionBot201803HOL_CSharp01.md)
- [2. Azure Bot Service テンプレート を使った Bot Framework アプリの開発 (C# 編)](EmotionBot201803HOL_CSharp02.md)

### Azure Bot Service の作成 (Node.js 編)
- [1. Azure Portal から Azure Bot Service の作成 (Node.js 編)](EmotionBot201803HOL_NodeJS01.md)
- [2. Azure Bot Service テンプレート を使った Bot Framework アプリの開発 (Node.js 編)](EmotionBot201803HOL_NodeJS02.md)

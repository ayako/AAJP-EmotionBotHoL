# Cognitive Services の無料サブスクリプションの申し込み方法
Microsoft Cognitive Services は 画像、文章、言語、情報を処理する機能を API 経由で利用できるサービスです。
Cognitive Services は無料で試用できますが、予めサブスクリプション申し込みが必要です。以下、Cognitive Services のサブスクリプションの申し込み手順を紹介します。

方法は2通りあります。

**1. 30日間無料版**
- [Cognitive Services の 試用申し込みサイト](https://azure.microsoft.com/ja-jp/try/cognitive-services/) から申込
- Microsoft / LinkedIn / GitHub / Facebook いずれかのアカウント に紐づけ

**2. Free Tier(F0): (当面)無料の料金プラン**
- [Azure Portal](https://portal.azure.com/) から申込
- Azure サブスクリプション & Microsoft アカウント に紐づけ

とにかく手軽に使ってみたい方は **1. 30日間無料版** を、これまでに Azure を使ったことがあって Azure サブスクリプションを持っている方は **2. Free Tier(F0)** がおススメです。


# 1. 30日間無料版の申し込み方法

## Microsoft アカウント
サブスクリプションの申し込みに必要となりますので、持っていない場合は取得しておきます。

>[Microsoft アカウント登録手続き](https://www.microsoft.com/ja-jp/msaccount/signup/default.aspx)

## Cognitive Services サブスクリプション申し込み

ブラウザから検索エンジンで [Try Cognitive] と検索し、*Cognitive Service Try experience | Microsoft Azure* というページを開きます。

![](/media/20170520_11.PNG)

*Cognitive Services を試す* というページで、*Emotion API* の行にある [作成] をクリックします。

![](/media/20170520_12.PNG)

サービス要件を確認して ☑ (チェック) をつけ、国/地域 を選択、[次へ] をクリックします。

![](/media/20170520_13.PNG)

[Microsoft] をクリックして、Microsoft アカウントでサインインします。

![](/media/20170520_14.PNG)

>サブスクリプション申し込み時に Email Verification (認証) が必要になる場合があります。メールに記載されている URL をクリックして認証が完了すると、次のステップに進めるようになります。

><img width="300" src="/media/20161203_02.PNG">


*正常にサブスクリプションに追加しました* と表示されたら完了です。
*キー1* に記載されている文字列が サブスクリプションキー になります。

![](/media/20170520_07.PNG)


# 2. Azure Portal から Free Tier(F0) 無料プラン の申し込み方法

## Azure サブスクリプション
サブスクリプションの申し込みに必要となりますので、持っていない場合は取得しておきます。

>[Azure の無料サブスクリプションの申し込み方法](AzureSubscriptionTrial.md)


## Azure Portal から Cognitive Services APIs のサービス作成

[Azure Portal](https://portal.azure.com/) にアクセスし、有効なサブスクリプションが紐づけられているアカウントでサインインします。

左バーの [+新規] をクリックし、*新規作成* パネルから [AI+Cognitive Services >] をクリックしてAPIを表示します。利用したい API が表示されない場合は、*すべて表示* をクリックします。

![](/media/20170613_01.PNG)

表示された API、または 検索を利用して 利用したい API を表示させます。

![](/media/20170613_02.PNG)

今回は以下の手順で Emotion API を申し込みます。(他の API も手順は同じです。)
Emotion API を選択して、*Emotion API* パネルにある [作成] をクリックします。

![](/media/20170613_03.PNG)

*Emotion API 作成* パネルで、名前 (EmotionAPIxxxなど認識しやすいもの) を入力し、お持ちの*サブスクリプション* (自動入力)、必要に応じて *場所* (Azure データセンターの拠点)を選択します。*価格レベル* はF0 (無料版) を選択します。*リソースグループ* は新規作成 or 既にお持ちのグループをご利用いただいて構いません。
サービス要件を確認して ☑ (チェック) をつけ、[作成] をクリックすると、サービスが作成されます。

![](/media/20170613_04.PNG)

作成されたら、左バーの *リソースグループ* からサービスを作成したリソースグループを選択して、作成したサービスを表示します。サービス名をクリックすると詳細が表示されます。

![](/media/20170613_05.PNG)

*エンドポイント* に記載されている URL をローカルに保存しておきます。
左列の *リソース管理* の欄にある [キー] をクリックします。

![](/media/20170613_06.PNG)

*キー1* に記載されている文字列がそれぞれの API キー になります。こちらをコピーしてローカルに保存しておきます。

![](/media/20170613_07.PNG)

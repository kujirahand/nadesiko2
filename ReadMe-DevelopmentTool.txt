■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
■               なでしこ２の開発環境の構築方法について
■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

なでしこ２の開発を、オープンソースのSharpDevelopにすることにしました。
オープンソースの開発をするのでできるだけオープンな方が良いだろうという判断です。

開発環境のダウンロードは以下：

- SharpDevelop
- http://sharpdevelop.codeplex.com/releases

リポジトリは以下：

- なでしこ2 (GoogleCode)
-- http://code.google.com/p/nadesiko2/

- ソースだけ見るなら、以下のＵＲＬで見られます！
- http://code.google.com/p/nadesiko2/source/browse/

以下の手順に沿って作業すると、
バージョン管理ツールで、なでしこのソースを編集できます。

なでしこ２開発環境のセットアップ方法は、次の通り。

- (1) SharpDevelopをインストール
-- http://sharpdevelop.codeplex.com/releases
- (2) NUnit 2.5.7以降をインストール (SharpDevelopに含まれてるかも?!要確認)
-- http://www.nunit.org/index.php?p=download
- (3) TortoiseSVN をインストール (SharpDevelopに含まれてるかも?!要確認)
-- http://sourceforge.jp/projects/tortoisesvn/
- (4) SVNで、以下のリポジトリをチェックアウトする
-- https://nadesiko2.googlecode.com/svn/trunk/

プロジェクトのセットアップ方法

- (5) SharpDevelop で取得したアーカイブの Nako2ForSharpDevelop.sln を開く
- (6) 実行ボタンをクリック



～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
あるいは・・・

●Visual Studio 2010 Express 日本語版
http://www.microsoft.com/japan/msdn/vstudio/express/

TestNako プロジェクトの参照設定で nunit.framework に▲で警告マークがついているなら、一度、これを削除して、改めて NUnit を追加し直します。
～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～



私のGitHubをご覧いただきありがとうございます。
このレポジトリは私が研究で行っている津波避難シミュレーションを構成するソースコード等を入れたものになっています。

【プログラムの説明】
1.csvファイルからスタート地点、ゴール地点候補の座標をそれぞれ読み込ます。
2.同様に道路の情報(点情報であるノードとノード間を繋ぐリンク)をcsvファイルから読み込みます。
2.各スタート地点から一番近いゴール地点を直線距離から求めます。
3.求めたゴール地点を終端とし、スタート地点からゴール地点までの最短経路探索を行います。
4.あらかじめ決められた避難開始時間と共に車を発生させ、3で求められた経路を基に避難を行います。
5.ゴール地点に到着したら避難完了とします。
6.Unity上の結果出力ボタンを押すとその時点での避難完了した車の情報がcsvファイルに出力されます。

【動作環境の構築方法】
1.Car-Simulation内にあるファイルをフォルダごとダウンロードします。
2.[Assets]内にある[Version1.unity]を開きます。
3.Unity上で再生ボタンを押すとプログラムが動作します。

【プログラムの改善すべき点】
元々、別の言語で書かれたプログラムで最短経路探索を行い、入力データとしてシミュレーション実行していました。
しかし、将来的にはシミュレーション実行中に再度最短経路探索を行いため、最短経路探索部分の組み込みを行っています。（現段階）
今はテストの為、簡易的なスタート、ゴール地点、リンク、ノードの情報を使っています。
将来的には研究対象地のデータに置き換える予定です。

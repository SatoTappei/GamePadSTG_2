★目標
	非同期処理、Task、async/awaitの勉強
	UniTaskの導入
	UniRxの導入
	コントローラーで動かすゲームの作成
	SAのカレッジで教わる技術を詰め込む
		Stateパターンを使った敵の制御
		オブジェクトプーリングを使った大量のオブジェクトの処理
		FactoryMethodパターンを使用した敵の生成
		Flyweightパターンを使った???
	3Dゲームの基本である、3人称視点カメラのアクションゲーム
	館のようなステージの自動生成

★タスク
	優先:UniRxを使用した箇所、プレイヤー周りやUI、マネージャーでDisposeの処理を書いていないので書く。

★備考
	URP導入済み
	カメラのガクガクを減らすためにFixedTimestepをデフォルトの0.002から0.0167に変更
	ヒエラルキーの並び順上から
		カメラ
		DirectionalLight
		Managers
		UICanvas
		EventSystem
		実体のあるもの
		シングルトンDDOL
	プレイヤーの移動方法の実装に関して、RigidBodyを使用した移動で進めていく。
		Rayを飛ばすやり方だとジャンプや崖から落ちたときの挙動が、現状だと作れないため

★導入アセット
	Newbie & Friends
	AllSkyFree

★外部素材
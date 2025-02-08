using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// ゲーム管理
/// </summary>
public class GameMainSystem : MainScriptBase
{
    /// <summary>インスタンス</summary>
    public static GameMainSystem Instance { get; private set; }

    #region 定数

    #endregion

    #region 変数・メンバー

    #region UI

    /// <summary>HPゲージ</summary>
    public UIHpGauge ui_hp;

    /// <summary>Exp</summary>
    public TMP_Text txt_exp;

    /// <summary>FPS表示</summary>
    public TMP_Text txt_fps;

    /// <summary>メニュー</summary>
    public UIInGameMenu inGameMenu;

    #endregion

    /// <summary>ゲームパラメータ</summary>
    public GameParameter prm_Game { get; private set; }
    /// <summary>プレイヤーパラメータ</summary>
    public PlayerParameter prm_Player { get; private set; }

    private Vector2Int player_loc;
    /// <summary>プレイヤー</summary>
    public PlayerScript playerScript { get; set; }

    /// <summary>ゲーム状態</summary>
    public enum GameState
    {
        Loading = 0,
        Active,
        Menu,
        Exiting,
    }
    public GameState state { get; private set; } = GameState.Loading;

    /// <summary>雑魚敵を持つ空オブジェクト</summary>
    public Transform smallEnemyParent;

    /// <summary>攻撃を持つ空オブジェクト</summary>
    public Transform attackParent;

    /// <summary>プレイヤーについて動く空オブジェクト</summary>
    public Transform playerBodyParent
    {
        get
        {
            return playerScript?.body_parent;
        }
    }

    /// <summary>武器管理</summary>
    public WeaponManager weaponManager { get; private set; }

    /// <summary>くじ引き管理</summary>
    public TreasureManager treasureManager { get; private set; }

    /// <summary>経過時間</summary>
    private float inGameTime;

    #endregion

    #region 基底

    /// <summary>コンストラクタでインスタンス設定</summary>
    public GameMainSystem() { Instance = this; }

    /// <summary>消滅時にインスタンスnull</summary>
    private void OnDestroy() { Instance = null; }

    /// <summary>
    /// 開始前
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();
        var manager = ManagerSceneScript.GetInstance();

        // パラメータ作成
        weaponManager = new WeaponManager();
        treasureManager = new TreasureManager();
        prm_Game = new GameParameter();
        prm_Game.InitParam();
        prm_Player = new PlayerParameter();
        prm_Player.Init();

        UpdateExpUI();

        // 初期配置フィールド読み込み
        RefreshFieldCell(true);
        // 読み込み待ち
        yield return new WaitWhile(() => manager.IsLoadingSubScene());

        //todo:キャラクター読み込み
        manager.LoadSubScene("GameSceneDrows", 0, 0);
        // 読み込み待ち
        yield return new WaitWhile(() => manager.IsLoadingSubScene());
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);
        state = GameState.Active;

        StartCoroutine(UpdateCoroutine());

        //todo:x秒毎にFPS表示
        StartCoroutine(Test_DisplayFPS());

        // 開始時刻
        inGameTime = 0f;
    }

    /// <summary>
    /// FPS表示てすと
    /// </summary>
    /// <returns></returns>
    private IEnumerator Test_DisplayFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            var fps = Mathf.Round(100.0f / Time.deltaTime) / 100f;
            txt_fps.SetText(fps.ToString());
        }
    }

    #endregion

    #region メイン処理
    /// <summary>
    /// メイン処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCoroutine()
    {
        var origin = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        while (true)
        {
            yield return null;
            inGameTime += origin.inGameDeltaTime;

            RefreshFieldCell();
        }

        //state = GameState.Exiting;
        //todo:ゲーム終了
    }
    #endregion

    #region フィールドマス管理

    /// <summary>
    /// フィールド生成
    /// </summary>
    /// <param name="init">初期化時</param>
    private void RefreshFieldCell(bool init = false)
    {
        var manager = ManagerSceneScript.GetInstance();

        if (init || (playerScript == null))
        {
            player_loc = Vector2Int.zero;
        }
        else
        {
            // プレイヤーの現在位置を取得
            var nowPos = playerScript.transform.position;
            var nowLoc = FieldUtil.GetFieldLoc(nowPos.x, nowPos.z);
            if (player_loc == nowLoc) return; // 更新の必要なし
            player_loc = nowLoc;
        }

        // 現在の位置から距離３以上のフィールドを破棄
        var subList = manager.GetSubSceneList();
        var releaseList = new List<GameFieldSystem>();
        foreach (var sub in subList)
        {
            if (sub is not GameFieldSystem) continue;
            var fld = sub as GameFieldSystem;
            var dist = FieldUtil.CalcLocationDistance(fld.fieldCell, player_loc);
            if (dist <= 2) continue;

            releaseList.Add(fld);
        }
        foreach (var fld in releaseList)
        {
            fld.ReleaseField();
        }

        // ローディング中のもの
        var loadingSubList = manager.GetSubSceneLoadingList();

        // 現在の位置から距離１までのフィールドを読み込み
        var createLocList = FieldUtil.GetAroundLocations(player_loc);
        //var createLocList = new List<Vector2Int>();
        createLocList.Add(player_loc);
        foreach (var loc in createLocList)
        {
            // すでにあったら無視
            if (subList.Any(sub =>
                sub is GameFieldSystem && (sub as GameFieldSystem).fieldCell == loc)
            )
                continue;
            if (loadingSubList.Any(sub => sub.prmList.Count == 2 &&
                sub.prmList[0] == loc.x && sub.prmList[1] == loc.y))
                continue;

            // 読み込み
            if (init)
                manager.LoadSubScene("GameSceneField01", loc.x, loc.y);
            else
            {
                var randomScenes = new List<string>()
                {
                    "GameSceneField01",
                    "GameSceneField_polygon1",
                    "GameSceneField_eye1",
                    "GameSceneField_bakyura1",
                    "GameSceneField_willy1",
                };

                var idx = Util.RandomInt(0, randomScenes.Count - 1);
                manager.LoadSubScene(randomScenes[idx], loc.x, loc.y);
            }
        }
    }

    #endregion

    #region 取得系

    public Vector3 GetPlayerCenter()
    {
        if (playerScript)
        {
            return playerScript.GetComponent<Collider>().bounds.center;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 敵の強さレート
    /// </summary>
    /// <returns></returns>
    public float GetEnemyRate()
    {
        //todo:正式な敵の強さ計算
        // 60秒間は固定
        if (inGameTime <= 60f) return 1f;

        // 300秒で＋100％
        return 1f + (inGameTime - 60f) / 300f;
    }

    /// <summary>
    /// ボスの強さレート
    /// </summary>
    /// <returns></returns>
    public float GetBossRate()
    {
        // 300秒間は固定
        if (inGameTime <= 300f) return 2f;

        // 500秒で＋100％
        return 2f + (inGameTime - 300f) / 500f;
    }

    #endregion

    #region その他

    /// <summary>
    /// メニューを開く
    /// </summary>
    public void OpenMenu()
    {
        state = GameState.Menu;
        inGameMenu.Open();

        StartCoroutine(WaitMenu());
    }

    /// <summary>
    /// メニュー閉じ待ち
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitMenu()
    {
        yield return new WaitWhile(() => inGameMenu.isActive);
        state = GameState.Active;
    }

    /// <summary>
    /// 経験値追加
    /// </summary>
    /// <param name="e"></param>
    public void AddExp(int e)
    {
        prm_Game.Exp += e;
        UpdateExpUI();
    }

    /// <summary>
    /// 経験値最新表示
    /// </summary>
    public void UpdateExpUI()
    {
        txt_exp.SetText(prm_Game.Exp.ToString());
    }

    #endregion
}

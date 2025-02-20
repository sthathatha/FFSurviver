using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム管理
/// </summary>
public class GameMainSystem : MainScriptBase
{
    /// <summary>インスタンス</summary>
    public static GameMainSystem Instance { get; private set; }

    #region 定数

    /// <summary>ザコ敵周期</summary>
    private const float SMALL_ENEMY_INTERVAL = 2f;

    /// <summary>ｘ個範囲内に１個だけ水フィールド</summary>
    private const int WATER_FIELD_INTERVAL = 2;//todo:水フィールド密度

    /// <summary>鏡の小怪物をｘ体倒すとボス出現</summary>
    private const int MIRROR_BOSS_BEATS = 10; //200;

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

    /// <summary>ザコテンプレート</summary>
    public EnemyScriptBase enemy_Eye1;
    /// <summary>ザコテンプレート</summary>
    public EnemyScriptBase enemy_Polygon1;
    /// <summary>ザコテンプレート</summary>
    public EnemyScriptBase enemy_Bakyura1;
    /// <summary>ザコテンプレート</summary>
    public EnemyScriptBase enemy_Willy1;

    /// <summary>鏡の怪物</summary>
    public BossScriptBase boss_Mirror;
    /// <summary>花の怪物</summary>
    public BossScriptBase boss_Flower;
    /// <summary>水の怪物</summary>
    public BossScriptBase boss_Water;
    /// <summary>月の怪物</summary>
    public BossScriptBase boss_Moon;
    /// <summary>つくよみちゃん</summary>
    public BossScriptBase boss_Tukuyomi;

    /// <summary>現在出現中のボス</summary>
    private BossScriptBase boss_Active;

    /// <summary>鏡の小怪物撃破数</summary>
    private int mirror_BeatCount = 0;
    /// <summary>の怪物出現フラグ</summary>
    public BossPopFlag bossPop_Mirror { get; private set; }
    /// <summary>鏡の怪物出現フラグ</summary>
    public BossPopFlag bossPop_Flower { get; private set; }
    /// <summary>花の怪物出現フラグ</summary>
    public BossPopFlag bossPop_Water { get; private set; }
    /// <summary>水の怪物出現フラグ</summary>
    public BossPopFlag bossPop_Moon { get; private set; }
    /// <summary>月の怪物出現フラグ</summary>
    public BossPopFlag bossPop_Tukuyomi { get; private set; }

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

    /// <summary>ボスを持つ空オブジェクト</summary>
    public Transform bossEnemyParent;

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

    /// <summary>雑魚敵出現判定用</summary>
    private float enemyControlTime;

    /// <summary>水フィールド位置</summary>
    private int waterFieldNum;

    /// <summary>地面に立っているか（空中ブロックに居る時false）</summary>
    public bool isStandingBase { get; set; }

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

        bossPop_Mirror = new BossPopFlag();
        bossPop_Flower = new BossPopFlag();
        bossPop_Water = new BossPopFlag();
        bossPop_Moon = new BossPopFlag();
        bossPop_Tukuyomi = new BossPopFlag();

        waterFieldNum = Util.RandomInt(0, WATER_FIELD_INTERVAL - 1);
        isStandingBase = true;

        UpdateExpUI();

        // カメラリセット
        manager.GetCamera3D().SetRotateTime(new Vector3(0, -0.15f, 1f));

        // 初期配置フィールド読み込み
        RefreshFieldCell(true);
        // 読み込み待ち
        yield return new WaitWhile(() => manager.IsLoadingSubScene());

        // キャラクター読み込み
        var sel = GameConstant.GetTempPID();
        if (sel == GameConstant.PlayerID.Drows) manager.LoadSubScene("GameSceneDrows", 0, 0);
        else if (sel == GameConstant.PlayerID.Eraps) manager.LoadSubScene("GameSceneEraps", 0, 0);
        else if (sel == GameConstant.PlayerID.Exa) manager.LoadSubScene("GameSceneExa", 0, 0);
        else if (sel == GameConstant.PlayerID.Worra) manager.LoadSubScene("GameSceneWorra", 0, 0);
        else if (sel == GameConstant.PlayerID.Koob) manager.LoadSubScene("GameSceneKoob", 0, 0);
        else manager.LoadSubScene("GameSceneYou", 0, 0);
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

        // 開始時刻
        inGameTime = 0f;
        enemyControlTime = 0f;

        StartCoroutine(UpdateCoroutine());

        //todo:x秒毎にFPS表示
        StartCoroutine(Test_DisplayFPS());
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
        var origin = OriginManager.Instance;

        while (playerScript.gameObject.activeInHierarchy)
        {
            inGameTime += origin.inGameDeltaTime;

            RefreshFieldCell();
            SmallEnemyControl(origin.inGameDeltaTime);
            BossPopControl();
            yield return null;
        }

        var manager = ManagerSceneScript.GetInstance();
        ManagerSceneScript.GetInstance().LoadMainScene("TitleScene", 0);
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
            else if (IsWaterField(loc.x, loc.y))
            {
                // 水
                manager.LoadSubScene("GameSceneFieldWater", loc.x, loc.y);
            }
            else
            {
                var randomScenes = new List<string>()
                {
                    "GameSceneField01",
                    "GameSceneField02",
                };

                var idx = Util.RandomInt(0, randomScenes.Count - 1);
                manager.LoadSubScene(randomScenes[idx], loc.x, loc.y);
            }
        }
    }

    /// <summary>
    /// 水フィールド判定
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsWaterField(int x, int y)
    {
        var yMod = y % WATER_FIELD_INTERVAL;
        if (yMod < 0) yMod += WATER_FIELD_INTERVAL;
        var xyMod = (x + y + y / WATER_FIELD_INTERVAL) % WATER_FIELD_INTERVAL;
        if (xyMod < 0) xyMod += WATER_FIELD_INTERVAL;

        return yMod == waterFieldNum && xyMod == 0;
    }

    #endregion

    #region ザコ敵管理

    /// <summary>
    /// 雑魚敵管理
    /// </summary>
    private void SmallEnemyControl(float delta)
    {
        if (!isStandingBase) return;

        enemyControlTime -= delta;
        if (enemyControlTime <= 0)
        {
            var max = inGameTime switch
            {
                > 480f => 3,
                > 50f => 2,
                _ => 1,
            };
            StartCoroutine(SmallEnemyPopCoroutine(Util.RandomInt(0, max)));
            enemyControlTime = SMALL_ENEMY_INTERVAL;
        }
    }

    /// <summary>
    /// 雑魚敵出現処理
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator SmallEnemyPopCoroutine(int type)
    {
        int popCount = Util.RandomInt(10, 20);
        var rotQ = Quaternion.Euler(0, Util.RandomFloat(0, 359f), 0);
        var baseDir = new Vector3(0, 0, FieldUtil.ENEMY_POP_DISTANCE);
        var center = GetPlayerCenter() + rotQ * baseDir;

        for (var i = 0; i < popCount; ++i)
        {
            // キャラ生成
            var rand = new Vector3(Util.RandomFloat(-10f, 10f), 0f, Util.RandomFloat(-10f, 10f));
            var enm = type switch
            {
                0 => Instantiate(enemy_Bakyura1, smallEnemyParent),
                1 => Instantiate(enemy_Eye1, smallEnemyParent),
                2 => Instantiate(enemy_Polygon1, smallEnemyParent),
                _ => Instantiate(enemy_Willy1, smallEnemyParent),
            };
            var p = center + rand;
            p.y = enm.transform.position.y;
            enm.transform.position = p;
            enm.transform.rotation = rotQ;
            enm.gameObject.SetActive(true);

            yield return null;
        }
    }

    #endregion

    #region ボス管理

    #region フラグ管理クラス

    /// <summary>
    /// ボスフラグ管理
    /// </summary>
    public class BossPopFlag
    {
        private bool canPopFlg = false;
        private bool popedFlg = false;

        /// <summary>出現しろフラグセット</summary>
        public void SetPop() { canPopFlg = true; }
        /// <summary>出現するべきか判定</summary>
        public bool WantPop() { return canPopFlg && !popedFlg; }
        /// <summary>出現済みフラグセット</summary>
        public void SetPoped() { popedFlg = true; }
    }

    #endregion

    /// <summary>
    /// 鏡の小怪物撃破
    /// </summary>
    public void AddMirrorBeat()
    {
        if (mirror_BeatCount < MIRROR_BOSS_BEATS)
        {
            mirror_BeatCount++;
            if (mirror_BeatCount == MIRROR_BOSS_BEATS) bossPop_Mirror.SetPop();
        }
    }

    /// <summary>
    /// 水の怪物、高いところから着地チェック
    /// </summary>
    /// <param name="pos"></param>
    public void HighJumpGrounding(Vector3 pos)
    {
        var nowLoc = FieldUtil.GetFieldLoc(pos.x, pos.z);
        if (IsWaterField(nowLoc.x, nowLoc.y))
        {
            // 水ならフラグオン
            bossPop_Water.SetPop();
        }
    }

    /// <summary>
    /// ボス撃破
    /// </summary>
    public void ActiveBossDefeat()
    {
        boss_Active = null;
    }

    /// <summary>
    /// ボス出現管理
    /// </summary>
    private void BossPopControl()
    {
        if (boss_Active != null) return;

        if (bossPop_Mirror.WantPop())
        {
            // 鏡の怪物
            boss_Active = Instantiate(boss_Mirror, bossEnemyParent);
            boss_Active.gameObject.SetActive(true);
            bossPop_Mirror.SetPoped();
        }
        else if (bossPop_Flower.WantPop())
        {
            //todo:花の怪物
        }
        else if (bossPop_Water.WantPop())
        {
            // 水の怪物
            boss_Active = Instantiate(boss_Water, bossEnemyParent);
            boss_Active.gameObject.SetActive(true);
            bossPop_Water.SetPoped();
        }
        else if (bossPop_Moon.WantPop())
        {
            //todo:月の怪物
        }
        else if (bossPop_Tukuyomi.WantPop())
        {
            //todo:つくよみちゃん
        }
    }

    #endregion

    #region 取得系

    /// <summary>
    /// プレイヤーの中心座標
    /// </summary>
    /// <returns></returns>
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
        const float b = 1f;
        const float x = 30f;
        const float y = 60f;

        // x秒間は固定
        if (inGameTime <= x) return b;

        // y秒で＋100％
        return b + (inGameTime - x) / y;
    }

    /// <summary>
    /// ボスの強さレート
    /// </summary>
    /// <returns></returns>
    public float GetBossRate()
    {
        const float b = 3f;
        const float x = 100f;
        const float y = 40f;
        // x秒間は固定
        if (inGameTime <= x) return b;

        // x秒で＋100％
        return b + (inGameTime - x) / y;
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

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void WaitGameover()
    {
        state = GameState.Exiting;
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// �Q�[���Ǘ�
/// </summary>
public class GameMainSystem : MainScriptBase
{
    /// <summary>�C���X�^���X</summary>
    public static GameMainSystem Instance { get; private set; }

    #region �萔

    #endregion

    #region �ϐ��E�����o�[

    #region UI

    /// <summary>HP�Q�[�W</summary>
    public UIHpGauge ui_hp;

    /// <summary>Exp</summary>
    public TMP_Text txt_exp;

    /// <summary>FPS�\��</summary>
    public TMP_Text txt_fps;

    /// <summary>���j���[</summary>
    public UIInGameMenu inGameMenu;

    #endregion

    /// <summary>�Q�[���p�����[�^</summary>
    public GameParameter prm_Game { get; private set; }
    /// <summary>�v���C���[�p�����[�^</summary>
    public PlayerParameter prm_Player { get; private set; }

    private Vector2Int player_loc;
    /// <summary>�v���C���[</summary>
    public PlayerScript playerScript { get; set; }

    /// <summary>�Q�[�����</summary>
    public enum GameState
    {
        Loading = 0,
        Active,
        Menu,
        Exiting,
    }
    public GameState state { get; private set; } = GameState.Loading;

    /// <summary>�G���G������I�u�W�F�N�g</summary>
    public Transform smallEnemyParent;

    /// <summary>�U��������I�u�W�F�N�g</summary>
    public Transform attackParent;

    /// <summary>�o�ߎ���</summary>
    private float inGameTime;

    #endregion

    #region ���

    /// <summary>�R���X�g���N�^�ŃC���X�^���X�ݒ�</summary>
    public GameMainSystem() { Instance = this; }

    /// <summary>���Ŏ��ɃC���X�^���Xnull</summary>
    private void OnDestroy() { Instance = null; }

    /// <summary>
    /// �J�n�O
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();
        var manager = ManagerSceneScript.GetInstance();

        // �p�����[�^�쐬
        prm_Game = new GameParameter();
        prm_Game.InitParam();
        prm_Player = new PlayerParameter();
        prm_Player.Init();

        UpdateExpUI();

        // �����z�u�t�B�[���h�ǂݍ���
        RefreshFieldCell(true);
        // �ǂݍ��ݑ҂�
        yield return new WaitWhile(() => manager.IsLoadingSubScene());

        //todo:�L�����N�^�[�ǂݍ���
        manager.LoadSubScene("GameSceneDrows", 0, 0);
        // �ǂݍ��ݑ҂�
        yield return new WaitWhile(() => manager.IsLoadingSubScene());
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);
        state = GameState.Active;

        StartCoroutine(UpdateCoroutine());

        //todo:x�b����FPS�\��
        StartCoroutine(Test_DisplayFPS());

        // �J�n����
        inGameTime = 0f;
    }

    /// <summary>
    /// FPS�\���Ă���
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

    #region ���C������
    /// <summary>
    /// ���C������
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
        //todo:�Q�[���I��
    }
    #endregion

    #region �t�B�[���h�}�X�Ǘ�

    /// <summary>
    /// �t�B�[���h����
    /// </summary>
    /// <param name="init">��������</param>
    private void RefreshFieldCell(bool init = false)
    {
        var manager = ManagerSceneScript.GetInstance();

        if (init || (playerScript == null))
        {
            player_loc = Vector2Int.zero;
        }
        else
        {
            // �v���C���[�̌��݈ʒu���擾
            var nowPos = playerScript.transform.position;
            var nowLoc = FieldUtil.GetFieldLoc(nowPos.x, nowPos.z);
            if (player_loc == nowLoc) return; // �X�V�̕K�v�Ȃ�
            player_loc = nowLoc;
        }

        // ���݂̈ʒu���狗���R�ȏ�̃t�B�[���h��j��
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

        // ���[�f�B���O���̂���
        var loadingSubList = manager.GetSubSceneLoadingList();

        // ���݂̈ʒu���狗���P�܂ł̃t�B�[���h��ǂݍ���
        var createLocList = FieldUtil.GetAroundLocations(player_loc);
        //var createLocList = new List<Vector2Int>();
        createLocList.Add(player_loc);
        foreach (var loc in createLocList)
        {
            // ���łɂ������疳��
            if (subList.Any(sub =>
                sub is GameFieldSystem && (sub as GameFieldSystem).fieldCell == loc)
            )
                continue;
            if (loadingSubList.Any(sub => sub.prmList.Count == 2 &&
                sub.prmList[0] == loc.x && sub.prmList[1] == loc.y))
                continue;

            // �ǂݍ���
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

    #region �擾�n

    public Vector3 GetPlayerCenter()
    {
        if (playerScript) return playerScript.gameObject.transform.position;

        return Vector3.zero;
    }

    /// <summary>
    /// �G�̋������[�g
    /// </summary>
    /// <returns></returns>
    public float GetEnemyRate()
    {
        //todo:�����ȓG�̋����v�Z
        // 60�b�Ԃ͌Œ�
        if (inGameTime <= 60f) return 1f;

        // 300�b�Ł{100��
        return 1f + (inGameTime - 60f) / 300f;
    }

    /// <summary>
    /// �{�X�̋������[�g
    /// </summary>
    /// <returns></returns>
    public float GetBossRate()
    {
        // 300�b�Ԃ͌Œ�
        if (inGameTime <= 300f) return 2f;

        // 500�b�Ł{100��
        return 2f + (inGameTime - 300f) / 500f;
    }

    #endregion

    #region ���̑�

    /// <summary>
    /// ���j���[���J��
    /// </summary>
    public void OpenMenu()
    {
        state = GameState.Menu;
        inGameMenu.Open();

        StartCoroutine(WaitMenu());
    }

    /// <summary>
    /// ���j���[���҂�
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitMenu()
    {
        yield return new WaitWhile(() => inGameMenu.isActive);
        state = GameState.Active;
    }

    /// <summary>
    /// �o���l�ǉ�
    /// </summary>
    /// <param name="e"></param>
    public void AddExp(int e)
    {
        prm_Game.Exp += e;
        UpdateExpUI();
    }

    /// <summary>
    /// �o���l�ŐV�\��
    /// </summary>
    public void UpdateExpUI()
    {
        txt_exp.SetText(prm_Game.Exp.ToString());
    }

    #endregion
}

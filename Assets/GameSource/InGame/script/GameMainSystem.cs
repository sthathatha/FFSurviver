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

    /// <summary>FPS�\��</summary>
    public TMP_Text txt_fps;

    private Vector2Int player_loc;
    /// <summary>�v���C���[</summary>
    public PlayerScript playerScript { get; set; }

    /// <summary>�Q�[�����</summary>
    public enum GameState
    {
        Loading = 0,
        Active,
        Exiting,
    }
    public GameState state { get; private set; } = GameState.Loading;

    /// <summary>�G���G������I�u�W�F�N�g</summary>
    public Transform smallEnemyParent;

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
    }

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
        while (true)
        {
            yield return null;

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
                    //"GameSceneField_polygon1",
                    //"GameSceneField_eye1",
                    "GameSceneField_bakyura1",
                    "GameSceneField_willy1",
                };

                var idx = Util.RandomInt(0, randomScenes.Count - 1);
                manager.LoadSubScene(randomScenes[idx], loc.x, loc.y);
            }
        }
    }

    #endregion
}

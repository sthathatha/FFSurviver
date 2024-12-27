using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���t�B�[���h�Ǘ�
/// </summary>
public class GameFieldSystem : SubScriptBase
{
    /// <summary>�G���G�e�I�u�W�F�N�g</summary>
    public Transform smallEnemies;

    /// <summary>���S���W</summary>
    public Vector2Int fieldCell { get; private set; }

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitStart()
    {
        objectParent.SetActive(false);
        yield return base.InitStart();
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="paramList"></param>
    /// <returns></returns>
    protected override IEnumerator InitCoroutine(List<int> paramList)
    {
        fieldCell = new Vector2Int(paramList[0], paramList[1]);

        yield return base.InitCoroutine(paramList);

        // ���S��ݒ�
        objectParent.transform.position = FieldUtil.GetBasePosition(fieldCell.x, fieldCell.y);
        objectParent.SetActive(true);

        // �G���G�����C���V�[���Ɉړ�
        if (smallEnemies != null)
        {
            for (var i = smallEnemies.childCount - 1; i >= 0; --i)
            {
                smallEnemies.GetChild(i).SetParent(GameMainSystem.Instance.smallEnemyParent, true);
            }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void ReleaseField()
    {
        Sleep();
        base.DeleteScene();
    }
}

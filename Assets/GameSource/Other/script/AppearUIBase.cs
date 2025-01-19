using System.Collections;
using UnityEngine;

/// <summary>
/// ������o�Ă���UI�̊�{
/// </summary>
public class AppearUIBase : MonoBehaviour
{
    #region �����o�[

    /// <summary>�B��鎞��X���W</summary>
    public float hide_X = 0f;
    /// <summary>�A�N�e�B�uX���W</summary>
    public float show_X = 100f;
    /// <summary>�\������</summary>
    public float show_time = 0.3f;

    /// <summary>�ړ����W</summary>
    private DeltaFloat moveX;

    /// <summary>�u���\��</summary>
    protected bool isImmediate;

    #endregion

    #region ���

    /// <summary>
    /// �\��
    /// </summary>
    private void Start()
    {
        moveX = new DeltaFloat();
    }

    #endregion

    #region ����

    /// <summary>
    /// �J��
    /// </summary>
    /// <param name="_immediate">���J��</param>
    /// <returns></returns>
    public IEnumerator Open(bool _immediate = false)
    {
        var manager = ManagerSceneScript.GetInstance();
        var basePos = transform.localPosition;

        if (_immediate)
        {
            // �����o�Ă���
            basePos.x = show_X;
            transform.localPosition = basePos;
            gameObject.SetActive(true);
        }
        else
        {
            // �ړ�
            isImmediate = _immediate;
            InitMenu();
            gameObject.SetActive(true);

            moveX.Set(hide_X);
            moveX.MoveTo(show_X, show_time, DeltaFloat.MoveType.LINE);
            while (moveX.IsActive())
            {
                moveX.Update(manager.validDeltaTime);
                basePos.x = moveX.Get();
                transform.localPosition = basePos;

                yield return null;
            }
        }

        StartCoroutine(UpdateMenu());
    }

    /// <summary>
    /// �E�֕���
    /// </summary>
    /// <param name="_immediate"></param>
    /// <returns></returns>
    public IEnumerator Close(bool _immediate = false)
    {
        var manager = ManagerSceneScript.GetInstance();
        var basePos = transform.localPosition;

        if (_immediate)
        {
            // ����������
            basePos.x = hide_X;
            transform.localPosition = basePos;
        }
        else
        {
            // �ړ�
            isImmediate = _immediate;

            moveX.Set(show_X);
            moveX.MoveTo(hide_X, show_time, DeltaFloat.MoveType.LINE);
            while (moveX.IsActive())
            {
                moveX.Update(manager.validDeltaTime);
                basePos.x = moveX.Get();
                transform.localPosition = basePos;

                yield return null;
            }
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// �J���O�̕\���X�V
    /// </summary>
    virtual protected void InitMenu() { }

    /// <summary>
    /// �J�����������Ƃ̍X�V�R���[�`��
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator UpdateMenu() { yield break; }

    #endregion


}

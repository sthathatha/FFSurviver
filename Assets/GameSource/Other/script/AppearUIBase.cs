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
    public float show_time = 0.15f;

    /// <summary>�ړ����W</summary>
    private DeltaFloat moveX;

    /// <summary>�u���\��</summary>
    protected bool isImmediate;

    /// <summary>�����������ς�</summary>
    private bool isInitialized = false;

    /// <summary>�ғ���</summary>
    public bool isActive { get; private set; } = false;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    private void Start()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void Initialize()
    {
        moveX = new DeltaFloat();
        InitStart();
        isInitialized = true;
    }

    /// <summary>
    /// �K���P���鏉��������
    /// </summary>
    virtual protected void InitStart() { }

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
        var rect = GetComponent<RectTransform>();
        var basePos = rect.anchoredPosition;

        // �ŏ���\���̏ꍇStart�������ĂȂ��ꍇ������̂�
        if (!isInitialized)
        {
            // Active�łȂ��Ƃł��Ȃ����������������肦��̂ŁA�������ꏊ�ɐݒ肵�ċN����Ԃɂ��Ă���
            basePos.x = hide_X;
            rect.anchoredPosition = basePos;
            gameObject.SetActive(true);

            Initialize();
            yield return null;
        }

        isActive = true;

        if (_immediate)
        {
            // �����o�Ă���
            basePos.x = show_X;
            rect.anchoredPosition = basePos;
            gameObject.SetActive(true);
        }
        else
        {
            // �ړ�
            isImmediate = _immediate;
            InitOpen();
            gameObject.SetActive(true);

            moveX.Set(hide_X);
            moveX.MoveTo(show_X, show_time, DeltaFloat.MoveType.LINE);
            while (moveX.IsActive())
            {
                moveX.Update(manager.validDeltaTime);
                basePos.x = moveX.Get();
                rect.anchoredPosition = basePos;

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
        var rect = GetComponent<RectTransform>();
        var basePos = rect.anchoredPosition;

        if (_immediate)
        {
            // ����������
            basePos.x = hide_X;
            rect.anchoredPosition = basePos;
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
                rect.anchoredPosition = basePos;

                yield return null;
            }
        }

        isActive = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �J���O�̕\���X�V
    /// </summary>
    virtual protected void InitOpen() { }

    /// <summary>
    /// �J�����������Ƃ̍X�V�R���[�`��
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator UpdateMenu() { yield break; }

    #endregion


}

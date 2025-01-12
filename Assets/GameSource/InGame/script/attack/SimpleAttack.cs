using UnityEngine;

/// <summary>
/// �V���v���ɒ����ړ����ď�����U��
/// </summary>
public class SimpleAttack : AttackParameter
{
    #region �����o�[

    /// <summary>���������������</summary>
    public bool is_hit_disappear = false;
    /// <summary>�����鎞��</summary>
    public float disappear_time = 1f;
    /// <summary>��ԃX�s�[�h</summary>
    public float speed = 1f;

    /// <summary>�ړ�����</summary>
    private Vector3 move_direction;
    /// <summary>��������</summary>
    private float valid_time = 0f;

    #endregion

    #region Unity����

    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// �X�V
    /// </summary>
    void Update()
    {
        var dt = ManagerSceneScript.GetInstance().validDeltaTime;
        valid_time -= dt;
        if (valid_time <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += move_direction * dt;
    }

    #endregion

    #region ����

    /// <summary>
    /// �J�n
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="direction"></param>
    public void Shoot(Vector3 startPos, Vector3 direction)
    {
        valid_time = disappear_time;
        transform.position = startPos;
        move_direction = direction.normalized * speed;
        gameObject.SetActive(true);
    }

    #endregion
}

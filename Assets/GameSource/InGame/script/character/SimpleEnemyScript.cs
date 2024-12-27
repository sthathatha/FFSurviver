using UnityEngine;

/// <summary>
/// �P���ɋ߂Â��Ă��邾���̓G
/// </summary>
public class SimpleEnemyScript : CharacterScript
{
    #region �萔

    /// <summary>�������׌y���̂��߈ړ��`�F�b�N�͂��b����</summary>
    private const float MOVE_CHECK_INTERVAL = 1f;

    #endregion

    #region �����o�[

    public float float_height = 0.6f;
    public float move_speed = 5f;

    /// <summary>�ړ���������^�C�}�[</summary>
    private float checkTimer = -1f;
    private Vector3 moveVector = Vector3.zero;

    #endregion

    /// <summary>
    /// �X�V
    /// </summary>
    protected override void UpdateCharacter()
    {
        var main = GameMainSystem.Instance;

        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        if (checkTimer < 0f)
        {
            checkTimer = MOVE_CHECK_INTERVAL;
            // rigidBody�g��Ȃ�

            // �v���C���[�ʒu
            var pPos = main.playerScript.transform.position;
            var dist = pPos - transform.position;
            dist.y = 0;
            if (dist.sqrMagnitude < 0.1f) return;   // �[���߂����͉������Ȃ�

            // �P�ʃx�N�g��
            dist = dist.normalized;

            // �ړ��x�N�g���ݒ�
            moveVector = dist * move_speed;

            // ���f�������ݒ�
            transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
        }
        else
        {
            checkTimer -= Time.deltaTime;
        }


        // �ړ�
        transform.position += moveVector * Time.deltaTime;

    }
}

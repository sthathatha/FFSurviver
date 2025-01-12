using UnityEngine;

/// <summary>
/// �P���ɋ߂Â��Ă��邾���̓G
/// </summary>
public class SimpleEnemyScript : EnemyScriptBase
{
    #region �萔

    /// <summary>�������׌y���̂��߈ړ��`�F�b�N�͂��b����</summary>
    private const float MOVE_CHECK_INTERVAL = 1f;

    #endregion

    #region �����o�[

    public float float_height = 0.6f;
    public float move_speed = 5f;
    public float check_interval = MOVE_CHECK_INTERVAL;

    /// <summary>�ړ���������^�C�}�[</summary>
    private float checkTimer = -1f;
    private Vector3 moveVector = Vector3.zero;

    #endregion

    /// <summary>
    /// �X�V
    /// </summary>
    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();

        var manager = ManagerSceneScript.GetInstance();
        var dt = manager.validDeltaTime;
        var main = GameMainSystem.Instance;

        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        if (checkTimer < 0f)
        {
            checkTimer = check_interval;

            // �v���C���[�ʒu
            var pPos = main.GetPlayerCenter();
            var dist = pPos - transform.position;
            dist.y = 0;
            if (dist.sqrMagnitude < 1f)
            {
                // �[���߂����͉������Ȃ�
                moveVector = Vector3.zero;
                return;
            }

            // �����_����]���}30�x
            var randRot = Quaternion.Euler(0, Util.RandomFloat(-30f, 30f), 0);
            dist = randRot * dist;

            // �P�ʃx�N�g��
            dist = dist.normalized;

            // �ړ��x�N�g���ݒ�
            moveVector = dist * move_speed;

            // ���f�������ݒ�
            transform.rotation = Quaternion.LookRotation(dist, new Vector3(0, 1, 0));
        }
        else
        {
            checkTimer -= dt;
        }


        // �ړ�
        transform.position += moveVector * dt;
    }

    /// <summary>
    /// �v���C���[���痣�ꂷ������
    /// </summary>
    /// <param name="playerCenter"></param>
    protected override void TooFarPlayer(Vector3 playerCenter)
    {
        base.TooFarPlayer(playerCenter);

        // ������
        Destroy(gameObject);
    }

    /// <summary>
    /// ���S��
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();
        Destroy(gameObject);
    }
}

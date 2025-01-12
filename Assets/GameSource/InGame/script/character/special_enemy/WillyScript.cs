using System.Collections;
using UnityEngine;

/// <summary>
/// �E�C���[
/// </summary>
public class WillyScript : EnemyScriptBase
{
    #region �萔

    /// <summary>�������׌y���̂��߈ړ��`�F�b�N�͂��b����</summary>
    private const float MOVE_CHECK_INTERVAL = 1f;
    /// <summary>�����x</summary>
    private const float MOVE_ACCEL = 15f;
    /// <summary>�ō����x</summary>
    private const float MOVE_MAX_SPEED = 25f;
    /// <summary>�]�񑬓x</summary>
    private const float ROT_SPEED = 100f;

    #endregion

    #region �����o�[

    /// <summary>�ړ��t�F�[�Y</summary>
    private enum MovePhase
    {
        /// <summary>�˂�����</summary>
        Dash,
        /// <summary>�]�񂷂�O</summary>
        Dash2,
        /// <summary>�������]��</summary>
        Brake,
    }
    private MovePhase phase;

    /// <summary>�ړ���������^�C�}�[</summary>
    private float checkTimer = -1f;

    /// <summary>�ړ��x�N�g��</summary>
    private Vector3 moveVector = Vector3.zero;

    /// <summary>�]�񎞂̉�]����</summary>
    private float brakeRot = 1f;

    #endregion

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator InitCharacter()
    {
        yield return base.InitCharacter();

        // �����ݒ�̓v���C���[�ʒu�ɓːi
        phase = MovePhase.Dash;
        checkTimer = MOVE_CHECK_INTERVAL;

        // �]�񒆂̓v���C���[�ʒu�܂ŉ�]����
        var main = GameMainSystem.Instance;
        var pPos = main.GetPlayerCenter();
        var dist = pPos - transform.position;
        dist.y = 0f;

        moveVector = dist.normalized * MOVE_MAX_SPEED / 2f;

        // ���f���̌���
        transform.rotation = Quaternion.LookRotation(moveVector, new Vector3(0, 1, 0));
    }

    #endregion

    #region ���

    /// <summary>
    /// �X�V
    /// </summary>
    protected override void UpdateCharacter()
    {
        var manager = ManagerSceneScript.GetInstance();
        var dt = manager.validDeltaTime;
        var main = GameMainSystem.Instance;

        if (GameMainSystem.Instance.state != GameMainSystem.GameState.Active) return;

        // ���b�ɂP��ʒu�X�V
        if (checkTimer < 0f)
        {
            checkTimer = MOVE_CHECK_INTERVAL;

            if (phase == MovePhase.Dash)
            {
                // �v���C���[�ʒu
                var pPos = main.GetPlayerCenter();
                var dist = pPos - transform.position;
                dist.y = 0;

                // �_�b�V�����͒ʂ�߂���܂ŉ����̂�
                if (Vector3.Dot(dist, moveVector) >= 0f) return;

                // �ʂ�߂��Ă�����]��҂��Ɉڍs
                phase = MovePhase.Dash2;

            }
            else if (phase == MovePhase.Dash2)
            {
                var pPos = main.GetPlayerCenter();
                var dist = pPos - transform.position;
                dist.y = 0;

                // ��]���������߂����Ɍ���
                brakeRot = FieldUtil.CalcNearRotation(moveVector, dist) * ROT_SPEED;
                //
                phase = MovePhase.Brake;
            }
        }
        else
        {

            if (phase == MovePhase.Dash || phase == MovePhase.Dash2)
            {
                checkTimer -= dt;
                // �Ђ����璼�i
                // ������x�܂ŉ���
                var spd = moveVector.magnitude;

                if (spd < MOVE_MAX_SPEED)
                {
                    var newSpd = spd + MOVE_ACCEL * dt;
                    moveVector *= newSpd / spd;
                }
            }
            else
            {
                // �]�񒆂̓v���C���[�ʒu�܂ŉ�]����
                var pPos = main.GetPlayerCenter();
                var dist = pPos - transform.position;

                // �Ԃ̊p�x����������Γːi���[�h�ɂȂ�
                var rotDelta = brakeRot * dt;
                var rotQuat = Quaternion.Euler(0, rotDelta, 0);
                var nextVector = rotQuat * moveVector;
                if (FieldUtil.CalcNearRotation(moveVector, dist) != FieldUtil.CalcNearRotation(nextVector, dist))
                {
                    phase = MovePhase.Dash;
                    return;
                }

                // �������Ȃ����]
                var spd = nextVector.magnitude;
                if (spd > MOVE_MAX_SPEED / 5f)
                {
                    var newSpd = spd - MOVE_ACCEL * dt;
                    nextVector *= newSpd / spd;
                }

                moveVector = nextVector;

                // ���f���̌���
                transform.rotation = Quaternion.LookRotation(moveVector, new Vector3(0, 1, 0));
            }
        }

        // �ړ�
        transform.position += moveVector * dt;
    }

    /// <summary>
    /// ���S��
    /// </summary>
    protected override void DamageDeath()
    {
        base.DamageDeath();
        Destroy(gameObject);
    }

    #endregion
}

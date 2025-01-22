using UnityEngine;

/// <summary>
/// �v���C���[�p�����[�^
/// </summary>
public class PlayerParameter
{
    #region �p�����[�^

    /// <summary>
    /// �X�e�[�^�X�Ǘ��N���X
    /// </summary>
    public class Status
    {
        /// <summary>���ݒl</summary>
        public int value { get; set; }
        /// <summary>�ő�l</summary>
        private int maxValue { get; set; }
        /// <summary>�����l</summary>
        private int upHeight { get; set; }
        /// <summary>�����R�X�g��{�l</summary>
        private int costBase { get; set; }
        /// <summary>���̋����R�X�g</summary>
        public int cost { get; set; }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="_initVal">�����l</param>
        /// <param name="_costBase">�R�X�g��{�l</param>
        /// <param name="_max">�ő�l</param>
        /// <param name="_upHeight">�オ�蕝</param>
        public Status(int _initVal, int _upHeight, int _max, int _costBase)
        {
            value = _initVal;
            upHeight = _upHeight;
            maxValue = _max;
            costBase = _costBase;
            cost = _costBase;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="_noCount">true:�񐔂ɃJ�E���g���Ȃ��i�A�C�e�������̏ꍇ�j</param>
        /// <returns>�㏸��</returns>
        public int PowerUp(bool _noCount = false)
        {
            var oldVal = value;
            value += upHeight;
            if (maxValue > 0 && value >= maxValue)
            {
                // MAX�ɂȂ�����R�X�g0
                value = maxValue;
                cost = 0;
            }
            else if (!_noCount)
            {
                // todo:�R�X�g�����v�Z
                cost += costBase;
            }

            return value - oldVal;
        }

        /// <summary>
        /// ���݂�Exp�ŋ����\���ǂ���
        /// </summary>
        /// <param name="nowExp"></param>
        /// <returns></returns>
        public bool CanPowerUp(int nowExp)
        {
            if (maxValue > 0 && value >= maxValue) return false;
            if (cost > nowExp) return false;

            return true;
        }
    }

    /// <summary>�����U����</summary>
    public Status stat_melee { get; set; }
    /// <summary>���@�U����</summary>
    public Status stat_magic { get; set; }
    /// <summary>�ő�HP</summary>
    public Status stat_maxHp { get; set; }
    /// <summary>�ړ����x</summary>
    public Status stat_speed { get; set; }
    /// <summary>�W�����v��</summary>
    public Status stat_jump { get; set; }

    #endregion


    /// <summary>
    /// ������
    /// </summary>
    public void Init()
    {
        // �L�����ɂ�菉����
        var pid = GameConstant.GetTempPID();
        var status = pid switch
        {
            GameConstant.PlayerID.Drows => GameConstant.InitStatus_Drows,
            GameConstant.PlayerID.Eraps => GameConstant.InitStatus_Eraps,
            GameConstant.PlayerID.Exa => GameConstant.InitStatus_Exa,
            GameConstant.PlayerID.Worra => GameConstant.InitStatus_Worra,
            GameConstant.PlayerID.Koob => GameConstant.InitStatus_Koob,
            _ => GameConstant.InitStatus_You,
        };

        stat_melee = new Status(status.melee.init, status.melee.up, status.melee.max, status.melee.cost);
        stat_magic = new Status(status.magic.init, status.magic.up, status.magic.max, status.magic.cost);
        stat_maxHp = new Status(status.maxHp.init, status.maxHp.up, status.maxHp.max, status.maxHp.cost);
        stat_speed = new Status(status.speed.init, status.speed.up, status.speed.max, status.speed.cost);
        stat_jump = new Status(status.jump.init, status.jump.up, status.jump.max, status.jump.cost);
    }
}

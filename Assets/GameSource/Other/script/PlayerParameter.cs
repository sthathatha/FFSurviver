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
        /// <summary>������</summary>
        private int upCount { get; set; }
        /// <summary>�����l</summary>
        private int upHeight { get; set; }
        /// <summary>�����R�X�g��{�l</summary>
        private int costBase { get; set; }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="initVal">�����l</param>
        /// <param name="costBase">�R�X�g��{�l</param>
        /// <param name="max">�ő�l</param>
        /// <param name="upHeight">�オ�蕝</param>
        public Status(int initVal, int upHeight, int max, int costBase)
        {
            this.value = initVal;
            this.upHeight = upHeight;
            this.maxValue = max;
            this.costBase = costBase;

            this.upCount = 0;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="_noCount">true:�񐔂ɃJ�E���g���Ȃ��i�A�C�e�������̏ꍇ�j</param>
        public void Increase(bool _noCount = false)
        {
            value += upHeight;
            if (!_noCount) upCount++;
        }

        /// <summary>
        /// ���̋����R�X�g
        /// </summary>
        /// <returns></returns>
        public int GetNextCost()
        {
            //todo:�R�X�g�v�Z
            return (upCount + 1) * costBase;
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
        //todo:�L�����ɂ�菉����
        stat_melee = new Status(50, 15, -1, 1);
        stat_magic = new Status(25, 0, 25, 0);
        stat_maxHp = new Status(100, 50, -1, 5);
        stat_speed = new Status(10, 2, 100, 10);
        stat_jump = new Status(1, 1, 3, 2000);
    }
}

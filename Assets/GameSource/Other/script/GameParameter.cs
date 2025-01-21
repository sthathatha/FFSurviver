using UnityEngine;

/// <summary>
/// �Q�[�����̂��̂̃p�����[�^
/// </summary>
public class GameParameter
{
    #region �����o�[

    /// <summary>�o���l</summary>
    public int Exp { get; set; }

    /// <summary>�{�X���j�t���O�P</summary>
    public bool Defeated_Boss1 { get; set; }
    /// <summary>�{�X���j�t���O�Q</summary>
    public bool Defeated_Boss2 { get; set; }
    /// <summary>�{�X���j�t���O�R</summary>
    public bool Defeated_Boss3 { get; set; }
    /// <summary>�{�X���j�t���O�S</summary>
    public bool Defeated_Boss4 { get; set; }
    /// <summary>�{�X���j�t���O�T</summary>
    public bool Defeated_Boss5 { get; set; }

    /// <summary>����������</summary>
    public int LotteryCount { get; set; }

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    public void InitParam()
    {
        Exp = 0;
        Defeated_Boss1 = false;
        Defeated_Boss2 = false;
        Defeated_Boss3 = false;
        Defeated_Boss4 = false;
        Defeated_Boss5 = false;
        LotteryCount = 0;
    }

}

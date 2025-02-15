using System.Collections;
using UnityEngine;

/// <summary>
/// エフェクト一瞬で終わる攻撃
/// </summary>
public class EffectOneShotAttack : AttackParameter
{
    /// <summary>パーティクル</summary>
    public ParticleSystem effect;

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="startPos"></param>
    public void Shoot(Vector3 startPos)
    {
        transform.position = startPos;
        gameObject.SetActive(true);

        StartCoroutine(ExpandCoroutine());
    }

    /// <summary>
    /// 広がる
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandCoroutine()
    {
        var origin = ManagerSceneScript.GetInstance().GetComponent<OriginManager>();

        // キープ時間
        var time = 0f;
        while (time < 0.2f)
        {
            yield return null;
            time += origin.inGameDeltaTime;
        }

        // 攻撃判定を消す
        GetComponent<Collider>().enabled = false;

        // エフェクトの終了を待つ
        yield return new WaitWhile(() => effect != null && effect.isPlaying);

        // 消える
        Destroy(gameObject);
    }
}

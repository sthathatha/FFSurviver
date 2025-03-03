using UnityEngine;

/// <summary>
/// 文字列
/// </summary>
public class Strings
{
    /*
     * !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~ 
ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ１２３４５６７８９０—―‘’‚‛“”„‟†‡•…′″‴‹›‼‾⁄、。，．・：；？！゛゜´｀¨＾￣＿ヽヾゝゞ〃々〆〇ー―※→←↑↓ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝｧｨｩｪｫｯｬｭｮｰｶﾞｷﾞｸﾞｹﾞｺﾞｻﾞｼﾞｽﾞｾﾞｿﾞﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟｳﾞぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞたァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタだちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミむめもゃやゅゆょよらりるれろゎわゐゑをんムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶ
 　「」『』【】[]（）｛｝、。＜＞！？＿＋－×÷￥＾＝＄＃＆～＊★／＼％
     * 
     * 
     * */

    #region お尋ね者表示

    public const string Wanted_Name_Boss1 = "鏡の怪物";
    public const string Wanted_Detail_Boss1 = "小さな怪物をたくさん倒すと現れる。その巨体に押しつぶされるとひとたまりもない。";
    public const string Wanted_Name_Boss2 = "花の怪物";
    public const string Wanted_Detail_Boss2 = "いつの間にか現れて遠くからこっそり攻撃してくるクソ花。近づくと逃げる。";
    public const string Wanted_Name_Boss3 = "水の怪物";
    public const string Wanted_Detail_Boss3 = "高すぎるところから水に飛び込むと、棲み処を荒らされた事に怒り襲いかかってくる。";
    public const string Wanted_Name_Boss4 = "月の怪物";
    public const string Wanted_Detail_Boss4 = "夜はアイツの縄張りだ。立ち止まってはならない。振り向いてはならない。それを見つけた時、それはお前を見つける。";
    public const string Wanted_Name_Boss5_X = "？？？？？？？";
    public const string Wanted_Name_Boss5 = "つくよみちゃん";
    public const string Wanted_Detail_Boss5 = "全ての怪物を倒した後に現れる。何かを探しているらしい。";

    #endregion

    #region 武器

    public const string Weapon_Empty_Name = "";

    public const string Weapon_Fireball_Name = "ファイアボール";
    public const string Weapon_Fireball_Desc = "【魔法】もよりの敵に貫通する火球を発射する";
    public const string Weapon_Thunder_Name = "エレキシールド";
    public const string Weapon_Thunder_Desc = "【魔法】まわりを回転する電気の球";
    public const string Weapon_Meteor_Name = "メテオ";
    public const string Weapon_Meteor_Desc = "【魔法】燃える隕石を落とし、しばらく炎の地形を残す";
    public const string Weapon_Leaf_Name = "木の葉乱舞";
    public const string Weapon_Leaf_Desc = "【魔法】周囲の敵を攻撃する風の刃";
    public const string Weapon_Quake_Name = "アースシェイカー";
    public const string Weapon_Quake_Desc = "【物理】着地する時に地面を揺らして周囲を攻撃";
    public const string Weapon_Bomb_Name = "小型爆弾";
    public const string Weapon_Bomb_Desc = "【物理】近くの敵に爆弾を投げつけて攻撃";
    public const string Weapon_Float_Name = "浮遊";
    public const string Weapon_Float_Desc = "【特殊】ジャンプボタン長押しでしばらく滞空できる";
    public const string Weapon_Option_Name = "オプション";
    public const string Weapon_Option_Desc = "【特殊】プレイヤーと同時に攻撃する光球\n特定の攻撃はできない";
    public const string Weapon_Cyclone_Name = "かまいたち";
    public const string Weapon_Cyclone_Desc = "【物理】もよりの敵に貫通する斬撃を飛ばす";
    public const string Weapon_Fireworks_Name = "ねずみ花火";
    public const string Weapon_Fireworks_Desc = "【物理】ランダムな方向に花火を飛ばして攻撃";

    #endregion

    #region 強化アイテム

    public const string Item_Heal_Name = "ヒール";
    public const string Item_Heal_Desc = "HPを50％回復する";

    public const string Item_Stat_All = "シェリフスター";
    public const string Item_Stat_All_Desc = "全ステータス2段階アップ";
    public const string Item_Stat_Melee = "Melee＋";
    public const string Item_Stat_Melee_Desc = "物理攻撃力1段階アップ";
    public const string Item_Stat_Magic = "Magic＋";
    public const string Item_Stat_Magic_Desc = "魔法攻撃力1段階アップ";
    public const string Item_Stat_Hp = "HP＋";
    public const string Item_Stat_Hp_Desc = "最大HP1段階アップ";
    public const string Item_Stat_Speed = "Speed＋";
    public const string Item_Stat_Speed_Desc = "移動速度1段階アップ";
    public const string Item_Stat_Jump = "Jump＋";
    public const string Item_Stat_Jump_Desc = "ジャンプ回数アップ";

    public const string Item_General_CoolTime = "クールタイム－10％";
    public const string Item_General_Size = "サイズ＋20％";
    public const string Item_General_Count = "攻撃数＋1";

    public const string Item_Float_Time = "浮遊時間増加";

    #endregion

    #region つくよみちゃんセリフ

    public const string Tukuyomi_Serif_Start1 = "古の時代、神々がパンドラに持たせた箱は\n不完全なものでした";
    public const string Tukuyomi_Serif_Start2 = "その内で膨らみ続けた災厄は\n僅かな隙から溢れ出し一瞬で世界を包みました";
    public const string Tukuyomi_Serif_Start3 = "せめて世界の外へ漏れ出さないよう抑えながら\n封じ込める方法を探していましたが";
    public const string Tukuyomi_Serif_Start4 = "あなたならあるいは…";
    public const string Tukuyomi_Serif_End1 = "完全な箱に求められるのは頑強さではなく\n如何なるモノも形を変えて受け入れる柔軟さ";
    public const string Tukuyomi_Serif_End2 = "憤怒、嫉妬、強欲…幾多の災いを内に飼いながら\nその最奥の希望の光は決して失わない";
    public const string Tukuyomi_Serif_End3 = "あなた方人間こそが\n正しく完全なパンドラの箱たりえるのかもしれません";

    #endregion

}

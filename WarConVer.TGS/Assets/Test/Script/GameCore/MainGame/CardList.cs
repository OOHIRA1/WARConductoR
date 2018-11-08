using UnityEngine;

//==カードIDを管理するクラス
public class CardList : MonoBehaviour
{

    [SerializeField] string[] card_Image_Id;

    public string[] Card_Image_Id
    {
        set { card_Image_Id = value; }
        get { return card_Image_Id; }
    }
    private void Start()
    {
        card_Image_Id = new string[]
	    {
	        "Vanargand",
	        "Hohenstaufen",
	        "Schwarzwald",
	        "Volsunga",
	        "Nachzehrer",
	        "Luzifer",
	        "Tobalcaine",
	        "Pegasus",
	        "VereFilius",
	        "Babylon",
	        "Heilige",
	        "Wegweiser",
	        "Swastika",
	        "Guilltine",
	        "Rusalka",
	        "dracula",
	        "undead",
	        "blackdragon",
	        "kmiusagiaa",
	        "suke",
	        "zombie",
	        "carbuncle",
	        "garuda",
	        "kennryuu",
	        "karasu",
	        "suke2",
	        "samechang",
	        "reaper",
	        "poseidonn",
	        "kettosi",
	        "cornius",
	        "gypsum",
	        "machine",
	        "n",
	        "MAX_CARD_NUM"
	    };
    }
    /// <summary>
    /// カード名
    /// 旧名
    /// あらし
    /// うま
    /// きし
    /// し
    /// へび
    /// るしるし
    /// パペット
    /// ペガサス
    /// 六翼
    /// 契約者
    /// 帝
    /// 案兄人
    /// 死の導き
    /// 魂狩り
    /// 魔女
    /// </summary>
    public enum Card
    {
        Vanargand,
        Hohenstaufen,
        Schwarzwald,
        Volsunga,
        Nachzehrer,
        Luzifer,
        Tobalcaine,
        Pegasus,
        VereFilius,
        Babylon,
        Heilige,
        Wegweiser,
        Swastika,
        Guilltine,
        Rusalka,
        dracula,
        undead,
        blackdragon,
        kmiusagiaa,
        suke,
        zombie,
        Carbuncle,
        garuda,
        kennryuu,
        karasu,
        suke2,
        samechang,
        reaper,
        poseidonn,
        kettosi,
        cornius,
        gypsum,
        machine,
        n,
        MAX_CARD_NUM
    };

    /// <summary>
    /// 動き方
    /// </summary>
    public enum Move
    {
        poan,
        king,
        queen,
        kight,
        rook,
        bishop,
        suke,
        Undead,
        MAX_MOVE_TYPE
    };

}
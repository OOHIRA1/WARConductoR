using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    /// <summary>
    /// ルール上変わることのない数値
    /// </summary>
    const int MANA_MAX_NUM = 12;
    const int DECK_MIN_NUM = 0;
    const int DECK_MAX_NUM = 20;
    const int HAND_CARD_WIDTH = -200;
    const int HAND_CARD_HIGTH = -638;
    const int ACTION_COST_POINT = 1;
    const int FAST_HAND_NUM = 4;
    const int MAX_HAND_NUM = 8;
    const int PLAYER_STATUS_NUM = 5;
    const int MAX_DECK_NUM = Deck1.MAX_DECK_NUM;
    const int MAX_CARD_ID = (int)CardList.Card.MAX_CARD_NUM;


    /// <summary>
    /// Spriteを充てるためのstring配列
    /// 名称はResouceのファイル名を参照するので合わせること。
    /// </summary>

    /// <summary>
    /// GameStartButton
    /// ReLoadButton
    /// NoReLoadButton
    /// tmp ReLoadの際初期手札をDestroyするための一時保管用配列
    /// CreatedCard の参照
    /// Player　の参照
    /// Card の参照
    /// Cardpfb　ゲーム上のカードの参照
    /// canvas campusの参照
    /// ResetHand 初期手札を入れ替えたか
    /// DrawCount ゲーム中にドローした数
    /// </summary>
	[SerializeField] GameObject GameStart = null;		//スタートボタン
	[SerializeField] GameObject ReLoadButton = null;	//リロードボタン
	[SerializeField] GameObject NoReLoad = null;		//リロードしないボタン
    [SerializeField] GameObject[] tmp;					//手札系の何か？？
//    [SerializeField] GameObject _player;
//    [SerializeField] GameObject Cardpfb;
//    [SerializeField] GameObject canvas;
//    [SerializeField] GameObject card_Date;
//    [SerializeField] GameObject Field1;
	[SerializeField] GameObject ComDeck = null;			//調停者デッキボタン
	[SerializeField] GameObject ImmDeck = null;			//不死デッキボタン
	[SerializeField] CreatedCard created = null;
    [SerializeField] Player player = null;
	[SerializeField] Card[] card = new Card[8];			//手札にあるカード
	[SerializeField] CardList _cardList = null;
//    [SerializeField] SceneMgr GetScene;
    [SerializeField] Filed[] GetFiled;
	[SerializeField] Deck1 Deck1 = null;
	[SerializeField] UnitMgr _unitMgr = null;
    [SerializeField] Sprite[] CardSprite;
//    [SerializeField] Sprite[] Font;
	[SerializeField] TestHndInstant testHndInstant = null;
//    [SerializeField] string Deck_Name = "";
//    [SerializeField] int SumonCost = 0;
    [SerializeField] int _unitName = 0;
    [SerializeField] int DrawCount = 0;
//    [SerializeField] int Cementery = 0;
    [SerializeField] int _samon_cost;
    [SerializeField] int _Player_Mana; //= 0;
    [SerializeField] bool ResetHand = false;
    [SerializeField] bool _cansamon;
    [SerializeField] bool _Load_Sprite;
//    [SerializeField] bool _player_Trune = true;


    public bool CanSamon
    {
        set { _cansamon = value; }
        get { return _cansamon; }
    }
    public int UnitName
    {
        set { _unitName = value; }
        get { return _unitName; }
    }
    public Sprite[] Get_Card_Sprite
    {
        get { return CardSprite; }
    }
    public bool Get_Load_Sprite
    {
        get { return _Load_Sprite; }
    }

    private void Awake()
    {
        
        CardSprite = Resources.LoadAll<Sprite>("Card");
//        Font = Resources.LoadAll<Sprite>("font");
    }
    /// <summary>
    /// tmp 配列の宣言
    /// Buttonの参照
    /// CardName Spriteの名前、並びをそろえて
    /// </summary>
    private void Start()
    {
        _Load_Sprite = true;
        //ゲームシーン所得
        //手札入れ替えのため、初期手札の保存用
        tmp = new GameObject[MAX_HAND_NUM];


        //手札を入れ替えたかどうか
        ResetHand = false;

    }
    public void Push_GameStart ()
    {
        ComDeck.SetActive(true);
        ImmDeck.SetActive(true);
    }
    //デッキ選択
    public void Deck_Select_Conciliator_Deck()
    {
        if (Deck1.Get_Ledy_Deck)
        {
            Deck1.Deck_Select_Num = 0;
            Deck1.Deck_Select();
            FarstHandDrow();
            Destroy(ComDeck);
            Destroy(ImmDeck);

        }
    }
    public void Deck_Select_Immortality()
    {
        if (Deck1.Get_Ledy_Deck)
        {
            Deck1.Deck_Select_Num = 1;
            Deck1.Deck_Select();
            FarstHandDrow();
            Destroy(ImmDeck);
            Destroy(ComDeck);
        }
    }
    //初期手札ドロー用呼び出せば使える
    public void FarstHandDrow()
    {
        //for文で手札をそろえる
        for (int i = 0; i < FAST_HAND_NUM; i++)
        {
            CreatCard();
            if (ResetHand)
            {
                testHndInstant.ReLoadPush();
            }
            else
            {
                testHndInstant.OnPushButtonDraw();
            }
            //playerの手札枚数更新
            player.HandNum = FAST_HAND_NUM;
        }
        //デバッグ用
        Destroy(GameStart);
        //リロード、リロード無しのボタンアクティブ
        ReLoadButton.SetActive(true);
        NoReLoad.SetActive(true);
    }

    //手札リロード、呼び出せば使える
    public void ReLoad( )
    {
        //手札の破棄
        for(int i = 0; i < FAST_HAND_NUM; i++)
        {
            card_Date_Null();
            //この下に、デッキの初期化
        }
        //リロードボタン、リロード無しボタン破棄
       // Destroy(ReLoadButton);
       // Destroy(NoReLoad);
        DrawCount = 0;
        //リロードした
        ResetHand = true;
        //初期手札関数呼び出し
        FarstHandDrow( );
        //手札の枚数更新
        player.HandNum = FAST_HAND_NUM;
    }

    //リロード無し関数、呼び出せば使える
    public void NoChange()
    {
        //リロード系ボタン破棄
        Destroy(ReLoadButton);
        Destroy(NoReLoad);
    }

    //呼び出しで１枚ドロー
    public void DrawFeis(  )
    {
        {
            //ドロー変数更新
            DrawCount++;
            //ドロー変数が３０を超えたならデッキ枚数を超えるので負け
            if( DrawCount > 30)
            {
                player.Dead = true;
                GameFin(player.Dead);
            }
            //ドロー変数宣言
//            int DrawNum = 0;
            //以下手札初期化と同じ動作
            CreatCard();
            //手札が８枚より多くなったら破棄
            if ( player.HandNum > MAX_HAND_NUM )
            {
                Destroy(tmp[0]);
                player.HandNum--;
                player.Cemetery++;
            }
        }
    }
    
    //召喚できるかの関数
    public bool CostCheck (int cost)
    {
        //本来ここにコストとplayerマナを見て召喚可能か確認
         if ( player.Mana >= cost )
         {
            _cansamon = true;
         }
         else
         {
               _cansamon = false;
         }
           
        return _cansamon;
    }

    //召喚関数
    public void Samon(string name, int cost, int Combat, int Power, int Move,int MovePoint, int ActionPoint, Image iconImage)
    {
        player.Mana -= cost;
        //unitMgrに値を渡し処理を任せる
        _unitMgr.Image_Name = name;
        _unitMgr._cost = cost;
        _unitMgr._com = Combat;
        _unitMgr._pow = Power;
        _unitMgr._move = Move;
        _unitMgr._move_point = MovePoint;
        _unitMgr._ap = ActionPoint;
        _unitMgr.IconImage = iconImage;
        _cansamon = false;
    }

   
    //いらないなら消すこの関数を
    public void MainTime()
    {
        //GetScene.GetFeise = SceneMgr.GameFeise.Main;
        if (player.Mana < MANA_MAX_NUM)
        {
            player.HandNum = DrawCount;
            player.Mana++;

        }
    }

    //エンドフェイズ関数
    public void EndFeis()
    {
        //現在はソロデバッグ用処理
        player.ActionPoint = 6;
        player.Update_Crystal_AP();
        player.Mana = 10 +1;
        DrawFeis();
    }

    //デュエル終了用関数
    public void GameFin( bool Dead )
    {
       // GetScene.GetFeise = SceneMgr.GameFeise.Resolut;
        if( Dead )
        {
            print("YOU LOSE");
        }
    }

    //効果関数
    public void Effect()
    {
    }

    /// <summary>
    /// カードを作る関数
    /// </summary>
    /// <param name="move">　カードの動き方</param>
    /// <param name="palam">　0 攻撃力, 1 体力, 2 召喚コスト, 
    ///                       3 AP, 4 EFFECT</param>
    public void CallBack( int move, int[] palam )
    {
        card[DrawCount].Combat = palam[0];
        card[DrawCount].Power = palam[1];
        card[DrawCount].Cost = palam[2];
        card[DrawCount].ActionPoint = palam[3];
        card[DrawCount].Move_Point = palam[4];
        card[DrawCount].Move = move;
    }
    
    public void CreatCard( )
    {
        //ドロー変数宣言
        int DrawNum = 0;

        DrawNum = UnityEngine.Random.Range(DECK_MIN_NUM, DECK_MAX_NUM);
        print(DrawNum);
        //カードデータを取得
        //カードリストのリストID番号がデッキ１のクラスのデッキの中のDrawNumの数値
        card[DrawCount].CloneCard(_cardList.Card_Image_Id[Deck1.Deck[DrawNum]]);
        created.CardNum = Deck1.Deck[DrawNum];
        created.Created(CallBack);
        //プレイヤーの手札枚数更新
        player.HandNum++;
        //プレイヤーのドローカウント更新
        DrawCount++;
    }

    private void card_Date_Null()
    {
        card[DrawCount].Combat = 0;
        card[DrawCount].Power =0;
        card[DrawCount].Cost = 0;
        card[DrawCount].ActionPoint = 0;
        card[DrawCount].Move_Point = 0;
        card[DrawCount].Move = 0;
        card[DrawCount].getSprite = null;
    }

}

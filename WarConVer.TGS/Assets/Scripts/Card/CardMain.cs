using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==カード機能クラス
//
//==使用方法：カードのGameObjectにアタッチ
public class CardMain : MonoBehaviour {
	[SerializeField] int _loadID = 0;							//読み込むカードID
	[SerializeField] SpriteRenderer _cardSpriteRenderer = null;	//カードのSpriteRenderer
	[SerializeField] CardDataLoader _cardDataLoader = null;
	[SerializeField] CardData _cardData = null;
	[SerializeField] int _actionCount = 0;						//移動回数
	const int _MAX_ACTION_COUNT = 3;							//移動回数の最大値

	//テスト用----------------------------------------------
	public enum EFFECT_TYPE { 
		ATTACK,
		MOVE,
		RECOVERY,
	} 

	[System.Serializable]
	public struct CardDates {
		public int hp;
		public int max_hp;
		public int attack_point;
		public Field.DIRECTION[ ] directions;
		public int distance;
		public int move_ap;
		public Field.DIRECTION[ ] effect_direction;
		public int effect_ap;
		public int effect_ditance;
		public int effect_damage;
		public int effect_recovery_point;
		public int mp;
		public EFFECT_TYPE effect_type;
		
	}
	public CardDates _cardDates = new CardDates( );
	//-------------------------------------------------------


	//===================================================================
	//アクセッサ
	public int loadID {
		get { return _loadID; }
		set { _loadID = value; }//カードをInstantiateした際にIDを設定する
	}

	public SpriteRenderer Card_Sprite_Renderer { 
		get { return _cardSpriteRenderer; }
		private set { _cardSpriteRenderer = value; }
	} 
	//===================================================================
	//===================================================================


	// Use this for initialization
	void Start () {
		_cardSpriteRenderer = GetComponent<SpriteRenderer>();
		_cardDataLoader = GameObject.Find ("CardDataLoader").GetComponent<CardDataLoader>();
		Load ();//カードデータの読み込み
		Debug.Log (_cardData);
		Debug.Log (_actionCount);
		_actionCount = _MAX_ACTION_COUNT;
	}


	//--カードデータを読み込む関数
	void Load( ) {
		_cardSpriteRenderer.sprite = (Sprite)Resources.Load<Sprite> ("Card/" + _loadID);//カード画像の読み込み
		_cardData = _cardDataLoader.GetCardDataFromID ( _loadID );
	}


	//================================================b==========================================================
	//public関数

	//ダメージ-----------------------------------
	public void Damage( int decrease_point  ) { 
		_cardDates.hp -= decrease_point;

		if ( _cardDates.hp < 0 ) { 
			_cardDates.hp = 0;	
		}

		//HPが０だったら破壊される
		if ( _cardDates.hp == 0 ) { 
			Death( );	
		}
	}
	//------------------------------------------

	//破壊
	public void Death( ) {
		Destroy( this.gameObject );	
	} 

	//===========================================================================================================
	//===========================================================================================================
}

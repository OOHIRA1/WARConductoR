using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPhase : Phase {
	public enum MAIN_PHASE_STATUS { 
		IDLE,
		CARD_MOVE,
		CARD_DETAILS,
		DIRECT_ATTACK,
		EFFECT_ATTACK,
		EFFECT_MOVE,
		EEFECT_RECOVERY,
		HAND_CARD_OPERATION,
		HAND_CARD_DETAILS,
		HANC_CARD_SUMMON
	}
	
	public static bool _precedenceOneTurnFlag = true;	//先行１ターン目かどうか(クラスが破棄されてもまたは他のクラスにもほしい情報なので pulbic static)
	
	const int LOSE_CEMETARY_POINT = 10;
	const float SHOW_DETAILS_HOLD_TIME = 0.5f;		//手札の詳細を表示するホールド時間(手札をタッチしたときホールド時間がx秒以下だったら表示する)

	MainSceneOperation _mainSceneOperation = null;
	Square _nowSquare					   = null;
	CardMain _card						   = null;
	CardMain _handCard					   = null;
	Participant _enemyPlayer			   = null;
	EnemyBehavior _enemyBehavior		   = null;
	UIActiveManager _uiActiveManager	   = null;
	Field _field						   = null;
	SceneTransition _sceneTransition	   = null;
	RayShooter _rayShooter				   = new RayShooter( );
	Vector3 _handCardPos				   = Vector3.zero;
	MAIN_PHASE_STATUS _mainPhaseStatus	   = MAIN_PHASE_STATUS.IDLE;
	bool _turnEndFlag					   = false;
	bool _enemyUpdateStartFlag			   = true;

	//ボタン
	GameObject _turnEndButton	   = null;


	public MainPhase( Participant turnPlayer, Participant enemyPlayer, MainSceneOperation mainSceneQperation, UIActiveManager uIActiveManager, Field field,
					  GameObject turnEndButton, EnemyBehavior enemyBehavior, SceneTransition sceneTransition ) {

		_turnPlayer = turnPlayer;
		_enemyPlayer = enemyPlayer;
		_mainSceneOperation = mainSceneQperation;
		_uiActiveManager = uIActiveManager;
		_field = field;
		_enemyBehavior = enemyBehavior;
		_sceneTransition = sceneTransition;

		_turnEndButton = turnEndButton;


		//敵プレイヤーターン中に表示するテキスト
        if ( _turnPlayer.gameObject.tag == ConstantStorehouse.TAG_PLAYER2 ) { 
            _uiActiveManager.TextActiveChanger( true, UIActiveManager.TEXT.NOW_ENEMY_TURN );    
        } else { 
            _uiActiveManager.TextActiveChanger( false, UIActiveManager.TEXT.NOW_ENEMY_TURN );
        }

		Debug.Log( _turnPlayer.gameObject.tag + "メインフェーズ" );


		//デバッグ用
		//debugSquare.On_Card = debugCard;
		//debugCard.transform.position = debugSquare.transform.position;
	}


	public override void PhaseUpdate( ) {
		LoseTerms( );
		InterruptionButtonClicked( );
		Interruption( );

		if ( _turnPlayer.gameObject.tag == "Player2" ) {
			EnemyTurnUpdate( );
			return;
		}

		PlayerTurnUpdate( );
	}
	

	public override bool IsNextPhaseFlag( ) {
		return _turnEndFlag;
	}


	void PlayerTurnUpdate( ) { 
		ActiveTurnEndButton( );
		TurnEndButtonClicked( );
		switch ( _mainPhaseStatus ) {
			case MAIN_PHASE_STATUS.IDLE:
				IdleStatis( );
				return;

			case MAIN_PHASE_STATUS.CARD_DETAILS:
				CardDetailsStatus( );
				return;

			case MAIN_PHASE_STATUS.CARD_MOVE:
				MoveCardStatus( );
				return;

			case MAIN_PHASE_STATUS.DIRECT_ATTACK:
				DirectAttack( );
				return;

			case MAIN_PHASE_STATUS.EFFECT_ATTACK:
				AttackEffect( );
				return;

			case MAIN_PHASE_STATUS.EFFECT_MOVE:
				MoveEffect( );
				return;

			case MAIN_PHASE_STATUS.EEFECT_RECOVERY:
				RecoveryEffect( );
				return;

			case MAIN_PHASE_STATUS.HAND_CARD_OPERATION:
				HnadOperationStatus( );
				return;

			case MAIN_PHASE_STATUS.HAND_CARD_DETAILS:
				HnadCardDetailsStatus( );
				return;

			case MAIN_PHASE_STATUS.HANC_CARD_SUMMON:
				HandCardSummon( );
				return;

			default:
				Debug.Log( "存在しないステータスです" );
				return;
		}
	}

	void EnemyTurnUpdate( ) { 
		if ( _enemyUpdateStartFlag ) {
			_enemyBehavior.StartEnemyUpdate( );
			_enemyUpdateStartFlag = false;
		}

		if ( !_enemyBehavior.Enemy_Update_Flag ) {
			if ( _precedenceOneTurnFlag ) _precedenceOneTurnFlag = false;
			_enemyBehavior.Enemy_Update_Flag = true;	//フラグリセット
			_enemyBehavior.StopEnemyUpdate( );
			_turnEndFlag = true;
		}
	}


	//待機状態-----------------------------------------
	void IdleStatis( ) {
		if ( _mainSceneOperation.MouseTouch( ) ) {
			FieldCardTouch( );
			HandCardTouch( );

			//ここに手札のカードを何かしらの条件で詳細を表示するを書く？
		}
	}
	//--------------------------------------------------

	
	//フィールドのカードがタッチされたら処理------------------------
	void FieldCardTouch( ) { 
		//カードとそのマスの取得
		_nowSquare = _rayShooter.RayCastSquare( );
		if ( _nowSquare == null ) return;
		_card = _nowSquare.On_Card;
		if ( _card == null ) return;
		
		ShowCardDetails( );	//カードの詳細画像の表示
		ShowCardOperationUI( );	//プレイヤーによって表示するUI
		
		_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_DETAILS;
	}
	//---------------------------------------------------------------


	//手札のカードがタッチされたら処理-------------------------------------------
	void HandCardTouch( ) { 
		_handCard = _rayShooter.RayCastHandCard( _turnPlayer.gameObject.tag );
		if ( _handCard == null ) return;

		_handCardPos = _handCard.transform.position;
		_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = false;
		_handCard.CardExpansion( );
		_mainPhaseStatus = MAIN_PHASE_STATUS.HAND_CARD_OPERATION;
	}
	//---------------------------------------------------------------------------


	//フィールドカードの詳細画像表示----------------------------------------------------------------------------------------------------
	void ShowCardDetails( ) { 
		_card.ShowCardDetail( );
	}
	//---------------------------------------------------------------------------------------------------------------------------


	//フィールドカードの操作系UIの表示処理---------------------------------------
	void ShowCardOperationUI( ) {
		_uiActiveManager.ShowCardOperationUI( _card, _turnPlayer, _nowSquare );
	}
	//-----------------------------------------------------------------------------

	
	//ボタン操作処理------------------------------------------------------------
	void CardDetailsStatus( ) {

		//戻るボタンを押したら
		if ( _mainSceneOperation.BackButtonClicked( ) ) { 
			_uiActiveManager.AllButtonActiveChanger( false );
			_card.DeleteCardDetail( );
			GetDataReset( );
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		//移動ボタンを押したら
		if ( _mainSceneOperation.MoveButtonClicked( ) ) { 
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.MOVE,
														 UIActiveManager.BUTTON.DIRECT_ATTACK,
														 UIActiveManager.BUTTON.EFFECT );

			_card.DeleteCardDetail( );
			_mainPhaseStatus = MAIN_PHASE_STATUS.CARD_MOVE;
			return;
		}

		//攻撃ボタンを押したら
		if ( _mainSceneOperation.AttackButtonClicked( ) ) {
			_uiActiveManager.AllButtonActiveChanger( false );
			_mainPhaseStatus = MAIN_PHASE_STATUS.DIRECT_ATTACK;
			_card.DeleteCardDetail( );
			return;
		}

		//効果ボタンを押したら
		if ( _mainSceneOperation.EffectButtonClicked( ) ) { 
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.MOVE,
														 UIActiveManager.BUTTON.DIRECT_ATTACK,
														 UIActiveManager.BUTTON.EFFECT );

			//効果の種類によって処理を変える
			switch ( _card.Card_Data._effect_type ) { 
				case CardData.EFFECT_TYPE.ATTACK:
					_uiActiveManager.ButtonActiveChanger( true, UIActiveManager.BUTTON.EFFECT_YES );
					_mainPhaseStatus = MAIN_PHASE_STATUS.EFFECT_ATTACK;
					break;

				case CardData.EFFECT_TYPE.MOVE:
					_mainPhaseStatus = MAIN_PHASE_STATUS.EFFECT_MOVE;
					break;

				case CardData.EFFECT_TYPE.RECOVERY:
					_uiActiveManager.ButtonActiveChanger( true, UIActiveManager.BUTTON.EFFECT_YES );
					_mainPhaseStatus = MAIN_PHASE_STATUS.EEFECT_RECOVERY;
					break;

				default:
					Debug.Log( "想定していない効果がステータスに来ています" );
					break;
			}

			_card.DeleteCardDetail( );
			return;
		}
	}
	//--------------------------------------------------------------------------------

	
	//カード移動状態処理----------------------------------------------------------------------------------------------------------------
	void MoveCardStatus( ) {
		List< Square > squares = _field.MovePossibleSquare( _card, _card.Card_Data._directionOfTravel, _nowSquare );

		_turnPlayer.SquareChangeColor( squares, true );		//移動できるマスの色を変える

		if ( _mainSceneOperation.MouseTouch( ) ) {
			Square square = _rayShooter.RayCastSquare( );	//マウスをクリックしたらレイを飛ばしてマスを取得する
			_turnPlayer.SquareChangeColor( squares, false );	//色をもとに戻す
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.BACK );

			if ( square != null ) {
				_turnPlayer.MoveCard( _card, _card.Card_Data._directionOfTravel, _card.Card_Data._necessaryAP, _nowSquare, square );	//移動できるかどうかを判定し移動できたら移動する
			}

			//情報リセット
			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		//戻るボタンを押したら
		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.BACK );
			_turnPlayer.SquareChangeColor( squares, false );	//色をもとに戻す

			//情報リセット
			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------


	//ダイレクトアタック処理----------------------------------------------------
	void DirectAttack( ) { 
		_turnPlayer.DirectAttack( _enemyPlayer, _card.Card_Data._necessaryAP, _card );
			GetDataReset( );
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
	}
	//--------------------------------------------------------------------------

	
	//手札操作状態処理----------------------------------------------------------------------
	void HnadOperationStatus( ) {
		_handCard.transform.position = _mainSceneOperation.getWorldMousePos( );
		List< Square > summonableSquares = _field.SummonSquare( _turnPlayer.gameObject.tag );

		if ( _turnPlayer.DecreaseMPointConfirmation( _handCard.Card_Data._necessaryMP ) ) { 
			_turnPlayer.SquareChangeColor( summonableSquares, true );
		}

		//手札を離したとき
		if ( !_mainSceneOperation.MouseConsecutivelyTouch( ) ) {
			if ( _mainSceneOperation.getHoldCount( ) <= SHOW_DETAILS_HOLD_TIME ) {	//離したのが一定時間内だったら
				_handCard.ShowCardDetail( );
				_uiActiveManager.ButtonActiveChanger( true, UIActiveManager.BUTTON.BACK );
				_turnPlayer.SquareChangeColor( summonableSquares, false );
				HandCardRestore( );
				_mainPhaseStatus = MAIN_PHASE_STATUS.HAND_CARD_DETAILS;
				return;
			} else {
				_turnPlayer.SquareChangeColor( summonableSquares, false );
				_mainPhaseStatus = MAIN_PHASE_STATUS.HANC_CARD_SUMMON;
				return;
			}
		}
	}
	//--------------------------------------------------------------------------------------
	

	//手札の詳細表示状態-------------------------------------------------------------------
	void HnadCardDetailsStatus( ) { 
		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_handCard.DeleteCardDetail( );
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.BACK );
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
		}
	}
	//-------------------------------------------------------------------------------------

	
	//召喚処理--------------------------------------------------------------------------------
	void HandCardSummon( ) {
		List< Square > summonableSquares = _field.SummonSquare( _turnPlayer.gameObject.tag );

		//MPが足りなかったら
		if ( !_turnPlayer.DecreaseMPointConfirmation( _handCard.Card_Data._necessaryMP ) ) {
			HandCardRestore( );
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		Square rayHitedSquare = _rayShooter.RayCastSquare( );
		//離したときにマスがなかったら
		if ( rayHitedSquare == null ) {
			HandCardRestore( );
			_handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		//召喚
		for ( int i = 0; i < summonableSquares.Count; i++ ) { 
			if ( rayHitedSquare.Index != summonableSquares[ i ].Index ) continue;
		
			_turnPlayer.SquareChangeColor( summonableSquares, false );
			_turnPlayer.Summon( _handCard, rayHitedSquare, _turnPlayer.gameObject.tag );
			_handCard = null;
			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}



		_turnPlayer.SquareChangeColor( summonableSquares, false );
		HandCardRestore( );
		_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
	}
	//----------------------------------------------------------------------------------------


	void HandCardRestore( ) { 
        _handCard.transform.position = _handCardPos;
        _handCard.gameObject.GetComponent< BoxCollider2D >( ).enabled = true;
        _handCard.CardExpansionRestore( );
    }


	//攻撃効果中処理------------------------------------------------------------------------------
	void AttackEffect( ) {
		List< Square > squares = _field.AttackEffectPossibleOnCardSquare( _card, _nowSquare );
		_turnPlayer.SquareChangeColor( squares, true );

		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_turnPlayer.SquareChangeColor( squares, false );
			_uiActiveManager.AllButtonActiveChanger( false );
	
			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		if ( _mainSceneOperation.EffectYesButtonClicked( ) ) {
			_turnPlayer.SquareChangeColor( squares, false );
			_turnPlayer.UseEffect( _card, _nowSquare );
			_uiActiveManager.AllButtonActiveChanger( false );

			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
	}
	//--------------------------------------------------------------------------------------------


	//移動効果中処理---------------------------------------------------------------------------------
	void MoveEffect( ) {
		List< Square > squares = _field.MovePossibleSquare( _card, _card.Card_Data._effect_direction, _nowSquare, _card.Card_Data._effect_distance );

		_turnPlayer.SquareChangeColor( squares, true );

		if ( _mainSceneOperation.MouseTouch( ) ) {
			Square square = _rayShooter.RayCastSquare( );		//マウスをクリックしたらレイを飛ばしてマスを取得する
			_turnPlayer.SquareChangeColor( squares, false );	//色をもとに戻す
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.BACK );

			if ( square != null ) {
				_turnPlayer.UseEffect( _card, _nowSquare, square );	//移動できるかどうかを判定し移動できたら移動する
			}
			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}

		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_turnPlayer.SquareChangeColor( squares, false );
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.BACK );

			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//---------------------------------------------------------------------------------------------------


	//回復効果中処理----------------------------------------------
	void RecoveryEffect( ) {
		if ( _mainSceneOperation.BackButtonClicked( ) ) {
			_uiActiveManager.AllButtonActiveChanger( false );

			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
		
		if ( _mainSceneOperation.EffectYesButtonClicked( ) ) {
			_turnPlayer.UseEffect( _card );
			_uiActiveManager.AllButtonActiveChanger( false );

			GetDataReset( );

			_mainPhaseStatus = MAIN_PHASE_STATUS.IDLE;
			return;
		}
	}
	//--------------------------------------------------------------


	void GetDataReset( ) { 
        _card = null;
        _nowSquare = null;
    }
	

	void ActiveTurnEndButton( ) { 
		if ( _mainPhaseStatus == MAIN_PHASE_STATUS.IDLE && !_turnEndButton.activeInHierarchy ) { 
			_uiActiveManager.ButtonActiveChanger( true, UIActiveManager.BUTTON.TURN_END_COLOR );
		}

		if ( _mainPhaseStatus != MAIN_PHASE_STATUS.IDLE && _turnEndButton.activeInHierarchy ) { 
			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.TURN_END_COLOR );
		}
	}

	void TurnEndButtonClicked( ) { 
		if ( _mainSceneOperation.TurnEndButtonClicked( ) ) {
			if ( _precedenceOneTurnFlag ) _precedenceOneTurnFlag = false;

			_uiActiveManager.ButtonActiveChanger( false, UIActiveManager.BUTTON.TURN_END_COLOR );
			_turnEndFlag = true;
		}
	}


	void InterruptionButtonClicked( ) {
		if ( _mainSceneOperation.InterruptionButtonClicked( ) )	{ 
			_uiActiveManager.InterruptionButtonActiveChanger( true, UIActiveManager.INTERRUPTION.INTERRUPTION_YES, UIActiveManager.INTERRUPTION.INTERRUPTION_NO );
			_uiActiveManager.InterruptionButtonActiveChanger( false, UIActiveManager.INTERRUPTION.INTERRUPTION );
		}
	}

	void Interruption( ) { 
		if ( _mainSceneOperation.InterruptionYesButtonClicked( ) ) { 
			_uiActiveManager.InterruptionButtonActiveChanger( false, UIActiveManager.INTERRUPTION.INTERRUPTION_YES, UIActiveManager.INTERRUPTION.INTERRUPTION_NO );
			_precedenceOneTurnFlag = true;
			_sceneTransition.Transition( "Title" );
		}

		if ( _mainSceneOperation.InterruptionNoButtonClicked( ) ) { 
			_uiActiveManager.InterruptionButtonActiveChanger( false, UIActiveManager.INTERRUPTION.INTERRUPTION_YES, UIActiveManager.INTERRUPTION.INTERRUPTION_NO );
			_uiActiveManager.InterruptionButtonActiveChanger( true, UIActiveManager.INTERRUPTION.INTERRUPTION );
		}
	}


	void LoseTerms( ) { 
		if ( _enemyPlayer.Lefe_Point <= 0 ) { 
			_enemyPlayer.Lose_Flag = true;
			_precedenceOneTurnFlag = true;
		}

		if ( _enemyPlayer.Cemetary_Point >= LOSE_CEMETARY_POINT ) { 
			_enemyPlayer.Lose_Flag = true;	
			_precedenceOneTurnFlag = true;
		}

		if ( _turnPlayer.Cemetary_Point >= LOSE_CEMETARY_POINT ) { 
			_turnPlayer.Lose_Flag = true;	
			_precedenceOneTurnFlag = true;
		}
	}
}


//多分マスを連打しまくると計算やばそう

//MediatorパターンyやObserverパターンなどを使って(多分意味的にはMediatorパターン？)ボタンやマウスが押されたら通知してもらってそのときだけそれに対するupdateをする、
//などをすると無駄な処理が減るかも？しかし、相互参照になるため要相談
//例えば移動ボタンが押されたときに通知してさらにカード詳細状態だったら処理する。逆にマウスがクリックされたときにカード詳細状態だったら処理しないなど

//今現在、普通によくないプログラム。あっちこっち変更しないといけなくて可読性もよくない。条件分岐もありすぎてごり押し感半端ない

//ボタンの表示を全部消すときに二つ以上あったら全部表示を切り替える処理、一つだけなら指定のものだけ表示を切り替える処理でやっている。
//無駄な処理をしている感じなのだろか？もしくはわかりづらいだろうか？


//中断処理を無理やり追加。あとでリファクタリングするべし
//デッキデータがリセットされないバグあり
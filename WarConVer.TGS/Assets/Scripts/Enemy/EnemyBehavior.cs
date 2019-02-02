using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyBehavior : MonoBehaviour {
	//エネミーの行動状態
	enum ENEMY_BEHAVIOR_STATUS { 
		SUMMON,	
		DIRECT_ATTACK,
		MOVE,
	}

	//移動する向きの優先順位
	enum PRIORITY_DIRECTION { 
		RIGHT_PRIORITY,
		LEFT_PRIORITY,
		NORMAR,
	}

	const int MAX_SUMMON_CARD_NUM = 3;

	[ SerializeField ] Field _field = null;
	[ SerializeField ] Participant _enemy = null;
	[ SerializeField ] Participant _player = null;
	[ SerializeField ] Hand _enemyHand = null;
	[ SerializeField ] Point _enemyMagicPoint = null;
	[ SerializeField ] Point _enemyActivePoint = null;
	[ SerializeField ] float _enemyUpdateTime = 3.0f;

	ENEMY_BEHAVIOR_STATUS _enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
	bool _enemyUpdateFlag = true;

	public bool Enemy_Update_Flag { 
		get { return _enemyUpdateFlag; }
		set { _enemyUpdateFlag = value; }
	}



	public void StartEnemyUpdate( ) {
		string coroutineName = "EnemyUpdate"; 
		StartCoroutine( coroutineName );		//代入しなおさないと止めた途中から始まってしまうらしい
	}

	public void StopEnemyUpdate( ) { 
		StopCoroutine( "EnemyUpdate" );	
	}


	IEnumerator EnemyUpdate( ) {

		while ( true ) {
			if ( !_enemyUpdateFlag ) yield break;

			if ( MainPhase._precedenceOneTurnFlag && _enemyBehaviorStatus != ENEMY_BEHAVIOR_STATUS.SUMMON ) { 
				_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
				_enemyUpdateFlag = false;
				yield break;
			}

			switch ( _enemyBehaviorStatus ) { 
				case ENEMY_BEHAVIOR_STATUS.SUMMON:
					EnemySummonUpdate( );
					Debug.Log( "S" );
					break;

				case ENEMY_BEHAVIOR_STATUS.DIRECT_ATTACK:
					EnemyDirectAttackUpdate( );
					Debug.Log( "D" );
					break;

				case ENEMY_BEHAVIOR_STATUS.MOVE:
					EnemyCardMoveUpdate( );
					Debug.Log( "M" );
					break;

				default:
					Assert.IsTrue( false, "エネミーの状態が想定外です" );
					break;
			}
			yield return new WaitForSeconds( _enemyUpdateTime );

		}

	}


	//召喚処理Update-------------------------------------------------------------------
	void EnemySummonUpdate( ) {

		List< Square > summonSquares = _field.SummonSquare( _enemy.gameObject.tag );
		if ( summonSquares.Count == 0 ) {
			_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.DIRECT_ATTACK;
			return;
		}
			

		CardMain summonCard = null;
		summonCard = SummonCardSearch( );
		if ( summonCard == null ) {
			_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.DIRECT_ATTACK;
			return;
		}


		Square summonSquare = null;
		//第一条件
		summonSquare = FirstPrioritySquareSearch( );
		if ( summonSquare != null ) {
			Summon( summonCard, summonSquare );
			return;
		}

		//第二条件
		summonSquare = SecondPrioritySquareSearch( );
		if ( summonSquare != null ) { 
			Summon( summonCard, summonSquare );
			return;
		}
		
		//第三条件
		summonSquare = ThirdPrioritySquareSearch( );
		if ( summonSquare != null ) { 
			Summon( summonCard, summonSquare );
			return;
		}

		if ( summonSquare == null ) {
			_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.DIRECT_ATTACK;
			return;
		}

	}
	//-----------------------------------------------------------------------------------------

	
	//ダイレクトアタック処理Update------------------------------------------------------------------------
	void EnemyDirectAttackUpdate( ) { 

		for ( int i = 0; i < ConstantStorehouse.SQUARE_ROW_NUM; i++ ) { 
			Square square = _field.getSquare( ( ConstantStorehouse.SQUARE_ROW_NUM * ConstantStorehouse.FIFTH_ROW_INDEX ) + i );	// ( 横のマス数 ×　五列目 ) + 左からの番号

			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != ConstantStorehouse.TAG_PLAYER2 ) continue;

			CardMain directAttackCard = square.On_Card;
			int nowAP = _enemyActivePoint.Point_Num;
			if ( directAttackCard.Card_Data._necessaryAP > nowAP ) continue;
			if ( directAttackCard.Action_Count >= directAttackCard.MAX_ACTION_COUNT ) continue;

			_enemy.DirectAttack( _player, directAttackCard.Card_Data._necessaryAP, directAttackCard );
			return;
		}

		_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.MOVE;
	}
	//-------------------------------------------------------------------------------------------------------


	//移動処理Update--------------------------------------------------
	void EnemyCardMoveUpdate( ) {

		if ( _enemy.Card_In_Field.Count == 0 ) {
			_enemyUpdateFlag = false;
			_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
			return;
		} 

		MoveCardSearch( );
	}
	//---------------------------------------------------------------

	
	//召喚するカードを探す-----------------------------------------------------
	CardMain SummonCardSearch( ) { 
		List< CardMain > _handCard = _enemyHand.Card;

		if ( _enemy.Card_In_Field.Count >= MAX_SUMMON_CARD_NUM ) return null; 

		//手札のMPを見て召喚できるカードを取り出す
		int max_mp = -1;
		CardMain summonCard = null;
		for ( int i = 0; i < _handCard.Count; i++ ) { 
			int nowMP = _enemyMagicPoint.Point_Num;

			if ( _handCard[ i ].Card_Data._necessaryMP > nowMP ) continue;	
			if( max_mp > _handCard[ i ].Card_Data._necessaryMP ) continue;

			if ( max_mp < _handCard[ i ].Card_Data._necessaryMP ) {
				//mpが大きかったら
				max_mp = _handCard[ i ].Card_Data._necessaryMP;
				summonCard = _handCard[ i ];
			} else { 
				//同じmpだったら
				int cardStatusTotal = _handCard[ i ].Card_Data._attack + _handCard[ i ].Card_Data._toughness;
				int summonCardStatusTotal = summonCard.Card_Data._attack + summonCard.Card_Data._toughness;
				
				//体力と攻撃力の合計比較
				if ( cardStatusTotal < summonCardStatusTotal ) continue;
				if ( cardStatusTotal > summonCardStatusTotal ) { 
					summonCard = _handCard[ i ];
					continue;
				}

				//移動方向の多さ比較
				if ( _handCard[ i ].Card_Data._directionOfTravel.Count < summonCard.Card_Data._directionOfTravel.Count ) continue;
				if ( _handCard[ i ].Card_Data._directionOfTravel.Count > summonCard.Card_Data._directionOfTravel.Count ) { 
					summonCard = _handCard[ i ];
					continue;
				}
			}
		}

		return summonCard;
	}
	//---------------------------------------------------------------------------------------------------------------------------------


	//第一優先召喚マス探し-----------------------------------------------------------------------
	Square FirstPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < ConstantStorehouse.SQUARE_ROW_NUM; i++ ) {	//前列は４マスまでしかない
			Square square = null;
			square = _field.getSquare( i );

			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag == _enemy.gameObject.tag ) continue;

			summonSquare = FirstPrioritySquareSearchAlgorithm( square );
			return summonSquare;
		}

		return null;
	}
	//-------------------------------------------------------------------------------------------


	//第一優先召喚マス探しの処理(ネストを浅くするため)-----------------------------------
	//あとでマジックナンバーを修正する
	Square FirstPrioritySquareSearchAlgorithm( Square onCardSquare ) {
		int index = onCardSquare.Index;
		for ( int j = 1; j < 4; j++ ) {	//横一列を探すのは最大三回まで(カードがあった場所の左右から探すという意味)
			Square summonSquare = null;

			if ( index + j < 4 ) {	//一列目の左端を超えてなかったら
				summonSquare = _field.getSquare( index + j );
				if ( summonSquare.On_Card == null ) { 
					return summonSquare;
				}
			}

			if ( index - j > -1 ) { //一列目の右端を超えていなかったら
				summonSquare = _field.getSquare( index - j );	
				if ( summonSquare.On_Card == null ) { 
					return summonSquare;
				}
			}
			
		}

		return null;
	}
	//----------------------------------------------------------------------------------


	//第二優先召喚マス探し-----------------------------------------------------------------
	Square SecondPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < _field.Max_Index - ConstantStorehouse.SQUARE_ROW_NUM; i++ ) {	//一列目を除くマスの数
			Square square = null;
			square = _field.getSquare( i + ConstantStorehouse.SQUARE_ROW_NUM );	//一列目を省く

			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag == _enemy.gameObject.tag ) continue;

			int summonIndex = square.Index % 4;
			summonSquare = _field.getSquare( summonIndex );
			if ( summonSquare.On_Card != null ) continue;
			return summonSquare;
			
		}

		return null;
	}
	//-----------------------------------------------------------------------------------


	//第三優先召喚マス探し-----------------------------------
	//出す順番が逆っぽいからあとで調べる順番を逆にする
	Square ThirdPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < ConstantStorehouse.SQUARE_ROW_NUM; i++ ) { 
			Square square = null;	
			square = _field.getSquare( i );
			if ( square.On_Card == null ) {
				summonSquare = square;
				return summonSquare;	
			}
		}
		
		return null;
	}
	//-------------------------------------------------------

	
	//移動するカードを探す-------------------------------------------------------------------------
	void MoveCardSearch( ) {
		if ( FirstPriorityMoveCard( ) ) { 
			return;	
		}

		if ( SecondPriorityMoveCard( ) ) { 
			return;	
		}

		_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
		_enemyUpdateFlag = false;
	}
	//------------------------------------------------------------------------------------------------


	//第一優先カード移動-------------------------------------------------------------------------------------
	//移動したかどうかをboolで返す
	bool FirstPriorityMoveCard( ) { 

		for ( int i = 0; i < ConstantStorehouse.SQUARE_ROW_NUM; i++ ) { 
			Square square = _field.getSquare( i );	
			
			if ( square == null ) continue;
			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != ConstantStorehouse.TAG_PLAYER1 ) continue;

			Square onEnemyCardSquare = null;
			PRIORITY_DIRECTION priority_direction = PRIORITY_DIRECTION.NORMAR;
			if ( i + 1 < 4 ) {	//一列目の左端を超えてなかったら
				onEnemyCardSquare = _field.getSquare( i + 1 );
				priority_direction = PRIORITY_DIRECTION.RIGHT_PRIORITY;
			}

			if ( i - 1 > -1 ) { //一列目の右端を超えていなかったら
				if ( onEnemyCardSquare == null ) {
					onEnemyCardSquare = _field.getSquare( i - 1 );
					priority_direction = PRIORITY_DIRECTION.LEFT_PRIORITY;
				}
			}

			if ( onEnemyCardSquare == null ) continue;
			if ( onEnemyCardSquare.On_Card == null ) continue;
			if ( onEnemyCardSquare.On_Card.gameObject.tag != ConstantStorehouse.TAG_PLAYER2 ) continue;

			CardMain enemyMoveCard = null;
			enemyMoveCard = onEnemyCardSquare.On_Card;

			int nowAP = _enemyActivePoint.Point_Num;
			if ( enemyMoveCard.Card_Data._necessaryAP > nowAP ) continue;
			if ( enemyMoveCard.Action_Count >= enemyMoveCard.MAX_ACTION_COUNT ) continue;

			if ( !EnemyCardMove( enemyMoveCard, onEnemyCardSquare, priority_direction ) ) continue;

			return true;

		}

		return false;
	}
	//------------------------------------------------------------------------------------------------------

	
	//第二優先カード移動-------------------------------------------------------------------------------------
	//移動したかどうかをboolで返す
	bool SecondPriorityMoveCard( ) {
		for ( int i = 0; i < _field.Max_Index; i++ ) {
			Square square = _field.getSquare( i );

			if ( square == null ) continue;
			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != ConstantStorehouse.TAG_PLAYER2 ) continue;

			CardMain enemyMoveCard = null;
			enemyMoveCard = square.On_Card;
			int nowAP = _enemyActivePoint.Point_Num;
			if ( enemyMoveCard.Card_Data._necessaryAP > nowAP ) continue;
			if ( enemyMoveCard.Action_Count >= enemyMoveCard.MAX_ACTION_COUNT ) continue;

			if ( !EnemyCardMove( enemyMoveCard, square, PRIORITY_DIRECTION.NORMAR ) ) continue ;

			return true;
		}

		return false;
	}
	//-----------------------------------------------------------------------------------------------------


	//移動できたら移動して移動できたかどうかを返す-----------------------------------------------------------
	bool EnemyCardMove( CardMain enemyMoveCard, Square nowSquare, PRIORITY_DIRECTION priority ) { 
		List< Field.DIRECTION > directions = EnemyDirectionSorting( enemyMoveCard, priority );
		if ( directions.Count == 0 ) return false;


		List< Field.DIRECTION > enemyDirection = new List< Field.DIRECTION >( );
		for ( int i = 0; i < directions.Count; i++ ) {
			enemyDirection.Add( directions[ i ] );
			
			List< Field.DIRECTION > preDirection = enemyMoveCard.Card_Data_Direction;
			enemyMoveCard.Card_Data_Direction = enemyDirection;  //一時的にエネミーのカードの移動先を書き換えている。

			List< Square > moveSquare = _field.MovePossibleSquare( enemyMoveCard, enemyMoveCard.Card_Data._directionOfTravel, nowSquare );

			if ( moveSquare.Count == 0 ) {
				enemyMoveCard.Card_Data_Direction = preDirection;
				enemyDirection.Remove( directions[ i ] );
				continue;
			}

			MoveCard( enemyMoveCard, nowSquare, moveSquare[ 0 ] );		//あとでこの関数の役割を分けないといけない
			enemyMoveCard.Card_Data_Direction = preDirection;
			return true;

		}

		return false;
	}
	//-------------------------------------------------------------------------------------------------------
	

	//エネミーカードの移動したくてかつできる場所を優先順位が高い順に抽出する-------------------------
	List< Field.DIRECTION > EnemyDirectionSorting( CardMain card, PRIORITY_DIRECTION priorityDirection ) {
		Field.DIRECTION[ ] enemyDirections = null;

		switch ( priorityDirection ) { 
			case PRIORITY_DIRECTION.RIGHT_PRIORITY:
				Field.DIRECTION[ ] right = { Field.DIRECTION.RIGHT };
				enemyDirections = right;
				break;

			case PRIORITY_DIRECTION.LEFT_PRIORITY:
				Field.DIRECTION[ ] left = { Field.DIRECTION.LEFT };
				enemyDirections = left;
				break;

			case PRIORITY_DIRECTION.NORMAR:
				Field.DIRECTION[ ] normarDirections = { Field.DIRECTION.FORWAED,
													  Field.DIRECTION.LEFT_FORWARD,
													  Field.DIRECTION.RIGHT_FORWARD,
													  Field.DIRECTION.RIGHT,
													  Field.DIRECTION.LEFT };

				enemyDirections = normarDirections;
				break;

			
		}

		List< Field.DIRECTION > direction = new List< Field.DIRECTION >( );
		for ( int i = 0; i < enemyDirections.Length; i++ ) {

			for ( int j = 0; j < card.Card_Data._directionOfTravel.Count; j++ ) {
				if ( card.Card_Data._directionOfTravel[ j ] != enemyDirections[ i ]  ) continue;
				direction.Add( card.Card_Data._directionOfTravel[ j ] );
			}

		}
		
		return direction;
	}
	//-----------------------------------------------------------------------------------------------

	
	//召喚----------------------------------------------------------------
	void Summon( CardMain card, Square square ) { 
		_enemy.Summon( card, square, ConstantStorehouse.TAG_PLAYER2 );	
	}
	//---------------------------------------------------------------------


	//移動------------------------------------------------------------------
	void MoveCard( CardMain card, Square nowSquare, Square moveSquare ) { 
		_enemy.MoveCard( card, card.Card_Data._directionOfTravel, card.Card_Data._necessaryAP, nowSquare, moveSquare );	
	}
	//-----------------------------------------------------------------------
}

//リファクタリング必須
//相手カードは今はよけない
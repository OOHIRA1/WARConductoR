using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyBehavior : MonoBehaviour {
	enum ENEMY_BEHAVIOR_STATUS { 
		SUMMON,	
		DIRECT_ATTACK,
		MOVE,
	}

	[ SerializeField ] Field _field = null;
	[ SerializeField ] Participant _enemy = null;
	[ SerializeField ] Participant _player = null;
	[ SerializeField ] Hand _enemyHand = null;
	[ SerializeField ] Point _enemyMagicPoint = null;
	[ SerializeField ] Point _enemyActivePoint = null;

	ENEMY_BEHAVIOR_STATUS _enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
	bool _enemyUpdateFlag = true;

	public bool Enemy_Update_Flag { 
		get { return _enemyUpdateFlag; }
		set { _enemyUpdateFlag = value; }
	}


	public void EnemyUpdate( ) {
		if ( !_enemyUpdateFlag ) return;

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
	}

	void EnemySummonUpdate( ) {

		//List< Square > summonSquares = _enemy.SummonSquare( _enemy.gameObject.tag );
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

	
	void EnemyDirectAttackUpdate( ) { 

		for ( int i = 0; i < 4; i++ ) { 
			Square square = _field.getSquare( ( 4 * 4 ) + i );	// ( 横のマス数 ×　五列目 ) + 左からの番号

			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != "Player2" ) continue;

			CardMain directAttackCard = square.On_Card;
			int nowAP = _enemyActivePoint.Point_Num;
			if ( directAttackCard.Card_Data._necessaryAP > nowAP ) continue;

			_enemy.DirectAttack( _player, directAttackCard.Card_Data._necessaryAP );
			return;
		}

		_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.MOVE;
	}

	public void EnemyCardMoveUpdate( ) {

		if ( _enemy.Card_In_Field.Count == 0 ) {
			_enemyUpdateFlag = false;
			_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
			return;
		} 

		//前列にいたら攻撃
		MoveCardSearch( );
	}


	CardMain SummonCardSearch( ) { 
		List< CardMain > _handCard = _enemyHand.Card;

		if ( _enemy.Card_In_Field.Count >= 3 ) return null; 

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


	//名前はあとで変えることになる
	//第一処理
	Square FirstPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < 4; i++ ) {	//前列は４マスまでしかない
			Square square = null;
			square = _field.getSquare( i );

			//if ( square.Index / 4 != 0 ) continue;
			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag == _enemy.gameObject.tag ) continue;

			summonSquare = SummonSquareSearch( square );
			return summonSquare;
		}

		return null;
	}


	//名前はあとで変えることになる
	//あとでマジックナンバーを修正する
	Square SummonSquareSearch( Square onCardSquare ) {
		int index = onCardSquare.Index;
		for ( int j = 1; j < 4; j++ ) {	//横一列を探すのは最大三回まで(カードがあった場所の左右から探すという意味)
			Square summonSquare = null;
			//index++;

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


	//第二処理
	Square SecondPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < _field.Max_Index - 4; i++ ) {	//一列目を除くマスの数
			Square square = null;
			square = _field.getSquare( i + 4 );	//一列目を省く

			//if ( square.Index / 4 != 0 ) continue;
			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag == _enemy.gameObject.tag ) continue;

			int summonIndex = square.Index % 4;
			summonSquare = _field.getSquare( summonIndex );
			if ( summonSquare.On_Card != null ) continue;
			return summonSquare;
			
		}

		return null;
	}


	//第三処理
	//出す順番が逆っぽいからあとで調べる順番を逆にする
	Square ThirdPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < 4; i++ ) { 
			Square square = null;	
			square = _field.getSquare( i );
			if ( square.On_Card == null ) {
				summonSquare = square;
				return summonSquare;	
			}
		}
		
		return null;
	}


	void MoveCardSearch( ) {
		CardMain enemyMoveCard = null;
		for ( int i = 0; i < _field.Max_Index; i++ ) {
			Square square = _field.getSquare( i );

			if ( square == null ) continue;
			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != "Player2" ) continue;

			enemyMoveCard = square.On_Card;
			int nowAP = _enemyActivePoint.Point_Num;
			if ( enemyMoveCard.Card_Data._necessaryAP > nowAP ) continue;
			if ( enemyMoveCard.Action_Count >= enemyMoveCard.MAX_ACTION_COUNT ) continue;

			List< Field.DIRECTION > directions = EnemyDirectionSorting( enemyMoveCard );

			if ( directions.Count == 0 ) continue;



			List<Field.DIRECTION> enemyDirection = new List< Field.DIRECTION >( );

			for ( int j = 0; j < directions.Count; j++ ) {
				//enemyDirection[ 0 ] = directions[ j ];
				enemyDirection.Add( directions[ j ] );

				//変更してください！！！！！エラーになります！	→　CardDataのアクセッサーだとその先の_directionOfTravelまでアクセスできないっぽい
				//enemyMoveCard.Card_Data._directionOfTravel = enemyDirection;	//エネミーのカードの移動先を書き換えている。いいのかはわからない
				List< Field.DIRECTION > preDirection = enemyMoveCard.Card_Data_Direction;
				enemyMoveCard.Card_Data_Direction = enemyDirection;  // →　Card_Data._directionOfTravelのアクセッサーを用意したらいけた
				//############################################################################################################################################

				//List< Square > moveSquare = _enemy.MovePossibleSquare( enemyMoveCard, square );
				List< Square > moveSquare = _field.MovePossibleSquare( enemyMoveCard, square );

				//相手カードがあったりしたらよける処理

				if ( moveSquare.Count == 0 ) {
					enemyMoveCard.Card_Data_Direction = preDirection;
					enemyDirection.Remove( directions[ j ] );
					continue;
				}

				MoveCard( enemyMoveCard, square, moveSquare[ 0 ] );		//あとでこの関数の役割を分けないといけない
				enemyMoveCard.Card_Data_Direction = preDirection;
				return;
				//_enemy.MoveCard( enemyMoveCard, square, moveSquare[ 0 ] );	//方向を一つしか送っていないのでリストの中身も一つしか入らない。
			}
		}

		_enemyBehaviorStatus = ENEMY_BEHAVIOR_STATUS.SUMMON;
		_enemyUpdateFlag = false;
	}


	List< Field.DIRECTION > EnemyDirectionSorting( CardMain card ) {
		Field.DIRECTION[ ] enemyDirections = { Field.DIRECTION.FORWAED,
											   Field.DIRECTION.LEFT_FORWARD,
											   Field.DIRECTION.RIGHT_FORWARD,
											   Field.DIRECTION.RIGHT,
											   Field.DIRECTION.LEFT };

		List< Field.DIRECTION > direction = new List< Field.DIRECTION >( );
		for ( int i = 0; i < enemyDirections.Length; i++ ) {

			for ( int j = 0; j < card.Card_Data._directionOfTravel.Count; j++ ) {
				if ( card.Card_Data._directionOfTravel[ j ] != enemyDirections[ i ]  ) continue;
				direction.Add( card.Card_Data._directionOfTravel[ j ] );
			}

		}
		
		return direction;
	}
	
	void Summon( CardMain card, Square square ) { 
		_enemy.Summon( card, square, "Player2" );	
	}

	void MoveCard( CardMain card, Square nowSquare, Square moveSquare ) { 
		_enemy.MoveCard( card, nowSquare, moveSquare );	
	}

}

//リファクタリング必須
//相手カードは今はよけない
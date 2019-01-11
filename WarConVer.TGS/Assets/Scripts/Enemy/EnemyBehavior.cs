using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
	[ SerializeField ] Field _field = null;
	[ SerializeField ] Participant _enemy = null;
	[ SerializeField ] Hand _enemyHand = null;
	[ SerializeField ] Point _enemyMagicPoint = null;

	void Start( ) {
		
	}


	public void EnemySummonUpdate( ) { 
		List< Square > summonSquares = _enemy.SummonSquare( _enemy.gameObject.tag );
		if ( summonSquares.Count == 0 ) return;

		CardMain summonCard = null;
		summonCard = SummonCardSearch( );
		if ( summonCard == null ) return;


		Square summonSquare = null;
		//第一条件
		summonSquare = FirstPrioritySquareSearch( );
		if ( summonSquare != null ) { 
			//召喚
			return;
		}

		//第二条件
		summonSquare = SecondPrioritySquareSearch( );
		if ( summonSquare != null ) { 
			//召喚
			return;
		}

		//第三条件
		summonSquare = ThirdPrioritySquareSearch( );
		if ( summonSquare != null ) { 
			//召喚
			return;
		}

		if ( summonSquare == null ) return;
	}

	public void EnemyCardMoveUpdate( ) { 
		MoveCardSearch( );
	}


	CardMain SummonCardSearch( ) { 
		List< CardMain > _handCard = _enemyHand.Card;

		if ( _enemy.Card_In_Field.Count >= 3 ) return null; 

		//手札のMPを見て召喚できるカードを取り出す
		int max_mp = -1;
		CardMain summonCard = null;
		for ( int i = 0; _handCard.Count < i; i++ ) { 
			int nowMP = _enemyMagicPoint.Point_Num;

			if ( _handCard[ i ]._cardDates.mp > nowMP ) continue;	
			if( max_mp > _handCard[ i ]._cardDates.mp ) continue;

			if ( max_mp < _handCard[ i ]._cardDates.mp ) {
				//mpがおなじだったら
				max_mp = _handCard[ i ]._cardDates.mp;
				summonCard = _handCard[ i ];
			} else { 
				//同じmpだったら
				int cardStatusTotal = _handCard[ i ]._cardDates.attack_point + _handCard[ i ]._cardDates.hp;
				int summonCardStatusTotal = summonCard._cardDates.attack_point + summonCard._cardDates.hp;
				
				//体力と攻撃力の合計比較
				if ( cardStatusTotal < summonCardStatusTotal ) continue;
				if ( cardStatusTotal > summonCardStatusTotal ) { 
					summonCard = _handCard[ i ];
					continue;
				}

				//移動方向の多さ比較
				if ( _handCard[ i ]._cardDates.directions.Length < summonCard._cardDates.directions.Length ) continue;
				if ( _handCard[ i ]._cardDates.directions.Length > summonCard._cardDates.directions.Length ) { 
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

			summonSquare = SummonSquareSearch( );
		}

		return summonSquare;
	}


	//名前はあとで変えることになる
	Square SummonSquareSearch( ) {
		int index = 0;
		for ( int j = 0; j < 3; j++ ) {	//横一列を探すのは最大三回まで
			Square summonSquare = null;
			index++;

			if ( j + index > 4 ) {	//一列目の左端を超えてなかったら
				summonSquare = _field.getSquare( j + index );
				if ( summonSquare.On_Card == null ) { 
					return summonSquare;
				}
			}

			if ( j - index > -1 ) { //一列目の右端を超えていなかったら
				summonSquare = _field.getSquare( j - index );	
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

		return summonSquare;
	}


	//第三処理
	Square ThirdPrioritySquareSearch( ) { 
		Square summonSquare = null;

		for ( int i = 0; i < 4; i++ ) { 
			Square square = null;	
			square = _field.getSquare( i );
			if ( square.On_Card == null ) { 
				return summonSquare;	
			}
		}
		
		return summonSquare;
	}


	void MoveCardSearch( ) {
		CardMain enemyMoveCard = null;
		for ( int i = 0; i < _field.Max_Index; i++ ) {
			Square square = _field.getSquare( i );

			if ( square == null ) continue;
			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != "Player2" ) continue;

			enemyMoveCard = square.On_Card;
			List< Field.DIRECTION > directions = EnemyDirectionSorting( enemyMoveCard );

			if ( directions == null ) continue;



			Field.DIRECTION[ ] enemyDirection = new Field.DIRECTION[ 1 ];

			for ( int j = 0; j < directions.Count; j++ ) { 
				enemyDirection[ 0 ] = directions[ j ];
				enemyMoveCard._cardDates.directions = enemyDirection;	//エネミーのカードの移動先を書き換えている。いいのかはわからない
				List< Square > moveSquare = _enemy.MovePossibleSquare( enemyMoveCard, square );

				if ( moveSquare == null ) continue;

				//_enemy.MoveCard( enemyMoveCard, square, moveSquare[ 0 ] );	//方向を一つしか送っていないのでリストの中身も一つしか入らない。
			}


			
		}
	}


	List< Field.DIRECTION > EnemyDirectionSorting( CardMain card ) {
		Field.DIRECTION[ ] enemyDirections = { Field.DIRECTION.FORWAED,
											   Field.DIRECTION.LEFT_FORWARD,
											   Field.DIRECTION.RIGHT_FORWARD };

		List< Field.DIRECTION > direction = new List< Field.DIRECTION >( );
		for ( int i = 0; i < enemyDirections.Length; i++ ) {

			for ( int j = 0; j < card._cardDates.directions.Length; j++ ) {
				if ( card._cardDates.directions[ j ] != enemyDirections[ i ]  ) continue;
				direction.Add( card._cardDates.directions[ j ] );
			}

		}
		
		return direction;
	}
}

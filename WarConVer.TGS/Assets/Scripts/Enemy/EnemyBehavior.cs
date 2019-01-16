using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
	[ SerializeField ] Field _field = null;
	[ SerializeField ] Participant _enemy = null;
	[ SerializeField ] Hand _enemyHand = null;
	[ SerializeField ] Point _enemyMagicPoint = null;
	[ SerializeField ] Point _enemyActivePoint = null;

	bool _summonUpdateFlag = true;
	bool _cardMoveUpdateFlag = true;
	bool _directAttackUpdateFlag = true;

	public bool Summon_Update_Flag { 
		get { return _summonUpdateFlag; }	
	}

	public bool Card_MoveUpdate_Flag { 
		get { return _cardMoveUpdateFlag; }	
	}

	public bool Direct_Attack_Update_Flag { 
		get { return _directAttackUpdateFlag; }	
	}


	public void EnemySummonUpdate( ) {
		if ( !_summonUpdateFlag ) return;

		//List< Square > summonSquares = _enemy.SummonSquare( _enemy.gameObject.tag );
		List< Square > summonSquares = _field.SummonSquare( _enemy.gameObject.tag );
		if ( summonSquares.Count == 0 ) {
			_summonUpdateFlag = false;
			return;
		}
			

		CardMain summonCard = null;
		summonCard = SummonCardSearch( );
		if ( summonCard == null ) {
			_summonUpdateFlag = false;
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
			_summonUpdateFlag = false;
			return;
		}

	}

	
	public void EnemyDirectAttackUpdate( ) { 
		if ( !_directAttackUpdateFlag ) return;
		if ( _summonUpdateFlag ) return;

		for ( int i = 0; i < 4; i++ ) { 
			Square square = _field.getSquare( ( 4 * 4 ) + i );	// ( 横のマス数 ×　五列目 ) + 左からの番号

			if ( square.On_Card == null ) continue;
			if ( square.On_Card.gameObject.tag != "Player2" ) continue;

			CardMain directAttackCard = square.On_Card;
			int nowAP = _enemyActivePoint.Point_Num;
			if ( directAttackCard._cardDates.mp > nowAP ) continue;

			_enemy.DirectAttack( _enemy, directAttackCard._cardDates.mp );
			return;
		}

		_directAttackUpdateFlag = false;
	}

	public void EnemyCardMoveUpdate( ) {
		if ( !_cardMoveUpdateFlag ) return;
		if ( _directAttackUpdateFlag ) return;
		if ( _summonUpdateFlag ) return;
		//_cardMoveUpdateFlag = false;

		if ( _enemy.Card_In_Field.Count == 0 ) {
			_cardMoveUpdateFlag = false;
			return;
		} 

		//前列にいたら攻撃
		MoveCardSearch( );
	}


	public void TestSummonUpdateFlag( ) {
		if ( _summonUpdateFlag ) return;	//意味ないかもだけど一応処理が終わるまで操作できないようにするために

		_summonUpdateFlag = true;	
	}

	public void TestCardMoveUpdateFlag( ) { 
		if ( _cardMoveUpdateFlag ) return;

		_cardMoveUpdateFlag = true;
	}


	CardMain SummonCardSearch( ) { 
		List< CardMain > _handCard = _enemyHand.Card;

		if ( _enemy.Card_In_Field.Count >= 3 ) return null; 

		//手札のMPを見て召喚できるカードを取り出す
		int max_mp = -1;
		CardMain summonCard = null;
		for ( int i = 0; i < _handCard.Count; i++ ) { 
			int nowMP = _enemyMagicPoint.Point_Num;

			if ( _handCard[ i ]._cardDates.mp > nowMP ) continue;	
			if( max_mp > _handCard[ i ]._cardDates.mp ) continue;

			if ( max_mp < _handCard[ i ]._cardDates.mp ) {
				//mpが大きかったら
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
			if ( enemyMoveCard._cardDates.mp > nowAP ) continue;

			List< Field.DIRECTION > directions = EnemyDirectionSorting( enemyMoveCard );

			if ( directions.Count == 0 ) continue;



			Field.DIRECTION[ ] enemyDirection = new Field.DIRECTION[ 1 ];

			for ( int j = 0; j < directions.Count; j++ ) { 
				enemyDirection[ 0 ] = directions[ j ];
				enemyMoveCard._cardDates.directions = enemyDirection;	//エネミーのカードの移動先を書き換えている。いいのかはわからない
				//List< Square > moveSquare = _enemy.MovePossibleSquare( enemyMoveCard, square );
				List< Square > moveSquare = _field.MovePossibleSquare( enemyMoveCard, square );

				if ( moveSquare.Count == 0 ) continue;

				MoveCard( enemyMoveCard, square, moveSquare[ 0 ] );		//あとでこの関数の役割を分けないといけない
				return;
				//_enemy.MoveCard( enemyMoveCard, square, moveSquare[ 0 ] );	//方向を一つしか送っていないのでリストの中身も一つしか入らない。
			}
		}

		_cardMoveUpdateFlag = false;
	}


	List< Field.DIRECTION > EnemyDirectionSorting( CardMain card ) {
		Field.DIRECTION[ ] enemyDirections = { Field.DIRECTION.FORWAED,
											   Field.DIRECTION.LEFT_FORWARD,
											   Field.DIRECTION.RIGHT_FORWARD,
											   Field.DIRECTION.RIGHT,
											   Field.DIRECTION.LEFT };

		List< Field.DIRECTION > direction = new List< Field.DIRECTION >( );
		for ( int i = 0; i < enemyDirections.Length; i++ ) {

			for ( int j = 0; j < card._cardDates.directions.Length; j++ ) {
				if ( card._cardDates.directions[ j ] != enemyDirections[ i ]  ) continue;
				direction.Add( card._cardDates.directions[ j ] );
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

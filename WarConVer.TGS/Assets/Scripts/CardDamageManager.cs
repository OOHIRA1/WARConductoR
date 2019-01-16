using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDamageManager {
	//バトルの結果
	public enum BATTLE_RESULT {
		NOT_BATTLE,
		BOTH_DEATH,
		PLAYER_WIN,
		PLAYER_LOSE,
		BOTH_ALIVE,
	}

	//攻撃効果のダメージによる処理----------------------------------------
	public void CardEffectDamage( Square onCardSquare, int damage ) {
		if ( onCardSquare == null ) {
			Debug.Log( "[エラー]マスがありません" );
			return;
		}

		CardMain damageCard = onCardSquare.On_Card;
		damageCard.Damage( damage );

		if ( damageCard.CARD_DATA._toughness == 0 ) { 
			damageCard.Death( );
			onCardSquare.On_Card = null;
		}
	}
	//-----------------------------------------------------------------


	//戦闘のダメージ処理によって戦闘の結果を返す------------------------------------------------------
	public BATTLE_RESULT CardBattleDamage( Square onPlayerCardSquare, Square onEnemyCardSquare ) {
		if ( onPlayerCardSquare == null || onEnemyCardSquare == null ) { 
			Debug.Log( "[エラー]マスがありません" );
			return BATTLE_RESULT.NOT_BATTLE;
		}

		CardMain playerCard = onPlayerCardSquare.On_Card;
		CardMain enemy_card  = onEnemyCardSquare.On_Card;

		playerCard.Damage( enemy_card._cardDates.attack_point );
		enemy_card.Damage( playerCard._cardDates.attack_point );

		if ( playerCard.CARD_DATA._toughness == 0 && enemy_card.CARD_DATA._toughness == 0 ) {
			onPlayerCardSquare.On_Card = null;
			onEnemyCardSquare.On_Card  = null;
			return BATTLE_RESULT.BOTH_DEATH;
		}

		if ( enemy_card.CARD_DATA._toughness == 0 ) {	
			onEnemyCardSquare.On_Card = null;
			return BATTLE_RESULT.PLAYER_WIN;
		}

		if ( playerCard.CARD_DATA._toughness == 0 ) {	
			onEnemyCardSquare.On_Card = null;
			return BATTLE_RESULT.PLAYER_LOSE;
		}

		if ( playerCard.CARD_DATA._toughness > 0 && enemy_card.CARD_DATA._toughness > 0 ) {
			return BATTLE_RESULT.BOTH_ALIVE;
		}

		Debug.Log( "[エラー]戦闘の結果がおかしいです" );
		return BATTLE_RESULT.NOT_BATTLE;
	}
	//------------------------------------------------------------------------------------------------------
}

//なんか一つの関数でいろんな別の処理してる気がするけどいいのかわからない

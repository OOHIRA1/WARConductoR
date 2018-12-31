using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDamageManager {
	//バトルの結果
	public enum BATTLE_RESULT {
		NOT_BATTLE,
		BOTH_DEATH,
		PLAYER_WIN,
		PLAYER_DEFEAT,
		BOTH_ALIVE,
	}


	public void CardEffectDamage( Square on_card_square, int damage ) {
		if ( on_card_square == null ) {
			Debug.Log( "[エラー]与えられたマスにカードがありません" );
			return;
		}

		CardMain damage_card = on_card_square.On_Card;
		damage_card.Damage( damage );

		if ( damage_card._cardDates.hp == 0 ) { 
			damage_card.Death( );
			on_card_square.On_Card = null;
		}
	}


	public BATTLE_RESULT CardBattleDamage( Square on_player_card_square, Square on_enemy_card_square ) {
		if ( on_player_card_square == null || on_enemy_card_square == null ) { 
			Debug.Log( "[エラー]与えられたマスにカードがありません" );
			return BATTLE_RESULT.NOT_BATTLE;
		}

		CardMain player_card = on_player_card_square.On_Card;
		CardMain enemy_card  = on_enemy_card_square.On_Card;

		player_card.Damage( enemy_card._cardDates.attack_point );
		enemy_card.Damage( player_card._cardDates.attack_point );

		if ( player_card._cardDates.hp == 0 && enemy_card._cardDates.hp == 0 ) {
			on_player_card_square.On_Card = null;
			on_enemy_card_square.On_Card  = null;
			return BATTLE_RESULT.BOTH_DEATH;
		}

		if ( enemy_card._cardDates.hp == 0 ) {	
			on_enemy_card_square.On_Card = null;
			return BATTLE_RESULT.PLAYER_WIN;
		}

		if ( player_card._cardDates.hp == 0 ) {	
			on_player_card_square.On_Card = null;
			return BATTLE_RESULT.PLAYER_DEFEAT;
		}

		if ( player_card._cardDates.hp > 0 && enemy_card._cardDates.hp > 0 ) {
			return BATTLE_RESULT.BOTH_ALIVE;
		}

		Debug.Log( "[エラー]戦闘の結果がおかしいです" );
		return BATTLE_RESULT.NOT_BATTLE;
	}
}

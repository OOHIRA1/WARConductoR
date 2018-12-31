using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManeger : MonoBehaviour {
	enum PHASE { 
		START,
		DRAW,
		MAIN,
		END,
	}

	[ SerializeField ] MainSceneOperation _main_scene_operation = null;
	[ SerializeField ] Participant _player1 = null;
	[ SerializeField ] Participant _player2 = null;
	
	Phase _phase = null;
	PHASE _phase_status = PHASE.START;

	//ボタン
	[ SerializeField ] GameObject _return_button		= null;
	[ SerializeField ] GameObject _move_button			= null;
	[ SerializeField ] GameObject _direct_attack_button = null;
	[ SerializeField ] GameObject _effect_button		= null;
	[ SerializeField ] GameObject _effect_yes_buuton	= null;

	//詳細系
	[ SerializeField ] GameObject _card_details_image = null;	//生成する詳細画像のプレハブ
	[ SerializeField ] GameObject _canvas			  = null;	//生成したあとに子にするため

	//テスト用
	[ SerializeField ] Square _now_square    = null;
	[ SerializeField ] CardMain _card		 = null;
	[ SerializeField ] CardMain _enemy_card  = null;
	[ SerializeField ] Square _enemy_square  = null;
	[ SerializeField ] CardMain _draw_card   = null;


	void Start( ) {
		_now_square.On_Card = _card;
		_card.gameObject.transform.position = _now_square.transform.position;
		_enemy_card.transform.position = _enemy_square.transform.position;
		_enemy_square.On_Card = _enemy_card;

		if ( _main_scene_operation == null ) { 
			Debug.Log( "[エラー]MainSceneOperationが参照を取れていない" );	
		}

		if ( _player1 == null ) { 
			Debug.Log( "[エラー]Participant(Player1)が参照を取れていない" );	
		}

		if ( _player2 == null ) { 
			Debug.Log( "[エラー]Participant(Player2)が参照を取れていない" );	
		}
	}

	
	void Update( ) {

		if ( _main_scene_operation == null ) {
			return;
		}

		if ( _player1 == null ) { 
			return;
		}

		if ( _player2 == null ) { 
			return;
		}

		if ( Input.GetKeyDown( KeyCode.A ) ) {
			ChangePhase( );
			_phase_status++;
			if ( ( int )_phase_status > ( int )PHASE.END ) { 
				_phase_status = PHASE.START;	
			}
		}

		if ( _phase == null ) return;

		_phase.PhaseUpdate( );

	}

	void ChangePhase( ) {
		if ( _phase != null ) { 
			_phase = null;	
		}

		switch ( _phase_status ) { 
			case PHASE.START:
				_phase = new StartPhase( );
				break;

			case PHASE.DRAW:
				_phase = new DrawPhase( );
				break;

			case PHASE.MAIN:
				_phase = new MainPhase( _player1, _player2, _main_scene_operation,
										_return_button, _move_button, _direct_attack_button, _effect_button, _effect_yes_buuton,
										_card_details_image, _canvas,
										_now_square, _card, _enemy_card, _enemy_square, _draw_card );
				break;

			case PHASE.END:
				_phase = new EndPhase( );
				break;
			
		}
	}
}

//MainPhaseに送るプレイヤーの参照は一つにしてターンによって送る参照を切り替えるようにしたほうがいいかも？
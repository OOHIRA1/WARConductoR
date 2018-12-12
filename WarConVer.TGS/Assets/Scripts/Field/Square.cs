using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
	[ SerializeField ] int _index = 0;	//マスの通し番号
	SpriteRenderer _sprite_renderer = null;
	Color _red = Color.red;
	Color originally_color = new Color( );
	CardMain _on_card = null;

	public CardMain On_Card { 
		get { return _on_card; }
		set { _on_card = value; }
	}

	void Awake( ) { 
		_sprite_renderer = gameObject.GetComponent< SpriteRenderer >( );
	}

	void Start( ) {
		originally_color = _sprite_renderer.color;
	}

	public int Index {
		get { return _index; }
		private set { _index = value; }
	}

	public void ChangeColor( bool is_red_flag ) {
		if ( is_red_flag ) { 
			_sprite_renderer.color = _red;
		} else { 
			_sprite_renderer.color = originally_color;	
		}
	}
}

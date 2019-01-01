using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
	[ SerializeField ] int _index = 0;	//マスの通し番号
	SpriteRenderer _spriteRenderer = null;
	Color _red = Color.red;
	Color _originallyColor = new Color( );
	CardMain _on_card = null;

	public CardMain On_Card { 
		get { return _on_card; }
		set { _on_card = value; }
	}

	public int Index {
		get { return _index; }
	}


	void Awake( ) { 
		_spriteRenderer = gameObject.GetComponent< SpriteRenderer >( );
	}

	void Start( ) {
		_originallyColor = _spriteRenderer.color;
	}


	public void ChangeColor( bool isRedFlag ) {
		if ( isRedFlag ) { 
			_spriteRenderer.color = _red;
		} else { 
			_spriteRenderer.color = _originallyColor;	
		}
	}
}

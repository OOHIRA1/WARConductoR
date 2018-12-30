using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberUIChange : ImagesDependedOnNumber {
	enum DIGIT { 
		ONE_PLACE,
		TENS_PLACE,
		MAX_PLACE,
	}

	[ SerializeField ] Sprite[ ] _numbers = new Sprite[ 10 ];

	int _previous_point = 0;	//処理をする前の値を覚えとくための変数

	void Awake( ) {
		_point = this.GetComponent< Point >( );

		_image = new SpriteRenderer[ ( int )DIGIT.MAX_PLACE ];

		//子になるゲームオブジェクトを取得する--------------------------------------------------
		var child_images = gameObject.GetComponentInChildren< Transform >( );
		if ( child_images == null ) { 
			Debug.Log( "[エラー]墓地の数字がない" );
			return;
		}

		int index = 0;
		foreach( Transform image in child_images ) { 
			_image[ index ] = image.gameObject.GetComponent< SpriteRenderer >( );
			index++;
		}
		if ( _image[ ( int )DIGIT.MAX_PLACE - 1 ] == null ) Debug.Log( "[エラー]数字が少ない" );
		//---------------------------------------------------------------------------------------
	}

	void Start( ) {
		_previous_point = _point.Point_Num;	
		
		ChangeImage( );
	}

	public override void UpdateteImages( ) {
		if ( _point.Point_Num == _previous_point ) { 
			return;	
		} else { 
			_previous_point = _point.Point_Num;	
		}

		ChangeImage( );
	}

	void ChangeImage( ) { 
		if ( _point.Point_Num < 0 || _point.Point_Num > 99 ) return;

		if ( _point.Point_Num < 10 ) { 
			_image[ ( int )DIGIT.ONE_PLACE ].sprite  = _numbers[ _point.Point_Num ];
			_image[ ( int )DIGIT.TENS_PLACE ].sprite = _numbers[ 0 ];
		}

		if ( _point.Point_Num > 9 ) {
			int point = _point.Point_Num;
			int one_place  = point % 10;
			point /= 10;
			int tens_place = point % 10;
			
			_image[ ( int )DIGIT.ONE_PLACE ].sprite  = _numbers[ one_place ];
			_image[ ( int )DIGIT.TENS_PLACE ].sprite = _numbers[ tens_place ];
		}
	} 

}
//マジックナンバーだけどどうしようか

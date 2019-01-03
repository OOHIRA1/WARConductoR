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

	int _previousPoint = 0;	//処理をする前の値を覚えとくための変数

	void Awake( ) {
		_point = this.GetComponent< Point >( );

		　_spriteRenderers = new SpriteRenderer[ ( int )DIGIT.MAX_PLACE ];

		//子になるゲームオブジェクトを取得する--------------------------------------------------
		var childImages = gameObject.GetComponentInChildren< Transform >( );
		if ( childImages == null ) { 
			Debug.Log( "[エラー]墓地の数字がない" );
			return;
		}

		int index = 0;
		foreach( Transform image in childImages ) { 
			　_spriteRenderers[ index ] = image.gameObject.GetComponent< SpriteRenderer >( );
			index++;
		}
		if ( 　_spriteRenderers[ ( int )DIGIT.MAX_PLACE - 1 ] == null ) Debug.Log( "[エラー]数字が少ない" );
		//---------------------------------------------------------------------------------------
	}


	void Start( ) {
		_previousPoint = _point.Point_Num;	
		
		ChangeImage( );
	}


	public override void UpdateteImages( ) {
		if ( _point.Point_Num == _previousPoint ) return;

		ChangeImage( );

		_previousPoint = _point.Point_Num;	
	}


	void ChangeImage( ) { 
		if ( _point.Point_Num < 0 || _point.Point_Num > 99 ) return;	//正の値と二桁まで対応

		//一桁なら
		if ( _point.Point_Num < 10 ) {	
			　_spriteRenderers[ ( int )DIGIT.ONE_PLACE ].sprite  = _numbers[ _point.Point_Num ];
			　_spriteRenderers[ ( int )DIGIT.TENS_PLACE ].sprite = _numbers[ 0 ];
		}

		//二桁なら
		if ( _point.Point_Num > 9 ) {
			int point = _point.Point_Num;
			int onePlace  = point % 10;
			point /= 10;
			int tensPlace = point % 10;
			
			　_spriteRenderers[ ( int )DIGIT.ONE_PLACE ].sprite  = _numbers[ onePlace ];
			　_spriteRenderers[ ( int )DIGIT.TENS_PLACE ].sprite = _numbers[ tensPlace ];
		}
	} 

}
//マジックナンバーだけどどうしようか

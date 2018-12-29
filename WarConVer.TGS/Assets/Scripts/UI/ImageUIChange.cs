using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageUIChange : ImagesDependedOnNumber {

	int _previous_point = 0;	//処理をする前の値を覚えとくための変数

	void Awake( ) {
		_point = this.GetComponent< Point >( );

		//子になるゲームオブジェクトを取得する----------------------------------------
		_image = new SpriteRenderer[ _point.Initial_Point ];
		var child_images = gameObject.GetComponentInChildren< Transform >( );
		if ( child_images == null ) { 
			Debug.Log( "[エラー]APになる画像がない" );
			return;
		}

		int index = 0;
		foreach( Transform image in child_images ) { 
			_image[ index ] = image.gameObject.GetComponent< SpriteRenderer >( );
			index++;
		}
		if ( _image[ _point.Initial_Point - 1 ] == null ) Debug.Log( "[エラー]画像が少ない" );
		//---------------------------------------------------------------------------
	}

	void Start( ) {
		_previous_point = _point.Initial_Point - _point.Point_Num;
	}

	//基底クラスのUpdateを呼ぶ

	//残りAPに応じて画像の表示を変化させる--------
	public override void UpdateteImages( ) {
		int decrease_num = _point.Initial_Point - _point.Point_Num;

		if ( decrease_num == _previous_point ) { 
			return;	
		} else { 
			_previous_point = decrease_num;	
		}

		for ( int i = 0; i < decrease_num; i++ ) {
			_image[ i ].gameObject.SetActive( false ); 
		}
	}
	//--------------------------------------------
}

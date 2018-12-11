using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageUIChange : ImagesDependedOnNumber {

	void Awake( ) {
		_point = this.GetComponent< Point >( );
		//子になるゲームオブジェクトを取得する----------------------------------------
		_image = new GameObject[ _point.Max_Point ];
		var child_images = gameObject.GetComponentInChildren< Transform >( );
		if ( child_images == null ) { 
			Debug.Log( "APになる画像がないよー" );
		} else {
			int i = 0;
			foreach( Transform image in child_images ) { 
				_image[ i ] = image.gameObject;
				i++;
			}

			if ( _image[ _point.Max_Point - 1 ] == null ) Debug.Log( "画像が少ないよー" );
		}
		//---------------------------------------------------------------------------
	}

	//基底クラスのUpdateを呼ぶ

	//残りAPに応じて画像の表示を変化させる--------
	public override void UpdateteImages( ) {
		int decrease_num = _point.Max_Point - _point.Point_Num;

		for ( int i = 0; i < decrease_num; i++ ) {
			_image[ i ].SetActive( false ); 
		}
	}
	//--------------------------------------------
}

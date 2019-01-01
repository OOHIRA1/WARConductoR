using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageUIChange : ImagesDependedOnNumber {

	int _previousDecreaseNum = 0;	//処理をする前の値を覚えとくための変数

	void Awake( ) {
		_point = this.GetComponent< Point >( );

		//子になるゲームオブジェクトを取得する
		　_spriteRenderers = new SpriteRenderer[ _point.Initial_Point ];
		var childImages = gameObject.GetComponentInChildren< Transform >( );
		if ( childImages == null ) { 
			Debug.Log( "[エラー]APになる画像がない" );
			return;
		}

		int index = 0;
		foreach( Transform image in childImages ) { 
			　_spriteRenderers[ index ] = image.gameObject.GetComponent< SpriteRenderer >( );
			index++;
		}
		if ( 　_spriteRenderers[ _point.Initial_Point - 1 ] == null ) Debug.Log( "[エラー]画像が少ない" );
	}

	void Start( ) {
		_previousDecreaseNum = _point.Initial_Point - _point.Point_Num;
	}

	//基底クラスのUpdateを呼ぶ

	//残りAPに応じて画像の表示を変化させる
	public override void UpdateteImages( ) {
		int decreaseNum = _point.Initial_Point - _point.Point_Num;

		if ( decreaseNum == _previousDecreaseNum ) return;	

		for ( int i = 0; i < decreaseNum; i++ ) {
			　_spriteRenderers[ i ].gameObject.SetActive( false ); 
		}

		_previousDecreaseNum = decreaseNum;	
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter {
	const float RAY_DIR = 100f;

	public Square RayCastSquare( ) {
		Vector3 mouse_pos = Input.mousePosition;
		//mouse_pos.z = 10f;
		Vector3 world_pos = Camera.main.ScreenToWorldPoint( mouse_pos );		//マウスのScreen座標をWorld座標に変換

		Debug.DrawRay( world_pos, new Vector3( 0,0,RAY_DIR ), Color.red, 999f, false );

		RaycastHit2D hit = Physics2D.Raycast( world_pos, new Vector3( 0,0,RAY_DIR ), LayerMask.NameToLayer( "Square" ) );	//クリックされた場所から真っすぐにRawを飛ばす
		if ( hit.collider == null ) return null;
		if ( hit.collider.gameObject.tag != "Square" ) return null;

		Debug.Log( hit.collider.gameObject.ToString( ) );
		return hit.collider.gameObject.GetComponent< Square >( );
	}

	public CardMain RayCastHandCard( ) { 
		Vector3 mouse_pos = Input.mousePosition;
		Vector3 world_pos = Camera.main.ScreenToWorldPoint( mouse_pos );		//マウスのScreen座標をWorld座標に変換

		Debug.DrawRay( world_pos, new Vector3( 0,0,RAY_DIR ), Color.red, 999f, false );

		RaycastHit2D hit = Physics2D.Raycast( world_pos, new Vector3( 0,0,RAY_DIR ), LayerMask.NameToLayer( "HandCard" ) );	//クリックされた場所から真っすぐにRawを飛ばす
		if ( hit.collider == null ) return null;
		if ( hit.collider.gameObject.tag != "HandCard" ) return null;

		Debug.Log( hit.collider.gameObject.ToString( ) );
		return hit.collider.gameObject.GetComponent< CardMain >( );
	}
}
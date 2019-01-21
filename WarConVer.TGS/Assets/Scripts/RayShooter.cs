using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter {
	const float RAY_DIR = 100.0f;
	const float RAY_DISTANCE = 999.0f;


	//マスだけを取得するRayを飛ばす----------------------------------------------------------------------------------------
	public Square RayCastSquare( ) {
		Vector3 mousePos = Input.mousePosition;
		//mouse_pos.z = 10f;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( mousePos );		//マウスのScreen座標をWorld座標に変換
	
		Debug.DrawRay( worldPos, new Vector3( 0, 0, RAY_DIR ), Color.red, RAY_DISTANCE, false );
		
		RaycastHit2D hit = Physics2D.Raycast( worldPos, new Vector3( 0, 0, RAY_DIR ), LayerMask.NameToLayer( ConstantStorehouse.LAYER_SQUARE ) );	//クリックされた場所から真っすぐにRawを飛ばす
		if ( hit.collider == null ) return null;
		if ( hit.collider.gameObject.tag != ConstantStorehouse.TAG_SQUARE ) return null;
	
		Debug.Log( hit.collider.gameObject.ToString( ) );
		return hit.collider.gameObject.GetComponent< Square >( );
	}
	//----------------------------------------------------------------------------------------------------------------------
	
	
	//手札のカードだけを取得するRayを飛ばす-----------------------------------------------------------------------------------
	public CardMain RayCastHandCard( string player ) { 
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint( mousePos );		//マウスのScreen座標をWorld座標に変換
	
		Debug.DrawRay( worldPos, new Vector3( 0, 0, RAY_DIR ), Color.red, RAY_DISTANCE, false );
	
		RaycastHit2D hit = Physics2D.Raycast( worldPos, new Vector3( 0, 0, RAY_DIR ), LayerMask.NameToLayer( ConstantStorehouse.LAYER_HAND_CARD ) );	//クリックされた場所から真っすぐにRawを飛ばす
		if ( hit.collider == null ) return null;
		if ( hit.collider.gameObject.tag != player ) return null;
	
		Debug.Log( hit.collider.gameObject.ToString( ) );
		return hit.collider.gameObject.GetComponent< CardMain >( );
	}
	//----------------------------------------------------------------------------------------------------------------------
}
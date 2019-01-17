using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==戦闘場所クラス
//
//==使用方法：戦闘場所となるGameObjectにアタッチ
[RequireComponent(typeof(BattleAnimation))]
public class AutoDestroyBattleSpace : MonoBehaviour {
	[ SerializeField ] BattleAnimation _battleAnimation = null;
	[ SerializeField ] SpriteRenderer _leftCardSpriteRenderer = null;
	[ SerializeField ] SpriteRenderer _rightCardSpriteRenderer = null;


	// Update is called once per frame
	void Update () {
		if (_battleAnimation.Is_Animation_Finished) {
			Destroy (this.gameObject);
		}
	}


	//=================================================================
	//public関数
	//--左のカードが勝つアニメーションを再生する関数
	public void StartLeftWinAnim( Sprite leftCardSprite, Sprite rightCardSprite ) {
		_leftCardSpriteRenderer.sprite = leftCardSprite;
		_rightCardSpriteRenderer.sprite = rightCardSprite;
		_battleAnimation.StartLeftWinAnim ();
	}

	//--右のカードが勝つアニメーションを再生する関数
	public void StartRightWinAnim( Sprite leftCardSprite, Sprite rightCardSprite ) {
		_leftCardSpriteRenderer.sprite = leftCardSprite;
		_rightCardSpriteRenderer.sprite = rightCardSprite;
		_battleAnimation.StartRightWinAnim ();
	}

	//--左右のカードが共に負けるアニメーションを再生する関数
	public void StartBothDeathAnim( Sprite leftCardSprite, Sprite rightCardSprite ) {
		_leftCardSpriteRenderer.sprite = leftCardSprite;
		_rightCardSpriteRenderer.sprite = rightCardSprite;
		_battleAnimation.StartBothDeathAnim ();
	}

	//--左右のカードが共に生き残るアニメーションを再生する関数
	public void StartBothAliveAnim( Sprite leftCardSprite, Sprite rightCardSprite ) {
		_leftCardSpriteRenderer.sprite = leftCardSprite;
		_rightCardSpriteRenderer.sprite = rightCardSprite;
		_battleAnimation.StartBothAliveAnim ();
	}
	//=================================================================
	//=================================================================
}

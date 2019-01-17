using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==エフェクト終了後に自動削除するエフェクト
//
//==使用方法：エフェクトを行う親GameObjectにアタッチ
public class AutoDestroyEffect : MonoBehaviour {
	ParticleSystem _particleSystem;
	AudioSource    _audioSource;

	// Use this for initialization
	void Start () {
		_particleSystem = GetComponentInChildren<ParticleSystem> ();
		_audioSource 	= GetComponentInChildren<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (!_particleSystem.isPlaying && !_audioSource.isPlaying) {
			Destroy ( this.gameObject );
		}
	}
}

using UnityEngine;

public class Main : MonoBehaviour {

	[SerializeField] GameMgr mgr = null;
	// Use this for initialization
	public void Game()
    {
        mgr.FarstHandDrow();

    }
}

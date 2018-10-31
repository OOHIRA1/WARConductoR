using UnityEngine;

public class TapAction : MonoBehaviour {

    [SerializeField] Sprite tapImage = null;
    [SerializeField] GameObject popUpImage = null;
    [SerializeField] GameObject gameMgr;
    

    // Use this for initialization
    void Start () {
    }
	
    public void SetActiveTure()
    {
        popUpImage.gameObject.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
	}
}

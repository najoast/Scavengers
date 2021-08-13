using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject soundManager;
    public GameObject uiManager;

    void Awake() {
        Debug.Log("Loader");
        if (GameManager.instance == null)
            Instantiate(gameManager);
        if (SoundManager.instance == null)
            Instantiate(soundManager);
        if (UIManager.instance == null)
            Instantiate(uiManager);
    }
}
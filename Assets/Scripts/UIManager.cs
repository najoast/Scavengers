using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public float levelStartDelay = 2f;

    private Text foodText;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void InitUI(int level) {
        doingSetup = true;

        foodText = GameObject.Find("FoodText").GetComponent<Text>();;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);
    }

    public void HideLevelImage() {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public bool IsDoingSetup() {
        return doingSetup;
    }

    public void ShowLevelText(string text, float duration) {
        levelText.text = text;
        levelImage.SetActive(true);
        if (duration > float.Epsilon) {
            Invoke("HideLevelImage", duration);
        }
    }

    public void ShowFoodChange(int changed, int total) {
        if (changed == 0)
            return;
        foodText.text = (changed > 0 ? "+" : "") + changed + " Food: " + total;
        // foodText.text = changed + " Food: " + total;
    }

    public void ShowFood(int food) {
        foodText.text = "Food: " + food;
    }
}
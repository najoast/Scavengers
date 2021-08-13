using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;
    public static GameManager instance = null;
    public BoardManager boardManager;
    [HideInInspector] public bool playerTurn = true;

    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        boardManager = GetComponent<BoardManager>();
        // Invoke("InitGame", 0);
        // InitGame();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        level++;
        InitGame();
        Debug.Log("OnLevelFinishedLoading " + level);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame() {
        enemies.Clear();
        boardManager.SetupScene(level);
        UIManager.instance.InitUI(level);
    }

    void Update() {
        if (playerTurn || enemiesMoving || UIManager.instance.IsDoingSetup())
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemy) {
        enemies.Add(enemy);
    }

    public void GameOver() {
        UIManager.instance.ShowLevelText("After "+level+" days, you starved.", 0);
        enabled = false;
    }

    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0) {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }
}

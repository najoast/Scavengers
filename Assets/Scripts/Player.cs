using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int wallDamage = 1;

    private Animator animator;
    private int food = 0;

    // effect sounds
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        SetFood(GameManager.instance.playerFoodPoints);
        base.Start();
    }

    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }

    private void Update() {
        if (!GameManager.instance.playerTurn)
            return;
        if (isMoving)
            return;
        
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
            vertical = 0;

        if (horizontal!=0 || vertical!=0)
            AttemptMove<Wall>(horizontal, vertical);
    }

    protected override bool AttemptMove<T>(int xDir, int yDir) {
        UpdateFood(-1);
        bool canMove = base.AttemptMove<T>(xDir, yDir);
        if (canMove) {
            Debug.Log("Play move sound");
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        CheckIfGameOver();
        GameManager.instance.playerTurn = false;
        return canMove;
    }

    protected override void OnCantMove<T>(T component) {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } else if (other.tag == "Food") {
            UpdateFood(pointsPerFood);
            other.gameObject.SetActive(false);
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
        } else if (other.tag == "Soda") {
            UpdateFood(pointsPerSoda);
            other.gameObject.SetActive(false);
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
        }
    }

    private void Restart() {
        SceneManager.LoadScene(0);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        // SceneManager.LoadScene("Main");
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        UpdateFood(-loss);
    }

    private void CheckIfGameOver() {
        if (food <= 0) {
            GameManager.instance.GameOver();
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
        }
    }

    private void SetFood(int val) {
        food = val;
        UIManager.instance.ShowFood(val);
    }

    private void UpdateFood(int val) {
        if (val == 0)
            return;
        food += val;
        UIManager.instance.ShowFoodChange(val, food);

        if  (val < 0) {
            CheckIfGameOver();
        }
    }
}
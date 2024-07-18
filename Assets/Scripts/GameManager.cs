using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private FroggerController _frogger;
    private Home[] _homes;
    private int _score;
    private int _lives;
    private int _time;

    public GameObject gameOverMenu;

    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text timeText;


    private void Awake()
    {
        _homes = FindObjectsOfType<Home>();  
        _frogger = FindObjectOfType<FroggerController>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        gameOverMenu.SetActive(false);

        SetScore(0);
        SetLives(3);
        NewLevel();
    }

    private void NewLevel()
    {
        for (int i = 0; i < _homes.Length; i++)
        {
            _homes[i].enabled = false;
        }

        Respawn();
    }

    private void Respawn()
    {
        _frogger.Respawn();

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private IEnumerator Timer(int duration)
    {
        _time = duration;
        timeText.text = _time.ToString();

        while (_time > 0)
        {
            yield return new WaitForSeconds(1);
            _time --;
            timeText.text = _time.ToString();
        }

        _frogger.Death();
    }

    public void Died()
    {
        SetLives(_lives - 1);

        if (_lives > 0)
        {
            Invoke(nameof(Respawn), 1f);
        } 
        else
        {
            Invoke(nameof(GameOver), 1f);
        }
    }

    private void GameOver()
    {
        _frogger.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PlayAgain());
    }

    private IEnumerator PlayAgain()
    {
        bool playAgain = false;

        while (!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playAgain = true;
            }

            yield return null;
        }

        NewGame();
    }

    public void AdvancedRow()
    {
        SetScore(_score + 10);
    }

    public void HomeOccupied()
    {
        _frogger.gameObject.SetActive(false);

        int bonusPoints = _time * 20;
        SetScore(_score + bonusPoints + 50);

        if (Cleared())
        {
            SetScore(_score + 1000);
            SetLives(_lives + 1);
            Invoke(nameof(NewLevel),1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < _homes.Length; i++)
        {
            if (!_homes[i].enabled)
            {
                return false;
            }
        }
        return true;
    }

    private void SetScore(int score)
    {
        this._score = score;
        
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this._lives = lives;
        livesText.text = lives.ToString();
    }
}

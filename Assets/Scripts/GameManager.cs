using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private FroggerController _frogger;
    private Home[] _homes;
    private int _score;
    private int _lives;
    private int _time;


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

        NewRound();
    }

    private void NewRound()
    {
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

        while (_time > 0)
        {
            yield return new WaitForSeconds(1);

            _time --;
        }

        _frogger.Death();
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
            Invoke(nameof(NewLevel),1f);
        }
        else
        {
            Invoke(nameof(NewRound), 1f);
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
        // ..ui
    }

    private void SetLives(int lives)
    {
        this._lives = lives;
        // ..ui
    }
}

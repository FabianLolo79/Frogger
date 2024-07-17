using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private FroggerController _frogger;
    private Home[] _homes;
    private int _score;
    private int _lives;


    private void Awake()
    {
        _homes = FindObjectsOfType<Home>();  
        _frogger = FindObjectOfType<FroggerController>();
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
        _frogger.Respawn();
    }

    public void HomeOccupied()
    {
        _frogger.gameObject.SetActive(false);

        if (Cleared())
        {
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

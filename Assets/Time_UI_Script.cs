using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_UI_Script : MonoBehaviour
{
    public bool isPaused = false;
    private float timeSpeed = 1f;
    [SerializeField] private Sprite playButton;
    [SerializeField] private Sprite pauseButton;
    [SerializeField] private Button pausePlayButton;

    void Start()
    {
        
    }

    void Update()
    {
        ChangeTime();
    }


    private void ChangeTime()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPaused == false)
        {
            Time.timeScale = 0f;
            isPaused = true;
            pausePlayButton.image.sprite = playButton;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isPaused == true)
        {
            Time.timeScale = timeSpeed;
            isPaused = false;
            pausePlayButton.image.sprite = pauseButton;
        }
        if (Input.GetKeyDown(KeyCode.Period) && timeSpeed < 33)
        {
            if (isPaused)
            {
                Time.timeScale = timeSpeed;
                isPaused = false;
                pausePlayButton.image.sprite = pauseButton;
            }
            else
            {
                timeSpeed *= 2;
                Time.timeScale = timeSpeed;
                print(timeSpeed + "x speed");
            }
        } else if (Input.GetKeyDown(KeyCode.Comma) && timeSpeed > 0.5f)
        {
            if (isPaused)
            {
                Time.timeScale = timeSpeed;
                isPaused = false;
                pausePlayButton.image.sprite = pauseButton;
            }
            else
            {
                timeSpeed /= 2;
                Time.timeScale = timeSpeed;
                print(timeSpeed + "x speed");
            }
        } else if (Input.GetKeyDown(KeyCode.Slash))
        {
            timeSpeed = 1f;
            Time.timeScale = timeSpeed;
            print(timeSpeed + "x speed");
        }
    }

    public void PauseTime()
    {
        if (isPaused == false)
        {
            Time.timeScale = 0f;
            isPaused = true;
            pausePlayButton.image.sprite = playButton;
        }
        else if (isPaused == true)
        {
            Time.timeScale = timeSpeed;
            isPaused = false;
            pausePlayButton.image.sprite = pauseButton;
        }
    }

    public void FastForward()
    {
        if (isPaused)
        {
            Time.timeScale = timeSpeed;
            isPaused = false;
            pausePlayButton.image.sprite = pauseButton;
        } else if (timeSpeed < 33)
        {
            timeSpeed *= 2;
            Time.timeScale = timeSpeed;
            print(timeSpeed + "x speed");
        }
    }

    public void SlowDown()
    {
        if (isPaused)
        {
            Time.timeScale = timeSpeed;
            isPaused = false;
            pausePlayButton.image.sprite = pauseButton;
        }
        else if (timeSpeed > 0.5f)
        {
            timeSpeed /= 2;
            Time.timeScale = timeSpeed;
            print(timeSpeed + "x speed");
        }
    }
}

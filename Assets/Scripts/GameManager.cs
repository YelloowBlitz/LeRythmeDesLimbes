﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Monsters
    public MonsterManager monsterManager;
    public TurretManager turretManager;
    public TilemapManager tilemapManager;

    public BeatManager beatManager;

    // Music
    private AudioMixer audioMixer;
    public MusicManager musicManager;

    public LevelInfos levelInfos;

    public Texture2D cursorTexture;

    [SerializeField] private float pulse = 0.25f;

    private int[] currentSentence;

    private float currentSentenceLength;

    private int[] nextSentence;


    private int progression;
    private int currentPart;


    private bool isPlaying = true;

    public int maxHP = 20;
    public Text HPText;

    private bool goNext = true;

    private bool ggEz = false;

    private float tempoCounter = -0.35f;

    private int pulseCounter = 0;
    private int sentenceIndice = 0;

    private bool isTuto = false;
    void Start()
    {
        currentSentence = musicManager.getFirstSentence(PlayerPrefs.GetInt("levelPlayed"));
        beatManager.loadSentence(currentSentence);
        
        currentSentenceLength = currentSentence.GetLength(0);
        tempoCounter = 0f;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        if (PlayerPrefs.GetInt("levelPlayed") == 0)
        {
            isTuto = true;
        }
    }


    private void Update(){
        if (isPlaying)
        {
            tempoCounter += Time.deltaTime;
            if (tempoCounter >= pulse)
            {
                beatManager.UpdateBeat();

                if (currentSentence[sentenceIndice] == 1)
                {
                    var x = levelInfos.getCurrentPatterns(PlayerPrefs.GetInt("levelPlayed"),currentPart);
                    monsterManager.updateMonsters(x);
                    turretManager.TempoUpdate();
                }
                sentenceIndice++;

                if (sentenceIndice >= currentSentenceLength)
                {
                    if (ggEz)
                    {
                        Win();
                    }
                    currentSentence = musicManager.getNextSentence(goNext);
                    beatManager.loadSentence(currentSentence);
                    currentSentenceLength = currentSentence.GetLength(0);
                    sentenceIndice = 0;
                    goNext = false;
                    
                }
                tempoCounter -= pulse;
                pulseCounter ++;
            }

            if (pulseCounter == 4)
            {
                tilemapManager.updateTiles();
                pulseCounter = 0;
            }
            
            
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoseLife()
    {
        maxHP-=1;
        HPText.text = maxHP.ToString();
        if (maxHP <=0 ) {
            SceneManager.LoadScene("LostScene");
        }
    }

    public void Progress()
    {
        progression++;
        goNext = levelInfos.shouldIGoNext(PlayerPrefs.GetInt("levelPlayed"),currentPart,progression);
        if (goNext)
        {
            progression -= levelInfos.getSoulRequired(PlayerPrefs.GetInt("levelPlayed"),currentPart);
            currentPart++;
            if (levelInfos.didIWin(PlayerPrefs.GetInt("levelPlayed"),currentPart))
            {
                ggEz = true;
            }
        }
    }

    public void Win()
    {
        SceneManager.LoadScene("VictoryMenu");
        Debug.Log("Bien joué mon grand");
    }

}

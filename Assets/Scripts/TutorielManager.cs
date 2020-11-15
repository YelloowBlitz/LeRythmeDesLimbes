﻿using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorielManager : MonoBehaviour
{
// Monsters
    public MonsterManager monsterManager;
    public TurretManager turretManager;
    public TilemapManager tilemapManager;

    // Music
    private AudioMixer audioMixer;
    public MusicManager musicManager;

    public LevelInfos levelInfos;

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

    private float tempoCounter = -0.35f;

    private int pulseCounter = 0;
    private int sentenceIndice = 0;

    void Start()
    {
        currentSentence = musicManager.getFirstSentence(0);
        currentSentenceLength = currentSentence.GetLength(0);
        tempoCounter = 0f;
    }


    private void Update(){
        if (isPlaying)
        {
            tempoCounter += Time.deltaTime;
            if (tempoCounter >= pulse)
            {
                if (currentSentence[sentenceIndice] == 1)
                {
                    monsterManager.updateMonsters();
                    turretManager.TempoUpdate();
                }
                sentenceIndice++;

                if (sentenceIndice >= currentSentenceLength)
                {
                    currentSentence = musicManager.getNextSentence(goNext);
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
        if (maxHP <=0) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Progress()
    {
        progression++;
        goNext = levelInfos.shouldIGoNext(0,currentPart,progression);
        if (goNext)
        {
            progression -= levelInfos.getSoulRequired(0,currentPart);
            currentPart++;
            if (levelInfos.didIWin(0,currentPart))
            {
                Win();
            }
        }
    }

    public void Win()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Bien joué mon grand");
    }

}
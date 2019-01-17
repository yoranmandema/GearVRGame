﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Tooltip("How long the player has to survive for. (In seconds)")]
    public float TotalGameTime = 300f;

    [Tooltip("The amount of energy needed to trigger a game-over event.")]
    public float TargetEnergy = 10000f;

    [Tooltip("How much energy has been wasted until now.")]
    public float EnergyWastage = 0f;

    [Tooltip("Modifier that influences how much energy will be wasted.")]
    public float EnergyWasteMultiplier = 1f;

    [Tooltip("Time between each enemy spawn event.")]
    public float EnemySpawnTime = 5f;

    [Tooltip("How many enemies can be in the scene at once.")]
    public float MaxEnemyCount = 10f;

    [HideInInspector]
    public bool GamePlayStarted = false;

    public static GameManager Instance;

    public GameObject[] EnableOnGameplayStart;

    public float TotalTime = 0f;

    void Awake () {
        SetGameStarted(false);

        var gameManagers = FindObjectsOfType<GameManager>();

        if (gameManagers.Length > 1) {
            Debug.LogError("More than 1 GameManager in scene");
        } else {
            Instance = this;
        }

        if (GameObject.FindGameObjectWithTag("IntroSequence") == null) SetGameStarted(true);
    }

    void Update() {
        if (GamePlayStarted) {
            TotalTime += Time.deltaTime;

            if (TotalTime > TotalGameTime) {
                DoWinEvent();
            }
        }
    }

    public void AddEnergyWaste (float amount) {
        if (GamePlayStarted) {
            EnergyWastage += amount * EnergyWasteMultiplier;

            if (EnergyWastage >= TargetEnergy && TotalTime < TotalGameTime) {
                DoGameOverEvent();
            }
        }
    }

    public void SetGameStarted (bool started) {
        GamePlayStarted = started;

        foreach (var obj in EnableOnGameplayStart) {
            obj.SetActive(GamePlayStarted);
        }
    }

    void DoGameOverEvent () {
        SetGameStarted(false);
        print("GAME OVER");
    }

    void DoWinEvent() {
        SetGameStarted(false);
        print("WIN");
    }
}

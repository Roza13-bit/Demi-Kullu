using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UIClass
{
    public class UIManager : MonoBehaviour
    {
        [Header("Cached References")]

        [SerializeField] private StateManager stateManagerSC;

        [SerializeField] private GameObject gameSceneCanvas;

        [SerializeField] private GameObject playButton;

        [SerializeField] private TextMeshProUGUI startPlayTimerTMP;

        [SerializeField] private TextMeshProUGUI heavyCDTimerTMP;

        [SerializeField] private TextMeshProUGUI ultimateCDTimerTMP;

        public GameObject aimCrosshair;

        [Header("Public Variables")]

        public SkillScriptableObject lightAttackSO;

        public Button lightAttackButton;

        public SkillScriptableObject heavyAttackSO;

        public Button heavyAttackButton;

        public SkillScriptableObject ultimateAttackSO;

        public Button ultimateAttackButton;

        // Private Variables.

        private Sprite _lightAttackSpriteMain;

        private Sprite _lightAttackSpritePressed;

        private Sprite _heavyAttackSpriteMain;

        private Sprite _heavyAttackSpritePressed;

        private Sprite _ultimateAttackSpriteMain;

        private Sprite _ultimateAttackSpritePressed;

        // private GameObject _gameSceneCanvas;

        // Sets the game canvas active. 
        // Starts the button sprites pull from the scriptable objects.
        public void LoadGameSceneCanvas()
        {
            gameSceneCanvas.SetActive(true);

            SetAttackButtonsSprites();

        }

        // Set the play button to inactive.
        // Play the game start countdown.
        public void TurnOffButtonAndTurnOnTimer()
        {
            playButton.SetActive(false);

            startPlayTimerTMP.gameObject.SetActive(true);

            StartCoroutine(StartPlayTimer());

        }

        // Game start countdown coroutine.
        public IEnumerator StartPlayTimer()
        {
            for (int x = 3; x >= 0; x--)
            {
                if (x == 0)
                {
                    startPlayTimerTMP.text = "GO";

                }
                else
                {
                    startPlayTimerTMP.text = "" + x;

                }

                yield return new WaitForSeconds(1f);

            }

            startPlayTimerTMP.gameObject.SetActive(false);

            stateManagerSC.PlayStartedEvent.Invoke();

            yield break;

        }

        // Sets all of the button sprites, from the scriptable
        // objects into the private variables.
        public void SetAttackButtonsSprites()
        {
            _lightAttackSpriteMain = lightAttackSO.skillSpriteMain;

            _lightAttackSpritePressed = lightAttackSO.skillSpritePressed;

            lightAttackButton.image.sprite = _lightAttackSpriteMain;

            _heavyAttackSpriteMain = heavyAttackSO.skillSpriteMain;

            _heavyAttackSpritePressed = heavyAttackSO.skillSpritePressed;

            heavyAttackButton.image.sprite = _heavyAttackSpriteMain;

            _ultimateAttackSpriteMain = ultimateAttackSO.skillSpriteMain;

            _ultimateAttackSpritePressed = ultimateAttackSO.skillSpritePressed;

            ultimateAttackButton.image.sprite = _ultimateAttackSpriteMain;

        }
        
        // Sets the heavy attack button to pressed sprite.
        public void SetHeavyAttackPressed()
        {
            heavyAttackButton.image.sprite = _heavyAttackSpritePressed;

        }

        // Sets the heavy attack button to unpressed sprite.
        public void SetHeavyAttackUnpressed()
        {
            heavyAttackButton.image.sprite = _heavyAttackSpriteMain;

        }

        // Starts the cooldown timer for the heavy attack.
        // Makes the heavy attack button unclickable.
        // Sets button sprite to unpressed after cd is finished.
        public IEnumerator SetHeavyAttackCooldown()
        {
            heavyAttackButton.interactable = false;

            heavyCDTimerTMP.gameObject.SetActive(true);

            for (int x = (int)heavyAttackSO.skillCooldown; x > 0; x--)
            {
                heavyCDTimerTMP.text = "" + x;

                yield return new WaitForSeconds(1f);

            }

            heavyCDTimerTMP.gameObject.SetActive(false);

            heavyAttackButton.interactable = true;

            SetHeavyAttackUnpressed();

        }

        // Sets the ultimate attack button to pressed sprite.
        public void SetUltimateAttackPressed()
        {
            Debug.Log("Started ultimate press.");

            ultimateAttackButton.image.sprite = _ultimateAttackSpritePressed;

        }

        // Sets the ultimate attack button to unpressed sprite.
        public void SetUltimateAttackUnpressed()
        {
            Debug.Log("Started ultimate unpress.");

            ultimateAttackButton.image.sprite = _ultimateAttackSpriteMain;

        }

        // Starts the cooldown timer for the ultimate attack.
        // Makes the ultimate attack button unclickable.
        // Sets button sprite to unpressed after cd is finished.
        public void StartUltimateCDTimer()
        { 
            StartCoroutine(SetUltimateAttackCooldown()); 

        }

        private IEnumerator SetUltimateAttackCooldown()
        {
            Debug.Log("Started ultimate cd.");

            ultimateAttackButton.interactable = false;

            ultimateCDTimerTMP.gameObject.SetActive(true);

            for (int x = (int)ultimateAttackSO.skillCooldown; x > 0; x--)
            {
                ultimateCDTimerTMP.text = "" + x;

                yield return new WaitForSeconds(1f);

            }

            ultimateCDTimerTMP.gameObject.SetActive(false);

            ultimateAttackButton.interactable = true;

            SetUltimateAttackUnpressed();

        }

    }

}
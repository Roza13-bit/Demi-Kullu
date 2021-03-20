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

        [SerializeField] private GameObject aimCrosshair;

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

        public void LoadGameSceneCanvas()
        {
            gameSceneCanvas.SetActive(true);

            SetAttackButtonsSprites();

        }

        public void TurnOffButtonAndTurnOnTimer()
        {
            playButton.SetActive(false);

            startPlayTimerTMP.gameObject.SetActive(true);

            StartCoroutine(StartPlayTimer());

        }

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

        public void SetLightAttackPressed()
        {
            aimCrosshair.SetActive(true);

            lightAttackButton.image.sprite = _lightAttackSpritePressed;

            heavyAttackButton.image.sprite = _heavyAttackSpriteMain;

            ultimateAttackButton.image.sprite = _ultimateAttackSpriteMain;

        }
        
        public void SetHeavyAttackPressed()
        {
            aimCrosshair.SetActive(false);

            lightAttackButton.image.sprite = _lightAttackSpriteMain;

            heavyAttackButton.image.sprite = _heavyAttackSpritePressed;

            ultimateAttackButton.image.sprite = _ultimateAttackSpriteMain;

        }

        public void SetUltimateAttackPressed()
        {
            aimCrosshair.SetActive(false);

            lightAttackButton.image.sprite = _lightAttackSpriteMain;

            heavyAttackButton.image.sprite = _heavyAttackSpriteMain;

            ultimateAttackButton.image.sprite = _ultimateAttackSpritePressed;

        }

    }

}
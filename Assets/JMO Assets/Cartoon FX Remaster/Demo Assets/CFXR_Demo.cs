﻿//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2025 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

#if UNITY_6000_0_OR_NEWER && CFXR_NEW_INPUT_SYSTEM
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Bloom = Kino.Bloom;
#if CFXR_URP_INSTALLED
using UnityEngine.Rendering.Universal;
#endif

namespace CartoonFX
{
    public class CFXR_Demo : MonoBehaviour
    {
        //----------------------------------------------------------------------------------------------------------------------------

        public Image btnSlowMotion;
        public Text lblSlowMotion;
        public Image btnCameraRotation;
        public Text lblCameraRotation;
        public Image btnShowGround;
        public Text lblShowGround;
        public Image btnCamShake;
        public Text lblCamShake;
        public Image btnLights;
        public Text lblLights;
        public Image btnBloom;
        public Text lblBloom;

        [Space]
        public Text labelEffect;

        public Text labelIndex;

        [Space]
        public GameObject groundURP;

        public GameObject groundBIRP;
        public Transform demoCamera;
        public GameObject eventSystem;
        public float rotationSpeed = 10f;
        public float zoomFactor = 1f;
        private GameObject ground;
        private MonoBehaviour bloom;

        private bool slowMotion = false;
        private bool rotateCamera = false;
        private bool showGround = true;

        //----------------------------------------------------------------------------------------------------------------------------

        [NonSerialized]
        public GameObject currentEffect;

        private GameObject[] effectsList;
        private int index = 0;

        private Vector3 camInitialPosition;
        private Quaternion camInitialRotation;

        private void Awake()
        {
            camInitialPosition = demoCamera.transform.position;
            camInitialRotation = demoCamera.transform.rotation;

            var list = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject effect = transform.GetChild(i).gameObject;
                list.Add(effect);

                var cfxrEffect = effect.GetComponent<CFXR_Effect>();
                if (cfxrEffect != null) cfxrEffect.clearBehavior = CFXR_Effect.ClearBehavior.Disable;
            }

            effectsList = list.ToArray();

            PlayAtIndex();
            UpdateLabels();

            bool isURP = GraphicsSettings.currentRenderPipeline != null;
            ground = isURP ? groundURP : groundBIRP;
            groundURP.SetActive(isURP);
            groundBIRP.SetActive(!isURP);

            bloom = demoCamera.GetComponent<Bloom>();
        #if CFXR_URP_INSTALLED
            if (isURP)
            {
                bloom = demoCamera.GetComponent<Volume>();
                var camUrpData = demoCamera.GetComponent<UniversalAdditionalCameraData>();
                if (camUrpData == null)
                {
                    camUrpData = demoCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
                }

                camUrpData.renderPostProcessing = true;
            }
        #endif
        #if UNITY_6000_0_OR_NEWER && CFXR_NEW_INPUT_SYSTEM
	        // use correct input system for UI
	        Destroy(eventSystem.GetComponent<StandaloneInputModule>());
	        eventSystem.AddComponent<InputSystemUIInputModule>();
        #endif
        }

        private void Update()
        {
            if (rotateCamera)
            {
                demoCamera.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
            }

            if (ButtonsPressed.PlayEffect)
            {
                if (currentEffect != null)
                {
                    var ps = currentEffect.GetComponent<ParticleSystem>();
                    if (ps.isEmitting)
                    {
                        ps.Stop(true);
                    }
                    else
                    {
                        if (!currentEffect.gameObject.activeSelf)
                        {
                            currentEffect.SetActive(true);
                        }
                        else
                        {
                            ps.Play(true);
                            CFXR_Effect[] cfxrEffects = currentEffect.GetComponentsInChildren<CFXR_Effect>();
                            foreach (CFXR_Effect cfxr in cfxrEffects)
                            {
                                cfxr.ResetState();
                            }
                        }
                    }
                }
            }

            if (ButtonsPressed.RestartEffect)
            {
                if (currentEffect != null)
                {
                    currentEffect.SetActive(false);
                    currentEffect.SetActive(true);
                }
            }

            if (ButtonsPressed.Left)
            {
                PreviousEffect();
            }

            if (ButtonsPressed.Right)
            {
                NextEffect();
            }

            if (ButtonsPressed.Mouse0)
            {
                Ray ray = demoCamera.GetComponent<Camera>().ScreenPointToRay(ButtonsPressed.MousePosition);
                if (Physics.Raycast(ray))
                {
                    if (currentEffect != null)
                    {
                        currentEffect.SetActive(false);
                        currentEffect.SetActive(true);
                    }
                }
            }

            if (ButtonsPressed.Mouse1 || ButtonsPressed.Mouse2)
            {
                ResetCam();
            }

            float scroll = ButtonsPressed.MouseScrollY;
            if (scroll != 0f)
            {
                demoCamera.transform.Translate(Vector3.forward * (scroll < 0f ? -1f : 1f) * zoomFactor, Space.Self);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------
        // UI

        public void NextEffect()
        {
            index++;
            WrapIndex();
            PlayAtIndex();
        }

        public void PreviousEffect()
        {
            index--;
            WrapIndex();
            PlayAtIndex();
        }

        public void ToggleSlowMo()
        {
            slowMotion = !slowMotion;

            Time.timeScale = slowMotion ? 0.33f : 1.0f;

            Color color = Color.white;
            color.a = slowMotion ? 1f : 0.33f;
            btnSlowMotion.color = color;
            lblSlowMotion.color = color;
        }

        public void ToggleCamera()
        {
            rotateCamera = !rotateCamera;

            Color color = Color.white;
            color.a = rotateCamera ? 1f : 0.33f;
            btnCameraRotation.color = color;
            lblCameraRotation.color = color;
        }

        public void ToggleGround()
        {
            showGround = !showGround;

            ground.SetActive(showGround);

            Color color = Color.white;
            color.a = showGround ? 1f : 0.33f;
            btnShowGround.color = color;
            lblShowGround.color = color;
        }

        public void ToggleCameraShake()
        {
            CFXR_Effect.GlobalDisableCameraShake = !CFXR_Effect.GlobalDisableCameraShake;

            Color color = Color.white;
            color.a = CFXR_Effect.GlobalDisableCameraShake ? 0.33f : 1.0f;
            btnCamShake.color = color;
            lblCamShake.color = color;
        }

        public void ToggleEffectsLights()
        {
            CFXR_Effect.GlobalDisableLights = !CFXR_Effect.GlobalDisableLights;

            Color color = Color.white;
            color.a = CFXR_Effect.GlobalDisableLights ? 0.33f : 1.0f;
            btnLights.color = color;
            lblLights.color = color;
        }

        public void ToggleBloom()
        {
            bloom.enabled = !bloom.enabled;

            Color color = Color.white;
            color.a = !bloom.enabled ? 0.33f : 1.0f;
            btnBloom.color = color;
            lblBloom.color = color;
        }

        public void ResetCam()
        {
            demoCamera.transform.position = camInitialPosition;
            demoCamera.transform.rotation = camInitialRotation;
        }

        public void PlayAtIndex()
        {
            if (currentEffect != null)
            {
                currentEffect.SetActive(false);
            }

            currentEffect = effectsList[index];
            currentEffect.SetActive(true);

            UpdateLabels();
        }

        private void WrapIndex()
        {
            if (index < 0) index = effectsList.Length - 1;
            if (index >= effectsList.Length) index = 0;
        }

        private void UpdateLabels()
        {
            labelEffect.text = currentEffect.name;
            labelIndex.text = string.Format("{0}/{1}", index + 1, effectsList.Length);
        }

        private static class ButtonsPressed
        {
        #if UNITY_6000_0_OR_NEWER && CFXR_NEW_INPUT_SYSTEM
	        internal static bool PlayEffect => Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
	        internal static bool RestartEffect => Keyboard.current != null && (Keyboard.current.deleteKey.wasPressedThisFrame || Keyboard.current.backspaceKey.wasPressedThisFrame);
	        internal static bool Left => Keyboard.current != null && Keyboard.current.leftArrowKey.wasPressedThisFrame;
	        internal static bool Right => Keyboard.current != null && Keyboard.current.rightArrowKey.wasPressedThisFrame;
	        internal static bool Mouse0 => Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
	        internal static bool Mouse1 => Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;
	        internal static bool Mouse2 => Mouse.current != null && Mouse.current.middleButton.wasPressedThisFrame;
	        internal static Vector2 MousePosition => Mouse.current != null ? Mouse.current.position.value : Vector2.zero;
	        internal static float MouseScrollY => Mouse.current != null ? Mouse.current.scroll.value.y : 0;
        #else
            internal static bool PlayEffect => Input.GetKeyDown(KeyCode.Space);

            internal static bool RestartEffect =>
                Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace);

            internal static bool Left => Input.GetKeyDown(KeyCode.LeftArrow);
            internal static bool Right => Input.GetKeyDown(KeyCode.RightArrow);
            internal static bool Mouse0 => Input.GetMouseButtonDown(0);
            internal static bool Mouse1 => Input.GetMouseButtonDown(1);
            internal static bool Mouse2 => Input.GetMouseButtonDown(2);
            internal static Vector2 MousePosition => Input.mousePosition;
            internal static float MouseScrollY => Input.mouseScrollDelta.y;
        #endif
        }
    }
}
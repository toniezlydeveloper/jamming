using System;
using System.Collections.Generic;
using Audio;
using Internal.Runtime.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Elements
{
    public class ProcessingWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform successWindow;
        [SerializeField] private Image indicator;
        [SerializeField] private float indicatorSpeed;
        [SerializeField] private RectTransform indicatorTransform;
        [SerializeField] private RectTransform maxPoint;
        [SerializeField] private RectTransform minPoint;
        [SerializeField] private InputActionReference processInput;
        [SerializeField] private CanvasGroup group;

        private float _normalizedPosition;
        private bool _shouldGoUp = true;
        private float _minPositionX;
        private float _maxPositionX;
        private List<float> _chances;
        private Action _success;
        private Action _fail;
        private int _index;
        
        public static bool IsProcessing { get; private set; }

        private void OnDestroy()
        {
            IsProcessing = false;
        }

        private void Update()
        {
            group.interactable = IsProcessing;
            group.blocksRaycasts = IsProcessing;
            group.alpha = IsProcessing ? 1 : 0;
            
            if (!IsProcessing)
            {
                return;
            }
            
            if (_shouldGoUp)
            {
                _normalizedPosition += Time.deltaTime;
            }
            else
            {
                _normalizedPosition -= Time.deltaTime;
            }

            if (_normalizedPosition >= 1f)
            {
                _shouldGoUp = false;
            }

            if (_normalizedPosition <= 0f)
            {
                _shouldGoUp = true;
            }

            _normalizedPosition = _normalizedPosition.Clamp(0f, 1f);

            indicatorTransform.position =
                minPoint.position + (maxPoint.position - minPoint.position) * _normalizedPosition;

            if (!processInput.action.triggered)
            {
                return;
            }
            
            // 1. Get positions in the same local coordinate space
            float indX = indicatorTransform.anchoredPosition.x;
            float halfIndWidth = indicatorTransform.rect.width * 0.5f;

            float swX = successWindow.anchoredPosition.x;
            float halfSwWidth = successWindow.rect.width * 0.5f;

            // 2. Compute their left/right bounds
            float indicatorLeft  = indX - halfIndWidth;
            float indicatorRight = indX + halfIndWidth;

            float swLeft  = swX - halfSwWidth;
            float swRight = swX + halfSwWidth;

            // 3. Check if they overlap
            bool isInside = indicatorRight > swLeft && indicatorLeft < swRight;

            if (isInside)
            {
                _index++;

                if (_index > _chances.Count - 1)
                {
                    FindAnyObjectByType<SfxPlayer>().Play(SfxType.UIClick);
                    _success.Invoke();
                    IsProcessing = false;
                    return;
                }
                else
                {
                    SetUp(_chances[_index]);
                    FindAnyObjectByType<SfxPlayer>().Play(SfxType.UIClick);
                    return;
                }
            }
            else
            {
                IsProcessing = false;
                FindAnyObjectByType<SfxPlayer>().Play(SfxType.UIClick);
                _fail.Invoke();
                return;
            }
        }

        public void Process(Action successCallback, Action failCallback, List<float> chances)
        {
            _chances = chances;
            _success = successCallback;
            _fail = failCallback;
            _index = 0;

            if (chances == null || chances.Count == 0)
            {
                successCallback.Invoke();
                return;
            }
            
            IsProcessing = true;
            SetUp(chances[0]);
        }

        private void SetUp(float normalizedChance)
        {
            // 1. Get background width in local space
            float bgWidth = background.rect.width;

            // 2. Compute success window width
            float successWidth = bgWidth * normalizedChance;

            // 3. Apply width
            Vector2 size = successWindow.sizeDelta;
            size.x = successWidth;
            successWindow.sizeDelta = size;

            // 4. Compute valid horizontal range inside background
            float maxPos = (bgWidth - successWidth) * 0.5f;
            float minPos = -maxPos;

            // 5. Pick a random x position
            float randomX = Random.Range(minPos, maxPos);

            // 6. Apply position (local space)
            Vector2 pos = successWindow.anchoredPosition;
            pos.x = randomX;
            successWindow.anchoredPosition = pos;
        }
    }
}
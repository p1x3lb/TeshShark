using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Components
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _fill;

        [SerializeField, MinValue(0f), MaxValue(1f), Range(0f, 1f)]
        private float _value;
        
        [SerializeField]
        private float _duration = 0.5f;
        
        public float Value => _value;

        public UniTask UpdateValue(float value, bool instantly = false, Action<float> onProgress = null, CancellationToken cancellationToken = default)
        {
            return UpdateValue(value, _duration, instantly, onProgress, cancellationToken);
        }
        
        public UniTask UpdateValue(float value, float duration, bool instantly = false,  Action<float> onProgress = null, CancellationToken cancellationToken = default)
        {
            if (instantly)
            {
                _value = value;
                UpdateProgressFill(value);
                return UniTask.CompletedTask;
            }

            return UpdateProgressCoroutine(value, duration, onProgress, cancellationToken);
        }

        private async UniTask UpdateProgressCoroutine(float value, float duration, Action<float> onProgress, CancellationToken cancellationToken)
        {
            var timeLeft = duration;
            var current = _value;
            while (timeLeft > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                timeLeft -= Time.deltaTime;
                float progress = Mathf.Lerp(current, value, 1.0f - timeLeft / duration);
                UpdateProgressFill(_value = progress);
                onProgress?.Invoke(_value);
                await UniTask.Yield();
            }
        }

        private void UpdateProgressFill(float fill)
        {
            _fill.sizeDelta = new Vector2(Mathf.Clamp01(fill) * ((RectTransform) _fill.parent).rect.width, _fill.sizeDelta.y);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateProgressFill(_value);
        }
#endif
    }
}
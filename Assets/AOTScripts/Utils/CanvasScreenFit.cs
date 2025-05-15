using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public class CanvasScreenFit : UIBehaviour
    {
        public Action ResolutionChange;
        public Action RectTransformChange;
        private int _lastScreenHeight = -1;

        private int _lastScreenWidth = -1;
        private CanvasScaler _scaler;

        protected override void Awake()
        {
            base.Awake();
            _scaler = GetComponent<CanvasScaler>();

            AdjustResolution();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            UtilsLog.LogRelease("CanvasScreenFit.OnRectTransformDimensionsChange: " + Screen.width + " " + Screen.height);
            base.OnRectTransformDimensionsChange();
            AdjustResolution();
            if (RectTransformChange != null)
            {
                RectTransformChange();
            }
        }

        private void AdjustResolution()
        {
            UtilsLog.LogRelease("CanvasScreenFit.AdjustResolution: " + Screen.width + " " + Screen.height);
            if (_scaler == null)
            {
                return;
            }

            UtilsLog.LogRelease("屏幕实际分辨率1: " + Screen.width + " " + Screen.height);

            if (Screen.width > 10000 || Screen.height > 10000)
            {
                UtilsLog.LogRelease("屏幕实际分辨率 error: " + Screen.width + " " + Screen.height);
                return;
            }

            if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
            {
                UtilsLog.LogRelease("屏幕实际分辨率2: " + Screen.width + " " + Screen.height);

                _lastScreenWidth = Screen.width;
                _lastScreenHeight = Screen.height;
                var rate = (float)Screen.width / Screen.height;
                if (rate > _scaler.referenceResolution.x / _scaler.referenceResolution.y)
                {
                    _scaler.matchWidthOrHeight = 1.0f;
                    Display.SetDesignSize(_scaler.referenceResolution.y * rate, _scaler.referenceResolution.y);
                }
                else
                {
                    _scaler.matchWidthOrHeight = 0.0f;
                    Display.SetDesignSize(_scaler.referenceResolution.x, _scaler.referenceResolution.x / rate);
                }

                if (ResolutionChange != null)
                {
                    ResolutionChange();
                }
            }
        }
    }


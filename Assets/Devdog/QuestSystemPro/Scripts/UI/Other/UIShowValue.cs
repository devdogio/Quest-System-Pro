using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    [System.Serializable]
    public partial class UIShowValue
    {
        [Header("Text")]
        public UnityEngine.UI.Text textField;
        public string textFormat = "{0}/{1}";
        public int roundToDecimals = 1;
        public bool clearTextWhenZero = true;

        [Header("Slider")]
        public UnityEngine.UI.Slider slider;
        public bool hideSliderWhenZero = false;

        [Header("Image fill")]
        public UnityEngine.UI.Image imageFill; // Used for fillAmount


        public void Repaint(float current, float max)
        {
            if (textField != null)
            {
                if (current < 0.0001f && clearTextWhenZero)
                {
                    textField.text = "";
                    SetActive(textField.gameObject, false);
                }
                else
                {
                    textField.text = string.Format(textFormat, System.Math.Round(current, roundToDecimals), System.Math.Round(max, roundToDecimals));
                    SetActive(textField.gameObject, true);
                }
            }

            if (slider != null)
            {
                slider.minValue = 0.0f;
                slider.maxValue = max;

                if (current <= 0.0001f)
                {
                    if (hideSliderWhenZero)
                    {
                        SetActive(slider.gameObject, false);
                    }
                    else
                    {
                        SetActive(slider.gameObject, true);
                    }
                }
                else
                {
                    SetActive(slider.gameObject, true);
                }

                // To avoid GC
                if (current != slider.value)
                {
                    slider.value = current;
                }
            }

            if (imageFill != null)
            {
                float n = current / max;

                SetActive(imageFill.gameObject, n > 0.001f);

                // To avoid GC
                if (n != imageFill.fillAmount)
                {
                    imageFill.fillAmount = n;
                }
            }
        }

        private void SetActive(GameObject b, bool set)
        {
            if (b.activeSelf != set)
            {
                b.SetActive(set);
            }
        }

        public void HideAll()
        {
            if (textField != null)
                textField.gameObject.SetActive(false);

            if (slider != null)
                slider.gameObject.SetActive(false);

            if (imageFill != null)
                imageFill.gameObject.SetActive(false);
        }
    }
}

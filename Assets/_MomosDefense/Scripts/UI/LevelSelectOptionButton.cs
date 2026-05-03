using MomosDefense.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MomosDefense.UI
{
    public sealed class LevelSelectOptionButton : MonoBehaviour
    {
        [SerializeField] private LevelDefinition level;
        [SerializeField] private Button button;
        [SerializeField] private Text label;
        [SerializeField] private Image background;
        [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 1f);
        [SerializeField] private Color selectedColor = new Color(0.95f, 0.82f, 0.4f, 1f);

        private LevelSelectShellController owner;

        public LevelDefinition Level => level;

        private void Awake()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            if (background == null)
            {
                background = GetComponent<Image>();
            }
        }

        private void OnEnable()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnPressed);
            }

            RefreshLabel();
        }

        private void OnDisable()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(OnPressed);
            }
        }

        public void Bind(LevelSelectShellController controller)
        {
            owner = controller;
            RefreshLabel();
        }

        public void SetSelected(bool isSelected)
        {
            if (background != null)
            {
                background.color = isSelected ? selectedColor : normalColor;
            }
        }

        private void OnPressed()
        {
            owner?.SelectLevel(level);
        }

        private void RefreshLabel()
        {
            if (label != null && level != null)
            {
                label.text = level.DisplayName;
            }
        }
    }
}

using UnityEngine;
using TMPro;

namespace SIM.UI
{
    public class Hint : MonoBehaviour
    {
        public string Text { get { return myText.text; } set { myText.text = value; } }

        [SerializeField] float timeToHide = .5f;
        [SerializeField] TextMeshProUGUI myText;

        float timer = 0;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeToHide) HideUI();
        }

        public void ShowUI()
        {
            timer = 0;
            gameObject.SetActive(true);
        }

        public void HideUI()
        {
            gameObject.SetActive(false);
        }
    }
}
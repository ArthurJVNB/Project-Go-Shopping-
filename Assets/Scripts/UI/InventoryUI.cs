using UnityEngine;
using SIM.Core;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace SIM.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] Inventory inventory;
        [SerializeField] Transform slotsContainer;
        [SerializeField] RectTransform slotTemplate;

        private void Awake() {
            if (!inventory)
            {
                Debug.LogWarning(typeof(InventoryUI) + " doesn't have a " + typeof(Inventory) +
                 " attached to it! Please do this if you want to use " + typeof(InventoryUI));
            }
        }

        private void OnEnable() => inventory.onInventoryChanged += UpdateUI;

        private void OnDisable() => inventory.onInventoryChanged -= UpdateUI;

        private void UpdateUI()
        {
            const int CELLS_PER_ROW = 4;
            
            float cellSizeX = slotTemplate.rect.width;
            float cellSizeY = slotTemplate.rect.height;
            
            float offsetX = slotTemplate.anchoredPosition.x;
            float offsetY = slotTemplate.anchoredPosition.y;
            
            int cellsInRow = 0;
            int currentRow = 0;

            float x = 0;
            float y = 0;

            ClearOldItems();

            ShowMoneyAmount();

            Item[] items = inventory.GetItems();
            for (int i = 0; i < items.Length; i++)
            {
                if (cellsInRow >= CELLS_PER_ROW)
                {
                    cellsInRow = 0;
                    currentRow--; // It goes from top to bottom
                }

                RectTransform uiItem = Instantiate(slotTemplate, slotsContainer);

                x = offsetX + cellsInRow * cellSizeX;
                y = offsetY + currentRow * cellSizeY;
                Vector2 position = new Vector2(x, y);

                uiItem.anchoredPosition = position;
                uiItem.Find("Item preview").GetComponent<Image>().sprite = items[i].UIImage;
                uiItem.gameObject.SetActive(true);

                cellsInRow++;
            }
        }

        private void ShowMoneyAmount()
        {
            // throw new NotImplementedException();
        }

        private void ClearOldItems()
        {
            foreach (Transform child in slotsContainer)
            {
                if (child != slotTemplate) Destroy(child.gameObject);
            }
        }
    }
}
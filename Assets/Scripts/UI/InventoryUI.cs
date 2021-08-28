using UnityEngine;
using SIM.Core;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

namespace SIM.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public Action<Item> onItemClicked;

        [SerializeField] Inventory inventory;
        [SerializeField] Transform slotsContainer;
        [SerializeField] InventoryButton slotTemplate;
        [SerializeField] TextMeshProUGUI moneyText;

        private void Awake()
        {
            if (!inventory)
            {
                Debug.LogWarning(typeof(InventoryUI) + " doesn't have a " + typeof(Inventory) +
                 " attached to it! Please do this if you want to use " + typeof(InventoryUI));
            }
        }

        private void OnEnable() => inventory.onInventoryChanged += UpdateUI;

        private void OnDisable() => inventory.onInventoryChanged -= UpdateUI;

        public void ShowUI()
        {
            UpdateUI();
            gameObject.SetActive(true);
        }

        public void HideUI()
        {
            gameObject.SetActive(false);
        }

        public void SwitchUI()
        {
            if (gameObject.activeInHierarchy) HideUI();
            else ShowUI();
        }

        private void UpdateUI()
        {
            const int CELLS_PER_ROW = 4;

            RectTransform template = slotTemplate.GetComponent<RectTransform>();

            float cellSizeX = template.rect.width;
            float cellSizeY = template.rect.height;

            float offsetX = template.anchoredPosition.x;
            float offsetY = template.anchoredPosition.y;

            int cellsInRow = 0;
            int currentRow = 0;

            float x = 0;
            float y = 0;

            ClearOldItems();

            UpdateMoneyAmount();

            Item[] items = inventory.GetItems();
            for (int i = 0; i < items.Length; i++)
            {
                Item currentItem = items[i];

                if (cellsInRow >= CELLS_PER_ROW)
                {
                    cellsInRow = 0;
                    currentRow--; // It goes from top to bottom
                }

                InventoryButton inventoryItem = Instantiate(slotTemplate, slotsContainer);
                RectTransform inventoryItemRect = inventoryItem.GetComponent<RectTransform>();

                x = offsetX + cellsInRow * cellSizeX;
                y = offsetY + currentRow * cellSizeY;
                Vector2 position = new Vector2(x, y);

                inventoryItemRect.anchoredPosition = position;
                inventoryItemRect.Find("Item preview").GetComponent<Image>().sprite = currentItem.InventoryImage;
                inventoryItemRect.gameObject.SetActive(true);

                inventoryItem.Item = currentItem;
                inventoryItem.onClicked += OnItemClicked;

                cellsInRow++;
            }
        }

        private void OnItemClicked(InventoryButton inventoryItem)
        {
            onItemClicked?.Invoke(inventoryItem.Item);
        }

        private void UpdateMoneyAmount()
        {
            moneyText.text = inventory.Money.ToString();
        }

        private void ClearOldItems()
        {
            foreach (Transform child in slotsContainer)
            {
                InventoryButton item = child.GetComponent<InventoryButton>();

                if (item != slotTemplate)
                {
                    item.onClicked -= OnItemClicked;
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
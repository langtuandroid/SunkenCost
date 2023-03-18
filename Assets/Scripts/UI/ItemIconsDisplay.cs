using Items;
using UnityEngine;

namespace UI
{
    public class ItemIconsDisplay : MonoBehaviour
    {
        [SerializeField] private Transform displayGrid;
        [SerializeField] private GameObject itemIconDisplayPrefab;
        
        private void Start()
        {
            foreach (var itemInstance in RunProgress.ItemInventory.ItemInstances)
            {
                Debug.Log(itemInstance.Title);
                AddItemToDisplay(itemInstance);
            }
        }

        private void AddItemToDisplay(ItemInstance itemInstance)
        {
            var newItemIconGameObject = Instantiate(itemIconDisplayPrefab, displayGrid);
            var newItemDisplay = newItemIconGameObject.GetComponent<ItemDisplay>();
            newItemDisplay.SetItemInstance(itemInstance);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfferManager : MonoBehaviour
{
    public static OfferManager Current;

    [SerializeField] private Transform cardGrid;
    [SerializeField] private GameObject designCardPrefab;
    [SerializeField] private GameObject itemOfferPrefab;
    [SerializeField] private GameObject stickOfferPrefab;
    [SerializeField] private GameObject moveOfferPrefab; 

    [SerializeField] private Transform bootyGrid;

    public List<DesignCard> DesignCards { get; private set; } = new List<DesignCard>();

    public int CardsOnTopRow => cardGrid.GetChild(0).GetChild(0).childCount;

    private void Awake()
    {
        if (Current)
        {
            Destroy(Current.gameObject);
        }
        else
        {
            Current = this;
        }
    }

    private void Start()
    {
        if (GameProgress.BattleNumber % 2 != 0)
        {
            bootyGrid.gameObject.SetActive(false);
            CreateCardOfferGrids();
        }
        else
        {
            
            var used = new[] {false, false, false};
            var optionsPlaced = 0;
            
            // Make two options, either in position 0 or 2
            // This is fuckin ugly code but I cbf
            while (optionsPlaced < 2)
            {
                var rand = Random.Range(0, 3);

                switch (rand)
                {
                    case 0:
                        if (used[0]) continue;
                        SpawnStick(optionsPlaced * 2);
                        optionsPlaced++;
                        used[0] = true;
                        break;
                    case 1:
                        if (used[1]) continue;
                        SpawnMove(optionsPlaced * 2);
                        optionsPlaced++;
                        used[1] = true;
                        break;
                    case 2:
                        if (used[2]) continue;
                        SpawnItem(optionsPlaced * 2);
                        optionsPlaced++;
                        used[2] = true;
                        break;
                }
            }
        }
    }

    private void SpawnStick(int position)
    {
        var stickOfferGameObject = Instantiate(stickOfferPrefab, bootyGrid.GetChild(0));
        stickOfferGameObject.transform.SetSiblingIndex(position);
    }
    
    private void SpawnMove(int position)
    {
        var moveOfferGameObject = Instantiate(moveOfferPrefab, bootyGrid.GetChild(0));
        moveOfferGameObject.transform.SetSiblingIndex(position);
    }

    private void SpawnItem(int position)
    {
        var itemOfferGameObject = Instantiate(itemOfferPrefab, bootyGrid.GetChild(0));
        itemOfferGameObject.transform.SetSiblingIndex(position);
        var itemOffer = itemOfferGameObject.GetComponent<ItemOffer>();

        if (itemOffer is null)
        {
            Debug.Log("Null Item!");
        }
        else
        {
            var itemId = ItemManager.GetRandomItem();
            var titleDesc = ItemManager.GetItemTitleAndDescription(itemId);
            itemOffer.ItemId = itemId;
            itemOffer.Sprite = ItemManager.GetItemSprite(itemId);
            itemOffer.Title = titleDesc[0];
            itemOffer.Desc = titleDesc[1];
        }
    }

    private void CreateCardOfferGrids()
    {
        cardGrid.gameObject.SetActive(true);
        
        var upperGridContent = cardGrid.GetChild(0).GetChild(0);
        foreach (var design in PlayerInventory.Deck)
        {
            //Debug.Log(design.Title);
            var newCardObject = Instantiate(designCardPrefab, upperGridContent);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();
            info.design = design;
            
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }
        
        var lowerGridContent = cardGrid.GetChild(1).GetChild(0);
        for (var i = 0; i < 3; i++)
        {
            var newCardObject = Instantiate(designCardPrefab, lowerGridContent);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();

            var designName = GetDesign();
            var designType = DesignManager.GetDesignType(designName);
            var design = (Design)Activator.CreateInstance(designType);
            info.design = design;
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }

        StartCoroutine(WaitForDesignCardsToInitialise());
    }

    private string GetDesign()
    {
        var currentProgress = GameProgress.BattleNumber;
        var maxProgress = 50.0;
        
        var chancesOnFirstBattle = new[] {0.8, 0.5, 0.05};
        var chancesOnLastPossibleBattle = new[] {0.2, 0.6, 0.2};

        var percentage = currentProgress / maxProgress;

        var currentChances = new double[chancesOnFirstBattle.Length];

        for (var i = 0; i < currentChances.Length; i++)
        {
            currentChances[i] = chancesOnFirstBattle[i] +
                                (chancesOnLastPossibleBattle[i] - chancesOnFirstBattle[i]) * percentage;
            
        }

        var randomNum = (double)Random.value;
        
        // Common
        if (randomNum <= currentChances[0])
        {
            return DesignManager.CommonDesigns[Random.Range(0, DesignManager.CommonDesigns.Count)];
        }

        randomNum -= currentChances[0];
            
        // Uncommon
        if (randomNum <= currentChances[1])
        {
            return DesignManager.UncommonDesigns[Random.Range(0, DesignManager.UncommonDesigns.Count)];
        }

        // Rare
        return DesignManager.RareDesigns[Random.Range(0, DesignManager.RareDesigns.Count)];
    }

    public void Merge(DesignCard cardBeingMerged, DesignCard cardBeingMergedInto)
    {
        DesignCards.Remove(cardBeingMerged);
        Destroy(cardBeingMerged.gameObject);
        cardBeingMergedInto.Design.LevelUp();
        OfferScreenEvents.Current.GridsUpdated();
    }

    public void AcceptOffer(string offerAccepted)
    {
        bootyGrid.gameObject.SetActive(false);

        if (offerAccepted == "Plank")
        {
            GameProgress.MaxPlanks++;
        }
        else if (offerAccepted == "Move")
        {
            GameProgress.MovesPerTurn++;
        }
        else
        {
            // Item
            PlayerInventory.Items.Add(offerAccepted);
        }
        
        CreateCardOfferGrids();
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        OfferScreenEvents.Current.GridsUpdated();
    }
}

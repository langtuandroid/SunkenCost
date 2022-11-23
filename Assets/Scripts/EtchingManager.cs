using System;
using System.Collections;
using System.Collections.Generic;
using Etchings;
using UnityEngine;

public class EtchingManager : MonoBehaviour
{
    public static EtchingManager current;
    
    public GameObject etchingPrefab;
    
    public List<ActiveEtching> etchingOrder = new List<ActiveEtching>();
    public int etchingCount = 0;
    
    public bool finishedProcessingEnemyMove = true;
    
    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    private void Start()
    {
        GameEvents.current.OnPlayerMovedStick += RefreshEtchingOrder;
        GameEvents.current.OnPlayerBoughtStick += RefreshEtchingOrder;
        GameEvents.current.OnBeginEnemyTurn += BeginEnemyTurn;
        GameEvents.current.OnBeginEnemyMove += BeginEnemyMovement;
        GameEvents.current.OnEnemyMoved += EnemyMoved;
        GameEvents.current.OnSticksUpdated += OnSticksUpdated;
    }

    private void Update()
    {
        if (ActiveEnemiesManager.current.finishedProcessingEnemyTurn && !finishedProcessingEnemyMove &&
            ActiveEnemiesManager.current.NumberOfActiveEnemies == 0)
        {
            finishedProcessingEnemyMove = true;
        }
    }
    
    public void CreateEtching(Stick stick, Design design)
    {
        // Destroy the dummy
        DestroyDummyEtching(stick);
        
        var etchingSlot = stick.transform.GetChild(1);
        
        // Over 1 because the dummy won't fully have been destroyed yet (same frame)
        if (etchingSlot.childCount > 1)
        {
            // There's already an etching here!
            var occupyingEtchingTransform = etchingSlot.GetChild(0);
            var occupyingEtching = occupyingEtchingTransform.GetComponent<ActiveEtching>();
            etchingOrder.Remove(occupyingEtching);

            // Add to the discard pile
            // TODO: ADD EXHAUSTING CARDS
            Deck.current.AddDesignToDiscardPile(occupyingEtching.design);

            Destroy(occupyingEtchingTransform.gameObject);
        }

        var newEtching = Instantiate(etchingPrefab, etchingSlot);
        var etching = newEtching.AddComponent(DesignManager.GetEtchingType(design.Category)).GetComponent<ActiveEtching>();
        etching.design = design;
        etchingSlot.transform.parent.GetComponent<Stick>().etching = etching;
        etchingCount++;
    }

    public void RemoveEtching(ActiveEtching etching)
    {
        etchingOrder.Remove(etching);
    }

    public void CreateDummyEtching(Stick stick, Design design)
    {
        var etchingSlot = stick.transform.GetChild(1);
        var occupyingEtching = stick.etching;
        if (occupyingEtching) occupyingEtching.SetVisible(false);

        var dummyEtchingGameObject = Instantiate(etchingPrefab, etchingSlot);
        var dummyEtching = dummyEtchingGameObject.AddComponent<DummyEtching>();
        dummyEtching.design = design;
    }

    public void DestroyDummyEtching(Stick stick)
    {
        var etchingSlot = stick.transform.GetChild(1);
        
        if (stick.etching)
        {
            Destroy(etchingSlot.GetChild(1).gameObject);
            stick.etching.SetVisible(true);
        }
        else
        {
            Destroy(etchingSlot.GetChild(0).gameObject);
        }
    }

    public void StickDestroyed()
    {
        RefreshEtchingOrder();
    }
    
    private void RefreshEtchingOrder()
    {
        var newEtchingOrder = new List<ActiveEtching>();
        for (var i = 1; i < StickManager.current.stickGrid.childCount; i++)
        {
            var stick = StickManager.current.stickGrid.GetChild(i).GetComponent<Stick>();
            if (stick == null) continue;
            if (stick.etching == null) continue;

            newEtchingOrder.Add(stick.etching);
        }

        etchingOrder = newEtchingOrder;
        GameEvents.current.EtchingsUpdated();
    }

    private void BeginEnemyTurn()
    {
        RefreshEtchingOrder();
    }
    
    private void BeginEnemyMovement()
    {
        StartCoroutine(TestCharPreMovementActivatedEtchings());
    }
    
    private void EnemyMoved()
    {
        StartCoroutine(TestCharMovementActivatedEtchings());
    }

    private void OnSticksUpdated()
    {
        // Test left to right
        foreach (var etching in etchingOrder)
        {
            // Only character activated etchings
            var stickUpdateActivatedEtching = etching as StickUpdateActivatedEtching;
            stickUpdateActivatedEtching?.DetectStickMovement();
        }
    }
    
    private IEnumerator TestCharPreMovementActivatedEtchings()
    {
        finishedProcessingEnemyMove = false;
        
        // Test left to right
        foreach (var etching in etchingOrder)
        {
            // Only character activated etchings
            var characterActivatedEtching = etching as CharPreMovementActivatedEffect;
            if (characterActivatedEtching is null) continue;
            
            if (characterActivatedEtching.DetectCharacterAboutToMove())
            {
                yield return new WaitForSeconds(GameManager.AttackTime);
            }

            // Only continue if there are enemies and etchings
            if (ActiveEnemiesManager.current.NumberOfActiveEnemies == 0 || ActiveEnemiesManager.current.CurrentEnemy.IsDestroyed) break;
        }
        
        // TODO FIX THIS
        yield return new WaitForSeconds(0.2f);
        finishedProcessingEnemyMove = true;
    }
    
    private IEnumerator TestCharMovementActivatedEtchings()
    {
        finishedProcessingEnemyMove = false;
        
        // Test left to right
        foreach (var etching in etchingOrder)
        {
            // Only character activated etchings
            var characterActivatedEtching = etching as CharMovementActivatedEtching;
            if (characterActivatedEtching is null) continue;
            
            if (characterActivatedEtching.DetectCharacterMovement())
            {
                yield return new WaitForSeconds(GameManager.AttackTime);
            }

            // Only continue if there are enemies and etchings
            if (ActiveEnemiesManager.current.NumberOfActiveEnemies == 0 || ActiveEnemiesManager.current.CurrentEnemy.IsDestroyed) break;
        }
        
        // TODO FIX THIS
        yield return new WaitForSeconds(0.2f);
        finishedProcessingEnemyMove = true;
    }
    
    
}

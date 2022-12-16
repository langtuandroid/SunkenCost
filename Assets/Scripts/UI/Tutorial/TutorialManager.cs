using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager current;
    
    [SerializeField] private GameObject tutorialTextPopupPrefab;
    [SerializeField] private GameObject tutorialTextPopupWithButtonPrefab;
    [SerializeField] private GameObject tutorialTextPopupWithButtonAndSkipButtonPrefab;
    [SerializeField] private GameObject arrowPrefab;

    private TutorialUIObject _popup;
    private TextMeshProUGUI _popupText;
    private TutorialUIObject _popupWithButton;
    private TextMeshProUGUI _popupWithButtonText;
    private TutorialUIObject _popupWithButtonAndSkipButton;
    private TutorialUIObject _arrow;
    private bool _keepArrow;
    
    [SerializeField] private Transform buyPlankButton;
    [SerializeField] private Transform nextRoundButton;
    [SerializeField] private Transform stickBoard;

    private Transform _highlightedItem;
    public RectTransform HighlightedItemRectTransform { get; private set; }
    private Transform _highlightedItemParent;
    private int _highlightedItemSiblingIndex;
    private Vector3 _highlightedItemLocalPosition;

    private List<Action> _tutorialSequence;
    private int _placeInTutorialSequence;
    
    // Keeps the enemy in the right place (-50 for first plank, 0 for others)
    private int _enemyX;
    
    // For when the first enemy reaches near the end
    private Enemy _firstEnemy;

    // Stops normal game logic running during the initial tutorial scene
    public bool InLockedTutorial { get; private set; } = true;

    // In case we quit the game while the extra tutorial stuff is waiting for events
    private bool _subscribedToOnBeingPlayerTurn = false;
    private bool _subscribedToOnSticksUpdated = false;
    
    public bool HighlightedEnemy { get; private set; }

    private void Awake()
    {
        current = this;

        // The tutorial popups - these will get moved around for each section of the tutorial
        _popup = Instantiate(tutorialTextPopupPrefab, transform).GetComponent<TutorialUIObject>();
        _popupText = _popup.GetComponentInChildren<TextMeshProUGUI>();
        
        _popupWithButton = Instantiate(tutorialTextPopupWithButtonPrefab, transform).GetComponent<TutorialUIObject>();
        _popupWithButtonText = _popupWithButton.GetComponentInChildren<TextMeshProUGUI>();
        _popupWithButtonAndSkipButton = Instantiate(tutorialTextPopupWithButtonAndSkipButtonPrefab, transform).GetComponent<TutorialUIObject>();
        
        _arrow = Instantiate(arrowPrefab, transform).GetComponent<TutorialUIObject>();

        // Create the tutorial sequence
        _tutorialSequence = new List<Action>()
        {
            SkipTutorial,
            GameIntroduction,
            WaitTilBoughtPlank,
            AskToDragPlankOffer,
            WaitForDraggedPlankOffer,
            ExplainEnemy,
            EnemyHover,
            EnemyMovement,
            EnemyTurnOrder,
            WaitTilBoughtAnotherPlank,
            AskToDragAnotherPlankOffer,
            WaitForDraggedPlankOffer,
            AskToMovePlank,
            WaitToMovePlank,
            PlayerMoves,
            WaitForNextTurn,
            WaitForPlayerMove,
            FinalMessage,
            StartSongAndWaitForEnemyOnLastStick,
            ShowDangerousEnemy,
            WaitForEnoughPlanksToZoom,
            ZoomTip,
            WaitForBoss,
            PointOutBoss,
            WaitToKillBoss
        };

    }

    private void Start()
    {
        BattleEvents.Current.OnOfferDesigns += ExecuteNextTutorialStep;
        BattleEvents.Current.OnStickAdded += ExecuteNextTutorialStep;
        BattleEvents.Current.OnBeginEnemyTurn += ExecuteNextTutorialStep;
        BattleEvents.Current.OnPlayerMovedStick += ExecuteNextTutorialStep;
        BattleEvents.Current.OnBeginPlayerTurn += ExecuteNextTutorialStep;
        SetVisibility(true);
        
        NextTurnButton.Current.CanClick(false);
        
        // Start the sequence
        _placeInTutorialSequence = -1;
        ExecuteNextTutorialStep();
    }
    
    public void ExecuteNextTutorialStep()
    {
        _popup.SetActive(false);
        _popupWithButton.SetActive(false);
        
        // So that the arrow doesn't jump in animation
        if (_keepArrow)
        {
            _keepArrow = false;
        }
        else
        {
            _arrow.SetActive(false);
        }

        _placeInTutorialSequence++;

        if (_placeInTutorialSequence < _tutorialSequence.Count)
        {
            _tutorialSequence[_placeInTutorialSequence].Invoke();
        }
        else
        {
            Debug.Log("Finished tutorial!");
            SetVisibility(false);
        }
    }

    #region Highlight
    
    private void Highlight(Transform itemToHighlight)
    {
        _highlightedItem = itemToHighlight;
        _highlightedItemParent = itemToHighlight.parent;
        _highlightedItemSiblingIndex = itemToHighlight.GetSiblingIndex();
        _highlightedItemLocalPosition = itemToHighlight.localPosition;
        HighlightedItemRectTransform = itemToHighlight.GetComponent<RectTransform>();
        //Debug.Log(_highlightedItemLocalPosition);
        
        itemToHighlight.SetParent(transform, true);
        
        // Just below the dimmer
        itemToHighlight.SetSiblingIndex(1);
    }

    private void UnHighlight()
    {
        if (!_highlightedItem) return;
        
        _highlightedItem.SetParent(_highlightedItemParent, true);
        _highlightedItem.SetSiblingIndex(_highlightedItemSiblingIndex);
        //_highlightedItem.localPosition = _highlightedItemLocalPosition;
        
        //Debug.Log(_highlightedItem.localPosition);
        _highlightedItem = null;
        HighlightedItemRectTransform = null;
    }

    private void HighlightEnemy(Transform enemyTransform)
    {
        HighlightedEnemy = true;
        Highlight(enemyTransform);
    }

    private void UnHighlightEnemy()
    {
        UnHighlight();
        HighlightedEnemy = false;
    }

    private void SetVisibility(bool visible)
    {
        TutorialDimmerPanel.current.SetVisible(visible);
    }
    
    #endregion

    public void SkipTutorial()
    {
        BattleEvents.Current.OnOfferDesigns -= ExecuteNextTutorialStep;
        BattleEvents.Current.OnStickAdded -= ExecuteNextTutorialStep;
        BattleEvents.Current.OnBeginEnemyTurn -= ExecuteNextTutorialStep;
        BattleEvents.Current.OnPlayerMovedStick -= ExecuteNextTutorialStep;
        BattleEvents.Current.OnBeginPlayerTurn -= ExecuteNextTutorialStep;
        
        NextTurnButton.Current.CanClick(true);

        Music.current.SelectSong(1);

        _popup.SetActive(false);
        _popupWithButton.SetActive(false);
        _arrow.SetActive(false);
        _popupWithButtonAndSkipButton.SetActive(false);
        SetVisibility(false);
        InLockedTutorial = false;
        
        Destroy(gameObject);

    }

    #region Tutorial Sequence

    private void GameIntroduction()
    {
        InLockedTutorial = true;
        _popupWithButtonAndSkipButton.Move(0, 0);
        SetVisibility(true);
    }
    
    private void WaitTilBoughtPlank()
    {

        _popupWithButtonAndSkipButton.SetActive(false);

        Highlight(buyPlankButton);
        
        _popup.Move(-640, 0);
        _popupText.text = "Start by buying your first plank.";


        _arrow.Move(-250, 0, - 1);
    }

    private void AskToDragPlankOffer()
    {
        UnHighlight();
        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text = "Drag one of these planks between the beach and your boat.";
    }

    private void WaitForDraggedPlankOffer()
    {
        SetVisibility(false);
    }

    private void ExplainEnemy()
    {
        SetVisibility(true);
        
        var currentEnemy = ActiveEnemiesManager.current.CurrentEnemy;
        HighlightEnemy(currentEnemy.transform);
        
        _popupWithButton.Move(570, 0);
        _popupWithButtonText.text = "Enemies move to the right. If they reach your boat, you lose a life.";

        _arrow.Move(180, 0);
        
        _keepArrow = true;
    }

    private void EnemyHover()
    {
        _popupWithButton.Move(570, 0);
        _popupWithButtonText.text = "Hover over an enemy at any time to see it's movement range and abilities.";
        
    }
    
    private void EnemyMovement()
    {
        _popupWithButton.Move(570, 40);
        _popupWithButtonText.text = "This number indicates how many planks the enemy will move next turn.";

        _arrow.Move(180, 40);
    }
    
    private void EnemyTurnOrder()
    {
        _popupWithButton.Move(450, 40);
        _popupWithButtonText.text = "This shows the order in which enemies will move. Since this is the only enemy, it will move first.";

        _arrow.Move(60, 40);
    }

    private void WaitTilBoughtAnotherPlank()
    {
        UnHighlightEnemy();
        Highlight(buyPlankButton);
        
        _popup.Move(-640, 0);
        _popupText.text = "Let's buy another plank. The first 3 planks are free - you'll have to defeat enemies to buy more.";
        
        _arrow.Move(-250, 0, -1);
    }
    
    private void AskToDragAnotherPlankOffer()
    {
        UnHighlight();
        BattleEvents.Current.OnOfferDesigns -= ExecuteNextTutorialStep;
        
        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text = "Drag this plank to either side of your first.";
    }

    private void AskToMovePlank()
    {
        SetVisibility(true);

        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text =
            "You can also rearrange your planks during your turn by dragging on them.";
    }

    private void WaitToMovePlank()
    {
        _popup.Move(-500, 0);
        _popupText.text =
            "Try moving a plank now, or buy another plank if you're happy with their current arrangement.";
        
        
        SetVisibility(false);
        BattleEvents.Current.OnOfferDesigns += HidePopup;
        NextTurnButton.Current.CanClick(false);
    }

    private void HidePopup()
    {
        BattleEvents.Current.OnOfferDesigns -= HidePopup;
    }

    private void PlayerMoves()
    {
        SetVisibility(true);
        BattleEvents.Current.OnPlayerMovedStick -= ExecuteNextTutorialStep;
        BattleEvents.Current.OnStickAdded -= ExecuteNextTutorialStep;
        UnHighlight();
        
        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text =
            "You get 3 moves per turn. You can use these on buying or moving planks.";
    }

    private void WaitForNextTurn()
    {
        Highlight(nextRoundButton);
        
        _popup.Move(-640, 100);
        _popupText.text = "You're out of moves this turn. Click next turn to play out the enemy's move!";
        
        _arrow.Move(-250, 0, -1);
        
        NextTurnButton.Current.CanClick(true);
    }

    private void WaitForPlayerMove()
    {
        UnHighlight();
        SetVisibility(false);
        BattleEvents.Current.OnBeginEnemyTurn -= ExecuteNextTutorialStep;
    }

    private void FinalMessage()
    {
        BattleEvents.Current.OnBeginPlayerTurn -= ExecuteNextTutorialStep;
        BattleEvents.Current.OnBeginPlayerTurn += CheckForEnemyOnLastStick;
        _subscribedToOnBeingPlayerTurn = true;
        SetVisibility(true);
        
        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text =
            "New enemies spawn every two turns. The boss arrives in 15 turns! Good luck.";
    }

    private void StartSongAndWaitForEnemyOnLastStick()
    {
        Music.current.SelectSong(1);
        SetVisibility(false);
        InLockedTutorial = false;
    }

    private void ShowDangerousEnemy()
    {
        HighlightEnemy(_firstEnemy.transform);
        SetVisibility(true);

        var enemyX = (int)_firstEnemy.transform.localPosition.x;
        var enemyY = (int)_firstEnemy.transform.localPosition.y;
        
        _popupWithButton.Move(-560, 0);
        _popupWithButtonText.text =
            "This enemy is about to land on your boat! You might want to move it's plank further away by dragging it.";
        
        _arrow.Move(-180, 0, -1);
    }

    private void WaitForEnoughPlanksToZoom()
    {
        UnHighlightEnemy();
        SetVisibility(false);
    }

    private void ZoomTip()
    {
        SetVisibility(true);
        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text =
            "You have a few planks now! Scroll with the mousewheel to view them or hold SHIFT while scrolling to zoom in or out";

    }

    private void WaitForBoss()
    {
        SetVisibility(false);
        BattleEvents.Current.OnBossSpawned += ExecuteNextTutorialStep;
    }

    private void PointOutBoss()
    {
        BattleEvents.Current.OnBossSpawned -= ExecuteNextTutorialStep;
        SetVisibility(true);

        _popupWithButton.Move(0, 0);
        _popupWithButtonText.text = "The boss has arrived! Make sure to hover over him to see his special ability. Good luck!";
    }

    private void WaitToKillBoss()
    {
        SetVisibility(false);
        Destroy(gameObject);
    }
    
    /* SET IN TUTORIAL FOR LATER BITS */
    
    
    #endregion


    private void CheckForEnemyOnLastStick()
    {
        // If an enemy has made it to the last stick
        var enemiesOnStick = ActiveEnemiesManager.current.GetEnemiesOnStick(StickManager.current.stickCount - 1).Where(e => e.NextMove > 0).ToList();
        if (enemiesOnStick.Count <= 0) return;
        
        _firstEnemy = enemiesOnStick[0];
        ExecuteNextTutorialStep();
        BattleEvents.Current.OnBeginPlayerTurn -= CheckForEnemyOnLastStick;
        _subscribedToOnBeingPlayerTurn = false;
        _subscribedToOnSticksUpdated = true;
        BattleEvents.Current.OnSticksUpdated += CheckForZoomOut;
    }

    private void CheckForZoomOut()
    {
        if (StickManager.current.stickCount < 5) return;
            
        BattleEvents.Current.OnSticksUpdated -= CheckForZoomOut;
        _subscribedToOnSticksUpdated = false;
        
        ExecuteNextTutorialStep();
    }

    private void OnDestroy()
    {
        if (_subscribedToOnBeingPlayerTurn) BattleEvents.Current.OnBeginPlayerTurn -= CheckForEnemyOnLastStick;
        if (_subscribedToOnSticksUpdated) BattleEvents.Current.OnSticksUpdated -= CheckForZoomOut;
    }
}

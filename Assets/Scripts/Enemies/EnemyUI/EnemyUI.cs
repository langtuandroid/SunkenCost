using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.EnemyUI
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Image _image;

        [SerializeField] private TooltipTrigger _tooltipTrigger;
        
        [SerializeField] private EnemyTurnOrderText _turnOrderText;
        [SerializeField] private EnemyHealthText _healthText;
        [SerializeField] private EnemyMovementText _movementText;
        
        [SerializeField] private Image _poisonImage;
        [SerializeField] private TextMeshProUGUI _poisonText;

        [SerializeField] private EnemySpeechBubble _speechBubble;

        public Image Image => _image;
        public TooltipTrigger TooltipTrigger => _tooltipTrigger;
        public EnemyTurnOrderText TurnOrderText => _turnOrderText;
        public EnemyHealthText HealthText => _healthText;

        public EnemyMovementText MovementText => _movementText;

        public Image PoisonImage => _poisonImage;
        public TextMeshProUGUI PoisonText => _poisonText;
        
        public EnemySpeechBubble SpeechBubble => _speechBubble;
    }
}
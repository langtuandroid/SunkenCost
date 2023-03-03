using UnityEngine;

namespace Challenges
{
    public enum ChallengeRewardType
    {
        None,
        Plank,
        Move,
        DesignOffer,
        ItemOffer,
        ChallengeOffer
    }
    
    [CreateAssetMenu(menuName = "ChallengeRewardArchetype")]
    public class ChallengeRewardArchetype : ScriptableObject
    {
        public ChallengeRewardType challengeRewardType;
        public string title;
        public Sprite sprite;
    }
}
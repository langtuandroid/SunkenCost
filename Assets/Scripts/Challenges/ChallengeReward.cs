using UnityEngine;

namespace Challenges
{
    public enum ChallengeRewardType
    {
        Plank,
        Move,
        DesignOffer,
        ItemOffer,
        ChallengeOffer
    }
    
    [CreateAssetMenu(menuName = "ChallengeRewardArchetype")]
    public class ChallengeReward : ScriptableObject
    {
        public ChallengeRewardType challengeRewardType;
        public string title;
        public Sprite sprite;
    }
}
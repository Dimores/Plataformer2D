using UnityEngine;

namespace Orby.Character
{
    public class CharacterData : ScriptableObject
    {
        [Header("Character Data - Combat Setup")]
        public int life;
        public int armour;
        public int damage;

        [Header("Character Data - Animation Setup")]
        public Animator characterAnimator;
    }
}

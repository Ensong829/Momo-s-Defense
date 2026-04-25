using UnityEngine;

namespace MomosDefense.Core
{
    public sealed class GameState : MonoBehaviour
    {
        [SerializeField] private int startingLives = 20;
        [SerializeField] private int startingGold = 120;

        public int Lives { get; private set; }
        public int Gold { get; private set; }
        public bool IsGameOver { get; private set; }

        private void Awake()
        {
            Lives = startingLives;
            Gold = startingGold;
        }

        public bool SpendGold(int amount)
        {
            if (amount < 0 || Gold < amount)
            {
                return false;
            }

            Gold -= amount;
            return true;
        }

        public void AddGold(int amount)
        {
            if (amount > 0)
            {
                Gold += amount;
            }
        }

        public void LoseLife(int amount = 1)
        {
            if (IsGameOver || amount <= 0)
            {
                return;
            }

            Lives = Mathf.Max(0, Lives - amount);
            IsGameOver = Lives == 0;
        }
    }
}


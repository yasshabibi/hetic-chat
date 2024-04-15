using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuto.aaaaa
{
    public class Player
    {
        private int MaxHealth { get; } = 100;
        public int Health { get; private set; } = 100;

        public float CalculateHealth()
        {
            return (float)Health / MaxHealth;
        }

        public void RemoveHealth(int value)
        {
            Health -= value;
        }
    }
}

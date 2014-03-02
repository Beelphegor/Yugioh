using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class AIPlayer : Player
    {
        public override void StartTurn()
        {
            base.StartTurn();
            MakeDesition();
            EndTurn();
        }

        void MakeDesition()
        {
            if(field.GetComponent<Field>().Monsters().Count < 5)
                OnMonsterSummon(hand.GetComponent<Hand>().Cards.First(x => x.cardMetadata.level <= 4));
        }

    }
}

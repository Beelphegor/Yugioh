using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
            var monstersOnField = field.GetComponent<Field>().Monsters().Count;
           
            if (monstersOnField > 0 && hand.GetComponent<Hand>().Cards.Any(x => x.cardMetadata.level > 4 && x.cardMetadata.level < 7))
            {
                var highestAttackMediumLevelMonsterOnHand =
                    hand.GetComponent<Hand>().Cards.Where(x => x.cardMetadata.level > 4 && x.cardMetadata.level < 7).OrderByDescending(x => x.cardMetadata.attack).FirstOrDefault();
                if (IsWorthToSummon(highestAttackMediumLevelMonsterOnHand))
                    OnSacrificeMonsterSummon(highestAttackMediumLevelMonsterOnHand);
                else
                {
                    var highestAttackLowLevelMonsterOnHand = hand.GetComponent<Hand>().Cards.Where(x => x.cardMetadata.level <= 4).OrderByDescending(x => x.cardMetadata.attack).FirstOrDefault();
                    OnMonsterSummon(highestAttackLowLevelMonsterOnHand);
                }
            }
            else if (monstersOnField > 1 && hand.GetComponent<Hand>().Cards.Any(x => x.cardMetadata.level > 6))
            {
                var highestAttackHighLevelMonsterOnHand =
                    hand.GetComponent<Hand>().Cards.Where(x => x.cardMetadata.level > 6).OrderByDescending(x => x.cardMetadata.attack).FirstOrDefault();
                if (IsWorthToSummon(highestAttackHighLevelMonsterOnHand))
                    OnSacrificeMonsterSummon(highestAttackHighLevelMonsterOnHand);
                else
                {
                    var highestAttackLowLevelMonsterOnHand = hand.GetComponent<Hand>().Cards.Where(x => x.cardMetadata.level <= 4).OrderByDescending(x => x.cardMetadata.attack).FirstOrDefault();
                    OnMonsterSummon(highestAttackLowLevelMonsterOnHand);
                }
            }
            else if(monstersOnField < 5)
            {
                var highestAttackLowLevelMonsterOnHand = hand.GetComponent<Hand>().Cards.Where(x => x.cardMetadata.level <= 4).OrderByDescending(x => x.cardMetadata.attack).FirstOrDefault();
                OnMonsterSummon(highestAttackLowLevelMonsterOnHand);
            }
        }

        bool IsWorthToSummon(Card monster)
        {
            var monstersOnField = field.GetComponent<Field>().Monsters().OrderBy(x => x.cardMetadata.attack).ToList();
            for (int i = 0; i < monster.GetMonstersNeededToSummon(); i++)
            {
                if (monstersOnField[i].cardMetadata.attack >= monster.cardMetadata.attack)
                {
                    return false;
                }
            }
            return true;
        }

        public override void OnMonsterSummon(Card card)
        {
            SummonMonster(card);
        }

        public override void OnSacrificeMonsterSummon(Card card)
        {
            var monstersOnField = field.GetComponent<Field>().Monsters().OrderBy(x => x.cardMetadata.attack).ToList();
            for (int i = 0; i < card.GetMonstersNeededToSummon(); i++)
            {
                field.GetComponent<Field>().RemoveMonsters(monstersOnField[i]);
                cardsOnGraveyard.Add(monstersOnField[i]); //probable esto tenga que ser graveyard.sendMonster
                monstersOnField[i].moveCardToGraveyard(graveyard.transform.position);
            }

            SummonMonster(card);
        }

    }
}

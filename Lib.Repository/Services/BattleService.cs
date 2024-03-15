using Lib.Repository.Entities;

namespace Lib.Repository.Services;

public static class BattleService
{
    public static Round GetFirstAttacker(Monster monsterA, Monster monsterB)
    {
        Round round = new Round();
        if (monsterA.Speed == monsterB.Speed)
        {
            if (monsterA.Attack > monsterB.Attack)
            {
                round.Attacker = monsterA;
                round.Attacked = monsterB;
            }
            else
            {
                round.Attacker = monsterB;
                round.Attacked = monsterA;
            }
        }
        else
        {
            if (monsterA.Speed > monsterB.Speed)
            {
                round.Attacker = monsterA;
                round.Attacked = monsterB;
            }
            else
            {
                round.Attacker = monsterB;
                round.Attacked = monsterA;
            }
        }
        return round;
    }

    public static int GetDamage(Monster attacker, Monster attacked)
    {
        return attacker.Attack <= attacked.Defense ? 1 : attacker.Attack - attacked.Defense;
    }
}

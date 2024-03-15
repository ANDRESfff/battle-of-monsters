using Lib.Repository.Entities;
using Lib.Repository.Repository;

namespace Business.Services
{
    public class BattleService : IBattleService
    {
        private readonly IBattleOfMonstersRepository _repository;

        public BattleService(IBattleOfMonstersRepository battleOfMonstersRepository) 
        {
            _repository = battleOfMonstersRepository;
        }

        public async Task<IEnumerable<Battle>> GetBattles()
        {
            IEnumerable<Battle> battles = await _repository.Battles.GetAllAsync();
            return battles;
        }

        public async Task RemoveBattle(int id)
        {
            await _repository.Battles.RemoveAsync(id);
            await _repository.Save();            
        }

        public async Task<Battle> GetBattleById(int id)
        {
            Battle battle = await _repository.Battles.FindAsync(id);
            return battle;
        }
        public async Task<Battle> CreateBattle(int? monsterA, int? monsterB)
        {
            try
            {
                Monster monsterModelA = await _repository.Monsters.FindAsync(monsterA);
                Monster monsterModelB = await _repository.Monsters.FindAsync(monsterB);

                if (monsterModelA == null || monsterModelB == null)
                    return null;

                Round round = GetFirstAttacker(monsterModelA, monsterModelB);

                while (monsterModelA.Hp > 0 && monsterModelB.Hp > 0)
                {
                    Monster attacker = round.Attacker;
                    Monster attacked = round.Attacked;
                    int damage = GetDamage(round.Attacker, round.Attacked);
                    round.Attacked.Hp -= damage;
                    round.Attacker = attacked;
                    round.Attacked = attacker;
                }

                Battle battleDb = new Battle()
                {
                    MonsterA = monsterModelA.Id,
                    MonsterB = monsterModelB.Id,
                    Winner = monsterModelA.Hp > 0 ? monsterModelA.Id : monsterModelB.Id
                };

                await _repository.Battles.AddAsync(battleDb);
                await _repository.Save();

                return battleDb;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

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
}

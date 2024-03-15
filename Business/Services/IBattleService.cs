using Lib.Repository.Entities;

namespace Business.Services
{
    public interface IBattleService
    {
        public Task<Battle> CreateBattle(int? monsterA, int? monsterB);
        public Task<IEnumerable<Battle>> GetBattles();
        public Task<Battle> GetBattleById(int id);
        public Task RemoveBattle(int id);
    }
}

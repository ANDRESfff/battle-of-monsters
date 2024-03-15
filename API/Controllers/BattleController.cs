using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Lib.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class BattleController : BaseApiController
{
    private readonly IBattleOfMonstersRepository _repository;

    public BattleController(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Battle> battles = await _repository.Battles.GetAllAsync();
        return Ok(battles);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Add([FromBody] Battle battle)
    {
        if (battle.MonsterA == null || battle.MonsterB == null)
            return BadRequest("Missing ID");

        Monster monsterA = await _repository.Monsters.FindAsync(battle.MonsterA);
        Monster monsterB = await _repository.Monsters.FindAsync(battle.MonsterB);

        if (monsterA == null || monsterB == null)
            return NotFound("Monster not found.");

        Round round = BattleService.GetFirstAttacker(monsterA, monsterB);

        while (monsterA.Hp > 0 && monsterB.Hp > 0)
        {
            Monster attacker = round.Attacker;
            Monster attacked = round.Attacked;
            int damage = BattleService.GetDamage(round.Attacker, round.Attacked);
            round.Attacked.Hp -= damage;
            round.Attacker = attacked;
            round.Attacked = attacker;
        }

        Battle battleDb = new Battle()
        {
            MonsterA = monsterA.Id,
            MonsterB = monsterB.Id,
            Winner = monsterA.Hp > 0 ? monsterA.Id : monsterB.Id
        };

        await _repository.Battles.AddAsync(battleDb);
        await _repository.Save();

        return Ok(battleDb);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        if (await _repository.Battles.FindAsync(id) == null)
            return NotFound();
        await _repository.Battles.RemoveAsync(id);
        await _repository.Save();
        return Ok();
    }
}



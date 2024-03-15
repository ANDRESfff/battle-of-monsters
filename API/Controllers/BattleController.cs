using Business.Services;
using Lib.Repository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class BattleController : BaseApiController
{
    private readonly IBattleService _battleService;

    public BattleController( IBattleService battleService)
    {
        _battleService = battleService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        IEnumerable<Battle> battles = await _battleService.GetBattles();
        return Ok(battles);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Add([FromBody] Battle battle)
    {
        if (battle.MonsterA == null || battle.MonsterB == null)
            return BadRequest("Missing ID");

        Battle battleDb = await _battleService.CreateBattle(battle.MonsterA, battle.MonsterB);

        if (battleDb == null)
            return NotFound("Monster not found.");

        return Ok(battleDb);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {

        if (await _battleService.GetBattleById(id) == null)
            return NotFound();

        await _battleService.RemoveBattle(id);
        return Ok();
    }
}



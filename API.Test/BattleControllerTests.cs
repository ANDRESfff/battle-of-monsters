using API.Controllers;
using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test;

public class BattleControllerTests
{
    private readonly Mock<IBattleOfMonstersRepository> _repository;

    public BattleControllerTests()
    {
        this._repository = new Mock<IBattleOfMonstersRepository>();
    }

    [Fact]
    public async void Get_OnSuccess_ReturnsListOfBattles()
    {
        this._repository
            .Setup(x => x.Battles.GetAllAsync())
            .ReturnsAsync(BattlesFixture.GetBattlesMock());

        BattleController sut = new BattleController(_repository.Object);
        ActionResult result = await sut.GetAll();
        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults?.Value.Should().BeOfType<Battle[]>();
    }

    [Fact]
    public async Task Post_BadRequest_When_StartBattle_With_nullMonster()
    {
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = null,
            MonsterB = monstersMock[1].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = null;
        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(() => null);

        int? idMonsterB = monstersMock[1].Id;
        Monster monsterB = monstersMock[1];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        BadRequestObjectResult objectResults = (BadRequestObjectResult)result;
        result.Should().BeOfType<BadRequestObjectResult>();
        Assert.Equal("Missing ID", objectResults.Value);
    }

    [Fact]
    public async Task Post_OnNoMonsterFound_When_StartBattle_With_NonexistentMonster()
    {
        // @TODO missing implementation
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[0].Id,
            MonsterB = monstersMock[1].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[0].Id;
        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(() => null);

        int? idMonsterB = monstersMock[1].Id;
        Monster monsterB = monstersMock[1];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        NotFoundObjectResult objectResults = (NotFoundObjectResult)result;
        result.Should().BeOfType<NotFoundObjectResult>();
        Assert.Equal("Monster not found.", objectResults.Value);

    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning()
    {
        // @TODO missing implementation
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[1].Id,
            MonsterB = monstersMock[0].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[1].Id;
        Monster monsterA = monstersMock[1];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[0].Id;
        Monster monsterB = monstersMock[0];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        OkObjectResult objectResults = (OkObjectResult)result;
        result.Should().BeOfType<OkObjectResult>();
        Battle battle = (Battle)objectResults.Value;

        Assert.Equal(battle.Winner, idMonsterA);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_And_Attack_Lower_Than_Defense()
    {
        // @TODO missing implementation
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[1].Id,
            MonsterB = monstersMock[0].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[1].Id;
        Monster monsterA = monstersMock[1];
        monsterA.Hp = 500;
        monsterA.Attack = 50;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[0].Id;
        Monster monsterB = monstersMock[0];
        monsterB.Attack = 51;
        monsterB.Hp = 5;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        OkObjectResult objectResults = (OkObjectResult)result;
        result.Should().BeOfType<OkObjectResult>();
        Battle battle = (Battle)objectResults.Value;

        Assert.Equal(battle.Winner, idMonsterA);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning()
    {
        // @TODO missing implementation
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[0].Id,
            MonsterB = monstersMock[1].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[0].Id;
        Monster monsterA = monstersMock[0];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[1].Id;
        Monster monsterB = monstersMock[1];

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        OkObjectResult objectResults = (OkObjectResult)result;
        result.Should().BeOfType<OkObjectResult>();
        Battle battle = (Battle)objectResults.Value;

        Assert.Equal(battle.Winner, idMonsterB);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_When_TheirSpeedsSame_And_MonsterA_Has_Higher_Attack()
    {
        // @TODO missing implementation
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[1].Id,
            MonsterB = monstersMock[0].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[1].Id;
        Monster monsterA = monstersMock[1];
        monsterA.Speed = 50;
        monsterA.Attack = 100;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[0].Id;
        Monster monsterB = monstersMock[0];
        monsterB.Speed = 50;
        monsterB.Attack = 50;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        OkObjectResult objectResults = (OkObjectResult)result;
        result.Should().BeOfType<OkObjectResult>();
        Battle battle = (Battle)objectResults.Value;

        Assert.Equal(battle.Winner, idMonsterA);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning_When_TheirSpeedsSame_And_MonsterB_Has_Higher_Attack()
    {
        // @TODO missing implementation
        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[0].Id,
            MonsterB = monstersMock[1].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[0].Id;
        Monster monsterA = monstersMock[0];
        monsterA.Speed = 50;
        monsterA.Attack = 50;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[1].Id;
        Monster monsterB = monstersMock[1];
        monsterB.Speed = 50;
        monsterB.Attack = 100;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        OkObjectResult objectResults = (OkObjectResult)result;
        result.Should().BeOfType<OkObjectResult>();
        Battle battle = (Battle)objectResults.Value;

        Assert.Equal(battle.Winner, idMonsterB);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning_When_TheirDefensesSame_And_MonsterA_Has_Higher_Speed()
    {
        // @TODO missing implementation

        Monster[] monstersMock = MonsterFixture.GetMonstersMock().ToArray();

        Battle b = new Battle()
        {
            MonsterA = monstersMock[1].Id,
            MonsterB = monstersMock[0].Id
        };

        this._repository.Setup(x => x.Battles.AddAsync(b));

        int? idMonsterA = monstersMock[1].Id;
        Monster monsterA = monstersMock[1];
        monsterA.Defense = 30;
        monsterA.Speed = 100;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterA))
            .ReturnsAsync(monsterA);

        int? idMonsterB = monstersMock[0].Id;
        Monster monsterB = monstersMock[0];
        monsterA.Defense = 30;
        monsterB.Speed = 50;

        this._repository
            .Setup(x => x.Monsters.FindAsync(idMonsterB))
            .ReturnsAsync(monsterB);

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Add(b);
        OkObjectResult objectResults = (OkObjectResult)result;
        result.Should().BeOfType<OkObjectResult>();
        Battle battle = (Battle)objectResults.Value;

        Assert.Equal(battle.Winner, idMonsterA);
    }

    [Fact]
    public async Task Delete_OnSuccess_RemoveBattle()
    {
        // @TODO missing implementation
        const int id = 1;
        Battle[] battles = BattlesFixture.GetBattlesMock().ToArray();

        this._repository
            .Setup(x => x.Battles.FindAsync(id))
            .ReturnsAsync(battles[0]);

        this._repository
           .Setup(x => x.Battles.RemoveAsync(id));

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Remove(id);
        OkResult objectResults = (OkResult)result;
        objectResults.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Delete_OnNoBattleFound_Returns404()
    {
        // @TODO missing implementation
        const int id = 1;
        Battle[] battles = BattlesFixture.GetBattlesMock().ToArray();

        this._repository
            .Setup(x => x.Battles.FindAsync(id))
            .ReturnsAsync(() => null);

        this._repository
           .Setup(x => x.Battles.RemoveAsync(id));

        BattleController sut = new BattleController(_repository.Object);

        ActionResult result = await sut.Remove(id);
        NotFoundResult objectResults = (NotFoundResult)result;
        objectResults.StatusCode.Should().Be(404);
    }
}

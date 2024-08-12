# REFACTOR

Review current code and plan to refactor for Battle simulator.

### Plan

High level plan to be updated continuously.

- [x] Review readme and code and criteria => any unimplemented rules? any misunderstood rules?
- [x] Review forks
- [x] Make plan, while keep track of fps per changes
- [x] Low hanging first, 1d to make game playable first
- [ ] Structural changes with edition, extension in mind
- [ ] Unit testing with new structure changes
- [ ] Edition/Tooling
- [ ] Some new extension

### Current state

- [ ] not ECS inheritance instead of composition
- [x] only arrow can hit target, what about other weapons?
- [x] attackRange encoded in Archer => new types of units?
- [ ] prefabs contain data type instead of prefabs as view to be attached to entities
- [x] base unit has a bunch of properties => what about new types of units, new behaviors/properties?
- [x] speed encoded in Archer row => new types of weapon?
- [ ] no separation of renderer logic and simulation logic
- [ ] project structure is by feature which is good (when we add hundreds) but would be difficult for artists (non-code) => assets related should be separated from code, while keeping by feature organization
- [x] no namespace
- [x] use of linq, GetComponentInChildren, MonoBehavior for units in hotpaths
- [x] not sealed
- [x] not good encapsulation, i.e. unnecessary visibility of public fields (use private SerializedField instead)
- [ ] not making use of record
- [x] access to Camera.main, animator not cached in hotpath
- [ ] Update loop on units versus one main loop
- [x] GetUnits on hotpaths with lot of allocation => at least reuse vector
- [x] hot path if ( army1.GetUnits().Count == 0 || army2.GetUnits().Count == 0 ) check on destroy instead
- [ ] algos to be reviewed => update for all units contains many loops through elements that kill performance
- [ ] hard coded switch with singleton BattleInstantiator.instance.army1Color;
- [ ] public enum ArmyStrategy could be extended using int
- [ ] no addressable/resource management => Object.Instantiate serialized prefab link
- [ ] no test unit
- [ ] no fps view on device (use ‘stats’ in editor)
- [x] public, protected and private naming convention: potentially non public with prefix _ since we still use MonoBehavior here
- [x] seems to not have autoformat in place, to body expression, explit modifiers
- [x] seems to use both var and explicite type => if no rules then let's go with var (and Rider suggestions)

### Forks

https://github.com/imranzaidiedu:

- avoid multiple classes in one file when possible
- avoid partial class when possible
- avoid passing null to methods if possible
- some meaningless commit titles and without description
- is weapon prefab and prefab database is an attempt at resource management?
- a big Properties class to hold properties usually not a good idea given ever changing nature of game
- big changes are hard to review so use with caution
- while abtracting strategy is needed, implementations should have correct name (namespace) and abstraction should expose as little as possible

### Design

So the main issue here is the algo that loop through all units via the main update, then each update will loop again through all the units for some computations (some time twice) to implement certain tasks.

Given no third party besides art or unit test allowed, we need to come up with some quick and dirty solutions that allow one main loop that compute everything needed and then cache the results for different tasks later on. This provides the performance boost, but might make the code a bit harder to read.

Then, the data design must allow adding more types of unit, weaponary, armies, without modifying existing data structure, meaning using composition/aggregation instead of inheritance.

The strategies should also be data-driven, meaning we should be able to add new ones without modifying existing code.

To support visual update with ease, the view should also be seraparated from the data, meaning the prefab should only contain the visual information (no more adding data component such as Archer, Warrior, ArcherRow into the prefab), thus some factories shall be implemented, the asset database shall be addressable. Some data database shall be also addressable so we can add more content without rebuilding the game.

### Current values

Since we move components around, we keep note of current values for later update...

UnitBase:
speed = 0.1f;

Archer:
attackRange = 20f;
health = 5;
defense = 0;
attack = 10;
maxAttackCooldown = 5f;
postAttackDelay = 1f;

Warriors:
attackRange = 2.5f;
health = 50;
defense = 5;
attack = 20;
maxAttackCooldown = 1f;
postAttackDelay = 0;

ArcherArrow:
speed = 2.5f;

### Performance tracking

FPS on M3 Pro in editor:

- 5.3 in the beginning
- 12.5 after reduce allocations and add caching on hotpaths
- 14.5 after avoid re-computing armies' center points per unit
- 18.9 after more caching
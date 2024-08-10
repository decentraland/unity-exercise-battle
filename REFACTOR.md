# REFACTOR

Review current code and plan to refactor for Battle simulator.

### Plan

High level plan to be updated continuously.

- [ ] Review readme and code and criteria => any unimplemented rules? any misunderstood rules?
- [ ] Review forks
- [ ] Make plan, while keep track of fps per changes
- [ ] Low hanging first, 1d to make game playable first
- [ ] Structural changes with edition, extension in mind
- [ ] Unit testing with new structure changes
- [ ] Edition/Tooling
- [ ] Some new extension

### Current state

- [ ] not ECS inheritance instead of composition
- [ ] only arrow can hit target, what about other weapons?
- [ ] attackRange encoded in Archer => new types of units?
- [ ] prefabs contain data type instead of prefabs as view to be attached to entities
- [ ] base unit has a bunch of properties => what about new types of units, new behaviors/properties?
- [ ] speed encoded in Archer row => new types of weapon?
- [ ] no separation of renderer logic and simulation logic
- [ ] project structure is by feature which is good (when we add hundreds) but would be difficult for artists (non-code) => assets related should be separated from code, while keeping by feature organization
- [ ] no namespace
- [ ] use of linq, GetComponentInChildren, MonoBehavior for units
- [x] not sealed
- [ ] not making use of record
- [ ] access to Camera.main, animator not cached in hotpath
- [ ] Update loop on units versus one main loop
- [ ] GetUnits on hotpaths with lot of allocation => at least reuse vector
- [ ] hot path if ( army1.GetUnits().Count == 0 || army2.GetUnits().Count == 0 ) check on destroy instead
- [ ] algos to be reviewed
- [ ] hard coded switch with singleton BattleInstantiator.instance.army1Color;
- [ ] public enum ArmyStrategy could be extended using int
- [ ] no addressable/resource management => Object.Instantiate serialized prefab link
- [ ] no test unit
- [ ] no fps view on device (use ‘stats’ in editor)
- [ ] seems to not have autoformat in place, body expression
- [ ] seems to use both var and explicite type => if no rules then let's go with var (and Rider suggestions)

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

### Performance tracking

FPS on M3 Pro:
- 5.3 in the beginning
- 12.5 after reduce allocations and add caching on hotpaths
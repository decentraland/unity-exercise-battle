# Battle Simulator

![](Battle.gif)

In this simulator, armies of varying units and sizes will battle using a set of very simple AI strategies.

## Launch Menu

This is the simulator entry point, in which the following options can be set per army:

- Warriors amount
- Archers amount
- Strategy type

Then, there's the "launch" button that will put the user on the battle phase.

## Battle

In this phase, the two armies will fight for victory. The battle will start according to the options selected at the
launch menu.

### Basic Rules

- Units can't overlap other units
- To avoid too much dispersion, units must be at least 80 meters close to the center point of all units
- All units have attack, speed, and defense values. When any unit attacks, the hit formula
  is `defender.health -= attacker.attack - defender.defense` .
- Any unit must not move distances that surpass its own speed on a single frame.
- All units have attack cooldown values. Any unit can't attack if the cooldown value is above zero.
- If health falls under zero, a given unit dies.
- When all the units for a given army dies, a ending menu is instantiated. This menu will have a button that can get the
  user to the launch menu to start a new simulation.

Armies are composed by the following unit types:

### Warrior

| Attribute | Value |
|---|---|
| Attack range | 2.5 meters |
| Attack cooldown | 1 second |
| Attack amount | 20 pts |
| Health | 50 pts |
| Defense | 5 pts |

### Archer

| Attribute | Value |
|---|---|
| Attack range | 20 meters |
| Attack cooldown | 5 seconds |
| Post-attack delay | 1 second |
| Attack amount | 10 pts |
| Health | 5 pts |
| Defense | 0 pts |

**Special rules:** When the archer attacks, it spawns an arrow that flies in a straight direction and impacts the target
from a distance. After the archer attacks, it has a `postAttackDelay` attribute that prevents further movement for its
given time.

### Strategies

There are only two strategies that are outlined below

**Basic**

Any Unit will just pick the nearest enemy, and advance to it. It will attack as soon the enemy falls within
its `attackRange` attribute.

**Defensive**

Any Unit will just pick the nearest enemy, and advance to it. Warriors will back off when their `attackCooldown > 0`.
Archers will try to get as far as possible but within their `attackRange`. If any enemy unit gets too near the Archer,
the Archer will try to move away but at the same time circle its foe.
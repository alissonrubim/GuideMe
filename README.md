# LazySnake
LazySnake it's a game, that you need to construct the best AI solution to solve the path.

Using my own engine, for 2D games, you just need move the "player" (that's an animal, like a snake) for the path using command lines.
The objective is get the target before your energy go out.

You have a energy bar, that starts with 30 energy.
Each time you move, the player user 1 energy bar. Turn to the sides, don't cost any energy. But walk, cost 1 energy bar.
You can eat food during the path, each food give you more 10 energy.
You can user a radar, for one direction, that give you the distance (in blocks) to the wall (or target).
You object is get at the target with the best code.

## Commands list

Comand | Cost | Description
------------ | ------------- | -------------
.TurnUpLeft() | 0 |
.TurnUp() | 0 |
.TurnUpRight() | 0 |
.TurnLeft() | 0  |
.TurnRight() | 0 |
.TurnBottomLeft() | 0 | 
.TurnBottom() | 0 | Turn the player for the bottom
.TurnBottomRight() | 0 | Turn the player for the right bottom corner
.Walk() | 1 | Walk one block in the direction that the player it's pointed
.FireSensor() | 4 | Fire the sensor for the directio that the player it's pointed. Return an integer that's is the number os free blocks in his front

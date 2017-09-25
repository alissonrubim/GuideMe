# The Lazy Boy
The Lazy Boy is a game, created for help TI students to improve their logic and makes better solutions for problems.
The idea is construct the best script for a device that the Marcus, a boy who does not see very well, uses to guide him in the paths.

The device can just see one meter (or block) in his front and detect what is on your front. Your objective is create a best script to go get the Marcus's house key using logic and resolve the path problems.

## The game rules
First of all, you need to understand that the device uses battery to work. So every time that's you walk with Marcus on the path, the battery goes down. You start with some battery energy and you need get the key before the energy ends, because without batery, Marcus can't see anymore.

In the way, you can get some extra battery to charge your device. It's important consider that on your script, because you need these batteries to continue walking.

Before walk, you can turn Marcus around, making him look in the eight possible positions, like the image below:

![Animation of player moviment](https://github.com/alissonrubim/LazySnake/blob/master/Screenshots/player-animation.gif)

Turning the player not cost any energy, but after turn the player, you need walk. Marcus just walk in the direction that he is looking, and you need to remember: walk cost one energy of the device, so you need have sure that you want walk in this direction.

So now you need to find the better way to get the key, before your energy ends. Good luck!

## The commands list
In the script, you need to create a method called **Loop**. 

 ```csharp
  public void LBAction Loop(LBPlayer player, LBTarget target){
     return LBAction.TurnLeft; //will turn Marcus, to the left
  }
 ```

This method receive some kind of informations. Like the Player object, and the target (key) position.
You can seet more in this examples:

###### The LBTarget object
 ```csharp
  public void LBAction Loop(LBPlayer player, LBTarget target){
     int posX = target.Position.X //The position of the target in the map
     int posY = target.Position.Y //The position of the target in the map
  }
 ```
 
Comand | Description
------------ | ------------- 
LBTarget.Position.X | Return's the number os block in the map, that's the position of the target in the row
LBTarget.Position.Y | Return's the number os block in the map, that's the position of the target in the col

###### The LBPlayer object
 ```csharp
  public void LBAction Loop(LBPlayer player, LBTarget target){
     int posX = player.Position.X //The position of the player in the map
     int posY = player.Position.Y //The position of the player in the map
     
     if(player.WhatsAhead() == LBObject.Target){
        return LBAction.Walk;
     }else{
        if(player.Direction == LBDirections.Left){
          return LBAction.Walk;
        }else{
          return LBAction.TurnLeft;
        }
     }
  }
 ```
###### The LBAction result object

The Loop method needs return an object, the **LBAction** thats will be used to command the player.

Comand | Cost | Description
------------ | ------------- | -------------
LBAction.TurnUpLeft | 0 |
LBAction.TurnUp | 0 |
LBAction.TurnUpRight | 0 |
LBAction.TurnLeft | 0  |
LBAction.TurnRight | 0 |
LBAction.TurnDownLeft | 0 | 
LBAction.TurnDown | 0 | Turn the player for the bottom
LBAction.TurnDownRight | 0 | Turn the player for the right bottom corner
LBAction.Walk | 1 | Walk one block in the direction that the player it's pointed
*LBAction.FireSensor | 4 | Fire the sensor for the directio that the player it's pointed. Return an integer that's is the number os free blocks in his front*


# More details about the code and the engine.
The code uses my own 2D game engine. It's just a engine, with some objects, some controllers and colision systems, thats work greatfull in this project. 

I'm costructing the engine in the same time i work with this project. So, i want, someday, have time to make the engine in a dll to make more easy to work with.
The experience working with sprites, animations and alot of recources is amazing. I always try to make the better code, the better system, for get the most easy engine possible.

### What's is missing in this project
I love this project, but i don't have the full time to work with it. I work in some nigths, studing the project in the most time, on the weekends.
There is a list of missing things that i hope to fix and make:

So i have a lot of ideas and things to do. If you like the project, and want make some difference in the IT schools, i will be glade to work with you. If you want, you can contact me by my Twitter or Email.







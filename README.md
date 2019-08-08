# Guide Me
Guide Me is a game, created for help IT students to improve their logic and find better solutions for problems.
The idea is construct the best script for a 'device' that the Marcus, a boy who is blind, it's using to guide him in the paths.

The device can just see one meter (or block) at front and detect what type of block is that (a wall, a empty space, a battery or a key). Your objective is create the best script to help Marcus's find his house key using logic and resolve the paths problems.

## The game rules
First of all, you need to understand that the device uses battery to work. So every time that's you walk (or use a device command), the battery goes down. You start with some battery energy and you need get the key before the energy ends, because without batery, Marcus can't guide himself anymore.

In the way, you can get some extra battery to charge your device. It's important consider that on your script, because you need these batteries to continue searching for the key.

Before walk, you can turn Marcus around, making him look in the eight possible positions, like the image below:

![Animation of player moviment](https://github.com/alissonrubim/LazySnake/blob/master/Screenshots/player-animation.gif)

Turning the player do not cost any energy from the device, but after turn the player, you need walk. Marcus just walk ahead in the direction that he is looking, and you need to remember: walk cost energy from the device, so you need be sure that you want walk in this direction.

Now you need to find the better way to find the key, before your energy ends. Good luck!

## Scripting ##



## The commands list
In the script, you need to create a method called **Loop**. 

 ```csharp
  public void LBAction Loop(LBPlayer player, LBTarget target){
     return LBAction.TurnLeft; //will turn Marcus, to the left
  }
 ```

This method receive two important informations:
   the LBPlayer object - This contains all the player functions.
   the LBTarget object - This contains the target position.

You can seet more in this examples:

!Important!
  This method runs at the ends of the each cycle, that it's composed by drawing functions and calculations methods. So if you stuck this function with a endless loop, for example, the other functions will never be called and the game will not refresh himself. Be aware of that! ;)

###### The LBTarget object
  This object is simple, it contains only the target position.
  Why? Because it's the only thing that you need to find it.

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
 This object contains only functions related with the device/player.
 Each function can have a cost of energy, so pay attention in that when you write your scrypt.

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
LBAction.TurnUpLeft | 0 | Turn the player to top left conner
LBAction.TurnUp | 0 | Turn the player to top
LBAction.TurnUpRight | 0 | Turn the player to top right conner
LBAction.TurnLeft | 0  | Turn the player to left
LBAction.TurnRight | 0 | Turn the player to right
LBAction.TurnDownLeft | 0 | Turn the player to left bottom coner
LBAction.TurnDown | 0 | Turn the player for the bottom
LBAction.TurnDownRight | 0 | Turn the player for the right bottom corner
LBAction.Walk | 1 | Walk one block in the direction that the player it's pointed
*LBAction.FireSensor | 4 | Fire the sensor for the directio that the player it's pointed. Return an integer that's is the number os free blocks in his front*


# More details about the code and the engine.
The code uses my own 2D game engine. It's just a engine, with some objects, some controllers and colision systems, thats work greatfull in this project. 

I'm costructing the engine in the same time I'm working in this project. So, someday, I intend to to make the engine in a .dll to make more easy to apply in another projects.
The experience working with sprites, animations and a lot of recources it's amazing. I'm always tring to make a better code, a better engine and create the most easy implementation as possible.
I'm not perfect and I'm sure that my code has some mistakes or stupid smell code. If you want to contribute, you're welcome to send me a e-mai or even create a Pull Request.

### What's is missing in this project
I love this project, but i don't have the full time to work with it. I'm working in it at some nigths and free weekends.
There is a list of missing things that I need to do:
    For a good release:
          [ ] Implement the energy bar at the GUI
          [ ] Implement the energy calculation
          [ ] Implement code runtime building to understand the code from the user.
          [ ] Play/Stop commands to stop running the code.
    Would be nice if have:
          [ ] Ant-locking system against endless loop.
          [ ] Two games running to do a competition between codes.
          [ ] A clock to register the time to find the key.          


I have a lot of ideas and things to do in this project. If you like it and want make some difference in the IT schools, I will be glade to work with you. You can reach me by my Twitter (@alissonrubim) or Email (alissonrubim@gmail.com).







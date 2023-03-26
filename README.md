# Asteroid - Unity DOTS

### How to play the game!
Playing Asteroid is easy: simply move your spaceship with the `WASD` keys and aim using your mouse. Click to fire a bullet. There are also three powerups in the game, each represented by a shiny sphere:

-   Yellow: Grants permanent invincibility until you collect another powerup.
-   Blue: Increases the number of bullets fired by your spaceship.
-   Red: Increases the speed of your spaceship's bullets.
### Short note about the Implementation Journey
Since I didn't have experience working on a production level DOTS game, I initially created a rough version of the Asteroid and planned to refine and polish the code later. This was done to avoid surprises along the way, but I wouldn't take this approach in a production project. After this 2 weeks, I feel like a defined structure from the beginning would have been better and I was afraid for no reason.

You may also notice that the commit sizes are large, and I pushed significant changes per commit (at least some of them). This is not something I would typically do in a production-level codebase, as I prefer to commit smaller chunks of changes with more meaningful commit messages.

Anyways, let's take a look at the project structure.


# Folder structure

I love the **Modular** folder structure in Unity and I think it really fits the ECS mindset as well. That's why I decided to go in that direction. A shot summary of how the Modular structure looks like is as follows:

- Assets/
-   Common/ (contains the project's main assets, configs, main scenes, etc. primarily for designers)
-   Modules/ (the primary modules directory)
    -   ModuleX/
        -   Runtime/ (contains main code, prefabs, etc. that belong to the module)
        -   Test/ (used to test the module in an isolated way)
    -   Common/ (contains shared code and assets between different modules)

# Modules
I really wanted to develop the game by using some loosely coupled modules. With this architecture, you have the most flexibility in your game to experiment different mechanics and features without having to develop or modify the code and do it just by changing numbers and configurations. However, I had some limitations that comes with the data oriented programming... . First, let's take a look at two of them:

## Limitations
### Couldn't use interfaces
In Data-Oriented programming you should give up unccertainty. Therefore I couldn't use interfaces for different implementation of my components. That's fair because the Burst compiler needs to know how much space each individual component takes. I even reached a solution to resolve this issue by creating an AssemblyInfo.cs and adding this line:

`[assembly: RegisterGenericComponentType(typeof(ConcreteImplementation))]`

But didn't work!
### Couldn't solve scriptable object issues
This one I'm pretty sure if I had more time, I could solve it. But the fact that I couldn't use scriptable objects for game configurations easily, was very annoying. I tried to have a `ConfigContainer` monobehaviour that keeps the scriptable objects and converts them to entities using `IDeclareReferencedPrefabs` and `IConvertGameObjectToEntity` but didn't have any luck!

Anyway, lets take a look at the most important modules:



## Movement Module

I tried my best to keep every module responsible for only one or maybe two tasks and stick to the `S` in the `SOLID`. I think I managed to do it for the Movement module. By adding `MovementComponent` to any Entity, it will start moving using the information provided to its constructor.
#### What would I change about this?
 I'm still not happy about the fact that you can not attach this component to any Entity in inspector to use it and you need to do it in code! The reason is that it needs a `MovementConfig` and you can only pass it through the constructor. (The playerspaceship is an exception) So I would definetly change the structure of this module to support that as well.
## Shooter Module

This is also a module that I tried to keep as simple as possible. The usage is straightforward. This time you can attach it to an Entity in Inspector along with a `ShooterConfig` and it will actually work! The component will shoot a bullet at a direction based on the configurations. However, you should set the direction somewhere in the code to give it a meaningful purpose!
#### What would I change about this?
 This is where I really missed an interface. I originally intended to have two implementation of `IShooterComponent`, first one `AutoShooterComponent` and second one `ManualShooterComponent` where you shoot by clicking or pressing a key. And at the end I solved it by introducing a `FireMode` enum which indicates those implementations and two systems that handle the different shooting logics. I'm still not happy with this but I didn't find any better way during the limited time I had.
## Collision Module

This is the most interesting and the most challening module in the game. Unfortunately, I reached here very late and couldn't put much thought in it. But let's start with the positive stuff:
#### Suff that I like:
* I really like how convenient it is to add this component to any Entity to make it collidable! 
* The components ending with a `Tag` keyword worked very well as collision identifiers
#### Suff that I don't like like:
* The collision detection and the collision resolvers are highly coupled with other components. I really wanted to have some kind of dynamic Collision Matrix system that I could define the collision behaviours between different tags outside of the Collision Module. It also would be awesome to take out all those collision resolver logic out of the `CollisionResolveSystem` and keep it small and separate from concrete behaviours.
* The collision detection runs two `For` loops which results in a `O(n^2)` complexity and is very slow when you want to have a big number of asteroids or colliders. I tried to optimize it by using a different data structure to store the nearest colliders instead of all of them. I used a `QuadTree` implementation, changed it a bit to use Unity's JobSystem and could resolve all the issues and errors. However, after running the game it was still super slow and somewho it couldn't detect all collisions! So I gave up on that and pushed the non-working version to the `quad-tree-attempt` branch. I'm interested to know what the problem was or what did I do wrong!
## Conclusion
The game is still not polished! I would've definitely taken a look at the glitch and flickering when I instantiate a new entity (It showed up after I refactored and separated different logics) Maybe I could fix it by adding some `[UpdateAfter]` here and there but it was too late for that. I would've also enhanced the logic of asteroid creation. They just get instantiated at the beginning of the game and die or get out of the screen eventually. Therefore the game session is super short!

All in all, My goal was to write a code that is easy to read and understandable. I also included plenty of documentation every where so that everything is clear. I'm really interested to hear your feedback and see how much I succeeded! 

Thanks a lot!

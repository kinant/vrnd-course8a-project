# README #

# Udacity VR Nanodegree Project 8a - Rube Goldber Project #

What I liked: Creating a game for HTC Vive. It proved to include a lot of challenges (such as the portal/teleporter), but I
had a ton of fun creating this. 

What I did not like: I feel like the course videos will not prepare most people for the challenges of this project. It was a lot
of work to get everything to work. 

Instructions: 
- This project was created with Unity Version 5.5.3f1. It ONLY works with HTC VIVE.
- Open Unity Project and play!
- There is a Windows .exe build provided in the Build folder. 

Controls:
- Left Controller: 
1. Press on the TOUCHPAD to teleport to a location. You can only telepor on the ground. 
2. Press and release the TRIGGER to grab/release objects. 

Right Controller:
1. Press the TOUCHPAD UP or DOWN axis to elevate the player UP or DOWN. 
2. Touch the TOUCHPAD on the LEFT or RIGHT axis to show the object menu. 
3. Scroll through the object menu by pressing the touchpad LEFT or RIGHT.
4. Spawn an object by pressing the TRIGGER while the menu is open. 
5. You can also use the right controller to grab/release objects by using the TRIGGER.

There is a tutorial scene that allows the player to get used to moving around and spawning and moving objects.
- NOTE: Controls are limited initially during the tutorial so the player can only focus on the task at hand. As
the player progresses, the other controls will be available. 

- NOTE: For the first part of the tutorial on movement, make sure to be inside the rectangular mesh to activate the
next step. Don't forget to look up and use the right touchpad UP to elevate the player to the top trigger (red rectangle).

- NOTE: You can exit the tutorial after learning how to move, and at any other time after this, by teleporting into the 
red rectangular trigger at the far back in the right of the scene. There is a sign to indicate that it is an exit. 

How to play:

After completing or skipping the tutorial, you will be sent to the first of six puzzles. The goal is to use the provided objects
in the menu to get the ball to the goal, while collecting all the stars in the scene. 

NOTE: SOME PUZZLES ARE INCREDIBLY HARD, BUT THEY ARE ALL SOLVABLE.

NOTE: If you have the project open in Unity, you can enable the solutions for the puzzles by the following:
		1. You can enable the solution gameobject in the hierarchy. It is inside the Puzzle# gameobject.
		2. You can select the LevelManager in the hierarchy, and check the box that says show solution. Then play the level. 

NOTE: A video of all the puzzles being solved is available at: https://www.youtube.com/watch?v=r4rAT3d3TDM

OTHER IMPORTANT INFORMATION:
- Please note that I decided to only allow the player to teleport to locations on the ground. The course videos showed how to teleport anywhere,
and as a result, you could teleport to places in the air. For this project, after play testing with two people, this proved to be very difficult. 
The puzzles require a certain accuracy that is not available with this method. That is why I decided on teleporting on the ground, but allowing the 
user to elevate themselves up or down using the up/down touchpad on the right controller. 

- As a result of the above point, the Rube Goldberg Object Menu ONLY shows up when the right touchpad is touched on the left or right sides. It will not
activate by touching all areas of the touchpad. 

- KNOWN ISSUE: The console shows an error for index out of bounds when the controllers are not active or not being tracked. Once they are, the errors stop.
I have no idea how to solve this issue. It was not covered in the course videos and I have tried many things, but I have not been able to fix this yet. 

I HOPE YOU ENJOY THE GAME!!!
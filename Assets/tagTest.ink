#bg room
#char set char1 0.8
#char set char2 0.2
Test: This is a demo of all the tag functions
Are you ready?
    *[yes]
    THEN LETS GO!!!
    *[no]
    ...I'll ignore that!
-First lets go thrugh the "char" functions! char should be the first tag whenever something about a character is changed
#char show char1
Shows character 1 at defult position
#char hide char1
Hides character 1, when hiding expession is always set to neutal
#char show char1 happy
Shows character 1 happy
#char show char2 sad
Shows character 2 sad
#char hide char2
#char set char1 0.2
Sets character 1's position to left
#char move char1 0.8
Character 1 moves from current position to right
#char move char1 0.5
Character 1 moves from current position to middle. any value from 0.2-0.8 will be positioned within the screen, you can go lower or higher but the character might apear ofscreen.
#char flip char1 right
We can also make the character look to the left or right!
//#char anim char1 jump
//Character 1 dose jumps animation

Then we have the background tag!
Right now the background is the defult one
#bg church
But we can change it!
And um thats it... you just change the background with the bg tag

Now onto the audio tags sfx and music!
*[okay!]
-#sfx boom 
plays boom sound effect until its finnished playing
#sfx boom 0.5
plays boom sound effect at 50% loudness
#music short_music once
Plays music withinout looping
#music short_music loop
Plays music with looping

#music stop
And you can also stop the music
Alright thats all the audio functions!
Now lets move onto the visual effects (not done)
#vfx screen_shake
Plays screen shake
#vfx screen_shake 2
Plays screen shake at 2 times speed 
#vfx fade_to_black
Plays fade to black
There are plenty more vfx one could add but this is it for now!

Lets move onto the last little thing, drumbroll please.
*[drumbroll]
-#txtspeed slow
..........
#txtspeed fast
TEXT SPEEEEEEEEEEEEEEEEEEEEEEEEEEED!!!
#txtspeed normal
for now theres slow fast and normal but i might make it a scale from 1 to 100
And thats the end!
#txtspeed fast
Thank you for paying attention 


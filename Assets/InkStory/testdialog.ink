//relationships
VAR relation_gab_more = 0
VAR relation_more_svinya = 0
VAR relation_gab_svinya = 0
//items
VAR gloves_on = false

//possestion
VAR suspicion = 0

->StartScene
===== StartScene ===
The cold bench beneath you creaks
You: My head...
You look around the echoing builing
You: ... A church?
Unknown voice: morė! 
You: huh?
Unknown voice: I'm so glad you woke up!
You: Gab?
Gab: Yeah, it's me!
Gab: Are you cold? Here take my gloves.
*It's okay[]. I'm not all that cold.
    Gab: Oh really? Hmm... I'm not sure if that is a good thing. You might have frostbite!
    Gab: Here, let me look at your hands.
    #vfx cutscene hands
    _
    Gab: Well... 
    Gab: I think we should light a fire either way, I'm not sure how long well have to be here...
*I would appriciate that[]...
    Gab: Really? Yeah here take them!
    ~gloves_on = true
    Gab: I hope they help atleast a little bit.
    Gab: But it is still freezing, we need to light a fire and warm you up.
    **Thank you
    Gab: No worries! I am glad you took them.
    **...
*My head hurts[].
    Gab: Oh that's not good at all.
    Gab: The cold probably got to you, we really have to warm you up.
    
-Gab: Um... 
Gab: Right, I was going to light a fire while you where passed out, but I...
Gab: Well I did not find anything to light it with.
*Where are we?
    Gab: I... I'm not quite sure actually.
    Gab: After I found you in the forest I tried carrying you back but I got lost and well... we ended up here.
    **[The forest?] I was in the forest?
    Gab: You were, I found you sitting under a tree in the middle of the forest...
    Gab: You mumbled something to me but i couldn't hear.
        ***...
            Gab: You... You don't remember?
        ***I don't think that happened
            Gab: What do you mean?
            You: Sorry, I don't remember being in the forest.
        ***I don't remember
            Gab: You... You don't remember?
    **[Carrying?] You tried carrying me?
*Why are you here?
    Gab: Right... You don't remember do you? 
*Who are you?
    #char gab dread
    Gab: ...What? 
    You: Sorry, I don't know why I said that...
    #char gab gleam
    Gab: Oh- okay!
    #char gab worried
    Gab: You had me worried there for a bit. 
- Gab: Um.. do you remember being at the festival?
->AskFestivalRemember
VAR asked_about_festival =false
=AskFestivalRemember
*[No] I don't.
    #char gab 
    Gab: Oh, okay, yes that is okay! Do you want me to tell you what happened?
    **No 
        Gab: Really? I understand, you probably want to get warm first!
        Gab: Let us go and try to light a fire, okay?
    **Yes
        #char gab gleam
        Gab: Alright! I'll try to be quick so you dont freeze to death.
        #char gab awkward
        Gab: Hehe...
        #char gab nautral
        Gab: Well um- right! So when we were at the festival... 
        ->BackStoryFestival
*[Yes] I remember everything.
    #char gab gleam
    Gab: Oh! Okay thats good! I was worried about you for a bit there.
    Something stirs within you
    Gab: Well then, we ought to ?
    **Yes[], Let's do it.
    ->LookForLighter
    **No[].
        Gab: ...
        {suspicion>-1: 
            Gab: You've been acting really strange...
            Gab: Did something happen?
        -else:
            Gab: Heh this is not the time for jokes, lets go.
            ->LookForLighter
        }
*...
    {not asked_about_festival:
        asked_about_festival =true
        Gab: Hello?
        Gab: ...Um
        Gab: Do you want to light a fire?
        ->AskFestivalRemember
    -else:
        Gab: ...
    }
-->BackStoryFestival

==== BackStoryFestival ===
Gab: Ofcourse you can! But hurry I think they're going to be fighting soon.
morė: Right. I'll get some for you too, just stay here.
Gab: Oh thank yo-
Gab: Like always, you run away before I can thank you.
I waited for you, the performance came and passed and you still didn't come back.
Gab: I wonder what's taking so long...
It wasn't like you to take so long but you seemed to be enjoying yourself so I didn't think much of it.

Gab: Um excuse me, you haven't happened to see morė around here have you?
Pancake lady: Hmm.. oh that one with the red hair right? 
Pancake lady: Oh right yes! They where standing around here a while ago but I think they ran away without taking any pancakes.
Pancake lady: Speaking of which, you better take some for both you and that short one. You kids need more meat on your bones if you want to survive this cold.
Gab: Hehe I'll get some later if that's okay. Do you happen to remember what direction morė ran off to?
Pancake lady: Ah what was it now, I think somewhere twoards that forest over theres. I think some other kids ran with them actually.
#char gab dread
Pancake lady: But sho now! Youre holding up the line.
Gab: Oh right, sorry.
//near forest
Gab: You're sure morė went inside?
Kanapinis: Look Gabijus, like i said no, I'm not sure it was your friend but yes, i saw someone with red hair run twoards there.
Gab: I'm going to go look! If you see my sister or parents please tell them.
Kanapinis: Uhuh I'll try.
Lasinis: Don't worry Gabijus, go look! We'll tell your parents! 
Gab: Thank you!
//in forest
Gab: I can't see anyone and the festival is to loud there's no way ill hear them.
I'm not sure for how long i looked but before i knew it the sun had set.
Ofcourse I... I wasn't going to give up on looking for you but I knew if i got lost aswell it would only be worse for us both.
But luckily as I was trying to find my way back i heard a really weird sound from somewhere deeper in the forest.
I wasnt sure what it was but at that point i didn't much care adout what it was. I ran twoards it and well I found you...

Gab: morė?
Gab: morė! What are you doing? 
Gab: Oh God you're freezing.
Gab:...
Gab: morė?
Gab: hello? morė please, please say something!
//snow starts
Gab: No please don't... Hold on for a bit okay? I'll get you back.

I... I was so sure I was going twoards the sound of the festival but...  Well the storm only got denser and I must have gotten turned around at some point.
I honestly have no idea how but when we made it out of the forest i couldn't see the village at all.
But I thought I saw something though the storm and walked twoards it. Turns out it was a church and lucky for us the front door was completly unlocked!
So um... that was how we ended up here.
->AfterBackStoryFestival

==== LookForLighter ===
Gab: I looked around near the altar but i have not looked much else where.
Gab: Where do you think it is best to look first?
*Altar
Gab: The altar?
Gab: Well i guess it is never bad to double check things.
Gab: Okay!
*Basement door

-tehe that is all
->END

==== LightFire ===
you light the fire the end
->END

=== AfterBackStoryFestival ===
//end of flashback
Gab: I'm really sorry, if I had only...
Gab: No! This is no time for that, how are you doing?
END
->END


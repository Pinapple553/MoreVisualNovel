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
A gold gust breezed past
The cold bench beneath you creaks
You: My head...
You look around the echoing builing
You: ... A church?
Unknown voice: Morė! 
You: huh?
Unknown voice: I'm so glad you woke up!
You: Gab?
Gab: Are you cold? You could take my gloves.
*My head hurts.
    Gab: The cold probably got to you, we have to warm you up.
*It's okay[.] I'm not that cold.
    Gab: Oh that's not good- have you gotten frostbite?
    Gab: Here, let me see your hands.
    Gab: Well I think we should light a fire either way, I'm not sure how long well have to be here...
*I'd appriciate that...
    Gab: Oh! Yeah here take them!
    ~gloves_on = true
    Gab:But it's still freezing, we need to warm you up.
    ** Thank you
    **...
    
-Gab: Um... 
Gab: I tried to light a fire while you where...
Gab: It could have gone better to say the least hehe.
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
*Why are we here?
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
    Gab: Well then we ought to try to light that fire no?
    **Yes[], Let's do it.
    ->LightFire
    **No[].
        Gab: ...
        {suspicion>-1: 
            Gab: You've been acting really strange...
            Gab: Did something happen?
        -else:
            Gab: Heh this is not the time for jokes, lets go.
            ->LightFire
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
Gab tells you about what he remembers
->END

==== LightFire ===
You light the fire
->END



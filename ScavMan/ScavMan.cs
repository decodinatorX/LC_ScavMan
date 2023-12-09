using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using ModelReplacement;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

// i hate coding lmfao
// the dll isnt updating if i just change the bundle only, so for every bundle change i'll add an exclamation mark after this comment
// !!!

// how in the hell do i go about adding compat???

namespace ScavyModelReplacement
{
    // MAIN REPLACING CLASS
    public class BodyReplacementKit : BodyReplacementBase
    {
        // the bone fuckery
        public override string boneMapFileName => "boneMapScav.json";

        // Asset loading and shit
        public override GameObject LoadAssetsAndReturnModel()
        {
            GameObject obj = Assets.MainAssetBundle.LoadAsset<GameObject>("scavy_the_model");
            Texture2D val = Assets.MainAssetBundle.LoadAsset<Texture2D>("scavy_the_texture"); 
            Assets.MainAssetBundle.LoadAsset<Texture2D>("scavy_the_texture_normal");
            SkinnedMeshRenderer componentInChildren = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            ((Renderer)componentInChildren).materials[0] = new Material(((Renderer)componentInChildren).material.shader);
            ((Renderer)componentInChildren).material.SetTexture("_MainTex", (Texture)(object)val);
            return obj;
        }

        // don't got any need for this script
        public override void AddModelScripts()
        {
        }
    }

    // main plugin stuff as well as dependency calling bs
    [BepInPlugin("decodinator.ScavyModelReplacement", "Scavy Model", "1.0")]
    [BepInDependency("meow.ModelReplacementAPI", BepInDependency.DependencyFlags.HardDependency)]
//    [BepInDependency("x753.More_Suits", BepInDependency.DependencyFlags.HardDependency)] commented for open sourcing for the time being
    public class Plugin : BaseUnityPlugin
    {
        // player patch
        [HarmonyPatch(typeof(PlayerControllerB))]
        public class PlayerControllerBPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void UpdatePatch(ref PlayerControllerB __instance)
            {
                _ = __instance.playerSteamId; // cock jizz
            }
        }

        // "WAKE THE FUCK UP SAMURI, WE HAVE A CITY TO BURN"
        private void Awake()
        {
            ModelReplacement.ModelReplacementAPI.RegisterSuitModelReplacement("Default", typeof(BodyReplacementKit));
            Assets.PopulateAssets();
            new Harmony("decodinator.ScavyModelReplacement").PatchAll();
            Logger.LogInfo("Plugin decodinator.ScavyModelReplacement is loaded!");
        }
    }

    // take a guess what this shit does genius
    // wht it do?
    public static class Assets
    {
        //point to the assetbundle
        //you forgot to put it with the dll again... didn't you?
        // NUH UH!!!
        public static string mainAssetBundleName = "scavman";
        public static AssetBundle MainAssetBundle = null;

        // this i presumes calls the assembly??
        private static string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        }

        // this loads all assets! YEAH!!
        // gj
        public static void PopulateAssets()
        {
            if ((Object)(object)MainAssetBundle == (Object)null)
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetAssemblyName() + "." + mainAssetBundleName))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(stream);
                }
            }
        }
    }
}
// Hello everybody my name is Markiplier and welcome to Five Nights at Freddys, an indie horror game that you guys suggested in mass, and I saw that Yamimash played it and he said that it was really really good; so I’m very eager to see what is up - and that is a terrifying animatronic bear *reads off script* family pizzeria looking for security guard to work the night shift. Oh, 12:00 A.M, the first night. If I didn’t want to stay the first night, why would I stay any more than five? Why would I say anymore than two - hello. Okay...Hello? Hello - oh, ah I can’t move. That’s a creepy skull...There’s creepy things on the wall - Oh, hello. *Phone Guy begins dialogue* “Hello, hello hello,” Hi! “Uh, I wanted to record a message for you, to help you get settled in on your first night.” Eugh.. “Um, I actually worked in that office before you, and I’m finishing up my last week now as a matter of fact. So, I know it can be a bit overwhelming..” Euuagh..! “But I’m here to tell you, there’s nothing to worry about,” Agh.. “You’ll do fine! So, let’s just focus on getting you through your first week..” Okay! Sounds go- “Ah, let’s see..First there’s an introductory greeting from the company that I’m supposed to read - i-it’s kind of a legal thing, you know, ahm - ‘Welcome to Freddy Fazbear’s Pizza-” Okay “‘..A magical place for kids and grownups alike-” *Mark wheezes indistinctly in the background* Heheha.. “..Where fantasy and fun come to life,” Eugha..! “”Freddy Fazbear entertainment is not responsible for damage to property or person, upon discovering that damage or death has occured, a missing person report will be filed within ninety days or as soon as property and premises had been thoroughly cleaned and bleached, and the carpet’s have been replaced,’ blah blah blah - now that might sound bad, I know, but-” Yeah! “-There’s really nothing to worry about! Uh, the animatronic characters here do get a bit quirky at night, but do I blame them? No, if I was forced to sing those same stupid songs for twenty years, and I never got a bath, I’d probably be a bit irritable at night too. So just remember, these characters hold a special place in the hearts of children, and you need show them a little respect, Right?” Okay! “-Okay-” Ha-okay! “So just be aware, the characters fo tend to wander a bit-” Nehaheugh- “They’re one some kinda of free-roaming mode-” hehauhuhugh! “Uhh.. Something about their servos locking up if they get turned off for two long,” Oohoohoo- “Uh, they used to be allowed to walk around during the day, too, but then there was the bite of eighty-seven.” The bite..?! “Yeah..” What bite?! “It’s amazing that the human body can live without the frontal lobe,” Why?! “Now concerning your safety, the only real risk to you as the night watch here, if any, is the fact that these characters - if they happen to see you after hours, they probably won’t recognize you as a person-” Oh..Oh! “They’ll most likely see you as a metal endoskeleton without it’s costume on. Now, since that’s against the rules here at Freddy Fazbear’s pizza, they’ll probably try tooo.. Forcefully stuff you inside a Freddy Fazbear suit.” Oh, I get it… “Uhm, now that wouldn’t be so bad if the suits themselves weren’t filled with cross-beams, wires, and animatronic devices-” Augh? “”Especially around the facial area,” uh-huh.. “Now you can imagine how having your head forcefully pressed inside one of those can cause a bit of discomfort-” Yeah! “-or death. “Uh, the only parts of you that would get to see the light of day would be your eyeballs and teeth that pop out of the front of the mask-” Euah! Oh! Why? What happened? “Now, they didn’t tell you these things when you signed up, but hey! First day should be a breeze, I’ll chat with you tomorrow, uh.. Check those cameras and remember to close the doors, but only if absolutely necessary.” That’s not good! “*incomprehensible* because of the power. Now, goodnight!” *Phone guy’s dialogue ends.* Goodnight? Oooh no! Oh that’s bad! I understand what I need to do. I need to watch the cams so that they don’t come after m- *startled gibberish* - Ho hi! There you aaaaaare, pretty bunny thiing… Ooh-kay, okay, okay, I get it, I get it, I get it- where’d you go- You’re still there? Alright, you stay there. I don’t know if it’s good that you’re staring at me - Oh my god, I thought it was weird that it couldn’t move, but this is totally different *brief pause* than any horror game I’ve ever played. So, what you gotta do incase you’re not getting it, is you gotta watch *pause to take a breath* the cameras to make sure they don’t come by, while you gotta watch power. Is he still there..? Hi, you’re still there - wait a minute, what? Did you move? Ok-ie, you didn’t move. You don’t move neither. You don’t move nothin’! Don’t see ya movin’, I don’t wanna see anything.. Hohoh, my god! This Is terrifying! Why do I leave the doors open, why isn’t there enough power? -Hi, okay you moved again. Hi, what’re you doing there? Might be gettin’ a little close to mee.. Uh oh, oh, oh no.. Oh no, no! No, no no no, noo no no no eughh! Close it, ngahh! Hagh, don’t look at me! Okay, you’re over there, alright.. It’s okay! Why can’t I even have enough power for lights? *takes a breath* stay right there, you douchebag! You stay right - aff, there! *another audible breath* ..God damn it! That is like- this is like the most terrifying game I’ve ever played! Ugh, they’re gonna pop out at me! Oh, god he’s gone - hi, okay you’re just gonna alternate between the two places, it’s totally fine, your other friends.. They ain’t movin’.. *softly* They ain’t move much.. I see where I am, you’re not near me! So that’s good.. Just gonna *pauses briefly* periodically check -How much longer do I need - I need less ‘til six A.M, am I gonna have enough power? And if I run out of power am I able to get by? Oh god… You stay right there! Why am I still using some power? Oooh, god.. Seriously I’m *gibberish* - This is like, this is like… Bad! You’re still there, kay - this is the first night! They said it should be easy the first night so I’m only assuming one of em’ is gonna be wandering around and its just the creepy bunny guy. Happy fun time at Freddies Fun Land.. Havin’ such a wonderful time.. Still there? Okay, you’re still there! I’m gonna name you Bunny *pause* Baaalliday- oh, god where’d he go?! He’s here! Hooh! Where’d he go? Hi again, okay.. You stay right thaff - there! And I don’t have to deal with you! ..Probably shouldn’t do that, I need to conserve power! God dammit that was like.. Half the damn thing, the doors were down! ..Still there? *softly* hookay, okay, ookay.. Heheuhahahaha..*faint tune begins playing* I hear that! ...I hear that! Oh god! Where’s the other one? Where is he? Eagh! Euugh! Whereishe? Where’d he go? Where’d he go, where’d he go? Where are both of them? Bothofem’- Hi, you’re really close to meee.. Oh god, it’s not six A.M. yet? Hi, okay, so I think I just need to keep the left door closed.. *Mark makes faint noises of panic that gradually grow in volume* uhh, hehauhaha! Uhh, uhuh - Not okay, not okay! Is he behind that door? No? Where’d he go? Where’d he -AH! Oh, hi, hi! Hi, hi, hi, hi, okay, okay, I don’t have much power left. What’re you gonna do? Is the other one still there? Ngh! *ambient noise rings out* ..Hi! Ooh, you moved again! Where, where, where where… Heehuhahaha! What do I do? What do I do? Oooooh, you’re still right behind that dohoor.. Hoo what happens if I open the door? Imma run out of power! Ooh, Imma run out of power! Ishethere? *gibberish that gives way to wheezing ensues* I don’t wanna die! Aaah, 1 percent power! *gasps for air* *doors come up and an ambient effect is triggered as the power goes out ingame* AH! Ooh no..! Oh no.. Noo, no no no no no no no! Oh, no no nono no.. *a jingle begins to play as Mark stares at the screen in fear, soon noticing the blinking eyes of the games mascot* HI! OH, GOD DAMMIT! HOW ARE YOU DOING? *jingle changes as the dialogue “6 AM” is put on screen* Did I make it? *Mark spews gibberish and throws his arms up, gasping for air* Yeah! Oh god, not again! Why would I do this? *gibberish* -my job? *deep breath* ookay, okay. So I ran out of power -but, *phone rings* oh hi, hi again. Do you have an se- sage advice for me? Yep, kay, yep. I know, yep, yep yep yep. What can I do for you? *phone continues to ring* I know! *Phone guy’s dialogue plays once again* oh god..”Hello, hello! Uh, well if you’re hearing this you made it to day two! Uh, congrats!” *wheezes briefly* hehe! “I won’t talk quite as long this time, since Freddy and his friends tend to become more active as the week progresses.” *whispered* what? “Uh, it might be a good idea to peek at those cameras while I talk, just to make sure everyone’s in their proper place-” No.. “Uh, interestingly enough, Freddy himself doesn’t come off stage very often, I’ve heard he becomes a lot more active in the dark though, so hey, I guess that’s one more reason not to run out of power, right?” Aeugh! “Uh - I also want to emphasize the importance of using your door light - uh, there are blind spots in your camera view, and those blind spots happen to be right outside your doors, so if you can’t find something, or someone on your cameras, make sure-” Eugh!”- to check the door light. Uh, you might only have a few seconds to react -uh, not that you would be in any danger, of course. Uh, far from that. Also, uhh, check on the curtain in Pirate Cove from time to time, the character in there seems to be weakened and becomes more active if the cameras remain off for long periods of time. I guess he doesn’t like being watched.” *gasp, shuts door* “I dunno. Anyway, I’m sure you have everything under control, uh, talk to you soon!” Where’s Pirate Cove? Why are you gonna leave me with this? Don’t leave me like this! Where’s - where’s big yellow? There’s big yellow- is he still there? Is he still there? Yes, you’re still there! Very good! Very good! Aoooh don’t like thiiis… Ishestillthere? *gibberish* Okay, he left, okay.. Okay! We’re okay! We’re gonna be fine! We’re gonna be totally fine! We’re gonna be fine - hello. Hello, bubsy - where’s the other guy? Where’s the other guy? Where is he? Where’s he? Where is he? Where is he? Where? Oh there, okay. He’s not the-aaaaaay.. Hey Freddy! How you doin’? Okay.. You gonna be nearby? Stay there, where’s the other one? Where’s the other one? Where’s the other one? There he is,okay! I am pani- I am losing my shit right now! I am not okay with this -oh god not again! Nononodon’tdothat! *faint tune plays in background* Don’t you be- oh god, Hi, he’s right outside the door! Eugh! Hi..HII! Okay, imma.. Keep an eye on you! Or not, where’d you go? Where’d you go? Kaaay.. God, this night is lasting forever! *ambience plays as an animatronic appears in the doorway* AH! *panicked gibberish* That’s not okay! Uh, uh, uh.. Okay, so one’s bei- hii.. ‘Let’s eat!’ Let’s eat what? Are you still there? Okay, he’s gone. Good! Stay gone. Forever, and ever. And ever and ever and ever- ooh, you’re coming back! Either that or you’re leaving! Ooh, I’m not gonna haveneoughpowertostaythenight! My butt is gonna be munched, I’m gonna be shoved into a teddy bear outfit, and they’re gonna laugh. Where is he? No thank you. *Yet another animatronic appears in the doorway* AH! *gibberish yet again* That bunny wants to get my gibblets.. But he can’t have em’! Not today.. He’ll never..Good thing Freddy is sitting in his houseee hi Mr.- wait, Bunny, you were just outside my door! ..Kay… Where’s the ducky? Is that Mr. Ra- no, no ducky, there.. Where’s m- hi! Heeheeheeheheeheeugh.. Hi Mr. Ducky.. God, this night is lasting so long! I just wanna go home! I never wanna play this game again! I’ll be a good boy. God dammit, this would be, like, terrifying if you controlled the cameras with, like, an Oculus Rift or something. Oh, my god. Cause you just move your head back and forth. Ho, my god.. Hi again, where’s the other one.. Where'd he go? Where’d he go? There he is.. Ok-ie, so as long as you two stay right there, you gonna be good. You look very pretty. Where was the Pirate Cove guy? I-oh, here is Pirate Cover, okay. So I just gotta, hoo, I just gotta keep an eye on you guys. Gonna be fine. Oh I- I bet using the camera takes up power too - I’m down to 34 percent! I got three hours to go! *faint music begins again* no.. You’re still there, you’re still there.. Still there, still there. You’re lookin’ at me now… HI, PIRATE COVE MAN! Uaagh! Ohgaaugh..! Oh, where’d they go? Still.. Still there? Pirate Cove maaan.. How ya doin’? Oh, man, I love workin’ at Disney World.. It’s my favorite. HI, what are you doing out of your cage? Please, gett back in! I don’t want you out here! Oh, he’s coming for me.. Oh, he’s coming for me! Oh, why do I have to watch three of them? I’m, like, legit freaking out right now.. I’m not okay with this.. Oh god, they moved.. They moved, you coming down the hallway, huh? Which one are ya? Not left Pirate Cove yet.. You’re still there.. Comin’ down that hallway.. Pirate Cove man.. How you doing, Pirate Cove man? -No! I got two horus left! No, no no! Noo! What is that sounds? Ooh, he’s right there. Well, he’s not here juuust yet.. I don’t want to run out of power.. Oh, the sounds, I don’t like em’... AH! Fuck no, ah god,*Mark then proceeds to get killed by one of the animatronics, thus losing the game* AAHH! Fuckin’ fuck! I tried to push it! Hohoh, my god! Oogh.. Ohh.. Oh, game over indeed. Oh, are those my eyeballs? Oohh.. Oh, hi… Okay, so that was five nights at Freddy’s. I couldn’t even survive two. God, dammit! Haah.. Oh, god! Oh, I tried to hit the door! I tried so bad! Ough.. Okay, okay. Thank you all so much for watching, check out the other scary games that I’ve played and if you want to play this for yourself you can check it out in the description below. If you really want me to play it again and try to beat it, let me know in the comments below! Thanks again, everybody, and as always, I will see you in the next video. Buh-bye!
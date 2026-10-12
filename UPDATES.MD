Qud-Fu, Mixed Martial Arts in Qud

-------- May 27th, 2024 

[o] |   Added the "Visceral Descriptors System." Adds randomized log-messages when attacking opponents while using a Qud-Fu stance.
[o] |   Each stance has their own defined attack-descriptors.
[o] |   You can toggle these log-prompts off and on using the associated command you can now find in your abilities list.

CHANGES TO COME!

[x] |   Player will be able to select and modify the frequency of log-prompts with an ingame command.
[x] |  Better more expanded descriptors that take into consideration what body-parts you attack with and the state of the enemy and yourself.
[x] |   Currently, due to the complexity of Death-Dacca Stance, it is using a simplified version of its attack-descriptors. Eventually, it will have different log-prompts based on whether you are actively grappling an opponent or not and will also have descriptors for when you use its grappling-techniques. 
[x] |   Working on a Dojo and Training system that will be added to give more of a story element to learning the martial arts and make them more difficult due to the power of each one.
[x] |   Quest to find and challenge, aid or train under "Masters" for fame, fortune and more. Such masters as one mentioned in the Book of Forms, "The Rising-Dawnstar, Arshiranandurah" and "The Amethyst-Shelled Tortiose, Kuyoka-Dora."

Please let me know if you see any grammatical or sequence errors in the creation of the prompts, sometimes the code used to make these boggle my brainstuff. And sometimes ... I just suck at writing!!! x.x 

--------

[o] |   Modified Damage parameters for hand-to-hand, should start off a bit more viable, tough still require combo ramping to be fully effective, deals more consistent damage as your increases in level. Previous damage formula might of been a little too restricting, and now leveling should feel more rewarding while using these skills.
[o] |   Reformated Braver names uses Epitepths so that they properly generate when being assigned to a sultan dungeon, etc. 
[o] |   Buffed the cleave chance on Slumberling Style. Now scales with your strength modifier and uses only have your level in the base formula.
[f] |   Fixed issue with other damage types effecting visceral descriptors.
[o] |   Nerfed the current damage scaling of comboing slightly, now penetration scales by +1 per each tenth of the current combo-meter bonus when you have Combination Strikes III
[o] |   Maximum combo counter has also been raised from 30 to 99.
[f] |   Sure-strikes is now working as intended! It has also been slightly more effective. 


--------

~Qud-Fu! MMA in Qud~ 

I've modified the overall scaling and penetration values of fist, they now show their scaling like other weapons do and update properly when you meet the criteria for each step of their level-based increases while having the Hand-to-Hand Skill tree, scale with your level, and are significantly more effect at later levels. Other natural weapons, or fist-weapons like Carbide Fist or Burrowing Claws now deal increased damage that scales with you level even if they don't necessarily show it. A more fleshed-out idea for this in the works, where they will also scale to your level and actually show their damage scaling in their stats, but with how many there are and the fact each will require its own scaling formulae, I am keeping it simple for now. This does mean that, as of this version, Default Fist potentially scale *much higher* than other fist/natural-weapons after you reach a certain threshold through leveling. I'm still deciding on whether that's going to be intentional regarding the mod's current design-direction. The prospect of your natural fist getting Ape God's level is actually something I believe should be possible since there is a case for it in the game. 

Sometimes, (and I'm still trying to determine why) your fist don't update properly or even reset. In the event that this happens, I created a debug wish to set your fist to the proper values, just type: "fistbeharder" as a wish command and the issue should be fixed!

I've made some small modifications around the mod as well and also expounded on and corrected some descriptions that may have been too vague or incorrect.


[o] |   Fixing Chaining Strikes to work Properly.
[f] |   Sure-Strikes works more consistently now.
[f] |   Slumberling Stance has been slightly modified, along with checks to ensure that certain enemies are valid targets, the scaling has been fixed to actually use you combo-gauge for the random dismemberment effects and such effects have been toned down a little as well.
[f] |   Edited all skills, attribute, and skill point requirements for the Hand-to-Hand skills to fit more in line with the default game's formulae.
[c] |   Edited fist mechanics, now they show their scaling on your default fist, scale with your level, and are significantly more effect at later levels.

--------

TODO;

- The Bonus from sure-strikes is not applying, make sure it only applies to the TO HIT MODIFIER, not the penetration. [x]
- Make all stances toggleable skills, not just ones you tap and activate. _____ [Deferred]
- take a look at mercy and ensure its working properly _____ [Deferred]
- Some of the skill descriptions are still too vague 
- Replace all of the stance and ability sounds with original sounds.
  ~~- Consider looking and making sure that the damage scaling is actually working for default fist~~
    - New issue, sometimes the fist reset, unsure why they are doing it, but it seems related to either learning a new mutation or doing something that checks when limbs are updated or something. May be because certain mutations or foods eaten update or change the fist properties resulting in them being reset.
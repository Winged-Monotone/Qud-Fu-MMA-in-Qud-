using System;
using XRL.World;
using XRL.World.Parts.Mutation;
using System.Collections.Generic;
using XRL.Rules;
using System.Linq;
using XRL.World.Effects;
using XRL.Language;
using XRL.World.Capabilities;
using UnityEngine;
using System.Text;
using System.Collections.Specialized;
using System.IO.Pipes;
using JapaneseNameListWW;
using System.IO;
using System.Xml;
using System.Runtime.CompilerServices;
using XRL.Names;
using XRL.World.Encounters;
using XRL.World.Parts;
using AiUnity.Common.Extensions;
using XRL.World.Parts.Skill;


namespace XRL.World.Parts
{

    [Serializable]
    public class JapaneseNameConstructor : IPart
    {
        private string[] JapanesePhonemFilters = { "CVV", "CCV", "CV", "VV", "VN", "V", "N" };
        private string Consonants = "BB̄ḆCÇC̄C̱DD̄ḎFGḠG̱HH̱JJ̄J̱KḴLL̄ḸḺMM̄M̱NN̄ṈÑPP̄P̱QQ̄R̄ṞṜRSŠS̄S̱TT̄ṮVV̄W̄WXX̄X̱YÝŸȲȲ́Ȳ̀Ȳ̃Y̱ŽZZ̄ẔŒ̄";
        private string Vowels = "AEIOUÀÁÂÃÄÈÉÊËÌÍÎÏÒÓÔÕÖÚÛÜÙĀĀ́Ā̀Ā̂Ā̃ǞĀ̈ǠA̱Å̄ǢĒḖḔĒ̂Ē̃Ê̄E̱Ë̄E̊̄ĪĪ́Ī̀Ī̂Ī̃I̱ŌṒṐŌ̂Ō̃ȪŌ̈ǬȬȰO̱Ø̄ŪŪ́Ū̀Ū̂Ū̃U̇ǕṺṲ̄U̱";
        private string Exception = "NN̄ṈÑ'";

        private Dictionary<string, Dictionary<string, int>> JapanesePhonemAdjacency;
        private Dictionary<string, Dictionary<string, int>> JapaneseSurnamePhonemAdjacency;
        public JapaneseNameConstructor()
        {
            JapanesePhonemAdjacency = new Dictionary<string, Dictionary<string, int>>();
            JapaneseSurnamePhonemAdjacency = new Dictionary<string, Dictionary<string, int>>();



            // AddPlayerMessage("PopulatingJapanesePhonems ...");
            // AddPlayerMessage("" + Char.ToUpper('ō'));



            foreach (string name in JapaneseNameListWW.JapaneseNamelist.JAPANESE_NAMES_FIRSTNAMES)
            {
                PopulateJapanesePhonemAdjacency(name);
            }
            foreach (string name in JapaneseNameListWW.JapaneseNamelist.JAPANESE_NAMES_SURNAMES)
            {
                PopulateJapaneseSurnamePhonemAdjacency(name);
            }


            // int Ran = Stat.Random(1, 4);
            // int SurRan = Stat.Random(1, 6);
            // GO.DisplayName = "&M" + "" + GenerateJapaneseSurName(Ran) + " " + GenerateJapaneseName(SurRan);


            // for (int i = 0; i < 30; i++)
            // {
            //     int Ran = Stat.Random(1, 4);
            //     int SurRan = Stat.Random(1, 6);
            //     AddPlayerMessage(GenerateJapaneseSurName(Ran) + " " + GenerateJapaneseName(SurRan));
            // }

            // AddPlayerMessage("Name Generation Complete ...");

            // AddPlayerMessage("Generating XML Names ...");
            // XMLConversion();
            // AddPlayerMessage("Generated XML Names ...");

        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
            || ID == GetDisplayNameEvent.ID
            || ID == AfterObjectCreatedEvent.ID
            ;
        }

        private List<string> DiscipleUnits = new List<string>()
        {
                "BraverSlumberDisciple",
                "BraverSaltBackDisciple",
                "BraverSaltHopDisciple",
                "BraverAstralDisciple",
                "BraverDawnDisciple",
                "BraverDisciple",
        };

        public override bool HandleEvent(AfterObjectCreatedEvent E)
        {
            string[] MasteryTitle = new string[7]
      {
                "Master",
                "Sifu",
                "Shogun",
                "Shogunate",
                "Grandmaster",
                "Shihan",
                "Souke",
      };
            string[] DiscipleTitle = new string[7]
      {
                "Disciple",
                "Protégé",
                "Pupil",
                "Studier",
                "Savant",
                "Apprentice",
                "Gakusei",
      };
            string[] MasteryPro = new string[20]
        {
                "Venerant",
                "Honorable",
                "Deadly",
                "Honorbound",
                "Honorous",
                "Shameless",
                "Lusting",
                "Lusty",
                "Lewd",
                "Proud",
                "Blithe",
                "Lighthearted",
                "Gay",
                "Stern",
                "Harsh",
                "Unbending",
                "Irate",
                "Raging",
                "Ardent",
                "Fervent",
        };

            int Ran = Stat.Random(1, 4);
            int SurRan = Stat.Random(1, 6);
            if (E.Object == ParentObject && !(DiscipleUnits.Any<string>()))
            {
                if (!E.Object.HasProperName)
                {
                    ParentObject.pRender.DisplayName = ("&M")
                    + (GenerateJapaneseSurName(Ran))
                    + " "
                    + (GenerateJapaneseName(SurRan))
                    + (", ");

                    ParentObject.pRender.DisplayName += (MasteryPro.GetRandomElement() + "-" + MasteryTitle.GetRandomElement());
                    if (ParentObject.Factions.Contains("BraversDawn"))
                        ParentObject.pRender.DisplayName += (" of the Dawning-Fist Ryu");
                    if (ParentObject.Factions.Contains("BraversSaltBack"))
                        ParentObject.pRender.DisplayName += (" of the Amethyst Shell Goyo-Ryu");
                    if (ParentObject.Factions.Contains("BraversSaltHopper"))
                        ParentObject.pRender.DisplayName += (" of the Scythe-Strike Ryu");
                    if (ParentObject.Factions.Contains("BraversAstralCabby"))
                        ParentObject.pRender.DisplayName += (" of the Void-Claw Ryu");
                    if (ParentObject.Factions.Contains("BraversSlumber"))
                        ParentObject.pRender.DisplayName += (" of the Slumbering-Fury Ryu");
                }
            }
            else
            {
                if (!E.Object.HasProperName && E.Object.HasPropertyOrTag("Role"))
                {
                    ParentObject.pRender.DisplayName = ("&M")
                    + (GenerateJapaneseSurName(Ran))
                    + " "
                    + (GenerateJapaneseName(SurRan))
                    + (", ");

                    ParentObject.pRender.DisplayName += (MasteryPro.GetRandomElement() + "-" + DiscipleTitle.GetRandomElement());
                    if (ParentObject.Factions.Contains("BraversDawn"))
                        ParentObject.pRender.DisplayName += (" of the Dawning-Fist Ryu");
                    if (ParentObject.Factions.Contains("BraversSaltBack"))
                        ParentObject.pRender.DisplayName += (" of the Amethyst Shell Goyo-Ryu");
                    if (ParentObject.Factions.Contains("BraversSaltHopper"))
                        ParentObject.pRender.DisplayName += (" of the Scythe-Strike Ryu");
                    if (ParentObject.Factions.Contains("BraversAstralCabby"))
                        ParentObject.pRender.DisplayName += (" of the Void-Claw Ryu");
                    if (ParentObject.Factions.Contains("BraversSlumber"))
                        ParentObject.pRender.DisplayName += (" of the Slumbering-Fury Ryu");
                }
            }

            return base.HandleEvent(E);

        }

        // public override bool HandleEvent(GetDisplayNameEvent E)
        // {
        //     string[] MasteryTitle = new string[7]
        // {
        //     "Master",
        //     "Sifu",
        //     "Shogun",
        //     "Shogunate",
        //     "Grandmaster",
        //     "Shihan",
        //     "Souke",
        // };
        //     string[] MasteryPro = new string[20]
        // {
        //     "Venerant",
        //     "Honorable",
        //     "Deadly",
        //     "Honorbound",
        //     "Honorous",
        //     "Shameless",
        //     "Lusting",
        //     "Lusty",
        //     "Lewd",
        //     "Proud",
        //     "Blithe",
        //     "Lighthearted",
        //     "Gay",
        //     "Stern",
        //     "Harsh",
        //     "Unbending",
        //     "Irate",
        //     "Raging",
        //     "Ardent",
        //     "Fervent",
        // };

        //     int Ran = Stat.Random(1, 4);
        //     int SurRan = Stat.Random(1, 6);
        //     if (E.Object == ParentObject)
        //     {

        //         ParentObject.pRender.DisplayName = ("&M")
        //         + (GenerateJapaneseSurName(Ran))
        //         + " "
        //         + (GenerateJapaneseName(SurRan))
        //         + (", ")
        //         + MasteryPro.GetRandomElement()
        //         + ("-")
        //         + MasteryTitle.GetRandomElement();

        //         if (ParentObject.Factions.Contains("BraversDawn"))
        //             E.AddBase(" of the Dawning-Fist Ryu", 2);
        //         if (ParentObject.Factions.Contains("BraversSaltBack"))
        //             E.AddBase(" of the Amethyst Shell Goyo-Ryu", 2);
        //         if (ParentObject.Factions.Contains("BraversSaltHopper"))
        //             E.AddBase(" of the Scythe-Strike Ryu", 2);
        //         if (ParentObject.Factions.Contains("BraversAstralCabby"))
        //             E.AddBase(" of the Void-Claw Ryu", 2);
        //         if (ParentObject.Factions.Contains("BraversSlumber"))
        //             E.AddBase(" of the Slumbering-Fist Ryu", 2);
        //     }

        //     if (ParentObject.HasPart("JapaneseNameConstructor"))
        //     {
        //         ParentObject.RemovePart<JapaneseNameConstructor>();
        //     }

        //     return base.HandleEvent(E);
        // }







        // public void XMLConversion()
        // {
        //     var writer = XmlWriter.Create(
        //         "JapaneseFirstName",
        //     new XmlWriterSettings
        //     {
        //         Indent = true,
        //         CloseOutput = true,
        //         CheckCharacters = false,
        //         IndentChars = "\t",
        //         NewLineChars = Environment.NewLine
        //     });
        //     writer.WriteStartElement("Stuff");
        //     foreach (var str in JapaneseNameListWW.JapaneseNamelist.JAPANESE_NAMES_FIRSTNAMES)
        //     {
        //         writer.WriteStartElement("Table");
        //         writer.WriteAttributeString("Name", str);
        //         writer.WriteEndElement();
        //     }
        //     writer.WriteFullEndElement();
        //     writer.Flush();
        //     writer.Close();
        //     writer.Dispose();
        // }

        public void PopulateJapanesePhonemAdjacency(string name)
        {
            List<string> Phonems = DecomposeName(name);
            if (Phonems != null)
            {
                for (int i = 0; i < Phonems.Count - 1; i++)
                {
                    Dictionary<string, int> Adjacency;

                    // AddPlayerMessage("Before Get Adjacency");

                    if (!JapanesePhonemAdjacency.TryGetValue(Phonems[i], out Adjacency))
                    {
                        Adjacency = new Dictionary<string, int>();
                        JapanesePhonemAdjacency[Phonems[i]] = Adjacency;
                    }

                    // AddPlayerMessage("After get adjacency name ...");

                    int Occurrence = 0;

                    // AddPlayerMessage("Get Adjacency Values ...");

                    if (Adjacency.TryGetValue(Phonems[i + 1], out Occurrence))
                    {
                        Adjacency[Phonems[i + 1]] = 0;
                        Occurrence = 0;
                    }

                    // AddPlayerMessage("After get adjacency values ...");

                    Adjacency[Phonems[i + 1]] = Occurrence + 1;
                }
            }
        }
        public void PopulateJapaneseSurnamePhonemAdjacency(string surname)
        {
            List<string> Phonems = DecomposeName(surname);
            if (Phonems != null)
            {
                for (int i = 0; i < Phonems.Count - 1; i++)
                {
                    Dictionary<string, int> Adjacency;

                    // AddPlayerMessage("Before Get Adjacency");

                    if (!JapaneseSurnamePhonemAdjacency.TryGetValue(Phonems[i], out Adjacency))
                    {
                        Adjacency = new Dictionary<string, int>();
                        JapaneseSurnamePhonemAdjacency[Phonems[i]] = Adjacency;
                    }

                    // AddPlayerMessage("After get adjacency name ...");

                    int Occurrence = 0;

                    // AddPlayerMessage("Get Adjacency Values ...");

                    if (Adjacency.TryGetValue(Phonems[i + 1], out Occurrence))
                    {
                        Adjacency[Phonems[i + 1]] = 0;
                        Occurrence = 0;
                    }

                    // AddPlayerMessage("After get adjacency values ...");

                    Adjacency[Phonems[i + 1]] = Occurrence + 1;
                }
            }
        }
        public string GenerateJapaneseSurName(int SoftLimit)
        {
            // AddPlayerMessage("Generating Name Begins ...");

            string CurrentPhonem = "Start";
            string name = "";
            int counter = 0;

            CurrentPhonem = PickNextPhonem(JapaneseSurnamePhonemAdjacency[CurrentPhonem]);
            // AddPlayerMessage("CurrentPhonem " + CurrentPhonem);

            while (CurrentPhonem != "End")
            {
                name += CurrentPhonem;
                if (counter < SoftLimit || !JapaneseSurnamePhonemAdjacency[CurrentPhonem].ContainsKey("End"))
                {
                    CurrentPhonem = PickNextPhonem(JapaneseSurnamePhonemAdjacency[CurrentPhonem]);
                    ++counter;
                }
                else
                { CurrentPhonem = "End"; }
                // AddPlayerMessage("CurrentPhonem " + CurrentPhonem);
            }

            // AddPlayerMessage("name " + name);


            return name;
        }
        public string GenerateJapaneseName(int SoftLimit)
        {
            // AddPlayerMessage("Generating Name Begins ...");

            string CurrentPhonem = "Start";
            string name = "";
            int counter = 0;

            CurrentPhonem = PickNextPhonem(JapanesePhonemAdjacency[CurrentPhonem]);
            // AddPlayerMessage("CurrentPhonem " + CurrentPhonem);

            while (CurrentPhonem != "End")
            {
                name += CurrentPhonem;
                if (counter < SoftLimit || !JapanesePhonemAdjacency[CurrentPhonem].ContainsKey("End"))
                {
                    CurrentPhonem = PickNextPhonem(JapanesePhonemAdjacency[CurrentPhonem]);
                    ++counter;
                }
                else
                { CurrentPhonem = "End"; }
                // AddPlayerMessage("CurrentPhonem " + CurrentPhonem);
            }

            // AddPlayerMessage("name " + name);


            return name;
        }

        private string PickNextPhonem(Dictionary<string, int> Adjacency)
        {
            int Total = 0;
            int Border = 0;
            string[] Phonems = Adjacency.Keys.ToArray();

            // AddPlayerMessage("Picking Next Phonem begin Foreach ...");


            foreach (int Occurrence in Adjacency.Values)
            {
                Total += Occurrence;
            }

            // AddPlayerMessage("total ..." + Total);

            // AddPlayerMessage("Finished picking phonem ...");

            int RandomizedPhonetics = Stat.Random(0, Total);
            bool HasFoundIndex = false;

            int index;
            for (index = 0; index < Adjacency.Count - 1 && !HasFoundIndex; index++)
            {
                // AddPlayerMessage("Setting Border");

                Border += Adjacency[Phonems[index]];

                // AddPlayerMessage("border =" + Border);

                if (RandomizedPhonetics < Border)
                {
                    HasFoundIndex = true;
                }
            }

            // AddPlayerMessage("returning phonem index ..." + index);
            // AddPlayerMessage("Phonems index ..." + Phonems[index]);


            return Phonems[index];
        }
        private bool isException(char c)
        {
            return Exception.Contains(Char.ToUpper(c));
        }
        private char LetterClassifier(char c)
        {
            if (Consonants.Contains(Char.ToUpper(c)))
            {
                return 'C';
            }
            else if (Vowels.Contains(Char.ToUpper(c)))
            {
                return 'V';
            }

            return 'x';
        }
        private bool GetMatchingFilter(string name, int offset, out string filter)
        {
            bool HasMatchedFilter = false;

            string currentFilter = "";


            for (int i = 0; i < JapanesePhonemFilters.Length && !HasMatchedFilter; i++)
            {
                HasMatchedFilter = true;
                currentFilter = JapanesePhonemFilters[i];

                // AddPlayerMessage("Filter: " + currentFilter);

                if (currentFilter.Length <= name.Length - offset)
                {
                    for (int charIndex = 0; charIndex < currentFilter.Length && HasMatchedFilter; charIndex++)
                    {
                        if (currentFilter[charIndex] != 'N')
                        {
                            if (LetterClassifier(name[charIndex + offset]) != currentFilter[charIndex])
                            {
                                HasMatchedFilter = false;
                            }
                        }
                        else
                            HasMatchedFilter = (isException(name[charIndex + offset]));
                    }
                }
                else
                {
                    HasMatchedFilter = false;
                }
            }
            if (!HasMatchedFilter)
            {
                filter = "";
                return false;
            }
            else
            {
                filter = currentFilter;
                return true;
            }
        }
        private List<string> DecomposeName(string name)
        {
            // V = Vowel
            // C = Consonant
            // N = N Exception

            // Phonems can be a Single V, CV, CVV, CCV, VN, NV, VV, N
            //  Order:
            // CVV, CCV, CV, VV, NV, VN, V, N
            // Phonems can't end with a C, except for N

            // AddPlayerMessage("name length " + name.Length);

            int charOffset = 0;

            List<string> Phonems = new List<string>();
            Phonems.Add("Start");

            while (charOffset < name.Length)
            {
                // AddPlayerMessage("Testing Offset :" + charOffset);

                string matchingFilter = "";

                if (GetMatchingFilter(name, charOffset, out matchingFilter))
                {
                    // AddPlayerMessage("Matched Filter " + matchingFilter);

                    Phonems.Add(name.Substring(charOffset, matchingFilter.Length));
                    charOffset += matchingFilter.Length;
                }
                else
                {
                    // AddPlayerMessage("Couldn't match filter.");

                    return null;
                }
            }

            Phonems.Add("End");

            return Phonems;
        }
    }
}
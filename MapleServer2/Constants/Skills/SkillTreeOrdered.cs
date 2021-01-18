﻿using System.Collections.Generic;
using System.Linq;
using MapleServer2.Enums;

namespace MapleServer2.Constants.Skills
{
    public static class SkillTreeOrdered
    {
        // TODO: Get a better solution for Ordered List, Split and DefaulSkills
        //
        #region Ordered list of skills.
        private static readonly List<int> None = new List<int>
        {
            0
        };
        private static readonly List<int> Knight = new List<int>
        {
            10100091,10100261,10100041,10100211,10100161,10100281,20000011,10100061,10100011,10100181,
            10100062,10100131,10100081,10100031,10100201,10100151,10100101,20000001,10100051,10100221,
            10100001,10100171,10100291,10100071,10100021,10100191,10100141,10100294,10100111,10100162,
            10100231,10100251,10100202,10100271,10100204,10100121,10100241
        };
        private static readonly List<int> Berserker = new List<int>
        {
            10200051,10200255,10200221,10200001,10200205,10200171,10200206,10200257,10200291,10200172,
            10200207,10200071,20000011,10200021,10200141,10200091,10200041,10200161,10200281,10200061,
            10200231,10200282,20000001,10200181,10200011,10200062,10200131,10200233,10200251,10200234,
            10200201,10200271,10200189,10200121,10200241,10200191,10200209,10200261,10200142,10200211,
            10200229,10200111,10200081,10200031,10200151,10200101,10200254
        };
        private static readonly List<int> Wizard = new List<int>
        {
            10300232,10300021,10300181,10300236,10300131,10300291,10300081,10300031,10300191,10300299,
            10300141,10300091,10300251,10300198,10300252,10300041,10300253,10300201,10300043,10300203,
            20000001,10300262,10300051,10300211,10300053,10300001,10300266,10300214,10300161,20000011,
            10300271,10300221,10300222,10300011,10300171,10300225,10300282,10300071,10300231,10300200,
            10300184,10300219,10300151,10300101,10300121,10300240,10300241,10300260,10300261,10300210,
            10300093,10300281,10300230,10300111,10300179,10300061
        };
        private static readonly List<int> Priest = new List<int>
        {
            10400091,10400193,10400041,10400211,10400161,20000011,10400061,10400231,10400011,10400181,
            10400081,10400031,10400151,20000001,10400101,10400271,10400186,10400051,10400221,10400001,
            10400171,10400121,10400291,10400071,10400241,10400191,10400072,10400141,10400261,10400111,
            10400281,10400131,10400301,10400251,10400021,10400242,10400293
        };
        private static readonly List<int> Archer = new List<int>
        {
            10500101,10500152,10500221,10500051,10500153,10500171,10500001,10500291,10500172,10500241,
            20000011,10500173,10500191,10500021,10500174,10500141,10500192,10500243,10500091,10500193,
            10500261,10500041,10500211,10500093,10500144,10500281,10500231,20000001,10500061,10500181,
            10500011,10500081,10500031,10500065,10500151,10500067,10500271,10500121,10500292,10500071,
            10500293,10500161,10500111,10500232,10500131,10500063,10500251,10500064,10500201
        };
        private static readonly List<int> HeavyGunner = new List<int>
        {
            10600044,10600231,10600061,10600181,10600011,10600131,10600081,10600201,20000011,10600031,
            10600101,10600271,10600051,10600221,10600171,10600001,10600291,10600121,10600071,10600191,
            20000001,10600021,10600261,10600091,10600211,10600041,10600281,10600284,10600251,10600285,
            10600286,10600151,10600172,10600223,10600241,10600141,10600192,10600161,10600111,10600213
        };
        private static readonly List<int> Thief = new List<int>
        {
            10700011,10700016,10700281,10700282,10700283,10700231,10700021,10700181,10700022,10700182,
            10700131,10700081,10700031,10700032,10700191,10700141,10700142,20000001,10700091,10700092,
            10700252,10700093,10700200,10700041,10700042,10700202,10700204,20000011,10700051,10700052,
            10700211,10700001,10700002,10700161,10700162,10700111,10700271,10700273,10700221,10700222,
            10700192,10700261,10700212,10700061,10700301,10700251,10700183,10700151,10700101,10700291,
            10700121,10700241,10700071
        };
        private static readonly List<int> Assassin = new List<int>
        {
            10800101,10800271,10800203,10800051,10800221,10800001,10800171,20000011,10800121,10800291,
            10800071,10800021,10800191,10800293,10800141,10800261,10800041,10800211,10800161,10800229,
            20000001,10800281,10800061,10800231,10800011,10800181,10800131,10800081,10800031,10800201,
            10800151,10800052,10800241,10800091,10800111,10800163,10800232,10800251
        };
        private static readonly List<int> Runeblade = new List<int>
        {
            10900033,10900193,01000005,10900034,01000007,10900196,10900091,10900092,10900198,01000011,
            01000012,10900094,01000013,10900201,10900095,01000014,10900203,10900151,10900205,10900153,
            10900207,10900101,10900261,10900155,10900103,10900157,10900051,10900211,10900105,10900212,
            10900107,10900001,10900213,10900161,10900214,10900111,20000001,10900061,10900221,10900062,
            10900222,10900063,10900011,10900223,10900171,10900065,10900225,10900173,10900281,10900175,
            20000011,10900071,10900177,10900231,10900072,10900021,10900074,10900181,10900076,10900131,
            10900291,10900292,10900081,10900293,10900294,01000001,10900031,10900191,10900032,01000003,
            10900163,10900232,10900251,10900271,10900121,10900241,10900141,10900041
        };
        private static readonly List<int> Striker =new List<int>
        {
            11000312,11000101,11000313,11000261,11000262,11000051,11000316,11000211,11000053,11000265,
            11000212,11000001,11000266,11000213,11000214,11000161,11000055,11000321,11000271,11000272,
            11000061,11000011,11000171,11000012,11000121,11000281,11000282,20000001,11000283,11000021,
            11000181,11000341,11000182,11000183,11000131,11000132,11000026,11000081,20000011,11000241,
            11000031,11000191,11000032,11000192,11000141,11000091,11000144,11000041,11000042,11000071,
            11000311,11000331,11000314,11000111,11000231,11000267,11000082,11000219,11000151,11000221,
            11000291
        };
        private static readonly List<int> SoulBinder = new List<int>
        {
            11100111,11100112,11100271,11100061,11100062,11100221,11100011,11100171,11100121,11100281,
            11100071,11100231,11100072,11100232,11100073,11100074,11100021,11100181,11100075,11100022,
            11100023,11100024,11100131,11100025,11100184,11100132,11100292,11100133,11100081,11100134,
            20000001,11100241,11100135,11100082,01200001,11100242,11100083,01200002,11100084,11100191,
            01200003,11100244,11100085,11100192,01200004,11100193,01200005,11100141,11100089,11100091,
            20000011,01200011,11100041,01200012,11100201,01200013,11100202,01200014,01200015,11100151,
            11100311,11100153,11100101,11100154,11100261,11100102,11100155,11100103,11100315,11100156,
            11100051,11100104,11100105,11100031,11100321,11100204,11100187,11100272,11100001,11100291,
            11100002,11100173,11100243,11100211,11100262,11100161,11100301,11100063,11100251
        };
        private static readonly List<int> GameMaster = new List<int>
        {
            0
        };
        #endregion

        /// <summary>
        /// Get the specific ordered skill list of each Job. Requiered for SkillBookTree
        /// </summary>
        public static List<int> GetListOrdered(Job job)
        {

            switch (job)
            {
                case Job.Knight:
                    return Knight;
                case Job.Berserker:
                    return Berserker;
                case Job.Wizard:
                    return Wizard;
                case Job.Priest:
                    return Priest;
                case Job.Archer:
                    return Archer;
                case Job.HeavyGunner:
                    return HeavyGunner;
                case Job.Thief:
                    return Thief;
                case Job.Assassin:
                    return Assassin;
                case Job.Runeblade:
                    return Runeblade;
                case Job.Striker:
                    return Striker;
                case Job.SoulBinder:
                    return SoulBinder;
                case Job.GameMaster:
                    return GameMaster;
                case Job.None:
                    return None;
                default:
                    return None;
            }
        }
    }
}

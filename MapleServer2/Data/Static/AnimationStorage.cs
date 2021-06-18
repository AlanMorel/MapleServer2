﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    // This is an in-memory storage to help with determining some metadata of items
    public static class AnimationStorage
    {
        private static readonly Dictionary<string, AnimationMetadata> Animations = new Dictionary<string, AnimationMetadata>();

        static AnimationStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-animation-metadata");
            List<AnimationMetadata> animations = Serializer.Deserialize<List<AnimationMetadata>>(stream);
            foreach (AnimationMetadata animation in animations)
            {
                Animations.Add(animation.ActorId, animation);
            }
        }

        public static List<SequenceMetadata> GetSequencesByActorId(string actorId)
        {
            return Animations.GetValueOrDefault(actorId.ToLower()).Sequence;
        }

        public static short GetSequenceIdBySequenceName(string actorId, string sequenceName)
        {
            Dictionary<string, SequenceMetadata> sequences = new Dictionary<string, SequenceMetadata>();
            foreach (SequenceMetadata sequence in GetSequencesByActorId(actorId))
            {
                sequences.Add(sequence.SequenceName, sequence);
            }
            if (sequences.ContainsKey(sequenceName))
            {
                return sequences.GetValueOrDefault(sequenceName).SequenceId;
            }
            else
            {
                return 0;
            }
        }
    }
}

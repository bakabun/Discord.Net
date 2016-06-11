﻿using Discord.API.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Model = Discord.API.Channel;

namespace Discord
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    internal class VoiceChannel : GuildChannel, IVoiceChannel
    {
        public int Bitrate { get; private set; }
        public int UserLimit { get; private set; }

        public VoiceChannel(Guild guild, Model model)
            : base(guild, model)
        {
        }
        public override void Update(Model model, UpdateSource source)
        {
            if (source == UpdateSource.Rest && IsAttached) return;

            base.Update(model, source);
            Bitrate = model.Bitrate;
            UserLimit = model.UserLimit;
        }
        
        public async Task ModifyAsync(Action<ModifyVoiceChannelParams> func)
        {
            if (func != null) throw new NullReferenceException(nameof(func));

            var args = new ModifyVoiceChannelParams();
            func(args);
            var model = await Discord.ApiClient.ModifyGuildChannelAsync(Id, args).ConfigureAwait(false);
            Update(model, UpdateSource.Rest);
        }

        public override Task<IGuildUser> GetUserAsync(ulong id)
        {
            throw new NotSupportedException();
        }
        public override Task<IReadOnlyCollection<IGuildUser>> GetUsersAsync()
        {
            throw new NotSupportedException();
        }
        public override Task<IReadOnlyCollection<IGuildUser>> GetUsersAsync(int limit, int offset)
        {
            throw new NotSupportedException();
        }

        private string DebuggerDisplay => $"{Name} ({Id}, Voice)";
    }
}

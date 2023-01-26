using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBOTForm.Commands
{
    public class Menu : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"You ping is : - {Context.Client.Latency}ms")
                .WithDescription("EPIC")
                .WithColor(Color.Magenta);

            await ReplyAsync("", false, builder.Build());
        }
    }
}

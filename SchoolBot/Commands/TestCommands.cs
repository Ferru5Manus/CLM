
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBot
{
    public class TestCommands : BaseCommandModule
    {   [Command("ping")]
        public async Task PingPong(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }
        [Command("add")]
        public async Task Add(CommandContext ctx, int numberOne, int numberTwo)
        {
            await ctx.Channel.SendMessageAsync((numberOne+numberTwo).ToString()).ConfigureAwait(false); 
            

        }
        
        [Command("clear")]
        [RequireRoles(RoleCheckMode.Any, "Admin","Teacher")]
        [Description("Clears chat")]
        public async Task Clear(CommandContext ctx)
        {
            var s =await ctx.Channel.GetMessagesAsync().ConfigureAwait(false);
            foreach (var item in s)
            {
                await item.DeleteAsync();
            }
            
        }
             
    }
}

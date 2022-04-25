
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolDatabaseRepository;
using System.IO;

namespace SchoolBot
{
    public class RoleManagementCommands : BaseCommandModule 
    {
        private FormsDatabaseRepository  repository = new FormsDatabaseRepository();
        
        [Command("joinform")]
        [Description("Joins you to your form:)")]
        public async Task JoinForm(CommandContext ctx, string formName)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Click emoji if you want to join "+formName+" form",
                ImageUrl = ctx.Client.CurrentUser.AvatarHash, 
                Color = DiscordColor.Green
            };
            var joinmessage = await ctx.Channel.SendMessageAsync(embed:joinEmbed).ConfigureAwait(false);
            var thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");
            await joinmessage.CreateReactionAsync(thumbsUpEmoji).ConfigureAwait(false);
            await joinmessage.CreateReactionAsync(thumbsDownEmoji).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();
            bool IsAdded = false;
            var reaction= await interactivity.WaitForReactionAsync(x => x.Message == joinmessage && x.User == ctx.User &&(x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji)).ConfigureAwait(false);
            if (reaction.Result.Emoji == thumbsUpEmoji)
            {
                var myList = File.ReadAllLines(@"C:\Tasks\FormsDiscordConfig.txt");
                foreach (var item in myList)
                {
                    string[] data = item.Split('|');
                    if (formName == data[0])
                    {
                        var role = ctx.Guild.GetRole(Convert.ToUInt64(data[1]));
                        await ctx.Member.GrantRoleAsync(role);

                        IsAdded = true;
                        break;
                    }
                   
                }
                if(IsAdded==false)
                {
                    await ctx.Channel.SendMessageAsync("Error, no such form found");
                }

            }
            else if (reaction.Result.Emoji == thumbsDownEmoji)
            {
            }
            await joinmessage.DeleteAsync();


        }
        [Command("addform")]
        [RequireRoles(RoleCheckMode.Any,"Admin")]
        [Description("Adds new form, type formName formId")]
        public async Task ConfigureFormsAdd(CommandContext ctx, string formName, string formId)
        {
            var f = File.ReadAllLines(@"C:\Tasks\FormsDiscordConfig.txt");
            bool IsHere = false;
            bool IsId = false; 
            foreach (var item in f)
            {
                string[] data = item.Split('|');
                if (data[0] == formName)
                {
                    IsHere = true;
                    break;
                }
            }
            var myList2 = File.ReadAllLines(@"C:\Tasks\FormsDiscordConfig.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split('|');
                if (formId == data[1])
                {
                    IsId = true;
                    break;
                }
            }
            if (IsHere == false || IsId == false)
            {
                
                using (StreamWriter sw = new StreamWriter(@"C:\Tasks\FormsDiscordConfig.txt", true, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync(formName+"|"+formId);
                    sw.Close();
                }
                await ctx.Channel.SendMessageAsync("OK!");
            }
            else{
                await ctx.Channel.SendMessageAsync("Error, such form or formId is already connected");
            }
           
        }
        [Command("removeform")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        [Description("Removes form, type formName formId")]
        public async Task ConfigureFormsRemove(CommandContext ctx, string formName, string formId)
        {
            var f = File.ReadAllLines(@"C:\Tasks\FormsDiscordConfig.txt");
            bool IsHere = false;
            bool IsId = false;
            foreach (var item in f)
            {
                if (item == formName)
                {
                    IsHere = true;
                    break;
                }
            }
            var myList2 = File.ReadAllLines(@"C:\Tasks\FormsDiscordConfig.txt");
            foreach (var item in myList2)
            {
                string[] data = item.Split('|');
                if (formId == data[1])
                {
                    IsId = true;
                    break;
                }
            }
            var myList = File.ReadAllLines(@"C:\Tasks\GuildDiscordId.txt");
            ulong GuildID = 0;
            foreach (var item in myList)
            {
                GuildID = Convert.ToUInt64(item);
                break;
            }
            var guild = await ctx.Client.GetGuildAsync(GuildID);
            await guild.GetRole(Convert.ToUInt64(formId)).DeleteAsync();
            if (IsHere ==true && IsId == true)
            {
                using (StreamWriter sw = new StreamWriter(@"C:\Tasks\FormsDiscordConfig.txt", false, System.Text.Encoding.Default))
                {
                    foreach (var item in myList2)
                    {
                        string[] data = item.Split("|");
                        if (data.Length > 1)
                        {
                            if (data[1] != formId)
                            {
                                await sw.WriteLineAsync(data[0] + "|" + data[1]);

                            }
                        }

                    }
                    sw.Close();
                }
                await ctx.Channel.SendMessageAsync("Ok!");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("There is no such forms");
            }
        }
        [Command("addnewschannel")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        [Description("Adds newschannel to group of channels,where news are being posted, type channelId")]
        public async Task ConfigureNewsChannelAdd(CommandContext ctx, string channelId)
        {   
            bool IsId = false;
            var myList2 = File.ReadAllLines(@"C:\Tasks\NewsChannelConfig.txt");
            foreach (var item in myList2)
            {
                
                if (channelId == item)
                {
                    IsId = true;
                    break;
                }
            }
            if (IsId == false)
            {

                using (StreamWriter sw = new StreamWriter(@"C:\Tasks\NewsChannelConfig.txt", true, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync(channelId);
                    sw.Close();
                }
                await ctx.Channel.SendMessageAsync("OK!");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Error, such channel is already connected");
            }
        }

        [Command("removenewschannel")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        [Description("Removes newschannel from group, where news are being posted, type channelId")]
        public async Task ConfigureNewsChannelDelete(CommandContext ctx, string channelId)
        {
            bool IsId = false;
            var myList2 = File.ReadAllLines(@"C:\Tasks\NewsChannelConfig.txt");
            foreach (var item in myList2)
            {

                if (channelId == item)
                {
                    IsId = true;
                    break;
                }
            }
            if (IsId == true)
            {

                using (StreamWriter sw = new StreamWriter(@"C:\Tasks\NewsChannelConfig.txt", false, System.Text.Encoding.Default))
                {
                    foreach (var item in myList2)
                    {
                       
                            if (item != channelId)
                            {
                                await sw.WriteLineAsync(item);

                            }
                        

                    }
                    sw.Close();
                }
                await ctx.Channel.SendMessageAsync("Ok!");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Error");
            }
        }
        [Command("addtaskschannel")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        [Description("Adds taskschannel to group of channels,where task alerts are being posted, type channelId")]
        public async Task ConfigureTasksChannelAdd( CommandContext ctx, string channelId)
        {
            bool IsId = false;
            var myList2 = File.ReadAllLines(@"C:\Tasks\TasksChannelConfig.txt");
            foreach (var item in myList2)
            {

                if (channelId == item)
                {
                    IsId = true;
                    break;
                }
            }
            if (IsId == false)
            {

                using (StreamWriter sw = new StreamWriter(@"C:\Tasks\TasksChannelConfig.txt", true, System.Text.Encoding.Default))
                {
                    await sw.WriteLineAsync(channelId);
                    sw.Close();
                }
                await ctx.Channel.SendMessageAsync("OK!");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Error, such channel is already connected");
            }
        }
        [Command("removetaskschannel")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        [Description("Removes taskschannel from group, where tasks alerts are being posted, type channelId")]
        public async Task ConfigureTasksChannelDelete(CommandContext ctx, string channelId)
        {
            bool IsId = false;
            var myList2 = File.ReadAllLines(@"C:\Tasks\TasksChannelConfig.txt");
            foreach (var item in myList2)
            {

                if (channelId == item)
                {
                    IsId = true;
                    break;
                }
            }
            if (IsId == true)
            {

                using (StreamWriter sw = new StreamWriter(@"C:\Tasks\TasksChannelConfig.txt", false, System.Text.Encoding.Default))
                {
                    foreach (var item in myList2)
                    {

                        if (item != channelId)
                        {
                            await sw.WriteLineAsync(item);

                        }


                    }
                    sw.Close();
                }
                await ctx.Channel.SendMessageAsync("Ok!");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Error");
            }
        }
        [Command("configureguildid")]
        [RequireRoles(RoleCheckMode.Any, "Admin")]
        [Description("Configures guild id")]
        public async Task ConfigureGuildId(CommandContext ctx, string guildId)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Tasks\GuildDiscordId.txt", false, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync(guildId); 
                sw.Close();
            }
            await ctx.Channel.SendMessageAsync("Done!");
        }
    }
}

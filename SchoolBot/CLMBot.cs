using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SchoolBot
{
    public class CLMBot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; } 
        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead(@"C:\CLM\CLM\SchoolBot\config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<Config>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                UseRelativeRatelimit = true
            };
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(2)
            }) ;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true,
                EnableDefaultHelp = true,
                IgnoreExtraArguments = false
                
            };
            

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<RoleManagementCommands>();
            await Client.ConnectAsync();

            await Task.Delay(-1);
            
        }
        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
        public async Task AddNew(DiscordClient client, string message)
        {
            var myList = File.ReadAllLines(@"C:\Tasks\NewsChannelConfig.txt");
            ulong ChannelID = 0;
            foreach (var item in myList)
            {
                ChannelID=Convert.ToUInt64(item);
                var msg = await new DiscordMessageBuilder()
       .WithContent(message).SendAsync(await client.GetChannelAsync(ChannelID)); ;
            }
            
        }
        public async Task ChangeNew(DiscordClient client, string message,string newmessage)
        {
            var myList = File.ReadAllLines(@"C:\Tasks\NewsChannelConfig.txt");
            ulong ChannelID = 0;
            foreach (var item in myList)
            {
                ChannelID = Convert.ToUInt64(item);
                var channel = await client.GetChannelAsync(ChannelID);
                var messages = await channel.GetMessagesAsync();

                foreach (var item2 in messages)
                {
                    if (item2.Content == message)
                    {
                        await item2.ModifyAsync(newmessage);

                    }
                }
            }
            
        }
        public async Task RemoveNew(DiscordClient client, string message)
        {
            var myList = File.ReadAllLines(@"C:\Tasks\NewsChannelConfig.txt");
            ulong ChannelID = 0;
            foreach (var item in myList)
            {
                ChannelID = Convert.ToUInt64(item);
                var channel = await client.GetChannelAsync(ChannelID);

                var messages = await channel.GetMessagesAsync();

                foreach (var item2 in messages)
                {
                    if (item2.Content == message)
                    {
                        await channel.DeleteMessageAsync(item2);

                    }
                }
            }
            
        }
        public async Task AddForm(DiscordClient client, string formName)
        {
            var myList = File.ReadAllLines(@"C:\Tasks\GuildDiscordId.txt");
            ulong GuildID = 0;
            foreach (var item in myList)
            {
                GuildID = Convert.ToUInt64(item);
                break;
            }
            var guild = await client.GetGuildAsync(GuildID);
            await guild.CreateRoleAsync(formName, null, DiscordColor.White, null);
            
        }
      
       
        public async Task TaskAlert(DiscordClient client, string formString)
        {
            var myList = File.ReadAllLines(@"C:\Tasks\TasksChannelConfig.txt");
            ulong ChannelID = 0;
            var f = File.ReadAllLines(@"C:\Tasks\FormsDiscordConfig.txt");
            ulong roleId = new ulong();
            foreach (var item in f)
            {
                string[] data = item.Split('|');
                if (data[0] == formString)
                {
                    roleId = Convert.ToUInt64(data[1]);
                }
            }
            var myList2 = File.ReadAllLines(@"C:\Tasks\GuildDiscordId.txt");
            ulong GuildID = 0;
            foreach (var item in myList2)
            {
                GuildID = Convert.ToUInt64(item);
                break;
            }
            var guild = await client.GetGuildAsync(GuildID);
            foreach (var item in myList)
            {
                
                var role = guild.GetRole(roleId);
                ChannelID = Convert.ToUInt64(item);
                var msg = await new DiscordMessageBuilder()
       .WithContent(( role.Mention)+ ", here is a new homework for you. Try to do it as soon as possible! Good luck!").WithAllowedMentions(new IMention[] { new RoleMention(role) })
.SendAsync(await client.GetChannelAsync(ChannelID)); ;
            }
        }
        
    }
}

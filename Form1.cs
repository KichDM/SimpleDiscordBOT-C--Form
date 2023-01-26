using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DiscordBOTForm
{
    public partial class Form1 : Form
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        public Form1()
        {
            _client = new DiscordSocketClient();
            _commandService = new CommandService();
            _serviceProvider = new ServiceCollection()
                .AddSingleton<IDiscordClient>(_client)
                .AddSingleton(_commandService)
                .BuildServiceProvider();

            _client.MessageReceived += ClientOnMessageReceived;
            _client.Log += _client_Log;

            _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider).GetAwaiter().GetResult();
            InitializeComponent();
        }
        private DiscordSocketClient Get_client()
        {
            return _client;
        }
        private Task _client_Log(LogMessage arg)
        {
            Invoke(new Action(() => ConsoleOutput.Text += arg.ToString() + Environment.NewLine));
            return Task.CompletedTask;
        }

        private async Task ClientOnMessageReceived(SocketMessage arg)
        {
            var text = arg.Content;

            if (text == "Hey")
            {
                await arg.Channel.SendMessageAsync("Heyy :D");
            }
            var message = arg as SocketUserMessage;
            if (message.Author.IsBot) return;
            var argPos = 0;
            if (!message.HasStringPrefix("!", ref argPos)) return;

            var context = new SocketCommandContext(_client, message);
            await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            if (string.IsNullOrEmpty(TokenBox.Text))
            {
                MessageBox.Show("Write the token");
                ConnectButton.Enabled = true;
                return;
            }

            await _client.LoginAsync(TokenType.Bot, TokenBox.Text);
            await _client.StartAsync();

            await _client.SetGameAsync("KichDM#6035 | Owner", null, ActivityType.Streaming);
        }

        private async void DisconnectButton_Click(object sender, EventArgs e)
        {
            await _client.StopAsync();
            ConnectButton.Enabled = true;
        }
    }
}

using BookShelfBot.Models;
using BookShelfBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace BookShelfBot
{
    class Program
    {
        private static readonly TelegramBotClient TelegramBotEndpoint = 
            new TelegramBotClient("");
        private static readonly UserManagementService _userManagementService = new UserManagementService();
        private static readonly BookManagementService _bookManagementService = new BookManagementService();

        static void Main(string[] args)
        {
            var me = TelegramBotEndpoint.GetMeAsync().Result;
            Console.Title = me.Username;

            TelegramBotEndpoint.OnMessage += BotOnMessageReceived;

            TelegramBotEndpoint.StartReceiving(Array.Empty<UpdateType>());


            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            TelegramBotEndpoint.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text) return;

            var userFrom = message.From;

            switch (message.Text.Split(' ').First())
            {
                case "/start":
                    
                    var loadUser = _userManagementService.CheckUserRegistration(userFrom.Id.ToString());

                    if (loadUser == null)
                    {
                        UserRegistrationModel model = new UserRegistrationModel()
                        {
                            ChatId = userFrom.Id.ToString(),
                            UserName = userFrom.Username
                        };

                        var registeredUser = _userManagementService.SignUpApplicationUser(model);

                        await TelegramBotEndpoint.SendTextMessageAsync(
                        message.Chat.Id,
                        $"Hello {registeredUser.UserName}, you are signed up!");

                        break;
                    }
                    else
                    {
                        await TelegramBotEndpoint.SendTextMessageAsync(
                        message.Chat.Id,
                        $"Hello {loadUser.UserName}, welcome back!");
                        break;
                    }
                case "/getBook":

                    if (_bookManagementService.CheckQuantityGet(userFrom.Id))
                    {
                        byte[] bytes = _bookManagementService.GetBook();
                       
                        await TelegramBotEndpoint
                            .SendDocumentAsync(
                                new ChatId(messageEventArgs.Message.Chat.Id),
                                new InputOnlineFile(new MemoryStream(bytes), "Book"));
                        _bookManagementService.AddBookRequestRecord(userFrom.Id);
                        
                    }
                    else
                    {
                        await TelegramBotEndpoint
                            .SendTextMessageAsync(
                                new ChatId(messageEventArgs.Message.Chat.Id),
                                $"You've already got 3 books today, try tomorrow");
                    }

                    break;                    
            }
        }

    }
}

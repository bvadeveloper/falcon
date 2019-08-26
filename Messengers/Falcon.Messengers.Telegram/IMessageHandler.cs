using Telegram.Bot;
using Telegram.Bot.Args;

namespace Falcon.Messengers.Telegram
{
    public interface IMessageHandler
    {
        void Subscribe(ITelegramBotClient botClient);

        void Message(object sender, MessageEventArgs e);

        void Error(object sender, ReceiveErrorEventArgs e);

        void Edit(object sender, MessageEventArgs e);

        void Callback(object sender, CallbackQueryEventArgs e);

        void Inline(object sender, InlineQueryEventArgs e);

        void InlineResult(object sender, ChosenInlineResultEventArgs e);

        void Update(object sender, UpdateEventArgs e);
    }
}
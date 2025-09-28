using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Trainer.SuperTux.Messages
{
    public class ApplicationLoadedMessage(ApplicationLoadedMessageParams applicationLoadedMessageParams) : ValueChangedMessage<ApplicationLoadedMessageParams>(applicationLoadedMessageParams)
    {
    }

    public class ApplicationLoadedMessageParams
    {
    }
}
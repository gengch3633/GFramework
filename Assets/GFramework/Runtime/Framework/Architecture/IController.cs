namespace Framework
{
    public interface IController : IBelongToArchitecture, ICanGetSystem, ICanGetModel, ICanSendCommand, ICanRegisterEvent
    {
    }
}
using Framework;

namespace GameFramework
{
    public interface IGameModel : IModel
    {

    }

    public class GameModel : AbstractModel, IGameModel
    {
        protected override void OnInit()
        {
        }
    }

}
using Framework;

namespace GameFramework
{
    public interface IGameModel : IModel
    {

    }

    public partial class GameModel : AbstractModel, IGameModel
    {
        protected override void OnInit()
        {
        }
    }

}
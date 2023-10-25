using Cysharp.Threading.Tasks;
using System;

namespace GameFramework
{
    public class GameUtils
    {
        public static async UniTask WaitForFrameDo(int frameCount, Action action)
        {
            await UniTask.DelayFrame(frameCount);
            action.Invoke();
        }
        public static async UniTask WaitForSecondsDo(float seconds, Action action)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            action.Invoke();
        }
    }
}
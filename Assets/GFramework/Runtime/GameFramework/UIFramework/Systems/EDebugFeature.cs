namespace GameFramework
{
    public enum EDebugFeature
    {
        SlowDownPlayCardAnim,
        TestTutorial3,
        TimeScaleDown,         //调试动画，动画减速
        UseHandCardDataForTest,         //使用本地数据初始化手牌
        ForceRoundFinishCardCount_30,//将当前局的平局的剩余纸牌数量设置为30
        OpenBackDoor,       //后门开启
        WatchRivalCard,     //看对手牌
        TrumpGameBotInfo,   //显示机器人信息
        NoAdsEditor         //编辑器模式下的无广告模式
    }
}


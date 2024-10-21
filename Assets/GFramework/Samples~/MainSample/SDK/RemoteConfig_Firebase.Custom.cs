using System.Collections.Generic;
using GameFramework;
using System.Linq;
using Newtonsoft.Json;

public partial class RemoteConfig_Firebase : ITypeLog
{
    private void InitConfigValues()
    {

        LogRemoteConfigs("InitConfigValues");
    }

    private void  LogRemoteConfigs(string tag)
    {
        var remoteList = new List<IRemoteGetValue>();
        remoteList.Add(remoteTestBool);
        remoteList.Add(remoteTestLong);
        remoteList.Add(remoteTestDouble);
        remoteList.Add(remoteTestString);

        var remoteValueDict = remoteList.ToDictionary(item => item.GetKey(), item => item.GetValue());
        GameUtils.Log(this, $"{tag}, remoteValueDict: {JsonConvert.SerializeObject(remoteValueDict)}");
    }

    private void SetConfigValues()
    {

        LogRemoteConfigs("SetConfigValues");
    }
}

**GFramework**是一套用于Unity3D的游戏框架。
**发布GFramework UPM包**
1. git subtree split --prefix=Assets/GFramework --branch upm
2. git tag 1.0.0 upm
3. git push origin upm --tags

**安装GFramework包**
1.  安装GFramework包 https://github.com/gengch3633/GFramework.git?path=Assets/GFramework
2.  安装UniTask包 https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
3.  安装UnityIngameDebugConsole包 https://github.com/yasirkula/UnityIngameDebugConsole.git
4.  使用Tools/UpdateScriptDefineSymbles指令更新自定义宏定义，配置文件在ScriptDefineInfo.xls中
5.  安装YooAssets包1.4.2-preview
    打开管理界面 Edit/Project Settings/Package Manager
    // 输入以下内容（中国版）
    Name: package.openupm.cn
    URL: https://package.openupm.cn
    Scope(s): com.tuyoogame.yooasset
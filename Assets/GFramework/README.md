**GFramework**是一套用于Unity3D的游戏框架。
**发布GFramework UPM包**
1. git subtree split --prefix=Assets/GFramework --branch upm
2. git tag 1.0.0 upm
3. git push origin upm --tags

**安装GFramework包**
1. 添加Package包 https://github.com/gengch3633/GFramework.git?path=Assets/GFramework
2. 添加Package包 https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask,
   添加 UNITASK_DOTWEEN_SUPPORT 到 Scripting Define Symbols 
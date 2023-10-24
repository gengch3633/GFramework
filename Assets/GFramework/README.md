**YooAsset**是一套用于Unity3D的资源管理系统，用于帮助研发团队快速部署和交付游戏。

它可以满足商业化游戏的各类需求，并且经历多款百万DAU游戏产品的验证。

更多信息请了解官方主页：https://github.com/tuyoogame/YooAsset

**发布UPM包**
1. git subtree split --prefix=Assets/GFramework --branch upm
2. git tag 1.0.0 upm
3. git push origin upm --tags

**版本更新**
**package.json 更新版本信息**

4. git subtree split
5. git tag 1.0.1 upm
6. git push origin upm --tags

通过Git URL 添加包
打开Package Manager Window (menu: Window > Package Manager), 选择 "Add package from git URL...", 在文本框中输入以下链接: https://github.com/gengch3633/GFramework.git#1.0.0
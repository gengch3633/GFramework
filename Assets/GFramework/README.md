**GFramework**是一套用于Unity3D的游戏框架。
**发布GFramework UPM包**
1. git subtree split --prefix=Assets/GFramework --branch upm
2. git tag 1.0.0 upm
3. git push origin upm --tags

**安装GFramework包**
1. 打开Package Manager Window (menu: Window > Package Manager)
2. 选择 "Add package from git URL...", 在文本框中输入以下链接: https://github.com/gengch3633/GFramework.git#1.0.0
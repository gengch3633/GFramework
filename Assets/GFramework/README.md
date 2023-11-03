**GFramework**是一套用于Unity3D的游戏框架。
**安装YooAssets包**
1.  打开管理界面 Edit/Project Settings/Package Manager
    // 输入以下内容（国际版）
    Name: package.openupm.com
    URL: https://package.openupm.com
    Scope(s): com.tuyoogame.yooasset

2. 打开管理界面 Edit/Windows/Package Manager
    在My Registries中安装YooAssets v1.5.1版本

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
# 3D塔防游戏

基于Unity引擎开发的3D塔防策略游戏，小组项目，负责战斗系统与UI交互模块。

## 项目介绍
玩家通过在地图上布置不同类型的防御塔，阻止敌人沿路径进攻基地。包含完整的战斗系统、经济系统与游戏流程。

## 技术栈
- Unity 3D
- C#
- 单例模式
- 协程
- 射线检测

## 负责模块
战斗与索敌系统：使用Physics.OverlapSphere物理检测和向量距离计算，自动索敌并计算出炮塔的转向目标，实现基础的锁定攻击逻辑
交互与资源管理：配合 UI 交互模块，处理鼠标左键点击放置/右键拆除炮塔的逻辑。通过 C# 静态变量及基础单例模式管理全局金币、血量数据，实现界面数值同步更新
多状态UI切换：搭建并管理了游戏运行时的主界面、暂停界面和游戏结束界面，实现了UI面板之间状态的正常过渡

## 演示视频
【3d塔防游戏】https://www.bilibili.com/video/BV1WKKz6PEex?vd_source=1edea17dae3abc0d0e87752abb5951d1

## 运行截图
<img width="1820" height="1055" alt="屏幕截图 2026-07-19 194511" src="https://github.com/user-attachments/assets/e14c9aa9-5446-40a1-83c9-1dcd8dd0a66f" />
<img width="1813" height="1063" alt="屏幕截图 2026-07-19 194519" src="https://github.com/user-attachments/assets/6ee5fac8-cc34-4343-889b-ca97359f009a" />
<img width="1820" height="1057" alt="屏幕截图 2026-07-19 194631" src="https://github.com/user-attachments/assets/97364638-d4a1-449b-af32-ffa0cc5f6483" />
<img width="1810" height="1057" alt="屏幕截图 2026-07-19 194642" src="https://github.com/user-attachments/assets/c242e6a6-b5d5-4659-8fdb-bd0a350317dd" />


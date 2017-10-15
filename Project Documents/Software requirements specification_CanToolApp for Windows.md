# CanToolApp for Windows 软件需求说明书

## 引言

### 编写目的
编写本软件需求说明书的目的是为了使本开发小组对本次将要开发的项目的需求达成共识，并为项目的开发和验收提供依据，使开发出来的软件能够更好的达到用户的需求，同时本文档亦作为软件概要设计的参考，为项目中的开发人员明确本项目的需求，包括功能需求和非功能需求说明。

### 背景
- 待开发的软件系统名称：CanToolApp for Windows
- 本项目的任务提出：《现代软件工程》课程任务
- 本项目的开发者：张久武 么红帅 郭思莹 曲晗东
- 本项目的最终用户：现代汽车使用者
- 该软件系统与其他系统的关系：本软件系统与CanTool装置进行通信；与测试小组一起完成软件系统的测试。

### 定义
1. CAN bus：是一种强大的车辆总线标准，旨在允许允许微控制器和设备在没有主机的应用中相互通信。
2. ECU：全称为Electronic Control Unit. A control device used in vehicle.
3. CanTool装置：用于CAN bus的CAN信息采集与发送的装置。
4. CAN message：CAN message 由CAN id，dlc，data构成。
5. CAN signal：Can Signal是分布在CANmessage中的CAN信号，具有一定物理意义。
6. CAN信息和信号数据库：用于存储CAN信息的组成信息和CAN信号的相关参数设置。
7. Little Endian/Big Endian：数据在存储空间中的保存的方式。
8. WebAPI：是用于Web服务器或Web浏览器的应用程序编程接口。

### 参考资料
《构建之法——现代软件工程》（邹欣 著）

## 任务概述

### 目标
本次开发的软件系统安装在上位机上的CanToolApp for Windows，这是CanTool系统的总体的一部分，用于上位机与CanTool装置进行通信，并完成CAN信息、信号的显示与设定，CanTool系统的总体框架图如图所示。
![CanTool系统框图](http://img.blog.csdn.net/20171015203100120?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### 用户的特点
本软件系统的最终用户是现代汽车的使用者，是大众普通人群，没有专业的计算机能力。因此要保证软件有良好的交互界面。

### 假定和约束
本软件开发工作的开发期限：截止到现代软件工程课程结束。

## 需求规定

### 对功能的规定
1. 能够搜索到本机所有可使用的COM口，并在弹出式ComboBox中以列表方式让用户选择CanTool装置在上位机中映射的COM口。

2. 能够设置CANtool装置的，包括CAN速率、CAN工作状态（open）、CAN初始化状态（close）(设定内容可保存到CanToolApp设定文件中)。

3. 能够同时接收到多个CAN信息

   (1)通过对CAN信息及CAN信号数据库解析，能够实现将CAN信息原始物理值实时显示在GUI界面中。

   (2)显示方式是可选择的，对于某个特定CAN信息可以选择仪表盘方式得到瞬时物理值信息，也可以通过实时物理值曲线查看特定CAN信息的物理值变化情况。

   (3)提供CAN信息保存功能，格式可以使CSV或者自定义。

4. 能够指定要发送的多个CAN信息

   (1)允许用户设定CAN信息中的CAN信号物理值

   (2)提供CAN信号的物理值转换成信号值的功能，发送给CanTool装置。

5. 实现加载用户提供的CAN信息和信号数据库

   (1)可以显示CAN信号在CAN信息的布局，可保存到CanToolApp设定文件。

   (2)可以树状结构显示在GUI界面中

   (3)可以另存为xml和JSON (JavaScript Object Notation)格式，也可以已将xml或Json格式的数据库，转换为CAN信息和信号数据库格式。

### 对性能的规定
1. 可用性
- 桌面用户界面应符合Windows 10标准。

2. 可靠性
- 系统应每天24小时可用,停机时间不超过10%。
- 在任何给定的时间，WebAPI系统最多支持20个并发用户对中央数据库，最多可同时支持多达50个并发用户与本地服务器。
- 上位机处理串口数据时，不应丢失CanTool装置的传送的数据。

3.时间特性
- Web API系统必须能够在2分钟内完成上载CAN数据。
- CanTool装置采集的CAN数据以最小50毫秒的间隔发送给上位机。

3. 支持性
- 开发的全过程应有文档支持。
- 源代码必须由git / github管理。

4. 安全性
- WebAPI系统必须防止任何未经授权的人员更改任何数据。
- WebAPI系统，只允许注册商更改任何信息。


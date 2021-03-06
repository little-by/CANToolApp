# CanToolApp for Windows 软件需求说明书

## 目录

  * [1. 引言](#1-引言)
    * [1.1 目的](#11-目的)
    * [1.2 背景](#12-背景)
    * [1.3 定义](#13-定义)
    * [1.4 参考文献](#14-参考文献)
  * [2. 项目概述](#2-项目概述)
    * [2.1 产品背景](#21-产品背景)
    * [2.2 产品描述](#22-产品描述)
    * [2.3 产品主要功能](#23-产品主要功能)
    * [2.4 用户特点](#24-用户特点)
    * [2.5 假设与约束](#25-假设与约束)
      * [2.5.1 假设](#251-假设)
      * [2.5.2 约束](#252-约束)
  * [3. 具体需求](#3-具体需求)
    * [3.1 功能需求](#31-功能需求)
      * [3.1.1 用例图](#311-用例图)
      * [3.1.2 典型场景描述](#312-典型场景描述)
      * [3.1.3 类图](#313-类图)
    * [3.2 性能需求](#32-性能需求)
      * [3.2.1 可用性](#321-可用性)
      * [3.2.2 可靠性](#322-可靠性)
      * [3.2.3 时间特性](#323-时间特性)
      * [3.2.4 支持性](#324-支持性)
      * [3.2.5 安全性](#325-安全性)
  * [4. 验收标准](#4-验收标准)
    * [4.1 文档验收标准](#41-文档验收标准)
    * [4.2 软件验收标准](#42-软件验收标准)
    * [4.3 界面验收标准](#43-界面验收标准)
    * [4.4 功能验收标准](#44-功能验收标准)


## 1. 引言

### 1.1 目的

编写本软件需求说明书的目的是为了使本开发小组对本次将要开发的项目的需求达成共识，并为项目的开发和验收提供依据，并依据此文档安排项目规划与进度，使开发出来的软件能够更好的达到用户的需求，同时本文档亦作为软件概要设计和测试的参考，为项目中的开发人员明确本项目的需求，包括功能需求和非功能需求说明。

### 1.2 背景

- 待开发的软件系统名称：CanToolApp for Windows
- 本项目的任务提出：《现代软件工程》课程任务
- 本项目的开发者：张久武 么红帅 郭思莹 曲晗东
- 本项目的最终用户：现代汽车使用者
- 该软件系统与其他系统的关系：本软件系统与CanTool装置进行通信；与测试小组一起完成软件系统的测试。

### 1.3 定义

1. CAN bus：是一种强大的车辆总线标准，旨在允许允许微控制器和设备在没有主机的应用中相互通信。
2. ECU：全称为Electronic Control Unit. A control device used in vehicle.
3. CanTool装置：用于CAN bus的CAN信息采集与发送的装置。
4. CAN message：CAN message 由CAN id，dlc，data构成。
5. CAN signal：Can Signal是分布在CANmessage中的CAN信号，具有一定物理意义。
6. CAN信息和信号数据库：用于存储CAN信息的组成信息和CAN信号的相关参数设置。
7. Little Endian/Big Endian：数据在存储空间中的保存的方式。
8. WebAPI：是用于Web服务器或Web浏览器的应用程序编程接口。

### 1.4 参考文献

- 《构建之法——现代软件工程》（邹欣 著  人民邮电出版社） 
- 《GB8567-2006计算机软件文档编制规范》

## 2. 项目概述

### 2.1 产品背景

在现代汽车控制技术中，汽车会使用多个电子控制装置（ECU）来对整车进行控制。ECU之间的信息交换依赖于CAN bus来完成，CanTool装置完成Can bus上的信息内容的检测和控制。为了实现CAN bus上的数据的显示和控制，需要使用我们开发的CanToolApp来和CanTool装置通信。

### 2.2 产品描述

本次开发的软件系统安装在上位机上的CanToolApp for Windows，这是CanTool系统的总体的一部分，用于上位机与CanTool装置进行通信，并完成CAN信息、信号的显示与设定，CanTool系统的总体框架图如图所示。
![CanTool系统框图](http://img.blog.csdn.net/20171015203100120?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### 2.3 产品主要功能

本产品主要有以下功能：
- 接收和解析CanTool装置发来的CAN信息。
- 保存CAN信息，支持多种格式保存。
- 封装和发送CAN信息给CanTool装置。

### 2.4 用户特点

本软件系统的最终用户是现代汽车的使用者，是大众普通人群，没有专业的计算机能力。因此要保证软件有良好的交互界面。

### 2.5 假设与约束

本软件开发工作的开发期限：截止到现代软件工程课程结束。

#### 2.5.1 假设

- 用户支持：假设本产品开发的各个环节中得到用户的有效支持和积极配合
- 技术支持：假设开发初期，项目设计合理，小组成员充分认识本产品的需求，认真学习相关知识。开发过程中遇到的技术问题可以及时得到老师的指导与帮助。开发后期，团队熟练掌握适用于该项目的技术，充分优化系统性能。
- 人员配合：假设团队成员的积极合作配合，项目开发过程中不会有突发情况导致项目成员无法正常参与开发工作。
- 小组配合：假设能够和测试小组相互合作配合，完成产品的测试。
- 时间限定：假设项目截止日期不会提前。
- 需求限定：假设项目需求确定后不会有太大改动。

#### 2.5.2 约束

- 人员约束：小组成员皆为软件工程专业研一学生，共 4人。
- 时间约束：本项的开发时间为8周，时间比较紧张，需要开发者合理规划时间。
- 管理约束：本次开发实行以一人担任组长，分工合作的模式进行。开发过程中遇到的问题通过小组会议讨论的得到解决。
- 技术约束：小组成员在相关技术水平方面存在一定的欠缺，缺乏相关的项目经验，需要在开发中并发学习多种技术和能力；在文档编写能力方面也有待提升。
- 其他约束：开发期间，小组成员还有其他科目的学习任务，将对项目进度造成一定影响。

## 3. 具体需求

### 3.1 功能需求

#### 3.1.1 用例图

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
   
系统的用例图如图所示。
![用例图](http://img.blog.csdn.net/20171017223941928?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

#### 3.1.2 典型场景描述

1.设置COM信息

| 用例名称   | 设置COM信息                                         |
| ----- | ---------------------------------------- |
| 描述  | 用户点击设置COM口，进入设置页面，选择设置可搜索到的想要使用的COM口       |
| 主要参与者  | 用户       |
| 次要参与者  | 系统       |
| 情景目标  | 可以设置可搜索到的想使用的COM的信息       |
| 前提条件  | 必须能够正常点击进入设置COM口页面       |
| 启动条件  | 可以搜索到本机可以使用的所有COM口       |
| 场景  | 1.用户点击进入设置COM口页面 2.系统搜索本机所有可使用的COM口 3.用户选择自己想使用的COM口 4.用户可以打开COM口 5.用户设置COM口的Baud Rate、Data Bits、Stop Bits和Parity       |
| 异常  | 1.不能正常进入设置COM口界面 2.搜索不到本机所有的COM口 3.不能正常打开COM口 4.设置不了COM信息       |

2.接收CAN信息

| 用例名称   | 接收CAN信息                                         |
| ----- | ---------------------------------------- |
| 描述  | 用户打开设置好的COM口来接收CAN信息，并且系统将解析并呈现接收到的CAN信息    |
| 主要参与者  | 用户       |
| 次要参与者  | 系统       |
| 情景目标  | 用户能够正常接收到CAN信息，并且系统能够正常解析CAN信息并呈现在主页面       |
| 前提条件  | 1.用户设置好COM口信息 2.必须能正常解析CAN信息       |
| 启动条件  | 用户打开COM口，选择速率，来接收CAN信息       |
| 场景  | 1.用户打开COM口 2.用户选择速率 3.收到CAN信息 4.系统解析CAN信息 5.将解析好的CAN信息呈现在主页面       |
| 异常  | 1.用户不能打开COM口 2.用户不能正常从COM口接收到数据 3.系统不能正确解释CAN信息 4.系统不能正常呈现解析后的CAN信息       |

3.发送CAN信息

| 用例名称   | 发送CAN信息                                         |
| ----- | ---------------------------------------- |
| 描述  | 用户选择CAN信息，并输入物理值和发送周期       |
| 主要参与者  | 用户       |
| 次要参与者  | 系统       |
| 情景目标  | 用户输入信息后可以正常发送给CanTool装置       |
| 前提条件  | 1.用户设置好COM口信息 2.有用户可以选择发送的Message和Signal       |
| 启动条件  | 用户点击发送按钮       |
| 场景  | 1.用户设置COM口信息 2.系统列出可选择的Messsage和Signal 3.用户选择Message和Signal，并输入物理值和发送周期 4.用户点击发送按钮 5.系统封装CAN信息 6.将CAN信息通过COM口发送给CanTool装置       |
| 异常  | 1.用户不能打开COM口 2.系统不能正常封装CAN信息 3.用户没有输入合理的物理值和发送周期      |

4.选择信号显示方式

| 用例名称   |  选择信号显示方式                                        |
| ----- | ---------------------------------------- |
| 描述  | 用户可以选择折线图或仪表盘显示CAN信息       |
| 主要参与者  | 用户       |
| 次要参与者  | 系统       |
| 情景目标  | 用户选择折线图或仪表盘来显示接收到的解析好的CAN信号值       |
| 前提条件  | 系统解析好并呈现出CAN信号       |
| 启动条件  | 用户选择显示方式       |
| 场景  | 1.系统解析好并呈现CAN信号 2.用户选择想要显示的CAN信号 3.用户选择显示方式 3.如果选择折线图，折线图能实时显示CAN信号值 4.如果选择仪表盘，仪表盘能正常显示CAN信号值       |
| 异常  | 1.系统没能解析好CAN信号 2.CAN信号没能正常呈现 3.折线图没能正常显示 4.仪表盘没能正常显示        |

#### 3.1.3 类图

系统的类图如图所示。
![类图](http://img.blog.csdn.net/20171025202958086?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)


### 3.2 性能需求

#### 3.2.1 可用性

- 桌面用户界面应符合Windows 10标准。
- 系统应具有一定的容错能力。
- 应考虑到实际用户的特点，要有良好的交互性。

#### 3.2.2 可靠性

- 系统应每天24小时可用,停机时间不超过10%。
- 在任何给定的时间，WebAPI系统最多支持20个并发用户对中央数据库，最多可同时支持多达50个并发用户与本地服务器。
- 上位机处理串口数据时，不应丢失CanTool装置的传送的数据。

#### 3.2.3 时间特性

- Web API系统必须能够在2分钟内完成上载CAN数据。
- CanTool装置采集的CAN数据以最小50毫秒的间隔发送给上位机。

#### 3.2.4 支持性

- 开发的全过程应有符合标准的文档支持。
- 源代码必须由git / github管理。

#### 3.2.5 安全性

- WebAPI系统必须防止任何未经授权的人员更改任何数据。
- WebAPI系统，只允许注册商更改任何信息。
- 数据存储在服务器端，定期备份，确保数据不丢失。

## 4 验收标准

### 4.1 文档验收标准

文档符合计算机软件文档编制规范。
包括：
- 软件需求规格说明书
- 软件设计说明书
- 软件测试报告

### 4.2 软件验收标准

软件一切功能正常，不卡顿，不闪退，在符合标准的windows系统上都可正常使用。

### 4.3 界面验收标准

界面运行流畅，不卡顿，不闪退，界面间正常切换。
用户界面：

| 序号   | 界面名称     |                   界面描述                   |
| ---- | -------- | -------------------------------------- |
| 1    | 首页页面     | 最上方有菜单栏，包括“文件”和设置，还有CAN信息输入框，输入框右侧有发送按钮。中间部分显示CANmessage及它对应的一系列CANsignal，页面最下方有显示变化曲线按钮和仪表盘显示按钮 |
| 2    | 设置COM口页面     | 左侧为COM口设置，用户可以下拉选择搜索到的COM口，右侧有Open按钮和Save按钮；下方用户可以下拉选择Baud Rate、Data Bits、Stop Bits和Parity；在最下方用户可以输入数据，包括message、signal、physical value和send Cycle。最后有个send按钮。右侧为CanTool设置，包括Open按钮、close按钮和version按钮，在下方用户可以选择速率并发送。最下方有个接收框，最后有clear按钮和exit按钮 |
| 3    | 显示变化曲线页面     | 左侧显示接收到的信号，右侧显示选择的信号的值的变化曲线 |
| 4    | 仪表盘显示页面     | 页面中间有个仪表盘，下方有个Textbox显示具体值，最下方有个change needle types按钮 |
| 5    | 保存成功页面     | 页面中间显示四个字“保存成功”，页面下方有个确认按钮 |

### 4.4 功能验收标准

按照需求四象限图进行验收，如图所示。
![四象限分析](http://images2017.cnblogs.com/blog/1237479/201710/1237479-20171011223456012-1721827159.png)

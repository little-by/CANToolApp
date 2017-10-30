# CanToolApp for Windows 软件设计说明书

## 引言

### 编写目的
编写本软件设计说明书的目的是根据项目的软件需求说明书，具体设计如何开发本次项目，并为项目的测试和验收提供依据。本文档可供项目设计开发人员及测试人员参考。

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
- 《构建之法——现代软件工程》（邹欣 著  人民邮电出版社）
- 《GB8567-2006计算机软件文档编制规范》


## 运行环境

### 硬件环境
1. 安装CanToolApp for Windows的主机硬件至少满足以下要求：
- CPU：主频不低于 2.0GHz
- 内存：不低于 1G
- 硬盘：不低于 20G
2.与CanToolApp通信的CanTool装置
使用老师提供的即可。

### 软件环境
操作系统：Win7及以上

## 程序系统的结构

### 功能结构图

本软件的功能结构如图所示。
![功能结构图](http://img.blog.csdn.net/20171017224004011?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
   

## 用户设置COM口信息模块设计说明

### 程序功能描述
根据本机搜索到的COM口，从中选择用户想要使用的COM口，并且可以设置置COM口的Baud Rate、Data Bits、Stop Bits和Parity等信息。

### 输入项
- 搜索到的所有COM口：以下拉列表的方式显示
- Baud Rate，以下拉列表的方式显示
- Date Bits：以下拉列表的方式显示
- Stop Bits：以下拉列表的方式显示
- Parity：以下拉列表的方式显示
- 打开COM口：以按钮方式显示

### 输出项
- 点击Open按钮后，如果打开成功，出现“Success”弹窗；如果失败，出现“Fail”弹窗。

### 流程
该模块的活动图如图所示。
![设置COM口](http://img.blog.csdn.net/20171029201142154?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)


## 用户接收CAN信息模块设计说明

### 程序功能描述
用户打开设置好的COM口来接收CAN信息，并且系统将解析并以树状形式呈现接收到的CAN信息。

### 输入项
- 用户设置好COM口
- 用户打开Cantool
- Rate：以下拉列表的方式显示
- 发送设置信息，以按钮的方式显示
- CanTool发送的信息

### 输出项
- 解析好的Can信息

### 流程
该模块的活动图如图所示。
![接收CAN信息](http://img.blog.csdn.net/20171029201045157?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

## 用户发送CAN信息模块设计说明

### 程序功能描述
用户选择CAN信息，并输入物理值和发送周期，系统可以将这些信息封装起来发送给CanTool装置。

### 输入项
- 用户选择Can信息
- 输入物理值，以文本框的方式显示
- 输入发送周期，以文本框的方式显示，这是可选字段
- 发送，以按钮的方式显示

### 输出项
- 点击Send按钮后，如果打开成功，出现“Success”弹窗；如果失败，出现“Fail”弹窗。

### 流程
该模块的活动图如图所示。
![发送CAN信息](http://img.blog.csdn.net/20171029201026013?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

## 用户选择信号显示方式模块设计说明

### 程序功能描述
用户可以选择折线图或LED或仪表盘显示CAN信息。

### 输入项
- 用户选择信号
- 选择显示方式

### 输出项
- 显示出信号值

### 流程
该模块的活动图如图所示。
![选择信号显示方式](http://img.blog.csdn.net/20171029201105378?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

## 数据库设计
数据库的ER图如图所示。
![ER图](http://img.blog.csdn.net/20171029201340575?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

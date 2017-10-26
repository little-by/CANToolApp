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
《构建之法——现代软件工程》（邹欣 著  人民邮电出版社）

## 基本设计概念和处理流程

## 运行环境

## 程序系统的结构
### 功能结构图

本软件的功能结构如图所示。
![功能结构图](http://img.blog.csdn.net/20171017224004011?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvR3N5U3Vuc2hpbmU=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

### 类图

## 数据库设计
### 数据库逻辑设计
### 数据库物理设计

## 接口设计

## 系统出错处理设计

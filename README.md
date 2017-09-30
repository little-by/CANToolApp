# CANToolApp
A CANToolApp developed by team25 from TJU

## Development Environment Introduction
  This is a VisualStudio 2015 project, the language we use is C#.
## Schedule
  *2017/9/27：Create the basic project structure and design some simple GUI.*
## CANToolApp需求文档
<ul>
1.能够<strong>搜索</strong>到本机所有<strong>可使用的COM口</strong>，并在弹出式ComboBox中以列表方式让用户选择CanTool装置在上位机中映射的COM口。<br/><br/>
2.能够<strong>设置CANtool装置</strong>的，包括CAN速率、CAN工作状态（open）、CAN初始化状态（close）(设定内容可保存到CanToolApp设定文件中)。<br/><br/>
3.能够同时<strong>接收到多个CAN信息</strong><br/><br/>
<ul>
(1)通过对CAN信息及CAN信号数据库解析，能够实现将CAN信息原始物理值实时显示在GUI界面中。<br/><br/>
(2)显示方式是可选择的，对于某个特定CAN信息可以选择仪表盘方式得到瞬时物理值信息，也可以通过实时物理值曲线查看特定CAN信息的物理值变化情况。<br/><br/>
(3)提供CAN信息保存功能，格式可以使CSV或者自定义。
</ul><br/>
4.能够指定要<strong>发送的多个CAN信息</strong><br/><br/>
<ul>
(1)允许用户设定CAN信息中的CAN信号物理值<br/><br/>
(2)提供CAN信号的物理值转换成信号值的功能，发送给CanTool装置。
</ul><br/>
5.实现<strong>加载用户提供的CAN信息和信号数据库</strong><br/><br/>
<ul>
(1)可以显示CAN信号在CAN信息的布局，可保存到CanToolApp设定文件。<br/><br/>
(2)可以树状结构显示在GUI界面中<br/><br/>
(3)可以另存为xml和JSON (JavaScript Object Notation)格式，也可以已将xml或Json格式的数据库，转换为CAN信息和信号数据库格式。
</ul>
</ul>

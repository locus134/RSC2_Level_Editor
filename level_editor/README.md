# <center>明星大厨2018-关卡编辑器</center>

功能列表
===
- 编辑餐厅基本信息。
- 编辑餐厅中出现的所有顾客和订单信息。
- 编辑餐厅中所有的关卡配置。
- 自动生成订单及过关条件。

编译及运行
===
在Mac下使用Visual Studio for Mac编译。
Windows下需要下载 Mono(或者.NetFramework)和[GTK#](http://www.mono-project.com/download/stable/#download-win)的支持。

使用方法
===
1. 先准备好基本的餐厅、食物、顾客数据库（使用外部JSON编辑器来编辑），目录结构如下：
    - GameData
        - cookware.json
        - customer.json
        - food.json
        - ingredient.json
        - levels
        - order.json
        - restaurant.json

2. 运行关卡编辑器并选择数据库目录（即上述GameData目录）所在位置。



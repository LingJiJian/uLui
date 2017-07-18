基于ugui组件封装，高级控件库uLui
====
![](picture/photo1.png)
![](picture/photo2.png)
特性
-------  
### 组件LWidget
1.LScrollView 滑动层<br>
2.LRichLabel 可复用的富文本<br>
3.LControlView 摇杆控件<br>
4.LGridView 网格容器<br>
5.LGridPageView 网格翻页容器<br>
6.LMovieClip 序列帧控件<br>
7.LPageView 翻页容器<br>
8.LButton 长按按钮<br>
9.LListView 复用列表<br>
10.LLabelAtlas 图集字<br>
11.LTableView 复用表<br>
12.LHUDView hud容器<br>
13.LSlider 滑块<br>
14.LProgress 进度条<br>
15.LExpandListView 可折叠列表<br>
16.LDragView 拖拽控件<br>
17.LSwitch 切换条<br>
18.LInputField 抬起输入框<br>

### 窗体LWindow
1.自动管理窗体层次<br>
2.窗体回收和缓存<br>
3.窗体逻辑跟控件解耦<br>

### 多线程网络库
整合protobuff

### 若干小工具
1.热更模块<br>
2.打包ab<br>
3.使用texturepacker图集<br>

### 整合Slua
1.已经可以用纯lua开发了哦 <br>

### 运行
①普通测试<br>
双击运行 Assets/Resources/Scenes/start 场景即可<br>
②热更测试<br>

### 如何热更？
1.首先配置热更<br>
①config.txt 中修改 Debug 为 0<br>
②config.txt 中修改 HotFix 为 1<br>
③config.txt 中修改 ResUrl 为 你的cdn地址<br>
2.准备打包ab<br>
①点击菜单栏Tools->HotfixConfig，然后选择对应的平台<br>
②然后会生成data.zip和version.ver，把他们放到cdn服务器下准备下载<br>
双击运行 Assets/Resources/Scenes/start 场景即可<br>

什么？这个例子太复杂？请看下面热更精简版演示：<br>
[https://github.com/LingJiJian/UnityHotFixDemo](https://github.com/LingJiJian/UnityHotFixDemo)<br />  

联系
-------
qq342854406  qq群347085657

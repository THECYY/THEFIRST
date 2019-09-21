import wx
import base64加密
import base64解密
import 构建二维码
import 字典与json字符串
number=int(input("请输入您想查验的功能:1:base64加密 2:base64解密 3:构建文本的二维码 4:字典与JSON字符串的转化"))
if number==1:
     base64加密.event1()
elif number==2:
     base64解密.event2()
elif number==3:
     构建二维码.event3()
elif number==4:
     字典与json字符串.event4()
else:
      print("请输入一到四的阿拉伯数字")
    

"""
class frame(wx.Frame):
   def __init__(self,*b):
      super(frame,self).__init__(*b)
      pal=wx.panel(self)
      title=wx.StaicText(pa1,label="请输入你想测试的项")    
      font = title.GetFont()
      font.PointSize += 10
      font = font.Bold()
      title.SetFont(font)
      sizer=wx.BoxSizer(wx.VERTICAL)
      sizer.Add(title, wx.SizerFlags().Border(wx.TOP|wx.LEFT, 25))
      pal.SetSizer(sizer)
  funcation1=wx.StaticText(panel,label="测试一：字符串加密",pos=(40,120))
  funcation2=wx.StaticText(panel,label="测试二：字符串解密",pos=(40,180))
  funcation3=wx.StaticText(panel,label="测试三：字典与json字符串",pos=(40,240))
  funcation4=wx.StaticText(panel,label="测试四：生成二维码",pos=(40,300)
button1=wx.Botton(frame,label="开始",pos=(80,120))
button2=wx.Botton(frame,label="开始",pos=(80,180))
button3=wx.Botton(frame,label="开始",pos=(80,240))
button4=wx.Botton(frame,label="开始",pos=(80,300))
botton1.Bind(wx.ENT.RUTTON,btbutton1)
botton2.Bind(wx.ENT.RUTTON,btbutton2)
botton3.Bind(wx.ENT.RUTTON,btbutton3)
botton4.Bind(wx.ENT.RUTTON,btbutton4)
def btbutton1():
       event1()
def btbutton2():  
       event2()
def btbutton3():  
      event3()
def btbutton4():
      event4()

      
=wx.Frame(parent=None,id=-1,title="自强studio测试-_-",pos=(100,100),size=(600,400))
if __name__ == '__main__':
    app = wx.App()
    frm = frame(None)
    frm.Show()
    app.MainLoop()
"""
       

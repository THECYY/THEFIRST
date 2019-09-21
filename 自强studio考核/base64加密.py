def event1():
 zero="00000000"
 string=str(input("请输入你想加密的数据"))
 s=""
 for i in range(len(string)):
  z=string[i]
  ten=ord(z)
  sumnumber=0
  tool=0
  while ten!=0:
    more=ten%2
    sumnumber=int(more)*10**tool+sumnumber
    tool+=1
    ten=(ten-ten%2)/2 
  the2k=str(sumnumber)
  if len(the2k)==8:
    sumnumber=str(sumnumber)
  else:
    the2k=zero[0:(8-len(the2k))]+the2k
  s=s+the2k
 if len(s)%6==0:
   s=s
 else:
   s=s+zero[0:(6-len(s)%6)]
 t=""
 for i in range(0,len(s),6):
   k=s[i]+s[i+1]+s[i+2]+s[i+3]+s[i+4]+s[i+5]
   m=0
   for j in range(6):
     p=k[j]
     p=int(p)
     m=p*(2**(5-j))+m
     if 0<=m<=25:
      stom=chr(m+65)
     elif 26<=m<=51:
      stom=chr(m+71)
     elif 52<=m<=61:
      stom=chr(m-4)
     elif hum==62:
      stom="+"
     else:
      stom="/"
   t=t+stom
 print("加密后的数据为："+t)


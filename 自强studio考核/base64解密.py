def event2(): 
  zero="00000000"
  string=str(input("请输入你想解密的代码"))
  allnumber=""
  for i in range(len(string)):
      asc=int(ord(string[i]))
      if 65<=asc<=90:
          asc=asc-65
      elif 97<=asc<=122:
          asc=asc-71
      elif 48<=asc<=57:
          asc=asc+4
      elif asc==43:
          asc=62
      elif asc==47:
          asc=63
      else:
          print("输入错误")
      sums=0
      number=0
      another=0
      while asc!=0:
        another=asc%2
        sums=sums+another*(10**number)
        asc=(asc-another)/2
        number+=1
      if len(str(int(sums)))==6:
          thenumber=str(int(sums))
      else:
          thenumber=zero[0:(6-len(str(int(sums))))]+str(int(sums))
      allnumber=allnumber+thenumber
  
  if len(allnumber)==6:
      print("被解码值至少两个字符呦") 
  elif len(allnumber)%8==0:
      allnumber=allnumber
  else:
      allnumber=allnumber[0:(len(allnumber)-len(allnumber)%8)]
  print("解码后的0 1值为"+allnumber)    
  result=""
  tens=0
  for j in range(0,len(allnumber),8):
      p=""
      tens=0
      for cc in range(j,j+8):
          p=p+allnumber[cc]
      for k in range(8):
          q=int(p[k])
          tens=tens+q**(7-k)
  result=result+chr(tens)
  print("原值为:(很有可能因为此ASCII码值所代表的字符无法显示而显示白框)"+result)




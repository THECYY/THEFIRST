def event4():
 try:
  import json
  bn=int(input("请输入你想要输入的键的个数"))
  listkey=[]
  listnumber=[]
  for fn in range(bn):
   listkey.append(input("请输入你的第"+str(fn+1)+"个键"))
   listnumber.append(input("请输入你的第"+str(fn+1)+"个值"))
  dictionary=dict(zip(listkey,listnumber))
 except:
  print("输入的键数一定要是证整数呦")

 jsonfile=json.dumps(dictionary)
 listdel=[]
 for i in range(len(listkey)-1):
     times=listnumber.count(listnumber[i])
     if times==1:
         continue
     else:
         for j in range((i+1),len(listnumber)):
          if listnumber[j]==listnumber[i]:
             listdel.append(i)
             merroy=listkey[i]
             listkey[j]=(listkey[i]+"','"+listkey[j])
             break
         else:
             continue
 try:
  p=0
  for u in range (len(listdel)):
   del listkey[listdel[u]-p]
   del listnumber[listdel[u]-p]
   p+=1
  newdictionary=dict(zip(listnumber,listkey))
  print("得到的字典是:")
  print(dictionary)
  print("json字符串为"+jsonfile)
  print("得到的新字典是:")
  print(newdictionary)
  print(type(dictionary),type(jsonfile))
 except:
  print("请确认您所输入的键值量等于值量，键最好不要重复哦")


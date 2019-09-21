def event3():
 import qrcode
 thefile=input("请输入你想转二维码的文本的路径")
 qr=qrcode.QRCode(
 version=2,
 error_correction=qrcode.constants.ERROR_CORRECT_M,
 box_size=4,
 border=4
 )
 qr.add_data(thefile)
 qr.make(fit=True)
 img=qr.make_image()
 img.save(input("请输入你想保存的文件名(不包含文件格式呦)")+".jpg")
     

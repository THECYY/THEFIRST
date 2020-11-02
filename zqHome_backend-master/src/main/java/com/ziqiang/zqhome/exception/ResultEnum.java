package com.ziqiang.zqhome.exception;


import com.baomidou.mybatisplus.extension.api.IErrorCode;

public enum ResultEnum implements IErrorCode {
    UNKONW_ERROR(-1, "未知错误"),
    SUCCESS(0, "成功"),
    ERROR(1, "失败"),

    //用户接口错误码
    SIGNUP_ERROR(10001, "注册失败，请检查学号(或用户名)和密码"),//也可能是API宕掉了
    HASSIGNUP_ERROR(11001, "该用户已认证/注册！"),
    NOTSIGN_ERROR(10002, "登录失败，该用户未注册"),
    LOGIN_ERROR(10003, "登录失败，请检查学号（或用户名）密码错误"),  //可能是自强API服务出错
    PERMISSION_TOKEN_EXPIRED(10004, "登录过期，请重新登录"),  //token过期
    PERMISSION_TOKEN_INVALID(10005, "用户信息解析异常，请重新登录"),  //token解析错误
    PERMISSION_SIGNATURE_ERROR(10006, "用户信息解析异常，请重新登录"),  //签名异常
    USER_NOT_LOGGED_IN(10007, "用户未登录"),

    //图片操作错误码
    UPLOAD_LIMIT(20001, "上传失败，已达到今日上传数目限制！"),
    DELETE_COMMENT_ERROR(20002, "删除失败，无法删除他人的评论"),
    DELETE_PIC_ERROR(20003, "删除失败，无法删除他人的图片"),
    UPLOAD_ERROR(20004, "上传失败，上传至文件服务器失败"),
    UPLOAD_EMPTY(20005, "上传失败，有空文件"),


    ;

    private long code;
    private String msg;

    ResultEnum(Integer code, String msg) {
        this.code = code;
        this.msg = msg;
    }

    public long getCode() {
        return code;
    }

    public String getMsg() {
        return msg;
    }
}


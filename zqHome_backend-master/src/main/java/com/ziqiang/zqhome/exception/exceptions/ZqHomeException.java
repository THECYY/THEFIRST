package com.ziqiang.zqhome.exception.exceptions;


import com.baomidou.mybatisplus.extension.api.IErrorCode;
import com.ziqiang.zqhome.exception.ResultEnum;

/**
 * @Author xuejun
 * @Description 该后端应用的异常类
 * @Date 16:58 2020/8/5
 **/
public class ZqHomeException extends RuntimeException {
    private static final long serialVersionUID = 1L;

    private IErrorCode errorCode;

    public ZqHomeException() {};

    public ZqHomeException(ResultEnum resultEnum) {
        super(resultEnum.getMsg());
        this.errorCode = resultEnum;
    }

    public IErrorCode getErrorCode() {
        return errorCode;
    }

    public void setErrorCode(IErrorCode errorCode) {
        this.errorCode = errorCode;
    }
}

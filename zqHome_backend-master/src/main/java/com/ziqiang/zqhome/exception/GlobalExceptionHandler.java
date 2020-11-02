package com.ziqiang.zqhome.exception;

import com.baomidou.mybatisplus.extension.api.R;
import com.ziqiang.zqhome.exception.exceptions.ZqHomeException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

/**
 * @Author xuejun
 * @Description 该后端应用的全局异常处理类
 * @Date 17:10 2020/8/5
 * @Param
 * @return
 **/
@RestControllerAdvice
public class GlobalExceptionHandler {
    private static final Logger logger = LoggerFactory.getLogger(GlobalExceptionHandler.class);

    @ExceptionHandler(Exception.class)
    public Object exceptionHandler(Exception e) {

        if (e instanceof ZqHomeException) {
            logger.error("业务异常："+e.getMessage(), e);
            ZqHomeException zqHomeException = (ZqHomeException)e;
            return R.failed(zqHomeException.getErrorCode());
        } else {
            logger.error("系统异常："+e.getMessage(), e);
            return R.failed(ResultEnum.UNKONW_ERROR);
        }
    }
}

package com.ziqiang.zqhome.utils;


import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

@Component
public class ConstantProperties {

    @Value("${zq-config.middlewareUrl}")
    private String middlewareUrl;

    @Value("${zq-config.secretKey}")
    private String secretKey;

    @Value("${zq-config.zqAuthUrl}")
    private String zqAuthUrl;

    public String getMiddlewareUrl() {
        return middlewareUrl;
    }

    public void setMiddlewareUrl(String middlewareUrl) {
        this.middlewareUrl = middlewareUrl;
    }

    public String getSecretKey() {
        return secretKey;
    }

    public void setSecretKey(String secretkey) {
        this.secretKey = secretkey;
    }

    public String getZqAuthUrl() {
        return zqAuthUrl;
    }

    public void setZqAuthUrl(String zqAuthUrl) {
        this.zqAuthUrl = zqAuthUrl;
    }
}

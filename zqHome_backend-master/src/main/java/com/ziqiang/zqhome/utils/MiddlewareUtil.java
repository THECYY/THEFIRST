package com.ziqiang.zqhome.utils;

import com.alibaba.fastjson.JSONObject;
import net.bytebuddy.asm.Advice;
import okhttp3.*;
import org.apache.http.auth.AUTH;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.io.IOException;

@Component
public class MiddlewareUtil {
    @Autowired
    ConstantProperties properties;

    //根据学号密码获取token，主动走信息门户验证
    public String getToken(String sid, String pwd, String check) throws IOException {
        JSONObject dataJson = new JSONObject();
        dataJson.put("sid", sid);
        dataJson.put("binding_type", "10");
        dataJson.put("pwd_info", pwd);
        dataJson.put("check", check);
        //FIXME:验证码字段附加


        OkHttpClient client = new OkHttpClient();
        RequestBody body = RequestBody.create(MediaType.parse("application/json; charset=utf-8"), String.valueOf(dataJson));
        Request request = new Request.Builder().url(properties.getMiddlewareUrl() + "auth/token/").
                addHeader("x-zswd-backend-token", properties.getSecretKey()).
                post(body).build();

        Response response = client.newCall(request).execute();

        JSONObject res = (JSONObject) JSONObject.parse(response.body().string());

        return (String)res.get("token");
    }

    //根据token获取中间层内的用户信息
    public String getTokenInfo(String token) throws IOException {
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder().url(properties.getMiddlewareUrl() + "auth/token/"+token+"/").
                addHeader("x-zswd-backend-token", properties.getSecretKey()).get().build();

        Response response = client.newCall(request).execute();

        String res = response.body().string();

        return res;
    }

    //根据token获取中间层内的用户信息
    public String getTokenInfoByAyth(String token) throws IOException {
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder().url(properties.getMiddlewareUrl() + "auth/token/"+token+"/").
                addHeader("x-zswd-backend-token", properties.getSecretKey()).get().build();
        Response response = client.newCall(request).execute();
        String res = response.body().string();
        return res;
    }

    //通过ZqAuth获得Token
    public String getTokenByZqAuth(String email,String vCode,String check) throws IOException {
        JSONObject dataJson = new JSONObject();
        dataJson.put("binding_type", "10");
        dataJson.put("email", email);
        dataJson.put("vCode", vCode);
        dataJson.put("check", check);
        OkHttpClient client = new OkHttpClient();
        RequestBody body = RequestBody.create(MediaType.parse("application/json; charset=utf-8"), String.valueOf(dataJson));
        Request request = new Request.Builder().url(properties.getZqAuthUrl() + "zq-user/loginByEmail?email="+email+"&vCode="+vCode).
                addHeader("x-zswd-backend-token", properties.getSecretKey()).
                post(body).build();

        Response response = client.newCall(request).execute();

        JSONObject res = (JSONObject) JSONObject.parse(response.body().string());

        Object data=res.get("data");
        JSONObject dJson=(JSONObject)data;
        Object token=dJson.get("token");
        return token.toString();
    }
    //通过ZqAuth获得验证码
    public String getVCode(String email) throws IOException {
        JSONObject dataJson = new JSONObject();
        dataJson.put("email", email);

        OkHttpClient client = new OkHttpClient();
        RequestBody body = RequestBody.create(MediaType.parse("application/json; charset=utf-8"), String.valueOf(dataJson));
        Request request = new Request.Builder().url(properties.getZqAuthUrl() + "zq-user/vCode?email="+email).
                addHeader("x-zswd-backend-token", properties.getSecretKey()).
                post(body).build();

        Response response = client.newCall(request).execute();
        JSONObject res = (JSONObject) JSONObject.parse(response.body().string());
        return (String)res.get("data");
    }
}

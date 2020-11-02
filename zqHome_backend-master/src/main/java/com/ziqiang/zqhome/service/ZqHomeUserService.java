package com.ziqiang.zqhome.service;

import com.alibaba.fastjson.JSONObject;
import com.ziqiang.zqhome.entity.ZqHomeUser;
import com.baomidou.mybatisplus.extension.service.IService;

import java.io.IOException;
import java.util.List;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author xuejun
 * @since 2020-08-05
 */
public interface ZqHomeUserService extends IService<ZqHomeUser> {
    String signUp(String sid, String pwd, String name) throws IOException;
    JSONObject login(String sid, String pwd) throws IOException;
    JSONObject getInfo(Long id);
    String getVCode(String email)throws IOException;
    JSONObject loginByEmail(String email, String vCode) throws IOException;
    String signUpByEmail(String sid, String pwd, String name)throws IOException;
}

package com.ziqiang.zqhome.controller;


import com.baomidou.mybatisplus.extension.api.R;
import com.ziqiang.zqhome.security.JwtIgnore;
import com.ziqiang.zqhome.security.JwtTokenUtil;
import com.ziqiang.zqhome.service.ZqHomeUserService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;

/**
 * <p>
 *  前端控制器
 * </p>
 *
 * @author xuejun
 * @since 2020-08-05
 */
@RestController
@RequestMapping("user")
@Api(tags = "用户相关接口", description = "提供用户表相关的Rest API")
@CrossOrigin
public class ZqHomeUserController {

    @Value("${audience.base64Secret}")
    private String base64Secret;

    @Autowired
    ZqHomeUserService zqHomeUserService;


    @PostMapping("/getVCode")
    @ApiOperation(value = "获取验证码接口", notes = "")
    @JwtIgnore
    public R getVCode(String email) throws IOException {
        return R.ok(zqHomeUserService.getVCode(email));
    }


    @PostMapping("/loginByEmail")
    @ApiOperation(value = "邮箱登录接口")
    @JwtIgnore
    public R loginByEmail(String email, String vCode) throws IOException {
        return R.ok(zqHomeUserService.loginByEmail(email, vCode));
    }


    @GetMapping("/test")
    @ApiOperation(value = "测试接口", notes = "该接口用于测试")
    @JwtIgnore
    public R test() {
        return R.ok("test OK");
    }


    @PostMapping("/signUp")
    @ApiOperation(value = "注册接口", notes = "")
    @JwtIgnore
    public R signUp(String sid, String pwd, String name) throws IOException {
        return R.ok(zqHomeUserService.signUp(sid, pwd, name));
    }

    @PostMapping("/login")
    @ApiOperation(value = "登录接口")
    @JwtIgnore
    public R login(String sid, String pwd) throws IOException {
        return R.ok(zqHomeUserService.login(sid, pwd));
    }

    @GetMapping("/getInfo")
    @ApiOperation(value = "获取用户信息接口", notes = "返回用户相关信息，譬如点赞/举报记录等")
    @JwtIgnore
    public R getInfo(@RequestHeader("Authorization") String token) {
        token = token.substring(7);
        Long id = Long.parseLong(JwtTokenUtil.getUserId(token, base64Secret));
        return R.ok(zqHomeUserService.getInfo(id));
    }
}

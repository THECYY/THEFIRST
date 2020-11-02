package com.ziqiang.zqhome.controller;

import com.baomidou.mybatisplus.extension.api.R;
import com.ziqiang.zqhome.security.JwtIgnore;
import com.ziqiang.zqhome.service.RedisService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/redis")
@Api(tags = "Redis相关接口", description = "提供Redis相关的Rest API")
@CrossOrigin
public class RedisController {
    @Autowired
    RedisService redisService;

    @GetMapping("/test")
    @ApiOperation(value = "测试redis数据接口", notes = "该接口用于redis数据测试")
    @JwtIgnore
    public R test(@RequestParam List<Integer> picIds) {
        return R.ok(redisService.getLike(picIds));
    }
}

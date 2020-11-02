package com.ziqiang.zqhome.controller;


import com.baomidou.mybatisplus.extension.api.R;
import com.ziqiang.zqhome.entity.ZqMessage;
import com.ziqiang.zqhome.security.JwtIgnore;
import com.ziqiang.zqhome.security.JwtTokenUtil;
import com.ziqiang.zqhome.service.ZqMessageService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.util.List;

/**
 * <p>
 *  前端控制器
 * </p>
 *
 * @author xuejun
 * @since 2020-09-21
 */
@RestController
@RequestMapping("/message")
@Api(tags = "图文消息相关接口", description = "提供图文消息表相关的RestAPI")
public class ZqMessageController {
    @Value("${audience.base64Secret}")
    private String base64Secret;

    @Autowired
    ZqMessageService zqMessageService;

    @GetMapping("/test")
    @ApiOperation(value = "测试接口", notes = "该接口用于测试")
    @JwtIgnore
    public R test() {
        return R.ok("test OK");
    }

    @PostMapping
    @ApiOperation(value = "发送图文消息接口")
    public R addMessage(ZqMessage message, Long kindId, @RequestParam("files") List<MultipartFile> files,
                        @RequestHeader("Authorization") String token) throws IOException {
        token = token.substring(7);
        Long id = Long.parseLong(JwtTokenUtil.getUserId(token, base64Secret));
        return R.ok(zqMessageService.addMessage(message, files, id, kindId));
    }

}

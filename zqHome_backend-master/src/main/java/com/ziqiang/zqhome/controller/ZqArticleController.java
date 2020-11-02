package com.ziqiang.zqhome.controller;


import com.baomidou.mybatisplus.extension.api.R;
import com.ziqiang.zqhome.entity.ZqArticle;
import com.ziqiang.zqhome.security.JwtIgnore;
import com.ziqiang.zqhome.service.ZqArticleService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

/**
 * <p>
 *  前端控制器
 * </p>
 *
 * @author xuejun
 * @since 2020-07-23
 */
@RestController
@RequestMapping("article")
@Api(tags = "文章相关接口", description = "提供文章表相关的Rest API")
@CrossOrigin
public class ZqArticleController {
    @Autowired
    ZqArticleService zqArticleService;

    @GetMapping("/test")
    @ApiOperation(value = "测试接口", notes = "该接口用于测试")
    @JwtIgnore
    public R test() {
        return R.ok("test OK");
    }

    @PostMapping("/newArticle")
    @ApiOperation(value = "新建文章", notes = "")
    public R newArticle(@RequestParam("title") String title, @RequestParam("content") String content,
                        @RequestParam("kind") String kind) {
        //TODO:需要从用户token中获取用户id
        Long id = (long) 1;
        return R.ok(zqArticleService.newArticle(title, content, kind, id));
    }

    @PostMapping("/getArticle")
    @JwtIgnore
    @ApiOperation(value = "获取首页文章", notes = "")
    public R latestArticle(Integer page, Integer count, String category) {
        return R.ok(zqArticleService.getLatestArticle(page, count, category));
    }

}

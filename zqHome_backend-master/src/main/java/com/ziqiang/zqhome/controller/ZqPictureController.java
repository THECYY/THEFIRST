package com.ziqiang.zqhome.controller;


import com.baomidou.mybatisplus.extension.api.R;
import com.ziqiang.zqhome.entity.ZqPicture;
import com.ziqiang.zqhome.entity.ZqPictureTags;
import com.ziqiang.zqhome.security.JwtIgnore;
import com.ziqiang.zqhome.security.JwtTokenUtil;
import com.ziqiang.zqhome.service.ZqPictureCommentService;
import com.ziqiang.zqhome.service.ZqPictureService;
import com.ziqiang.zqhome.service.ZqPictureTagsService;
import com.ziqiang.zqhome.utils.RedisUtil;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

/**
 * <p>
 *  前端控制器
 * </p>
 *
 * @author xuejun
 * @since 2020-06-11
 */
@RestController
@RequestMapping("picture")
@Api(tags = "图片相关接口", description = "提供图片表相关的Rest API")
@CrossOrigin
public class ZqPictureController {

    @Autowired
    private ZqPictureService zqPictureService;

    @Autowired
    ZqPictureCommentService zqPictureCommentService;

    @Autowired
    ZqPictureTagsService zqPictureTagsService;

    @Autowired
    private RedisUtil redisUtil;

    @Value("${audience.base64Secret}")
    private String base64Secret;

    @GetMapping("/test")
    @ApiOperation(value = "测试接口", notes = "该接口用于测试")
    @JwtIgnore
    public R test() {
        return R.ok("test OK");
    }

    @PostMapping("/upload")
    @ApiOperation(value = "上传图片接口", notes="可通过该接口进行图片上传")
    public R uploadPic(@RequestParam("file") MultipartFile file, String content, Long id, Long kindId) {
        String res = zqPictureService.uploadPic(file, content, id, kindId);
        return R.ok(res);
    }

    @PostMapping("/getPic")
    @ApiOperation(value = "获取图片接口", notes="可通过该接口进行图片获取")
    @JwtIgnore
    public R getPic(Integer page, Integer counts,
                    @RequestParam(required = false, defaultValue = "false") String order) {
        List<ZqPicture> res = zqPictureService.getPic(page, counts, order);
        return R.ok(res);
    }

    @GetMapping("/getPic/{id}")
    @ApiOperation(value = "获取图片详细信息接口", notes="可通过该接口进行指定图片详细信息的获取")
    @JwtIgnore
    public R getPicById(@PathVariable String id) {
        ZqPicture res = zqPictureService.getPicInfo(id);
        res.setLikes((Integer) this.redisUtil.hget(id, "like"));
        res.setViews((Integer) this.redisUtil.hget(id, "view"));
        return R.ok(res);
    }

    @GetMapping("/getTags")
    @ApiOperation(value = "获取获取图文内容Tag接口", notes="")
    @JwtIgnore
    public R getTags() {
        List<ZqPictureTags> tags = zqPictureTagsService.list();
        return R.ok(tags);
    }

    @GetMapping("/delPic/{id}")
    @ApiOperation(value = "删除图片接口", notes = "该接口用于删除指定图片的评论")
    @JwtIgnore
    public R delPic(@RequestHeader("Authorization") String token, @PathVariable  String id) {
        token = token.substring(7);
        Long Uid = Long.parseLong(JwtTokenUtil.getUserId(token, base64Secret));
        return R.ok(zqPictureService.delPic(Uid, id));
    }

    @GetMapping("/total")
    @ApiOperation(value = "获取图片总数接口", notes="可通过该接口进行图片总数的获取")
    @JwtIgnore
    public R total() {
        return R.ok(zqPictureService.count());
    }

    @PostMapping("/likePic")
    @ApiOperation(value = "点赞图片接口", notes="可通过该接口进行图片点赞")
    public R likePic(String id, Long Uid) {
        return R.ok(zqPictureService.likePic(id, Uid));
    }

    @GetMapping("/viewPic/{id}")
    @ApiOperation(value = "浏览图片接口", notes="可通过该接口进行图片浏览")
    @JwtIgnore
    public R viewPic(@PathVariable  String id) {
        return R.ok(zqPictureService.viewPic(id));
    }

    @PostMapping("/comment")
    @ApiOperation(value = "评论接口", notes = "该接口用于发表评论")
    public R comment(Long picId, Long fromUid, Long toUid, String content) {
        zqPictureService.commentPic(picId.toString());
        return R.ok(zqPictureCommentService.comment(picId, fromUid, toUid, content));
    }

    @GetMapping("/getComment/{id}")
    @ApiOperation(value = "获取评论接口", notes = "该接口用于获取指定图片的评论")
    @JwtIgnore
    public R getComment(@PathVariable  String id) {
        return R.ok(zqPictureCommentService.getComment(id));
    }

    @GetMapping("/delComment/{id}")
    @ApiOperation(value = "删除评论接口", notes = "该接口用于删除指定图片的评论")
    @JwtIgnore
    public R delComment(@RequestHeader("Authorization") String token, @PathVariable  String id) {
        token = token.substring(7);
        Long Uid = Long.parseLong(JwtTokenUtil.getUserId(token, base64Secret));
        return R.ok(zqPictureCommentService.delComment(Uid, id));
    }

    @PostMapping("/report")
    @ApiOperation(value="举报接口")
    public R report(Long picId, Long Uid, String reason) {
        return R.ok(zqPictureService.report(picId, Uid, reason));
    }

}

package com.ziqiang.zqhome.service.impl;

import com.alibaba.fastjson.JSONObject;
import com.aliyun.oss.OSS;
import com.aliyun.oss.OSSClientBuilder;
import com.aliyun.oss.model.PutObjectRequest;
import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.baomidou.mybatisplus.core.metadata.IPage;
import com.baomidou.mybatisplus.extension.plugins.pagination.Page;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.ziqiang.zqhome.entity.ZqHomeUser;
import com.ziqiang.zqhome.entity.ZqPicture;
import com.ziqiang.zqhome.entity.ZqPictureTags;
import com.ziqiang.zqhome.entity.ZqPictureUserAction;
import com.ziqiang.zqhome.exception.ResultEnum;
import com.ziqiang.zqhome.exception.exceptions.ZqHomeException;
import com.ziqiang.zqhome.mapper.ZqPictureMapper;
import com.ziqiang.zqhome.mapper.ZqPictureUserActionMapper;
import com.ziqiang.zqhome.service.*;
import com.ziqiang.zqhome.utils.RedisUtil;
import io.swagger.models.auth.In;
import org.apache.ibatis.annotations.Update;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.cache.CacheProperties;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;

import java.io.ByteArrayInputStream;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * <p>
 *  服务实现类
 * </p>
 *
 * @author xuejun
 * @since 2020-06-11
 */
@Service("ZqPictureService")
public class ZqPictureServiceImpl extends ServiceImpl<ZqPictureMapper, ZqPicture> implements ZqPictureService {
    @Autowired
    RedisUtil redisUtil;

    @Autowired
    ZqPictureUserActionService zqPictureUserActionService;

    @Autowired
    ZqHomeUserService zqHomeUserService;

    @Autowired
    ZqPictureTagsService zqPictureTagsService;

    @Value("${zq-config.PICUPLOADLIMIT}")
    Integer PICUPLOADLIMIT;


    // TODO:重构此处代码
    @Override
    public String uploadPic(@RequestParam("file") MultipartFile file, String content, Long id, Long kindId) {
        QueryWrapper<ZqPictureUserAction> queryWrapper  = new QueryWrapper<>();

        //检查是否超出当日上传数量限制
        Date date = new Date();
        SimpleDateFormat dateFormat = new SimpleDateFormat("YYYY-MM-dd");
        String startDayTime = dateFormat.format(date) + " 00:00:00";

        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        cal.add(Calendar.DAY_OF_YEAR,1);
        Date addDate = cal.getTime();
        String endDayTime = dateFormat.format(addDate) + " 00:00:00";

        List<ZqPictureUserAction> record = this.zqPictureUserActionService.list(queryWrapper.eq("Uid", id).
                eq("action", "upload").apply("UNIX_TIMESTAMP(create_time) >= UNIX_TIMESTAMP('"+startDayTime+"')").
                apply("UNIX_TIMESTAMP(create_time) < UNIX_TIMESTAMP('"+endDayTime+"')"));
        if (record.size() >= PICUPLOADLIMIT) {
            throw new ZqHomeException(ResultEnum.UPLOAD_LIMIT);
        }

        //开始上传
        if (!file.isEmpty()) {
            try {
                String endpoint = "oss-cn-shanghai.aliyuncs.com";
                String accessKeyId = "LTAI4GAfPC7VErR7b7A2tYiS";
                String accessKeySecret = "3LKyWIc2ynMx88jchT0fMsVIYwr2re";
                String bucketName = "zq-home";

                //将图片信息写入数据库

                ZqPicture zqPicture = new ZqPicture();
                zqPicture.setOwnerId(id);
                zqPicture.setPicInfo(content);

                ZqPictureTags tag = zqPictureTagsService.getOne(new QueryWrapper<ZqPictureTags>().eq("id", kindId));
                zqPicture.setPicKind(tag.getName());
                zqPicture.setPicUrl(bucketName+"."+endpoint+"/"+file.getOriginalFilename());

                this.save(zqPicture);

                //上传至阿里云OSS

                OSS ossClient = new OSSClientBuilder().build("http://"+endpoint, accessKeyId, accessKeySecret);

                PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, file.getOriginalFilename(), new ByteArrayInputStream(file.getBytes()));

                ossClient.putObject(putObjectRequest);

                ossClient.shutdown();

                //初始化图片在redis上的数据
                redisUtil.hset(Long.toString(zqPicture.getId()), "like", 0);
                redisUtil.hset(Long.toString(zqPicture.getId()), "view", 0);

                //记录上传行为
                ZqPictureUserAction zqPictureUserAction = new ZqPictureUserAction();
                zqPictureUserAction.setUid(id);
                zqPictureUserAction.setPicId(Long.parseLong("-1"));
                zqPictureUserAction.setAction("upload");
                this.zqPictureUserActionService.save(zqPictureUserAction);

                return "上传成功";
            } catch (Exception e) {
                e.printStackTrace();
                return "upload failed: " + e.getMessage();
            }
        } else {
            return "upload failed: Empty file";
        }
    }

    @Override
    public List<ZqPicture> getPic(Integer page, Integer count, String order) {
        QueryWrapper<ZqPicture> wrapper = new QueryWrapper();
        //只选取被举报数小于等于3的
        //TODO:抽离为配置文件
        wrapper.le("reports", 3);
        Page<ZqPicture> picturePage = new Page<>(page, count);
        IPage<ZqPicture> pictureIPage;
        //默认按时间进行排序，order默认为false，即默认按最新时间进行排序
        if (order.equals("最新发布")) {
            pictureIPage =
                    this.getBaseMapper().selectPage(picturePage, wrapper.orderByDesc("create_time").select());
        } else if (order.equals("最早发布")) {
            pictureIPage =
                    this.getBaseMapper().selectPage(picturePage, wrapper.orderByAsc("create_time").select());
        } else if (order.equals("最多浏览")) {
            pictureIPage =
                    this.getBaseMapper().selectPage(picturePage, wrapper.orderByDesc("views").select());
        } else if (order.equals("最多赞数")) {
            pictureIPage =
                    this.getBaseMapper().selectPage(picturePage, wrapper.orderByDesc("likes").select());
        } else if (order.equals("最多评论")) {
            pictureIPage =
                    this.getBaseMapper().selectPage(picturePage, wrapper.orderByDesc("comments").select());
        } else {
            return new ArrayList<>();
        }
        List<ZqPicture> records = pictureIPage.getRecords();
        //为返回数据装填redis中的相关数据
        for (ZqPicture i: records) {
            String id = Long.toString(i.getId());
            i.setLikes((Integer) this.redisUtil.hget(id, "like"));
            i.setViews((Integer) this.redisUtil.hget(id, "view"));
        }
        return records;
    }

    public String delPic(Long Uid, String id) {
        //检查是不是自己的图片
        ZqPicture zqPicture = this.getOne(new QueryWrapper<ZqPicture>().eq("id", id));
        if (!zqPicture.getOwnerId().equals(Uid)) throw new ZqHomeException(ResultEnum.DELETE_PIC_ERROR);

        //删除
        this.remove(new QueryWrapper<ZqPicture>().eq("id", id));
        return "OK";
    }

    @Override
    public ZqPicture getPicInfo(String id) {
        ZqPicture picture = this.baseMapper.selectById(id);
        long owner_id = picture.getOwnerId();
        ZqHomeUser zqHomeUser = zqHomeUserService.getById(owner_id);
        picture.setName(zqHomeUser.getNickName());
        return picture;
    }

    @Async
    public String likePic(String id, Long Uid) {
        QueryWrapper<ZqPictureUserAction> queryWrapper  = new QueryWrapper<>();

        //检查是否点赞过，如果点赞过取消点赞
        ZqPictureUserAction record = this.zqPictureUserActionService.getOne(queryWrapper.eq("Uid", Uid).
                eq("picId", id).
                eq("action", "like"));

        //取消点赞
        if (record != null) {
            this.redisUtil.hdecr(id, "like", 1);
            this.baseMapper.deLike(id);
            //删除点赞记录
            this.zqPictureUserActionService.remove(new QueryWrapper<ZqPictureUserAction>().eq("Uid", Uid).
                    eq("picId", id).
                    eq("action", "like"));
            return "OK";
        }

        //没点过赞，正常点赞
        this.redisUtil.hincr(id, "like", 1);
        this.baseMapper.like(id);

        //记录点赞行为
        ZqPictureUserAction zqPictureUserAction = new ZqPictureUserAction();
        zqPictureUserAction.setUid(Uid);
        zqPictureUserAction.setPicId(Long.parseLong(id));
        zqPictureUserAction.setAction("like");
        this.zqPictureUserActionService.save(zqPictureUserAction);
//        this.redisUtil.sSet("like", id);
        return "OK";
    }

    @Async
    public String viewPic(String id) {
        this.redisUtil.hincr(id, "view", 1);
        this.baseMapper.view(id);
//        this.redisUtil.sSet("view", id);
        return "OK";
    }

    @Override
    public Boolean commentPic(String id) {
        this.baseMapper.comment(id);
        return true;
    }

    @Override
    public String report(Long picId, Long Uid, String reason) {
        QueryWrapper<ZqPictureUserAction> queryWrapper  = new QueryWrapper<>();

        //检查是否举报过
        ZqPictureUserAction record = this.zqPictureUserActionService.getOne(queryWrapper.eq("Uid", Uid).
                eq("picId", picId).
                eq("action", "report"));
        if (record != null) {
            return "OK";
        }
        this.baseMapper.report(picId.toString());

        //记录举报行为
        ZqPictureUserAction zqPictureUserAction = new ZqPictureUserAction();
        zqPictureUserAction.setUid(Long.parseLong(Uid.toString()));
        zqPictureUserAction.setPicId(Long.parseLong(picId.toString()));
        zqPictureUserAction.setAction("report");
        this.zqPictureUserActionService.save(zqPictureUserAction);
        return "OK";
    }



    //定时刷新Redis数据到Mysql中
//    @Override
//    @Scheduled(fixedRate = 3000)
//    public void refreshRedisToMysql() {
//        Set<Object> likeIds = this.redisUtil.sGet("like");
//        Set<Object> viewIds = this.redisUtil.sGet("view");
//        for (Object likeId: likeIds) {
//            (Long)likeId
//        }
//
////        (Integer) this.redisUtil.hget(id, "like");
////        (Integer) this.redisUtil.hget(id, "view")
//        System.out.println("test");
//    }

    //    @Override
//    public String uploadPic() {
//        // Endpoint以杭州为例，其它Region请按实际情况填写。
//        String endpoint = "http://oss-cn-shanghai.aliyuncs.com";
//        // 阿里云主账号AccessKey拥有所有API的访问权限，风险很高。强烈建议您创建并使用RAM账号进行API访问或日常运维，请登录 https://ram.console.aliyun.com 创建RAM账号。
//        String accessKeyId = "LTAI4GAfPC7VErR7b7A2tYiS";
//        String accessKeySecret = "3LKyWIc2ynMx88jchT0fMsVIYwr2re";
//
//        // 创建OSSClient实例。
//        OSS ossClient = new OSSClientBuilder().build(endpoint, accessKeyId, accessKeySecret);
//
//        // 创建PutObjectRequest对象。
//        String content = "Hello OSS";
//        // <yourObjectName>表示上传文件到OSS时需要指定包含文件后缀在内的完整路径，例如abc/efg/123.jpg。
//        PutObjectRequest putObjectRequest = new PutObjectRequest("zq-home", "test.txt", new ByteArrayInputStream(content.getBytes()));
//
//        // 上传字符串。
//        ossClient.putObject(putObjectRequest);
//
//        // 关闭OSSClient。
//        ossClient.shutdown();
//        return "OK";
//    }
}

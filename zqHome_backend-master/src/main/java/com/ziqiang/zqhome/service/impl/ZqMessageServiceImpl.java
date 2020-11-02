package com.ziqiang.zqhome.service.impl;

import com.aliyun.oss.OSS;
import com.aliyun.oss.OSSClientBuilder;
import com.aliyun.oss.model.PutObjectRequest;
import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.ziqiang.zqhome.entity.ZqMessage;
import com.ziqiang.zqhome.entity.ZqPicture;
import com.ziqiang.zqhome.entity.ZqPictureTags;
import com.ziqiang.zqhome.entity.ZqPictureUserAction;
import com.ziqiang.zqhome.exception.ResultEnum;
import com.ziqiang.zqhome.exception.exceptions.ZqHomeException;
import com.ziqiang.zqhome.mapper.ZqMessageMapper;
import com.ziqiang.zqhome.service.ZqMessageService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.ziqiang.zqhome.service.ZqPictureService;
import com.ziqiang.zqhome.service.ZqPictureTagsService;
import com.ziqiang.zqhome.service.ZqPictureUserActionService;
import com.ziqiang.zqhome.utils.RedisUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Map;

/**
 * <p>
 *  服务实现类
 * </p>
 *
 * @author xuejun
 * @since 2020-09-21
 */
@Service("ZqMessageService")
public class ZqMessageServiceImpl extends ServiceImpl<ZqMessageMapper, ZqMessage> implements ZqMessageService {
    @Autowired
    ZqPictureService zqPictureService;

    @Autowired
    ZqPictureUserActionService zqPictureUserActionService;

    @Autowired
    ZqPictureTagsService zqPictureTagsService;

    @Autowired
    RedisUtil redisUtil;

    @Value("${zq-config.PICUPLOADLIMIT}")
    Integer PICUPLOADLIMIT;

    @Value("${zq-config.aliyunOSS.accessKeyId}")
    String accessKeyId;

    @Value("${zq-config.aliyunOSS.accessKeySecret}")
    String accessKeySecret;

    @Value("${zq-config.aliyunOSS.bucketName}")
    String bucketName;

    @Override
    @Transactional(rollbackFor = Exception.class)
    public String addMessage(ZqMessage message, List<MultipartFile> files, Long ownerId, Long kindId) throws IOException {
        this.save(message);
        Long messageId = message.getId();
        for (MultipartFile file:  files) {
            if (file.isEmpty()) throw new ZqHomeException((ResultEnum.UPLOAD_EMPTY));

            try {
                upload(ownerId, messageId, kindId, "",file);
            } catch (Exception e) {
                throw new ZqHomeException(ResultEnum.UPLOAD_ERROR);
            }

        }
        return "OK";
    }

    // TODO:考虑异步上传
    // TODO:整体防止重复提交
    @Transactional(rollbackFor = Exception.class)
    public boolean upload(Long ownerId, Long messageId, Long kindId, String content, MultipartFile file) throws IOException {
        //检查是否超出当日上传数量限制
        if (checkLimit(ownerId)) throw new ZqHomeException(ResultEnum.UPLOAD_LIMIT);

        String endpoint = "oss-cn-shanghai.aliyuncs.com";

        //将图片信息写入数据库

        ZqPicture zqPicture = new ZqPicture();
        zqPicture.setOwnerId(ownerId);
        zqPicture.setMessageId(messageId);
        zqPicture.setPicInfo(content);

        //获取标签
        ZqPictureTags tag = zqPictureTagsService.getOne(new QueryWrapper<ZqPictureTags>().eq("id", kindId));
        zqPicture.setPicKind(tag.getName());

        zqPicture.setPicUrl(bucketName+"."+endpoint+"/"+file.getOriginalFilename());

        zqPictureService.save(zqPicture);

        //上传至阿里云OSS

        OSS ossClient = new OSSClientBuilder().build("http://"+endpoint, accessKeyId, accessKeySecret);

        PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName,
                file.getOriginalFilename(), new ByteArrayInputStream(file.getBytes()));

        ossClient.putObject(putObjectRequest);

        ossClient.shutdown();

        //初始化图片在redis上的数据
        redisUtil.hset(Long.toString(zqPicture.getId()), "like", 0);
        redisUtil.hset(Long.toString(zqPicture.getId()), "view", 0);

        //记录上传行为
        ZqPictureUserAction zqPictureUserAction = new ZqPictureUserAction();
        zqPictureUserAction.setUid(ownerId);
        zqPictureUserAction.setPicId(Long.parseLong("-1"));
        zqPictureUserAction.setAction("upload");
        this.zqPictureUserActionService.save(zqPictureUserAction);

        return true;
    }

    //检查是否超出当日上传数量限制
    public boolean checkLimit(Long ownerId) {
        QueryWrapper<ZqPictureUserAction> queryWrapper  = new QueryWrapper<>();

        Date date = new Date();
        SimpleDateFormat dateFormat = new SimpleDateFormat("YYYY-MM-dd");
        String startDayTime = dateFormat.format(date) + " 00:00:00";

        Calendar cal = Calendar.getInstance();
        cal.setTime(date);
        cal.add(Calendar.DAY_OF_YEAR,1);
        Date addDate = cal.getTime();
        String endDayTime = dateFormat.format(addDate) + " 00:00:00";

        List<ZqPictureUserAction> record = zqPictureUserActionService.list(queryWrapper.eq("Uid", ownerId).
                eq("action", "upload").apply("UNIX_TIMESTAMP(create_time) >= UNIX_TIMESTAMP('"+startDayTime+"')").
                apply("UNIX_TIMESTAMP(create_time) < UNIX_TIMESTAMP('"+endDayTime+"')"));
        return record.size() >= PICUPLOADLIMIT;
    }
}

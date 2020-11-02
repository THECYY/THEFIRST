package com.ziqiang.zqhome.service.impl;

import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.ziqiang.zqhome.entity.ZqHomeUser;
import com.ziqiang.zqhome.entity.ZqPicture;
import com.ziqiang.zqhome.entity.ZqPictureComment;
import com.ziqiang.zqhome.exception.ResultEnum;
import com.ziqiang.zqhome.exception.exceptions.ZqHomeException;
import com.ziqiang.zqhome.mapper.ZqPictureCommentMapper;
import com.ziqiang.zqhome.mapper.ZqPictureMapper;
import com.ziqiang.zqhome.service.ZqHomeUserService;
import com.ziqiang.zqhome.service.ZqPictureCommentService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * <p>
 *  服务实现类
 * </p>
 *
 * @author xuejun
 * @since 2020-07-15
 */
@Service("ZqPictureCommentService")
public class ZqPictureCommentServiceImpl extends ServiceImpl<ZqPictureCommentMapper, ZqPictureComment> implements ZqPictureCommentService {

    @Autowired
    ZqHomeUserService zqHomeUserService;


    @Override
    public String comment(Long picId, Long fromUid, Long toUid, String content) {
        ZqPictureComment zqPictureComment = new ZqPictureComment();
        zqPictureComment.setPicId(picId);
        zqPictureComment.setFromUid(fromUid);
        zqPictureComment.setToUid(toUid);
        zqPictureComment.setContent(content);
        
        this.save(zqPictureComment);

        return "OK";
    }

    @Override
    public List getComment(String picId) {
        List<ZqPictureComment> comments = this.list(new QueryWrapper<ZqPictureComment>().eq("pic_id", picId));

        for (ZqPictureComment i: comments) {
            Long fromId = i.getFromUid();
            String fromName = zqHomeUserService.getBaseMapper().selectOne(new QueryWrapper<ZqHomeUser>().
                    eq("id", fromId)).getNickName();
            i.setFromName(fromName);


            Long toId = i.getToUid();
            if (toId == Long.parseLong("-1")) {
                continue;
            }
            String toName = zqHomeUserService.getBaseMapper().selectOne(new QueryWrapper<ZqHomeUser>().
                    eq("id", toId)).getNickName();
            i.setToName(toName);
        }

        return comments;
    }

    @Override
    public String delComment(Long Uid, String id) {
        //检验是否是自己的图片
        ZqPictureComment zqPictureComment = this.getOne(new QueryWrapper<ZqPictureComment>().eq("id", id));
        if (!zqPictureComment.getFromUid().equals(Uid)) throw new ZqHomeException(ResultEnum.DELETE_COMMENT_ERROR);

        //删除操作
        this.remove(new QueryWrapper<ZqPictureComment>().eq("id", id));
        return "删除成功!";
    }


}

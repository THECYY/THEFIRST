package com.ziqiang.zqhome.service;

import com.ziqiang.zqhome.entity.ZqPictureComment;
import com.baomidou.mybatisplus.extension.service.IService;

import java.util.List;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author xuejun
 * @since 2020-07-15
 */
public interface ZqPictureCommentService extends IService<ZqPictureComment> {
    String comment(Long picId, Long fromUid, Long toUid, String content);
    List getComment(String picId);
    String delComment(Long Uid, String id);
}

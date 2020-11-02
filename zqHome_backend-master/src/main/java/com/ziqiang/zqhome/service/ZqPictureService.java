package com.ziqiang.zqhome.service;

import com.ziqiang.zqhome.entity.ZqPicture;
import com.baomidou.mybatisplus.extension.service.IService;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author xuejun
 * @since 2020-06-11
 */
public interface ZqPictureService extends IService<ZqPicture> {
    String uploadPic(MultipartFile file, String content, Long id, Long kindId);
    List<ZqPicture> getPic(Integer page, Integer count, String order);
    String delPic(Long Uid, String id);
    ZqPicture getPicInfo(String id);
    String likePic(String id, Long Uid);
    String viewPic(String id);
    Boolean commentPic(String id);
    String report(Long picId, Long Uid, String reason);
//    void refreshRedisToMysql();
}

package com.ziqiang.zqhome.service;

import com.ziqiang.zqhome.entity.ZqMessage;
import com.baomidou.mybatisplus.extension.service.IService;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.util.List;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author xuejun
 * @since 2020-09-21
 */
public interface ZqMessageService extends IService<ZqMessage> {
    String addMessage(ZqMessage message, List<MultipartFile> files, Long ownerId, Long kindId) throws IOException;
}

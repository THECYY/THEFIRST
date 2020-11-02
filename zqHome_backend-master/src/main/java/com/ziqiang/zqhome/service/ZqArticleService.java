package com.ziqiang.zqhome.service;

import com.ziqiang.zqhome.entity.ZqArticle;
import com.baomidou.mybatisplus.extension.service.IService;

import java.util.List;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author xuejun
 * @since 2020-07-23
 */
public interface ZqArticleService extends IService<ZqArticle> {
    String newArticle(String title, String content, String kind, Long id);
    List<ZqArticle> getLatestArticle(Integer page, Integer count, String category);
}

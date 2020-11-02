package com.ziqiang.zqhome.service.impl;

import com.alibaba.fastjson.JSONObject;
import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.baomidou.mybatisplus.core.metadata.IPage;
import com.baomidou.mybatisplus.extension.plugins.pagination.Page;
import com.ziqiang.zqhome.entity.ZqArticle;
import com.ziqiang.zqhome.entity.ZqPicture;
import com.ziqiang.zqhome.mapper.ZqArticleMapper;
import com.ziqiang.zqhome.service.ZqArticleService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.ziqiang.zqhome.utils.RedisUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * <p>
 *  服务实现类
 * </p>
 *
 * @author xuejun
 * @since 2020-07-23
 */
@Service("ZqArticleService")
public class ZqArticleServiceImpl extends ServiceImpl<ZqArticleMapper, ZqArticle> implements ZqArticleService {
    @Autowired
    RedisUtil redisUtil;

    @Override
    public String newArticle(String title, String content, String kind, Long id) {
        ZqArticle article = new ZqArticle();
        article.setTitle(title);
        article.setContent(content);
        article.setKind(kind);
        article.setOwnerId(id);

        //TODO:这里的JSON只能放一个，需要完善，做不做TAG？

        baseMapper.insert(article);

        return "OK";
    }

    public List<ZqArticle> getLatestArticle(Integer page, Integer count, String category) {
        QueryWrapper<ZqArticle> wrapper = new QueryWrapper();
        Page<ZqArticle> articlePage = new Page<>(page, count);
        IPage<ZqArticle> articleIPage;
        //默认按时间进行排序，order默认为false，即默认按最新时间进行排序

        if (category != null) {
            wrapper.eq("kind", category);
        }

        articleIPage =
                this.getBaseMapper().selectPage(articlePage, wrapper.orderByDesc("create_time").select());

        List<ZqArticle> records = articleIPage.getRecords();
        //为返回数据装填redis中的相关数据
        for (ZqArticle i: records) {
            String id = Long.toString(i.getSerialVersionUID());
            i.setLikeCount((Long) this.redisUtil.hget(id, "like"));
            i.setViewCount((Long) this.redisUtil.hget(id, "view"));
        }

        return records;
    }
}

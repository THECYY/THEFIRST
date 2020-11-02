package com.ziqiang.zqhome.mapper;

import com.ziqiang.zqhome.entity.ZqPicture;
import com.baomidou.mybatisplus.core.mapper.BaseMapper;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Update;

/**
 * <p>
 *  Mapper 接口
 * </p>
 *
 * @author xuejun
 * @since 2020-06-11
 */
@Mapper
public interface ZqPictureMapper extends BaseMapper<ZqPicture> {
    @Update("update zq_picture set likes=likes+1 where id=#{id}")
    Boolean like(String id);

    @Delete("update zq_picture set likes=likes-1 where id=#{id}")
    Boolean deLike(String id);

    @Update("update zq_picture set views=views+1 where id=#{id}")
    Boolean view(String id);

    @Update("update zq_picture set comments=comments+1 where id=#{id}")
    Boolean comment(String id);

    @Update("update zq_picture set reports=reports+1 where id=#{id}")
    Boolean report(String id);
}

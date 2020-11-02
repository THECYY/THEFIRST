package com.ziqiang.zqhome.service;



import com.ziqiang.zqhome.utils.RedisUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service("RedisService")
public class RedisService {
    @Autowired
    private RedisUtil redisUtil;

    public List<HashMap<Object, Object>> getLike(List<Integer> picIds) {
        ArrayList<HashMap<Object, Object>> picInfos = new ArrayList<>();
        for (Integer picId: picIds) {
            HashMap<Object, Object> picInfo = (HashMap<Object, Object>) redisUtil.hmget(Integer.toString(picId));
            picInfos.add(picInfo);
        }
        return picInfos;
    }
}

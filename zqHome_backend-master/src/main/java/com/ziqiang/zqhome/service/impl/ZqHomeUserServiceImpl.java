package com.ziqiang.zqhome.service.impl;

import com.alibaba.fastjson.JSONObject;
import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.ziqiang.zqhome.entity.ZqHomeUser;
import com.ziqiang.zqhome.entity.ZqPicture;
import com.ziqiang.zqhome.entity.ZqPictureUserAction;
import com.ziqiang.zqhome.exception.ResultEnum;
import com.ziqiang.zqhome.exception.exceptions.ZqHomeException;
import com.ziqiang.zqhome.mapper.ZqHomeUserMapper;
import com.ziqiang.zqhome.security.Audience;
import com.ziqiang.zqhome.security.JwtTokenUtil;
import com.ziqiang.zqhome.service.ZqHomeUserService;
import com.ziqiang.zqhome.service.ZqPictureService;
import com.ziqiang.zqhome.service.ZqPictureUserActionService;
import com.ziqiang.zqhome.utils.MiddlewareUtil;
import io.jsonwebtoken.Claims;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.List;

/**
 * <p>
 *  服务实现类
 * </p>
 *
 * @author xuejun
 * @since 2020-08-05
 */
@Service("ZqHomeUserService")
public class ZqHomeUserServiceImpl extends ServiceImpl<ZqHomeUserMapper, ZqHomeUser> implements ZqHomeUserService {

    private static Logger logger = LoggerFactory.getLogger(ZqHomeUserService.class);

    @Autowired
    private MiddlewareUtil middlewareUtil;

    @Autowired
    private Audience audience;

    @Autowired
    private ZqPictureUserActionService zqPictureUserActionService;

    @Autowired
    private ZqPictureService zqPictureService;

    @Override
    public String signUp(String sid, String pwd, String name) throws IOException {
        //拿token，在线验证
        String token = middlewareUtil.getToken(sid, pwd, "1");
        if (token != null) {
            ZqHomeUser a = this.baseMapper.selectOne(new QueryWrapper<ZqHomeUser>().eq("sid", sid));
            if (a != null) {
                throw new ZqHomeException(ResultEnum.HASSIGNUP_ERROR);
            }
            String info = middlewareUtil.getTokenInfo(token);
            JSONObject data = (JSONObject) JSONObject.parse(info);
            ZqHomeUser zqHomeUser = new ZqHomeUser();
            zqHomeUser.setSid(Long.parseLong(sid));
            zqHomeUser.setOwnerId(((Integer)data.get("owner_id")).longValue());
            zqHomeUser.setRole("student");
            zqHomeUser.setNickName(name);

            this.baseMapper.insert(zqHomeUser);
            return "注册成功";
        } else {
            throw new ZqHomeException(ResultEnum.SIGNUP_ERROR);
        }
    }

    @Override
    public String getVCode(String email)throws IOException{
        String vCode=middlewareUtil.getVCode(email);
        if(vCode!=null){
            System.out.println(vCode);
            return vCode;
        }else{
            System.out.println("???");
            throw new ZqHomeException(ResultEnum.ERROR);
        }
    }

    public String signUpByEmail(String sid, String pwd, String name)throws IOException{
        //拿token，在线验证

        String token = middlewareUtil.getTokenByZqAuth(sid, pwd, "1");
        System.out.println(token);
        if (token != null) {
            ZqHomeUser a = this.baseMapper.selectOne(new QueryWrapper<ZqHomeUser>().eq("sid", sid));
            if (a != null) {
                throw new ZqHomeException(ResultEnum.HASSIGNUP_ERROR);
            }
            String info = middlewareUtil.getTokenInfo(token);
            JSONObject data = (JSONObject) JSONObject.parse(info);
            ZqHomeUser zqHomeUser = new ZqHomeUser();
            zqHomeUser.setSid(Long.parseLong(sid));
            zqHomeUser.setOwnerId(((Integer)data.get("owner_id")).longValue());
            zqHomeUser.setRole("student");
            zqHomeUser.setNickName(name);

            this.baseMapper.insert(zqHomeUser);
            return "注册成功";
        } else {
            throw new ZqHomeException(ResultEnum.SIGNUP_ERROR);
        }
    }

    @Override
    public JSONObject loginByEmail(String email, String vCode) throws IOException{
        //拿token
        String token = middlewareUtil.getTokenByZqAuth(email,vCode,"0");
        if (token != null) {
            JSONObject res = new JSONObject();
            //生成token返回给前端
            logger.info("### 登录成功, token={} ###", token);
            res.put("token", token);
            return res;
        } else {
            throw new ZqHomeException(ResultEnum.LOGIN_ERROR);
        }
    }

    @Override
    public JSONObject login(String sid, String pwd) throws IOException {
        ZqHomeUser zqHomeUser = this.baseMapper.selectOne(new QueryWrapper<ZqHomeUser>().eq("sid", sid));
        if (zqHomeUser == null) {
            throw new ZqHomeException(ResultEnum.NOTSIGN_ERROR);
        }
        //拿token，本地匹配，检验密码
        String token = middlewareUtil.getToken(sid, pwd, "0");
        if (token != null) {
            JSONObject res = new JSONObject();
            //生成token返回给前端
            String JwtToken = JwtTokenUtil.createJWT(Long.toString(zqHomeUser.getId()), Long.toString(zqHomeUser.getSid()),
                    zqHomeUser.getRole(), audience);
            logger.info("### 登录成功, token={} ###", token);

            res.put("token", JwtToken);
            res.put("id", zqHomeUser.getId());
            res.put("name", zqHomeUser.getNickName());
            return res;
        } else {
            throw new ZqHomeException(ResultEnum.LOGIN_ERROR);
        }
    }

    @Override
    public JSONObject getInfo(Long id) {
        List<ZqPicture> picList = zqPictureService.list(new QueryWrapper<ZqPicture>()
                .eq("owner_id", id));
        System.out.println(picList.size());
        List<ZqPictureUserAction> likeList = zqPictureUserActionService.list(new QueryWrapper<ZqPictureUserAction>()
                .eq("Uid", id).eq("action", "like"));
        List<ZqPictureUserAction> reportList = zqPictureUserActionService.list(new QueryWrapper<ZqPictureUserAction>()
                .eq("Uid", id).eq("action", "report"));
        List<ZqPictureUserAction> uploadList = zqPictureUserActionService.list(new QueryWrapper<ZqPictureUserAction>()
                .eq("Uid", id).eq("action", "upload"));

        ZqHomeUser user = this.getOne(new QueryWrapper<ZqHomeUser>().eq("id", id));
        JSONObject res = new JSONObject();
        res.put("Uid", user.getId());
        res.put("sid", user.getSid());
        res.put("name", user.getNickName());
        res.put("picList", picList);
        res.put("like", likeList);
        res.put("report", reportList);
        res.put("upload", uploadList);
        return res;
    }


}